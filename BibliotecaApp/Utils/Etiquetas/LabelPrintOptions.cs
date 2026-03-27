using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

using ZXing;
using ZXing.Common;

namespace BibliotecaApp.Utils.Etiquetas
{
    public sealed class LabelPrintOptions
    {
        public int StartPosition { get; set; } = 1;     // 1..30
        public float OffsetXmm { get; set; } = 0f;
        public float OffsetYmm { get; set; } = 0f;
        public string PrinterName { get; set; }
        public bool GroupByGenre { get; set; } = true;
        public bool ShowPrintDialog { get; set; } = true;
        public bool Preview { get; set; } = false;

        // Novo: suporte a salvar direto em arquivo (ex.: Microsoft Print to PDF)
        public bool PrintToFile { get; set; } = false;
        public string OutputFilePath { get; set; }
    }

    public sealed class EtiquetaLabelItem
    {
        public string CodigoBarras { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
    }

    public static class LabelPrinterService
    {
        private const int Colunas = 3;
        private const int Linhas = 10;

        private const float LabelWidthMm = 66.7f;
        private const float LabelHeightMm = 25.4f;
        private const float HorizontalPitchMm = 69.8f;
        private const float VerticalPitchMm = 25.4f;

        private const float PageMarginLeftMm = 4.8f;
        private const float PageMarginRightMm = 4.8f;
        private const float PageMarginTopMm = 12.7f;
        private const float PageMarginBottomMm = 12.7f;

        private const float CornerRadiusMm = 3.0f;
        private const float PageWidthMm = 215.9f;
        private const float PageHeightMm = 279.4f;

        private static readonly Font TitleFont = new Font("Segoe UI", 9.0f, FontStyle.Bold, GraphicsUnit.Point);
        private static readonly Font InfoFont = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point);
        private static readonly Font CodeFont = new Font("Segoe UI", 7.0f, FontStyle.Regular, GraphicsUnit.Point);

