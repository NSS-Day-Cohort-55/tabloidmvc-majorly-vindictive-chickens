using TabloidMVC.Models;
using Microsoft.AspNetCore.Http;

namespace TabloidMVC.Models.ViewModels
{
    public class PostImageViewModel
    {
        public PostImage PostImage { get; set; }
        public IFormFile File { get; set; }
    }
}
