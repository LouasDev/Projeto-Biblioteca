using System;
using System.IO;
using PdfSharp.Fonts;

namespace BibliotecaApp.Utils.Etiquetas
{
    internal sealed class SegoeUiFontResolver : IFontResolver
    {
        public const string FamilyName = "Segoe UI";
        private const string REGULAR = "SEGOEUI#R";
        private const string BOLD = "SEGOEUI#B";
        private const string ITALIC = "SEGOEUI#I";
        private const string BOLDITALIC = "SEGOEUI#Z";

        private static byte[] _regular;
        private static byte[] _bold;
        private static byte[] _italic;
        private static byte[] _boldItalic;

        public string DefaultFontName => FamilyName;

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            bool isEmpty = string.IsNullOrWhiteSpace(familyName);
            string noSpaces = isEmpty ? string.Empty : familyName.Replace(" ", string.Empty);

            if (isEmpty ||
                familyName.Equals(FamilyName, StringComparison.OrdinalIgnoreCase) ||
                noSpaces.Equals("segoeui", StringComparison.OrdinalIgnoreCase) ||
                familyName.IndexOf("segoe", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (isBold && isItalic) return new FontResolverInfo(BOLDITALIC);
                if (isBold) return new FontResolverInfo(BOLD);
                if (isItalic) return new FontResolverInfo(ITALIC);
                return new FontResolverInfo(REGULAR);
            }

            if (isBold && isItalic) return new FontResolverInfo(BOLDITALIC);
            if (isBold) return new FontResolverInfo(BOLD);
            if (isItalic) return new FontResolverInfo(ITALIC);
            return new FontResolverInfo(REGULAR);
        }

        public byte[] GetFont(string faceName)
        {
            switch (faceName)
            {
                case REGULAR: return _regular ?? (_regular = LoadFromFontsFolder("segoeui.ttf"));
                case BOLD: return _bold ?? (_bold = LoadFromFontsFolder("segoeuib.ttf"));
                case ITALIC: return _italic ?? (_italic = LoadFromFontsFolder("segoeuii.ttf"));
                case BOLDITALIC: return _boldItalic ?? (_boldItalic = LoadFromFontsFolder("segoeuiz.ttf"));
                default: return null;
            }
        }

        private static byte[] LoadFromFontsFolder(string fileName)
        {
            string fontsDir = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            string path = Path.Combine(fontsDir, fileName);
            if (!File.Exists(path))
                throw new FileNotFoundException("Fonte não encontrada no Windows\\Fonts.", path);
            return File.ReadAllBytes(path);
        }
    }
}