using System.Collections.Generic;
using System;

namespace TabloidMVC.Models.ViewModels
{
    public class PostReactionViewModel
    {
        public Post Post { get; set; }
        public List<Reaction> ReactionOptions { get; set; }
        public List<int> ReactionIds { get; set; }
        public List<Reaction> PostReactions { get; set; }
        public int UserId { get; set; }
    }
}