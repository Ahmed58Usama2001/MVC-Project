using Microsoft.AspNetCore.Identity;

namespace Demo.DAL.Models
{
	public class ApplicationUser:IdentityUser
	{
        public bool IsAgree { get; set; }

        public string FName { get; set; }
        public string LName { get; set; }
    }
}
