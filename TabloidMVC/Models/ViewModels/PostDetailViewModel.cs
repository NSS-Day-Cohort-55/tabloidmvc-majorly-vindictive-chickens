using System;
using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostDetailViewModel
    {
        public Post Post { get; set; }
        public List<int> PostTagIds { get; set; }
        public List<Tag> Tags { get; set; }
        public List<int> PostReactionIds { get; set; }
        public List<Reaction> Reactions { get; set; }
        public Subscription Subscription { get; set; }

    }
}
