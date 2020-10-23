using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Http;

namespace Backend.Services
{
    public interface IImageService
    {
        public bool IsImage(IFormFile img);
        public List<ImageModel> GetImages();
        public ImageModel GetImage(string Id);
        public Task<ImageModel> UploadImage(UploadModel file);
        public ImageModel UpdateImage(string Id, ImageModel description);
        public string DeleteImage(string Id);
    }
}
