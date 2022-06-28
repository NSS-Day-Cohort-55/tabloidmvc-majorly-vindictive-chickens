using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {

        void Add(Comment comment);
        List<Comment> GetCommentByPostId(int id);
    }
}
