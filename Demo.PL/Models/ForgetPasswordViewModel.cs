using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
	public class ForgetPasswordViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Email is invalid")]
		public string Email { get; set; }
	}
}
