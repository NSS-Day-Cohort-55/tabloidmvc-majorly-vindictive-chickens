using System;
using System.Collections.Generic;
using System.Linq;

namespace TabloidMVC.Models.ViewModels
{
    public class PostDetailViewModel
    {
        public Post Post { get; set; }
        public List<int> PostTagIds { get; set; }
        public List<Tag> Tags { get; set; }
        public List<int> PostReactionIds { get; set; }
        public List<string> Reactions { get; set; }
        public List<Reaction> ReactionList { get; set; }

        public int ReactionCount(List<Reaction> Reactions, string url)
        {
            int x = Reactions
            .Count(r => r.ImageLocation == url);
            return x;
        }
    }
}
