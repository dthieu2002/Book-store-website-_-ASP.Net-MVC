using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_ASPDotNet2022.Models
{
    public class Book
    {
        [Key]
        [Display(Name="Id:")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name:")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Genre:")]
        [StringLength(100)]
        public string? Gender { get; set; }

        [Required]
        [Display(Name = "Page number:")]
        [Range(0, int.MaxValue)]
        public int Page { get; set; }

        [Required]
        [Display(Name = "Author:")]
        [StringLength(100)]
        public string? Author { get; set; }

        [Required]
        [Display(Name = "Price:")]
        [DataType(DataType.Currency)]
        [Range(minimum:0, maximum: 100000)]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Release date:")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Display(Name = "Inventory quantity:")]
        [Range(0, int.MaxValue)]
        public int InventoryQuantity { get; set; }

        [Required]
        [Display(Name = "Book image:")]
        [StringLength(400)]
        public string? ImageSrc { get; set; }


    }
}
