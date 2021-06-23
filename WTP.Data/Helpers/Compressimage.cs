using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using WTP.Data.Interfaces;

namespace WTP.Data.Helpers
{
    public class Compressimage : ICompressimage
    {
        private string imagePath;
        private string imageName;
        private IFormFile imageFile;

        public Compressimage(string imagePath, string imageName, IFormFile imageFile)
        {
            this.imagePath = imagePath;
            this.imageName = imageName;
            this.imageFile = imageFile;
            Resize();
        }

        public void Resize()
        {
            int width = 200;
            int height = 200;
            Image image = Image.FromStream(imageFile.OpenReadStream(), true, true);
            var newImage = new Bitmap(width, height);
            using (var a = Graphics.FromImage(newImage))
            {
                a.DrawImage(image, 0, 0, width, height);
                newImage.Save(imagePath);
            }
        }
        //public void Compress()
        //{
        //    try
        //    {
        //        using (var image = new FileStream(imagePath, FileMode.Create))
        //        {
        //            float maxHeight = 900.0f;
        //            float maxWidth = 900.0f;
        //            int newWidth;
        //            int newHeight;
        //            string extension;
        //            Bitmap originalBMP = new Bitmap(sourcePath);
        //            int originalWidth = originalBMP.Width;
        //            int originalHeight = originalBMP.Height;

        //            if (originalWidth > maxWidth || originalHeight > maxHeight)
        //            {

        //                // To preserve the aspect ratio  
        //                float ratioX = (float)maxWidth / (float)originalWidth;
        //                float ratioY = (float)maxHeight / (float)originalHeight;
        //                float ratio = Math.Min(ratioX, ratioY);
        //                newWidth = (int)(originalWidth * ratio);
        //                newHeight = (int)(originalHeight * ratio);
        //            }
        //            else
        //            {
        //                newWidth = (int)originalWidth;
        //                newHeight = (int)originalHeight;

        //            }
        //            Bitmap bitMAP1 = new Bitmap(originalBMP, newWidth, newHeight);
        //            Graphics imgGraph = Graphics.FromImage(bitMAP1);
        //            extension = Path.GetExtension(targetPath);
        //            if (extension == ".png" || extension == ".gif")
        //            {
        //                imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
        //                imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);


        //                bitMAP1.Save(targetPath, image.RawFormat);

        //                bitMAP1.Dispose();
        //                imgGraph.Dispose();
        //                originalBMP.Dispose();
        //            }
        //            else if (extension == ".jpg")
        //            {

        //                imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
        //                imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
        //                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
        //                Encoder myEncoder = Encoder.Quality;
        //                EncoderParameters myEncoderParameters = new EncoderParameters(1);
        //                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
        //                myEncoderParameters.Param[0] = myEncoderParameter;
        //                bitMAP1.Save(targetPath, jpgEncoder, myEncoderParameters);

        //                bitMAP1.Dispose();
        //                imgGraph.Dispose();
        //                originalBMP.Dispose();

        //            }


        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;

        //    }
        //}

        //public static ImageCodecInfo GetEncoder(ImageFormat format)
        //{

        //    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

        //    foreach (ImageCodecInfo codec in codecs)
        //    {
        //        if (codec.FormatID == format.Guid)
        //        {
        //            return codec;
        //        }
        //    }
        //    return null;
        //}


    }

}
