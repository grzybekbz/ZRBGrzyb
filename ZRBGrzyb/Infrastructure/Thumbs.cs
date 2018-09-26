using Microsoft.Extensions.Configuration;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ZRBGrzyb.Infrastructure {

    public class Thumbs {

        public static void Create(string fileName, IConfiguration config) {

            const int size = 500;
            const long quality = 75;

            string photosDirectory = config["Data:Grzyb:PhotosDirectory"];

            var inputPath = Path.Combine(Directory.GetCurrentDirectory(),
                photosDirectory, fileName);
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(),
                photosDirectory + "/thumbs", fileName);

            using (var image = new Bitmap(Image.FromFile(inputPath))) {

                int width, height;
                if (image.Width > image.Height) {
                    width = size;
                    height = Convert.ToInt32(image.Height * size / (double)image.Width);
                } else {

                    width = Convert.ToInt32(image.Width * (size - 100) / (double)image.Height);
                    height = size - 100;
                }
                var resized = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(resized)) {

                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, width, height);

                    using (var output = new FileStream(outputPath, FileMode.Create)) {

                        var qualityParamId = Encoder.Quality;
                        var encoderParameters = new EncoderParameters(1);
                        encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                        var codec = ImageCodecInfo.GetImageDecoders()
                            .FirstOrDefault(cod => cod.FormatID == ImageFormat.Jpeg.Guid);
                        resized.Save(output, codec, encoderParameters);
                    }
                }
            }
        }
    }
}
