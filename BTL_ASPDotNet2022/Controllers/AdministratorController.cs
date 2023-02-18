using BTL_ASPDotNet2022.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BTL_ASPDotNet2022.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly BTL_ASPDotNet2022Context _context;

        public AdministratorController(IHttpContextAccessor contextAccessor, BTL_ASPDotNet2022Context context)
        {
            _contextAccessor = contextAccessor;
            _context = context;
        }

        public IActionResult Index()
        {
            // Authorize ( not login or is custormer => redirect to NotFound() ) 
            var UsernameLogin = HttpContext.Session.GetString("UsernameLogin");
            if (string.IsNullOrEmpty(UsernameLogin)) return NotFound();
            else if (UsernameLogin.Equals("customer")) return NotFound();



            // Calculate number of order and value of this ( follow day and month) 
            DateTime currentDate = DateTime.Now;

            ViewBag.DailyOrder = _context.Order.Where(
                m =>
                    m.OrderDate.Value.Date == currentDate.Date
            ).Count();

            ViewBag.MonthlyOrder = _context.Order.Where(
                m =>
                    m.OrderDate.Value.Month == currentDate.Month &&
                    m.OrderDate.Value.Year == currentDate.Year
                ).Count();

            return View();
        }   


    }
}
