using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        void Update(Post post);
        void Delete(Post post);
        List<Post> GetAllPublishedPosts();
        List<Post> GetAllPostsByUser(int id);
        Post GetPublishedPostById(int id);
        Post GetPostByPostId(int id);
        Post GetUserPostById(int id, int userProfileId);
        void InsertTag(int postId, int tagId);
        void DeleteTag(int postId, int tagId);
    }
}