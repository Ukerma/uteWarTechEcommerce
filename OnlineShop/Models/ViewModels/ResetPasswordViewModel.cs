using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Recovery Code")]
        public int? RecoveryCode { get; set; }

        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
    }
}
