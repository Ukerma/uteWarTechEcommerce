using System.ComponentModel.DataAnnotations;
namespace OnlineShop.Models.ViewModels
{
    public class RecoveryPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

    }
}
