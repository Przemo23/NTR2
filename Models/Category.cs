using System;
using System.ComponentModel.DataAnnotations;

namespace NTR02.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [StringLength(64)]
        public string Name { get; set; }
    }
}
