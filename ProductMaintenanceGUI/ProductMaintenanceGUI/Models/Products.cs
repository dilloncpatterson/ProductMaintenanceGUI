using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProductMaintenanceGUI.Models
{
    public partial class Products
    {
        public Products()
        {
            Incidents = new HashSet<Incidents>();
            Registrations = new HashSet<Registrations>();
        }

        [Key]
        [StringLength(10)]
        public string ProductCode { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18, 1)")]
        public decimal Version { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ReleaseDate { get; set; }

        [InverseProperty("ProductCodeNavigation")]
        public virtual ICollection<Incidents> Incidents { get; set; }
        [InverseProperty("ProductCodeNavigation")]
        public virtual ICollection<Registrations> Registrations { get; set; }
    }
}
