using Microsoft.AspNetCore.Mvc.Rendering;

namespace BTL_ASPDotNet2022.Models
{
    public class OrderFilter
    {
        public List<Order>? OrderList { get; set; }
        public SelectList? StatusList { get; set; }
        public string? StatusFilter { get; set; }
        public string? SearchString { get; set; }
    }
}
