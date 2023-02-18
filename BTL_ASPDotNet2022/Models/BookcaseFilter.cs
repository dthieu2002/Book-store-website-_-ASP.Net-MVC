using Microsoft.AspNetCore.Mvc.Rendering;

namespace BTL_ASPDotNet2022.Models
{
    public class BookcaseFilter
    {
        public List<Book>? BookList { get; set; }
        public SelectList? BookTitleList { get; set; }

        public List<string>? BookTitleRenderList { get; set; }
        public string? BookTitleFilter { get; set; }

    }
}
