﻿using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostTagViewModel
    {
        public Post Post { get; set; }
        public List<Tag> TagOptions { get; set; }
        public List<int> TagIds { get; set; }
        public List<Tag> PostTags { get; set; }
    }
}