using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZADALKHAIR.Models
{
    public class FeedBack
    {
        [Key]
        public int FeedBackID { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        [StringLength(maximumLength: 255)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Phone]
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(maximumLength: 255)]
        [Display(Name = "Counrty Code")]
        public String CounrtyCode { get; set; }

        [Required]
        [Display(Name = "Massege")]
        public String Massege { get; set; }
        public bool FeedBackSatuts { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime SatutsUpdate { get; set; }

        [Required]
        [Display(Name = "Rating")]
        public double Rate { get; set; }

        [Display(Name = "Sex")]
        public string Sex { get; set; }

    }
}
