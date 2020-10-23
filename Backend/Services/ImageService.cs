using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services
{
    public class ImageService : IImageService
    {
        private List<ImageModel> _images;
        private readonly IWebHostEnvironment _environment;
        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _images = new List<ImageModel>();
        }

        public bool IsImage(IFormFile img)
        {
            int ImageMinimumBytes = 512;

            if (img.ContentType.ToLower() != "image/jpg" &&
                    img.ContentType.ToLower() != "image/jpeg" &&
                    img.ContentType.ToLower() != "image/pjpeg" &&
                    img.ContentType.ToLower() != "image/gif" &&
                    img.ContentType.ToLower() != "image/x-png" &&
                    img.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            if (Path.GetExtension(img.FileName).ToLower() != ".jpg"
            && Path.GetExtension(img.FileName).ToLower() != ".png"
            && Path.GetExtension(img.FileName).ToLower() != ".gif"
            && Path.GetExtension(img.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            try
            {
                if (!img.OpenReadStream().CanRead)
                {
                    return false;
                }

                if (img.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                img.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            finally
            {
                img.OpenReadStream().Position = 0;
            }

            return true;
        }

        public async Task<ImageModel> UploadImage(UploadModel file)
        {
            ImageModel img = new ImageModel();
            if (file.MyImage != null && file.MyImage.Length > 0 && IsImage(file.MyImage))
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                uploadPath = Path.Combine(uploadPath, "Images\\");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var uniqFileName = Guid.NewGuid().ToString();
                var filename = Path.GetFileName(uniqFileName + "." + file.MyImage.FileName.Split(".")[1].ToLower());
                string fullPath = uploadPath + filename;

                var filePath = Path.Combine(uploadPath, filename);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    await file.MyImage.CopyToAsync(fileStream);


                img.Id = file.Id;
                img.Caption = file.Caption;
                img.Description = file.Description;
                img.Path = filePath;
                img.Score = 0;

                _images.Add(img);
                return img;
            }
            else
                return img;
        }

        public List<ImageModel> GetImages()
        {
            return _images;
        }

        public ImageModel GetImage(string id)
        {
            var img = _images.Where(x => x.Id == id).FirstOrDefault();
            return img;
        }

        public string DeleteImage(string id)
        {
            var img = _images.Where(x => x.Id == id).FirstOrDefault();
            int index = _images.IndexOf(img);
            File.Delete(_images[index].Path);
            _images.RemoveAt(index);
            return id;
        }

        public ImageModel UpdateImage(string id, ImageModel updatedImage)
        {
            var img = _images.Where(x => x.Id == id).FirstOrDefault();
            int index = _images.IndexOf(img);
            _images[index] = updatedImage;
            return updatedImage;
        }
    }
}
