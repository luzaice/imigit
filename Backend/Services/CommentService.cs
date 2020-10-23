using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Services
{
    public class CommentService : ICommentService
    {
        private List<CommentModel> _comments;

        public CommentService()
        {
            _comments = new List<CommentModel>();
        }

        public List<CommentModel> GetComments(string imageId)
        {
            return _comments.Where(x=>x.ImageId==imageId).ToList();
        }

        public CommentModel GetComment(string imageId, string id)
        {
            var com = _comments.Where(x => x.ImageId == imageId && x.Id == id).SingleOrDefault();
            return com;
        }
        public CommentModel AddComment(string imageId, CommentModel com)
        {
            com.ImageId = imageId;
            com.Score = 0;
            _comments.Add(com);
            return com;
        }
        public string DeleteComment(string imageId, string id)
        {
            var com = _comments.Where(x => x.ImageId == imageId && x.Id == id).SingleOrDefault();
            _comments.Remove(com);

            return id;
        }

        public CommentModel UpdateComment(string imageId, string id, CommentModel updatedComment)
        {
            var com = _comments.Where(x => x.ImageId == imageId && x.Id == id).FirstOrDefault();
            int index = _comments.IndexOf(com);

            _comments[index].Text = updatedComment.Text;
            return _comments[index];
        }
    }
}
