using Microsoft.EntityFrameworkCore;
using BTL_ASPDotNet2022.Data;
using BTL_ASPDotNet2022.Models;

namespace RazorPagesMovie.Models
{
    public static class AccountSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BTL_ASPDotNet2022Context(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BTL_ASPDotNet2022Context>>()))
            {
                if (context == null || context.Account == null)
                {
                    throw new ArgumentNullException("Null RazorPagesMovieContext");
                }

                // Look for any movies.
                if (context.Account.Any())
                {
                    return;   // DB has been seeded
                }

                context.Account.AddRange(
                    new Account
                    {
                        Username = "ThuanHieu",
                        Password = "2002",
                        FullName = "Đàm Thuận Hiếu",
                        BirthDay = DateTime.Parse("2002-5-10"),
                        Gender = "Female",
                        Address = "Mai Động - Hương Mạc",
                        Phone = "0964837511",
                        Role = "master",
                        RegisterDate = DateTime.Now

                    },
                    new Account
                    {
                        Username = "StaffA",
                        Password = "12345",
                        FullName = "Nghiêm Văn Hào",
                        BirthDay = DateTime.Parse("2004-11-20"),
                        Gender = "Female",
                        Address = "Khu phố Mai Động",
                        Phone = "020042004",
                        Role = "staff",
                        RegisterDate = DateTime.Now
                    },
                    new Account
                    {
                        Username = "ThuongHuyen",
                        Password = "2005",
                        FullName = "Danh Ca Thương Huyền",
                        BirthDay = DateTime.Parse("2005-12-15"),
                        Gender = "Male",
                        Address = "Khu phố Đồng Hương",
                        Phone = "09856283249",
                        Role = "customer",
                        RegisterDate = DateTime.Now
                    }
                );
                context.SaveChanges();
            }
        }
    }
}