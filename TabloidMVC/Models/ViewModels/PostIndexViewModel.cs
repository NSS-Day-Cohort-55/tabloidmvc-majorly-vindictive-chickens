using System;
using System.Collections.Generic;
using System.Linq;

namespace TabloidMVC.Models.ViewModels
{
    public class PostIndexViewModel
    {
        public List<Post> Posts { get; set; }
        public string ReadTime(Post post)
        {
            int readTime = post.Content.Split().Count();
            int readTimeMin = 0;
            if (readTime / 265 >= 0)
            {
                readTimeMin = readTime / 265;
            }
            else
            {
                readTimeMin = 1;
            }

            if(readTime % 265 > 0)
            {
                readTimeMin++;
            };

            if(readTimeMin <= 1)
            {
                return $"{readTimeMin} min";
            }
            else
            {
                return $"{readTimeMin} mins";
            }
        }
    }
}
