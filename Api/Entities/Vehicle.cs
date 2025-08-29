using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalAPI.Entities
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = default!;

        [Required]
        [StringLength(255)]
        public string Model { get; set; } = default!;

        [Required]
        [StringLength(50)]
        public string Make { get; set; } = default!;

        [Required]
        public int Year { get; set; } = default!;
    }
}