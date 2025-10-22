using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.DTOs.Email
{
    public class EmailRequestDto
    {
        public string? To { get; set; }
        public List<string>? ToRange { get; set; } = new List<string>();
        public required string Subject { get; set; }
        public required string? BodyHtml { get; set; }
    }
}
