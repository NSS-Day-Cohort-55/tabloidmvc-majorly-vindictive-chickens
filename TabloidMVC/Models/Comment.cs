using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;




namespace TabloidMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public int UserProfileId { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedDateTime { get; set; }


    }
}
