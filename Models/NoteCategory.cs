using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTR02.Models
{
    public class NoteCategory
    {   [Key]
        public int NoteCategoryID { get; set; }
        [ForeignKey("Category")]
        public int CategoryID { get; set;}
        [ForeignKey("Note")]
        public int NoteID {get; set;}
    
    }
}