        public static void PrintLabels(IEnumerable<EtiquetaLabelItem> sourceItems, LabelPrintOptions options)
        {
            if (sourceItems == null) throw new ArgumentNullException(nameof(sourceItems));
            if (options == null) options = new LabelPrintOptions();

            var items = sourceItems.ToList();
            if (items.Count == 0) return;

            if (options.GroupByGenre)
                items = GroupByGenre(items);

            int currentIndex = 0;
            int startPosition = Math.Max(1, Math.Min(Colunas * Linhas, options.StartPosition));
            float offsetXmm = options.OffsetXmm;
            float offsetYmm = options.OffsetYmm;

            var doc = new PrintDocument { OriginAtMargins = false };

            try
            {
                doc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                var custom = new PaperSize("US Letter 8.5x11 in", MmToHi(PageWidthMm), MmToHi(PageHeightMm));
                doc.DefaultPageSettings.PaperSize = custom;
                doc.DefaultPageSettings.Landscape = false;
            }
            catch
            {
                try
                {
                    var letter = doc.PrinterSettings.PaperSizes
                        .Cast<PaperSize>()
                        .FirstOrDefault(p => p.Kind == PaperKind.Letter);
                    if (letter != null)
                    {
                        doc.DefaultPageSettings.PaperSize = letter;
                        doc.DefaultPageSettings.Landscape = false;
                    }
                }
                catch { }
            }

            if (!string.IsNullOrWhiteSpace(options.PrinterName))
            {
                try { doc.PrinterSettings.PrinterName = options.PrinterName; } catch { }
            }

            // Novo: salva direto no arquivo indicado (driver que suporta PrintToFile, ex.: Microsoft Print to PDF)
            if (options.PrintToFile && !string.IsNullOrWhiteSpace(options.OutputFilePath))
            {
                try
                {
                    doc.PrinterSettings.PrintToFile = true;
                    doc.PrinterSettings.PrintFileName = options.OutputFilePath;
                }
                catch { /* alguns drivers podem ignorar */ }
            }

            doc.PrintPage += (s, e) =>
            {
                var ps = e.PageSettings.PaperSize;
                int pageW = ps.Width, pageH = ps.Height;
                int hardX = (int)Math.Round(e.PageSettings.HardMarginX);
                int hardY = (int)Math.Round(e.PageSettings.HardMarginY);

                int originPageX = -hardX + MmToHi(offsetXmm);
                int originPageY = -hardY + MmToHi(offsetYmm);
                var pageRect = new Rectangle(originPageX, originPageY, pageW, pageH);

                int insetL = MmToHi(PageMarginLeftMm);
                int insetR = MmToHi(PageMarginRightMm);
                int insetT = MmToHi(PageMarginTopMm);
                int insetB = MmToHi(PageMarginBottomMm);

                var layout = new Rectangle(
                    pageRect.Left + insetL,
                    pageRect.Top + insetT,
                    Math.Max(0, pageRect.Width - insetL - insetR),
                    Math.Max(0, pageRect.Height - insetT - insetB));

                var headerRect = new Rectangle(
                    pageRect.Left + insetL,
                    pageRect.Top,
                    Math.Max(0, pageRect.Width - insetL - insetR),
                    insetT);

                int wLabel = MmToHi(LabelWidthMm);
                int hLabel = MmToHi(LabelHeightMm);
                int pitchX = MmToHi(HorizontalPitchMm);
                int pitchY = MmToHi(VerticalPitchMm);

                int labelsPerPage = Colunas * Linhas;
                int skip = Math.Max(0, startPosition - 1);
                int pageCapacity = Math.Max(0, labelsPerPage - skip);

                string headerText = BuildPageHeaderGenresText(items, currentIndex, pageCapacity);
                using (var brush = new SolidBrush(Color.Black))
                {
                    var fmtHeader = new StringFormat(StringFormatFlags.NoWrap)
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.EllipsisCharacter
                    };
                    e.Graphics.DrawString(headerText, new Font("Segoe UI", 10.0f, FontStyle.Bold), brush, headerRect, fmtHeader);
                }

                var oldSmoothing = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (var pen = new Pen(Color.LightGray))
                {
                    for (int r = 0; r < Linhas; r++)
                    {
                        for (int c = 0; c < Colunas; c++)
                        {
                            int x = layout.Left + c * pitchX;
                            int y = layout.Top + r * pitchY;
                            var cell = new Rectangle(x, y, wLabel, hLabel);

                            using (var path = CreateRoundedRect(cell, MmToHi(CornerRadiusMm)))
                            {
                                e.Graphics.DrawPath(pen, path);

                                if (skip > 0) { skip--; continue; }
                                if (currentIndex >= items.Count) continue;

                                var state = e.Graphics.Save();
                                try
                                {
                                    e.Graphics.SetClip(path);
                                    var item = items[currentIndex++];
                                    int pad = MmToHi(1.5f);
                                    var contentRect = new Rectangle(cell.Left + pad, cell.Top + pad, cell.Width - pad * 2, cell.Height - pad * 2);
                                    DrawLabel(e.Graphics, contentRect, item);
                                }
                                finally
                                {
                                    e.Graphics.Restore(state);
                                }
                            }
                        }
                    }
                }

                e.Graphics.SmoothingMode = oldSmoothing;

                startPosition = 1; // próximas páginas sempre iniciam do slot 1
                e.HasMorePages = currentIndex < items.Count;
            };

            if (options.Preview)
            {
                using (var prev = new PrintPreviewDialog())
                {
                    prev.Document = doc;
                    prev.ShowDialog();
                }
                return;
            }

            if (options.ShowPrintDialog)
            {
                using (var pd = new PrintDialog())
                {
                    pd.Document = doc;
                    if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        doc.Print();
                }
            }
            else
            {
                doc.PrintController = new StandardPrintController();
                doc.Print();
            }
        }

