﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backlog.Models
{
    public class List
    {
        [Key]
        public int ListId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public ApplicationUser User{ get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
