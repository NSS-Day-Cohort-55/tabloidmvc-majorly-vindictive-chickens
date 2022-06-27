using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TabloidMVC.Models
{
    public class UserType
    {
        public int Id { get; set; }

        [DisplayName("UserType")]
        public string Name { get; set; }
    }
}