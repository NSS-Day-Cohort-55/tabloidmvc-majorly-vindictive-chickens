using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostsByUserViewModel
    {
        public List<Post> Posts { get; set; }
        public List<UserProfile> UserProfiles { get; set; }
        public int SelectedProfileId { get; set; }
    }
}
