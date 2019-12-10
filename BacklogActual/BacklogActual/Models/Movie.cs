using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace BacklogActual.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PublishedDate { get; set; }
        public ApplicationUser UserId { get; set; }


    }
}