using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_ASPDotNet2022.Models
{
    public class Order_Processing
    {
        [Key]
        public virtual int? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }


        public DateTime? ProcessingDate { get; set; }

        public virtual string? Username { get; set; }
        [ForeignKey("Username")]
        public virtual Account? Account { get; set; }
    }
}
