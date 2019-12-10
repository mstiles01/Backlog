using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BacklogActual.Models
{
    public class List
    {
        [Key]
        public int ListId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ApplicationUser UserId { get; set; }
    }
}