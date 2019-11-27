using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace NTR02.Models
{
    public class Note
    {
        [Key]
        public int NoteID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime NoteDate { get; set; }
        [Required]
        [StringLength(64)]
        public string Title { get; set; }
        [StringLength(int.MaxValue)]
        public string Description { get; set; }
        [NotMapped]
        public List<Category> Categories {get; set;}
    }
}
