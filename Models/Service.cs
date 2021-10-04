using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Service Image")]
        public string ServiceImage { get; set; }

        [Display(Name = "Approved")]
        public bool ServiceApproved { get; set; } = false;

        public DateTime CreatedAt { get; set; }
    }
}
