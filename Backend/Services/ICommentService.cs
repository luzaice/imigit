using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public interface ICommentService
    {
        public List<CommentModel> GetComments(string imageId);
        public CommentModel GetComment(string ImageId, string Id);
        public CommentModel AddComment(string ImageId, CommentModel NewComment);
        public CommentModel UpdateComment(string ImageId, string Id, CommentModel updatedComment);
        public string DeleteComment(string ImageId, string Id);
    }
}
