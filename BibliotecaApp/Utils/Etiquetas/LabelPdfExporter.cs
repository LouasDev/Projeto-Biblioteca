using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using ZXing;
using ZXing.Common;

namespace BibliotecaApp.Utils.Etiquetas
{
    public static class LabelPdfExporter
    {
        private const int Colunas = 3;
        private const int Linhas = 10;

        private const double LabelWidthMm = 66.7;
        private const double LabelHeightMm = 25.4;
        private const double HorizontalPitchMm = 69.8;
        private const double VerticalPitchMm = 25.4;

        private const double PageMarginLeftMm = 4.8;
        private const double PageMarginRightMm = 4.8;
        private const double PageMarginTopMm = 12.7;
        private const double PageMarginBottomMm = 12.7;

        private const double CornerRadiusMm = 3.0;
        private const double PageWidthMm = 215.9;
        private const double PageHeightMm = 279.4;

        // REMOVIDO: campos estáticos de fontes que causavam o TypeInitializerException
        // private static readonly XFont TitleFont = ...
        // private static readonly XFont InfoFont  = ...
        // private static readonly XFont CodeFont  = ...

        public static void ExportPdf(List<EtiquetaLabelItem> items, LabelPrintOptions options, string filePath, int barcodeDpi = 200)
        {
            if (items == null || items.Count == 0) throw new ArgumentException("Sem itens para exportar.");
            if (options == null) options = new LabelPrintOptions();

            // Configura o resolver ANTES de criar qualquer XFont
            if (GlobalFontSettings.FontResolver == null)
                GlobalFontSettings.FontResolver = new SegoeUiFontResolver();

            // Cria fontes depois do resolver estar definido
            var titleFont = new XFont("Segoe UI", 9, XFontStyleEx.Bold);
            var infoFont  = new XFont("Segoe UI", 8, XFontStyleEx.Regular);
            var codeFont  = new XFont("Segoe UI", 7, XFontStyleEx.Regular);

            if (options.GroupByGenre)
                items = GroupByGenre(items);

            int currentIndex = 0;
            int startPosition = Math.Max(1, Math.Min(Colunas * Linhas, options.StartPosition));

            using (var doc = new PdfDocument())
            {
                var mm = 72.0 / 25.4; // pt por mm

                while (currentIndex < items.Count)
                {
                    var page = doc.AddPage();
                    page.Width = PageWidthMm * mm;
                    page.Height = PageHeightMm * mm;
                    page.Orientation = PdfSharp.PageOrientation.Portrait;

                    using (var gfx = XGraphics.FromPdfPage(page))
                    {
                        double layoutLeft = (PageMarginLeftMm + options.OffsetXmm) * mm;
                        double layoutTop  = (PageMarginTopMm  + options.OffsetYmm) * mm;

                        double wLabel = LabelWidthMm * mm;
                        double hLabel = LabelHeightMm * mm;
                        double pitchX = HorizontalPitchMm * mm;
                        double pitchY = VerticalPitchMm * mm;

                        double headerX = (PageMarginLeftMm) * mm;
                        double headerY = 0;
                        double headerW = (PageWidthMm - PageMarginLeftMm - PageMarginRightMm) * mm;
                        double headerH = (PageMarginTopMm) * mm;

                        int labelsPerPage = Colunas * Linhas;
                        int skip = Math.Max(0, startPosition - 1);
                        int pageCapacity = Math.Max(0, labelsPerPage - skip);

                        string headerText = BuildPageHeaderGenresText(items, currentIndex, pageCapacity);
                        var headerRect = new XRect(headerX, headerY, headerW, headerH);
                        string headerDraw = Ellipsize(gfx, headerText, titleFont, headerRect.Width);
                        gfx.DrawString(headerDraw, new XFont("Segoe UI", 10, XFontStyleEx.Bold), XBrushes.Black, headerRect, XStringFormats.Center);

                        var pen = new XPen(XColors.LightGray, 0.5);
                        using (var ms = new MemoryStream(256 * 1024))
                        {
                            for (int r = 0; r < Linhas; r++)
                            {
                                for (int c = 0; c < Colunas; c++)
                                {
                                    double x = layoutLeft + c * pitchX;
                                    double y = layoutTop + r * pitchY;
                                    var cell = new XRect(x, y, wLabel, hLabel);

                                    var path = CreateRoundedPathPdf(cell, CornerRadiusMm * mm);
                                    gfx.DrawPath(pen, path);

                                    if (skip > 0) { skip--; continue; }
                                    if (currentIndex >= items.Count) continue;

                                    var item = items[currentIndex++];
                                    gfx.Save();
                                    gfx.IntersectClip(path);

                                    double pad = 1.5 * mm;
                                    var content = new XRect(cell.X + pad, cell.Y + pad, cell.Width - 2 * pad, cell.Height - 2 * pad);
                                    // PASSA as fontes para o desenho
                                    DrawLabelPdf(gfx, content, item, ms, barcodeDpi, titleFont, infoFont, codeFont);

                                    gfx.Restore();
                                }
                            }
                        }

                        startPosition = 1; // próximas páginas do lote começam no 1
                    }
                }

                doc.Save(filePath);
            }
        }

