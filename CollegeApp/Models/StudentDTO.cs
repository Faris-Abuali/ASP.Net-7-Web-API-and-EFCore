using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using CollegeApp.Validators;

namespace CollegeApp.Models
{
    public class StudentDTO
    {
        [ValidateNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        [StringLength(30)]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        //[Remote(action: "VerifyEmail", controller: "StudentController")]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        //[Range(10, 100)]
        //public int Age { get; set; }

        //public string Password { get; set; }

        //[Compare(nameof(Password))]
        //public string ConfirmPassword { get; set; }

        //[DateCheck]
        //public DateTime AdmissionDate { get; set; }
    }
}
