using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {

        void Add(Comment comment);
        List<Comment> GetCommentByPostId(int id);
        void Delete(Comment comment);

        public Comment GetCommentById(int id);
    }
}
