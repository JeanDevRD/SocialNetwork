using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.User
{
    public class EditProfileViewModel
    {
        public required string Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [DataType(DataType.Text)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        [DataType(DataType.PhoneNumber)]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [DataType(DataType.Text)]
        public required string UserName { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? Profile { get; set; }
        [Required(ErrorMessage = "Se requiere la contraseña para editar el perfil")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Required(ErrorMessage = "Se requiere confirmar la contraseña")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }
    }
}
