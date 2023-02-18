using Microsoft.AspNetCore.Mvc.Rendering;

namespace BTL_ASPDotNet2022.Models
{
    public class AccountFilter
    {
        public List<Account>? Accounts { get; set; }
        public SelectList? Roles { get; set; }
        public string? RoleFilter { get; set; }
        public string? SearchString { get; set; }
    }
}
