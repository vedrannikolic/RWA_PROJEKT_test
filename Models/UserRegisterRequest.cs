using System;
using System.ComponentModel.DataAnnotations;

namespace Integration_modul.Models
{
	public class UserRegisterRequest
	{
		[Required(ErrorMessage = "Username is required.")]
		[MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "First name is required.")]
		[MinLength(2, ErrorMessage = "First name must be at least 2 characters.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last name is required.")]
		[MinLength(2, ErrorMessage = "Last name must be at least 2 characters.")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm password is required.")]
		[Compare("Password", ErrorMessage = "Passwords do not match.")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email address format.")]
		public string Email { get; set; }

		[Phone(ErrorMessage = "Invalid phone number format.")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Country of residence is required.")]
		public int CountryOfResidenceId { get; set; }
	
    }
}