        private static List<EtiquetaLabelItem> GroupByGenre(List<EtiquetaLabelItem> items)
        {
            var map = new Dictionary<string, List<EtiquetaLabelItem>>(StringComparer.OrdinalIgnoreCase);
            var order = new List<string>();
            foreach (var it in items)
            {
                var g = NormalizeGenre(it.Genero);
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

        private static string NormalizeGenre(string genero)
            => string.IsNullOrWhiteSpace(genero) ? "Sem gênero" : genero.Trim();

        private static string BuildPageHeaderGenresText(IList<EtiquetaLabelItem> items, int startIndex, int pageCapacity)
        {
            if (pageCapacity <= 0 || startIndex >= items.Count)
                return "Vários gêneros";
            int endExclusive = Math.Min(items.Count, startIndex + pageCapacity);
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var list = new List<string>();
            for (int i = startIndex; i < endExclusive; i++)
            {
                var g = NormalizeGenre(items[i].Genero);
                if (set.Add(g)) list.Add(g);
            }
            return list.Count == 1 ? $"Gênero: {list[0]}" : "Gêneros: " + string.Join(", ", list);
        }

        private static void DrawLabel(Graphics g, Rectangle bounds, EtiquetaLabelItem item)
        {
            int vGap = MmToHi(0.7f);
            int hGap = MmToHi(1.0f);
            int minShelfUnderline = MmToHi(18.0f);

            int titleAreaH = (int)Math.Round(bounds.Height * 0.28);
            int infoAreaH = (int)Math.Round(bounds.Height * 0.18);
            int barcodeAreaH = Math.Max(8, bounds.Height - titleAreaH - infoAreaH - vGap * 2);

            Rectangle titleRect = new Rectangle(bounds.Left, bounds.Top, bounds.Width, titleAreaH);
            Rectangle infoRect = new Rectangle(bounds.Left, titleRect.Bottom + vGap, bounds.Width, infoAreaH);
            Rectangle barcodeRect = new Rectangle(bounds.Left, infoRect.Bottom + vGap, bounds.Width, barcodeAreaH);

            using (var brush = new SolidBrush(Color.Black))
            {
                var fmtTitle = new StringFormat(StringFormatFlags.NoWrap)
                {
                    Trimming = StringTrimming.EllipsisCharacter,
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Near
                };
                g.DrawString(item.Titulo ?? "", TitleFont, brush, titleRect, fmtTitle);

                string genero = item.Genero ?? "";
                string shelfLabel = "Prateleira:";
                var shelfLabelSize = g.MeasureString(shelfLabel, InfoFont);

                int shelfLabelW = (int)Math.Ceiling(shelfLabelSize.Width);
                int maxGenreW = Math.Max(0, infoRect.Width - shelfLabelW - minShelfUnderline - hGap);

                var genreRect = new Rectangle(infoRect.Left, infoRect.Top, maxGenreW, infoRect.Height);
                var fmtGenre = new StringFormat(StringFormatFlags.NoWrap)
                {
                    Trimming = StringTrimming.EllipsisCharacter,
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Near
                };
                g.DrawString(genero, InfoFont, brush, genreRect, fmtGenre);

                int shelfLabelX = genreRect.Right + hGap;
                var shelfLabelRect = new Rectangle(shelfLabelX, infoRect.Top, shelfLabelW, infoRect.Height);
                g.DrawString(shelfLabel, InfoFont, brush, shelfLabelRect, fmtGenre);

                int underlineLeft = shelfLabelRect.Right + hGap;
                int underlineRight = infoRect.Right;
                int underlineY = infoRect.Top + infoRect.Height / 2 + 2;
                if (underlineRight > underlineLeft)
                {
                    using (var pen = new Pen(Color.Black, 1))
                        g.DrawLine(pen, underlineLeft, underlineY, underlineRight, underlineY);
                }

                // Barcode
                int codeTextHeight = (int)Math.Max(12, CodeFont.GetHeight(g));
                Rectangle barcodeOnlyRect = new Rectangle(
                    barcodeRect.Left,
                    barcodeRect.Top,
                    barcodeRect.Width,
                    Math.Max(8, barcodeRect.Height - codeTextHeight - 2));
                Rectangle codeTextRect = new Rectangle(
                    barcodeRect.Left,
                    barcodeOnlyRect.Bottom + 2,
                    barcodeRect.Width,
                    codeTextHeight);

                string code = item.CodigoBarras ?? "";
                if (!string.IsNullOrWhiteSpace(code))
                {
                    int widthPx = Math.Max(32, (int)Math.Round(barcodeOnlyRect.Width * g.DpiX / 100f));
                    int heightPx = Math.Max(24, (int)Math.Round(barcodeOnlyRect.Height * g.DpiY / 100f));
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
                            g.DrawImage(bmp, barcodeOnlyRect);
                        }
                    }
                    catch { /* fallback: somente texto */ }
                }

                var fmtCode = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(code, CodeFont, brush, codeTextRect, fmtCode);
            }
        }

        private static GraphicsPath CreateRoundedRect(Rectangle rect, int radius)
        {
            int r = Math.Max(0, Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2));
            int d = r * 2;
            var path = new GraphicsPath();
            if (r <= 0) { path.AddRectangle(rect); path.CloseFigure(); return path; }

            path.AddArc(rect.Left, rect.Top, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static int MmToHi(float mm) => (int)Math.Round(mm / 25.4f * 100f);
    }
}