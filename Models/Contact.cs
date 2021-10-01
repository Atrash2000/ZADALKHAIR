using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZADALKHAIR.Models
{
    public class Contact
    {
        [Required]
        [Key]
        public int ContactID { get; set; }
        [EmailAddress]
        [Required]
        [Display(Name = "Email")]
        public String ContactEmail { get; set; }
        [Required]
        [StringLength(maximumLength: 255)]
        [Display(Name = "Name")]
        public String ContactName { get; set; }
        [Required]
        [StringLength(maximumLength: 14)]
        [Display(Name = "Phone Number")]
        public String ContactPhoneNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 255)]
        [Display(Name = "Counrty Code")]
        public String ContactCounrty { get; set; }
        [Required]
        [Display(Name = "Subject")]
        public String ContactSubject { get; set; }
        [Required]
        [StringLength(maximumLength: 765)]
        [Display(Name = "Massege")]
        public String ContactMassege { get; set; }
        public bool ContactSatuts { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime SatutsUpdate { get; set; }
    }
}