        private static List<EtiquetaLabelItem> GroupByGenre(List<EtiquetaLabelItem> items)
        {
            var map = new Dictionary<string, List<EtiquetaLabelItem>>(StringComparer.OrdinalIgnoreCase);
            var order = new List<string>();
            foreach (var it in items)
            {
                var g = string.IsNullOrWhiteSpace(it.Genero) ? "Sem gênero" : it.Genero.Trim();
                if (!map.TryGetValue(g, out var list))
                {
                    list = new List<EtiquetaLabelItem>();
                    map[g] = list;
                    order.Add(g);
                }
                list.Add(it);
            }
            var result = new List<EtiquetaLabelItem>(items.Count);
            foreach (var g in order) result.AddRange(map[g]);
            return result;
        }

        private static string BuildPageHeaderGenresText(IList<EtiquetaLabelItem> items, int startIndex, int pageCapacity)
        {
            if (pageCapacity <= 0 || startIndex >= items.Count) return "Vários gêneros";
            int endExclusive = Math.Min(items.Count, startIndex + pageCapacity);
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var list = new List<string>();
            for (int i = startIndex; i < endExclusive; i++)
            {
                var g = string.IsNullOrWhiteSpace(items[i].Genero) ? "Sem gênero" : items[i].Genero.Trim();
                if (set.Add(g)) list.Add(g);
            }
            return list.Count == 1 ? $"Gênero: {list[0]}" : "Gêneros: " + string.Join(", ", list);
        }

