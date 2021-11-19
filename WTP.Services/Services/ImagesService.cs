using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace WTP.Services.Services
{
    public class ImagesService
    {
        public string SaveImage(IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine( "Images", imageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
                ResizeImage(imagePath, imageFile);

                return imageName;
            }
            throw new Exception();
        }

        private void ResizeImage(string imagePath, IFormFile imageFile)
        {
            int width = 200;
            int height = 200;
            Image image = Image.FromStream(imageFile.OpenReadStream(), true, true);
            var newImage = new Bitmap(width, height);
            using var a = Graphics.FromImage(newImage);
            a.DrawImage(image, 0, 0, width, height);
            newImage.Save(imagePath);
        }

        public void DeleteImage(string imagePath)
        {
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }
    }
}