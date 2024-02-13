using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Email is invalid")]
		public string Email { get; set; }	
		
		[Required(ErrorMessage = "UserName is required")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "First Name is required")]
		public string FName { get; set; }	

		[Required(ErrorMessage = "Last Name is required")]
		public string LName { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[MinLength(5, ErrorMessage = "Minimum password length is 5")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare(nameof(Password), ErrorMessage = "Confirm Password Doesn't match Password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; }
    }
}
