using System;
using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostDetailViewModel
    {
        public Post Post { get; set; }
        public List<int> PostTagIds { get; set; }
        public List<Tag> Tags { get; set; }

        public PostImage PostImage { get; set; }
        
    }
}
