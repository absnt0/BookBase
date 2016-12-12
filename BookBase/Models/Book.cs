using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookBase.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Display(Name = "Tytuł książki")]
        [Required(ErrorMessage = "Pole tytuł jest wymagane.")]
        [MinLength(2, ErrorMessage = "Wprowadzony tytuł jest za krótki.")]
        [MaxLength(255, ErrorMessage = "Wprowadzony tytuł jest za długi.")]
        public string Title { get; set; }

        [Display(Name = "Data wydania")]
        [Required(ErrorMessage = "Pole Data Wydania jest wymagane.")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessage = "Podana data nie jest prawidłowa.")]
        public DateTime DateOfRelease { get; set; }

        [Display(Name = "Numer ISBN")]
        [Required(ErrorMessage = "Pole Numer ISBN jest wymagane.")]
        public string ISBN { get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage = "Pole Autor jest wymagane.")]

        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }

        
        public override string ToString()
        {
            return string.Format("{0} - {1}", Title, AuthorId);
        }
    }
}