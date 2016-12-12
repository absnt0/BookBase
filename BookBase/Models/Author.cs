using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookBase.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage = "Pole Autor jest wymagane.")]
        [MinLength(2, ErrorMessage = "Pole Autor jest za krótkie.")]
        [MaxLength(50, ErrorMessage = "Pole Autor jest za długie. (Max = 50)")]
        [Remote("DoesAuthorExist", "Author", ErrorMessage = "Autor już istnieje.")]
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public override string ToString()
        {
            return Name.ToString();
        }

        public bool HasBooks()
        {
            return (Books.Count > 0);
        }
    }
}