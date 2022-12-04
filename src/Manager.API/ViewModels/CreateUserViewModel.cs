using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Manager.API.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MinLength(3, ErrorMessage = "Name must be greater than 3 characters")]
        [MaxLength(80, ErrorMessage = "Name must be less than 80 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [MinLength(10, ErrorMessage = "Email must be greater than 10 characters")]
        [MaxLength(80, ErrorMessage = "Email must be less than 80 characters")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email is invalid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be greater than 6 characters")]
        [MaxLength(80, ErrorMessage = "Password must be less than 80 characters")]
        public string Password { get; set; }
    }
}