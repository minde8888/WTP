using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;
using System.Linq;


namespace WTP.Services.Services
{
    public class ImagesService
    {
        public string SaveImage(IFormFile imageFile, string path)
        {
            if (imageFile != null)
            {
                string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine(path, "Images", imageName);

                ResizeImage(imagePath, imageName, imageFile);

                return imageName;
            }
            throw new Exception();
        }

        private void ResizeImage(string imagePath, string imageName, IFormFile imageFile)
        {
            int width = 200;
            int height = 200;
            Image image = Image.FromStream(imageFile.OpenReadStream(), true, true);
            var newImage = new Bitmap(width, height);
            using var a = Graphics.FromImage(newImage);
            a.DrawImage(image, 0, 0, width, height);
            newImage.Save(imagePath);
        }
    }
}
