using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.User
{
    public class ForgotPasswordRequestViewModel
    {
        [Required(ErrorMessage = "Se require el Nombre de usuario")]
        [DataType(DataType.Text)]
        public required string UserName { get; set; }
    }
}
