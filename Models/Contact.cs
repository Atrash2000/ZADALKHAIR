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
        public String ContactEmail { get; set; }
        [Required]
        [StringLength(maximumLength: 255)]
        public String ContactName { get; set; }
        [Required]
        [StringLength(maximumLength: 10)]
        public String ContactPhoneNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 255)]
        public String ContactCounrty { get; set; }
        [Required]
        public String ContactSubject { get; set; }
        [Required]
        [StringLength(maximumLength: 765)]
        public String ContactMassege { get; set; }
        public bool ContactSatuts { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime SatutsUpdate { get; set; }
    }
}
