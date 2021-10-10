using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZADALKHAIR.Models
{
    public class Service
    {
        [Key]
        [Display(Name = "No.")]
        public int ServiceID { get; set; }

        [Required]
        [StringLength(maximumLength: 255)]
        [Display(Name = "Title")]
        public string ServiceTitle { get; set; }

        [Required]
        [Display(Name = "Discription")]
        public string ServiceDiscription { get; set; }

        [Required]
        [Display(Name = "Starting Price")]
        public double ServiceStartingPrice { get; set; }

        [Required]
        [Display(Name = "Features")]
        public string features { get; set; }
        public string ServiceImage { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Service Picture")]
        [NotMapped]
        public IFormFile ServicePic { get; set; }
    }
}
