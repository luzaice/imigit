using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class ImageModel
    {
        public string Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }
        public string Path { get; set; }
    }


    public class UploadModel
    {
        public string Id { get; set; }
        public IFormFile MyImage { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
    }
}
