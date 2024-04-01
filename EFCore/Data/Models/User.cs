using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace northwindAPI.EFCore.Data.Models
{
    public class User:IdentityUser
    {
        [Required]
        public string? FirstName {get; set;}
         [Required]
        public string? LastName {get; set;}
         [Required]
        public string? Password {get; set;}
    }
}