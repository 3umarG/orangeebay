using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orange_Bay.DTOs.Auth
{
    public class UpdateProfileDto
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required] public string FullName { get; set; }

        public string? Phone { get; set; }
        [Required] public int UserId { get; set; }

    }
}
