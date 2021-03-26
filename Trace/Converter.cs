﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using SkiaSharp;

namespace Trace
{
    static class Converter
    {
        public static void SaveAsSvg(SKBitmap source, IEnumerable<SKPath> paths, string filename, string fillColor = "#000000")
        {
            var sb = new StringBuilder();

            sb.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{source.Width}\" height=\"{source.Height}\">");

            foreach (var path in paths)
            {
                sb.AppendLine($"    <path fill=\"{fillColor}\" d=\"{path.ToSvgPathData()}\"/>");
            }

            sb.AppendLine($"</svg>");

            var svg = sb.ToString();

            File.WriteAllText(filename, svg);
        }

        public static void SaveAsPng(SKBitmap source, IEnumerable<SKPath> paths, string filename, string fillColor = "#000000")
        {
            using var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                IsAntialias = true,
                Color = SKColor.Parse(fillColor)
            };

            using var bitmap = new SKBitmap(source.Width, source.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            using var canvas = new SKCanvas(bitmap);

            canvas.Clear(SKColors.White);

            foreach (var path in paths)
            {
                canvas.DrawPath(path, paint);
            }

            using var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
            using var file = File.Create(filename);

            data.SaveTo(file);
        }
    }
}