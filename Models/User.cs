using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZADALKHAIR.Models
{
    public class User
    {
       
        [Key]
        public int UserID { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Display(Name = "First Name")]
        [StringLength(maximumLength: 255)]
        [Required]
        public string UserFirstName { get; set; }
        
        [Display(Name = "Last Name")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string UserLastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public string UserPhoneNumber { get; set; }

        [Required]
        [Display(Name = "Country Code")]
        public string USerCountryCode { get; set; }

        [Required]
        [Display(Name = "Role Type")]
        public string UserRoleType { get; set; }

        [Display(Name = "User Crated Date")]
        public DateTime UserCreateAt { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(150)]
        public string UserPassword { get; set; }

        [StringLength(maximumLength: 3000)]
        public string UserProfilePic { get; set; }

        [Display(Name = "Profile Picture")]
        [NotMapped]
        public IFormFile ProfilePic { get; set; }

    }
    public class Login
    {
        public static int ID { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
