using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_ASPDotNet2022.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        // FK 
        [Range(0, 100)]
        public virtual string? Username { get; set; }

        [ForeignKey("Username")]
        public virtual Account? Account { get; set; }

        public DateTime? OrderDate { get; set; }

        [Range(0, 20)]
        public string? OrderStatus { get; set; }
    }
}
