using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_ASPDotNet2022.Models
{
    public class Order_Details
    {
        [Key]
        public virtual int? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        public virtual int? BookId  { get; set; }
        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }

        public int? Amount { get; set; }
    }
}
