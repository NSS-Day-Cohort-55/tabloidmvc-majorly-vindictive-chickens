using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostsByCategoryViewModel
    {
        public List<Post> Posts { get; set; }

        public List<Category> Categories { get; set; }
        public int SelectedCategoryId { get; set; }
    }
}
