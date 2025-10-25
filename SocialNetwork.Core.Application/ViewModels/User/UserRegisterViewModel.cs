using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.User
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [DataType(DataType.Text)]
        public required string FirstName { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        [DataType(DataType.Text)]
        public required string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        [DataType(DataType.Text)]
        public required string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]   
        public required string PasswordHash { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }
        [DataType(DataType.Upload)]
        public IFormFile? Profile { get; set; }
    }
}
