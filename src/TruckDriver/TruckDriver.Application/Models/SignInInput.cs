using System.ComponentModel.DataAnnotations;

namespace TruckDriver.Application.Models
{
    public class SignInInput
    {
        [Required]
        public string Key { get; set; }
    }
}