        // Ajustado: recebe as fontes como parâmetros
        private static void DrawLabelPdf(
            XGraphics gfx, XRect bounds, EtiquetaLabelItem item,
            MemoryStream sharedStream, int barcodeDpi,
            XFont titleFont, XFont infoFont, XFont codeFont)
        {
            const double mm = 72.0 / 25.4;

            double vGap = 0.7 * mm;
            double hGap = 1.0 * mm;
            double minShelfUnderline = 18.0 * mm;

            double titleH = Math.Round(bounds.Height * 0.28);
            double infoH = Math.Round(bounds.Height * 0.18);
            double barcodeH = Math.Max(8, bounds.Height - titleH - infoH - 2 * vGap);

            var titleRect = new XRect(bounds.Left, bounds.Top, bounds.Width, titleH);
            var infoRect = new XRect(bounds.Left, titleRect.Bottom + vGap, bounds.Width, infoH);
            var barcodeRect = new XRect(bounds.Left, infoRect.Bottom + vGap, bounds.Width, barcodeH);

            string titulo = Ellipsize(gfx, item.Titulo ?? "", titleFont, titleRect.Width);
            gfx.DrawString(titulo, titleFont, XBrushes.Black, titleRect, XStringFormats.TopLeft);

            string genero = item.Genero ?? "";
            string shelfLabel = "Prateleira:";
            double shelfLabelW = gfx.MeasureString(shelfLabel, infoFont).Width;
            double maxGenreW = Math.Max(0, infoRect.Width - shelfLabelW - minShelfUnderline - hGap);

            string generoFit = Ellipsize(gfx, genero, infoFont, maxGenreW);
            var genreRect = new XRect(infoRect.Left, infoRect.Top, maxGenreW, infoRect.Height);
            gfx.DrawString(generoFit, infoFont, XBrushes.Black, genreRect, XStringFormats.TopLeft);

            double shelfLabelX = genreRect.Right + hGap;
            var shelfRect = new XRect(shelfLabelX, infoRect.Top, shelfLabelW, infoRect.Height);
            gfx.DrawString(shelfLabel, infoFont, XBrushes.Black, shelfRect, XStringFormats.TopLeft);

            double underlineLeft = shelfRect.Right + hGap;
            double underlineRight = infoRect.Right;
            double underlineY = infoRect.Top + infoRect.Height / 2.0 + 1.0;
            if (underlineRight > underlineLeft)
            {
                gfx.DrawLine(new XPen(XColors.Black, 0.6), underlineLeft, underlineY, underlineRight, underlineY);
            }

            string code = item.CodigoBarras ?? "";

            double codeTextH = Math.Max(12, gfx.MeasureString("0", codeFont).Height);
            var barcodeOnlyRect = new XRect(barcodeRect.Left, barcodeRect.Top, barcodeRect.Width, Math.Max(8, barcodeRect.Height - codeTextH - 2));
            var codeTextRect = new XRect(barcodeRect.Left, barcodeOnlyRect.Bottom + 2, barcodeRect.Width, codeTextH);

            if (!string.IsNullOrWhiteSpace(code))
            {
                int widthPx = Math.Max(32, (int)Math.Round(barcodeOnlyRect.Width * barcodeDpi / 72.0));
                int heightPx = Math.Max(24, (int)Math.Round(barcodeOnlyRect.Height * barcodeDpi / 72.0));

                try
                {
                    var writer = new BarcodeWriter
                    {
                        Format = BarcodeFormat.CODE_128,
                        Options = new EncodingOptions
                        {
                            PureBarcode = true,
                            Margin = 0,
                            Width = widthPx,
                            Height = heightPx
                        }
                    };

                    using (var bmp = writer.Write(code))
                    {
                        sharedStream.Position = 0;
                        sharedStream.SetLength(0);
                        bmp.Save(sharedStream, ImageFormat.Png);
                        sharedStream.Position = 0;

                        using (var ximg = XImage.FromStream(sharedStream))
                        {
                            gfx.DrawImage(ximg, barcodeOnlyRect);
                        }
                    }
                }
                catch { /* fallback: texto apenas */ }
            }

            gfx.DrawString(code, codeFont, XBrushes.Black, codeTextRect, XStringFormats.Center);
        }

        private static string Ellipsize(XGraphics gfx, string text, XFont font, double maxWidth)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            var size = gfx.MeasureString(text, font);
            if (size.Width <= maxWidth) return text;

            const string ell = "...";
            int left = 0, right = text.Length, fit = 0;
            while (left <= right)
            {
                int mid = (left + right) / 2;
                string candidate = text.Substring(0, Math.Max(0, mid)) + ell;
                if (gfx.MeasureString(candidate, font).Width <= maxWidth)
                {
                    fit = mid; left = mid + 1;
                }
                else right = mid - 1;
            }
            return fit > 0 ? text.Substring(0, fit) + ell : ell;
        }

        private static XGraphicsPath CreateRoundedPathPdf(XRect rect, double radius)
        {
            double r = Math.Max(0, Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2.0));
            double d = r * 2.0;
            var path = new XGraphicsPath();
            if (r <= 0) { path.AddRectangle(rect); path.CloseFigure(); return path; }

            path.AddArc(rect.Left, rect.Top, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}