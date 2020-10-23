using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class CommentModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public int Score { get; set; }

        public string ImageId { get; set; }
    }
}
