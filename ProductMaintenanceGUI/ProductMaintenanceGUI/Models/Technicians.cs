using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProductMaintenanceGUI.Models
{
    public partial class Technicians
    {
        public Technicians()
        {
            Incidents = new HashSet<Incidents>();
        }

        [Key]
        [Column("TechID")]
        public int TechId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [InverseProperty("Tech")]
        public virtual ICollection<Incidents> Incidents { get; set; }
    }
}
