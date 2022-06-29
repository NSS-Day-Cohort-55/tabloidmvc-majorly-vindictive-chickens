using System;
using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostsByTagViewModel
    {
        public List<Post> Posts { get; set; }

        public List<Tag> Tags { get; set; }
        public int SelectedTagId { get; set; }
    }
}
