//using Microsoft.AspNetCore.Http;
//using System.Drawing;
//using WTP.Data.Interfaces;

//namespace WTP.Data.Helpers
//{
//    public class Compressimage : ICompressimage
//    {
//        public void Resize(string imagePath, string imageName, IFormFile imageFile)
//        {
//            int width = 200;
//            int height = 200;
//            Image image = Image.FromStream(imageFile.OpenReadStream(), true, true);
//            var newImage = new Bitmap(width, height);
//            using var a = Graphics.FromImage(newImage);
//            a.DrawImage(image, 0, 0, width, height);
//            newImage.Save(imagePath);
//        }
//    }
//}
