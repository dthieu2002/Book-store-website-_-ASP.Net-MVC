using Microsoft.EntityFrameworkCore;
using BTL_ASPDotNet2022.Data;
using BTL_ASPDotNet2022.Models;

namespace RazorPagesMovie.Models
{
    public static class BookSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BTL_ASPDotNet2022Context(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BTL_ASPDotNet2022Context>>()))
            {
                if (context == null || context.Book == null)
                {
                    throw new ArgumentNullException("Null RazorPagesMovieContext");
                }

                // Look for any movies.
                if (context.Book.Any())
                {
                    return;   // DB has been seeded
                }

                context.Book.AddRange(
                    new Book
                    {
                        Name = "An nam truyện",
                        Gender = "History",
                        Page= 332,
                        Author = "Châu Hải Đường",
                        Price = 10,
                        ReleaseDate = DateTime.Parse("2018-1-1"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/an_nam_truyen.jpg"
                    },
                    new Book
                    {
                        Name = "Anna karenina",
                        Gender = "Novel",
                        Page = 964,
                        Author = "Leo Tolstoy",
                        Price = 25,
                        ReleaseDate = DateTime.Parse("1878-1-1"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/anna_karenina.jpg"
                    },
                    new Book
                    {
                        Name = "Bão táp triều Trần",
                        Gender = "History",
                        Page = 445,
                        Author = "Hoàng Quốc Hải",
                        Price = 12,
                        ReleaseDate = DateTime.Parse("2010-9-17"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/bao_tap_trieu_tran.jpg"
                    },
                    new Book
                    {
                        Name = "Cảm xúc là kẻ thù số một của thành công",
                        Gender = "Self helping",
                        Page = 264,
                        Author = "TS. Lê Thẩm Dương",
                        Price = 6,
                        ReleaseDate = DateTime.Parse("2016-1-1"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/CamXucLaKeThuSo1CuaThanhCong.jpg"
                    },
                    new Book
                    {
                        Name = "Đắc Nhân Tâm",
                        Gender = "Self helping",
                        Page = 432,
                        Author = "Dale Carnegie",
                        Price = 100,
                        ReleaseDate = DateTime.Parse("1936-1-1"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/dac_nhan_tam.jpg"
                    },
                    new Book
                    {
                        Name = "Lịch sử nội chiến Việt Nam từ 1771 - 1802",
                        Gender = "History",
                        Page = 448,
                        Author = "Tạ Chí Đại Trường",
                        Price = 20,
                        ReleaseDate = DateTime.Parse("2012-10-31"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/lich_su_noi_chien_vn_1771-1802.jpg"
                    },
                    new Book
                    {
                        Name = "Middlemarch",
                        Gender = "Novel",
                        Page = 904,
                        Author = "George Eliot, Michal Faber (Introduction)",
                        Price = 50,
                        ReleaseDate = DateTime.Parse("1872-1-1"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/middlemarch.jpg"
                    },
                    new Book
                    {
                        Name = "Sử việt 12 khúc tráng ca",
                        Gender = "History",
                        Page = 272,
                        Author = "Dũng Phan",
                        Price = 11,
                        ReleaseDate = DateTime.Parse("2017-5-25"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/su_viet_12_khuc_trang_ca.jpg"
                    },
                    new Book
                    {
                        Name = "Thủ lĩnh bộ lạc",
                        Gender = "Self helping",
                        Page = 482,
                        Author = "Dave Logan, John King and Halee Fischer-Wright",
                        Price = 8,
                        ReleaseDate = DateTime.Parse("2019-12-1"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/thu_linh_bo_lac.jpg"
                    },
                    new Book
                    {
                        Name = "Tuổi trẻ đáng giá bao nhiêu?",
                        Gender = "Self helping",
                        Page = 292,
                        Author = "Rosie Nguyễn",
                        Price = 33,
                        ReleaseDate = DateTime.Parse("2016-10-7"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/TuoiTreDangGiaBaoNhieu.jpg"
                    },
                    new Book
                    {
                        Name = "Việt nam sử lược",
                        Gender = "History",
                        Page = 647,
                        Author = "Trần Trọng Kim",
                        Price = 52,
                        ReleaseDate = DateTime.Parse("1919-1-1"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/viet_nam_su_luoc.jpg"
                    },
                    new Book
                    {
                        Name = "Vua Gia Long và Người Pháp",
                        Gender = "History",
                        Page = 674,
                        Author = "Thụy Khuê",
                        Price = 70,
                        ReleaseDate = DateTime.Parse("2017-3-1"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/vua_gia_long_va_nguoi_phap.jpg"
                    },
                    new Book
                    {
                        Name = "Why we want you to be rich?",
                        Gender = "Self helping",
                        Page = 345,
                        Author = "Donald J. Trump",
                        Price = 18,
                        ReleaseDate = DateTime.Parse("2006-10-30"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/WhyWeWantYouToBeRich.jpg"
                    },

                    // Computer science book 
                    new Book
                    {
                        Name = "The Pragmatic Programmer",
                        Gender = "Programming",
                        Page = 320,
                        Author = "Andrew Hunt - David Thomas",
                        Price = 38.92M,
                        ReleaseDate = DateTime.Parse("1999-10-1"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/the_pragmatic_programmer.jpg"
                    },
                    new Book
                    {
                        Name = "The Clean Coder: A Code of Conduct for Professional Programmers",
                        Gender = "Programming",
                        Page = 256,
                        Author = "Robert Martin",
                        Price = 44.99M,
                        ReleaseDate = DateTime.Parse("2011-5-13"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/the_clean_coder.jpg"
                    },
                    new Book
                    {
                        Name = "Code complete",
                        Gender = "Programming",
                        Page = 857,
                        Author = "Steve McConnell",
                        Price = 53.99M,
                        ReleaseDate = DateTime.Parse("1993-1-1"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/code_complete.jpg"
                    },
                    new Book
                    {
                        Name = "The Mythical Man-Month: Essays on Software Engineering, Anniversary Edition",
                        Gender = "Programming",
                        Page = 336,
                        Author = "Frederick Brooks Jr",
                        Price = 38.69M,
                        ReleaseDate = DateTime.Parse("1995-8-2"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/the_mythical_man_month.jpg"
                    },
                    new Book
                    {
                        Name = "Advanced Linux Programming",
                        Gender = "Programming",
                        Page = 340,
                        Author = "CodeSourcery LLC - Mark Mitchell - Alex Samuel",
                        Price = 8.30M,
                        ReleaseDate = DateTime.Parse("2001-6-11"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/advanced_linux_programming.jpg"
                    }
                    ,
                    new Book
                    {
                        Name = "The C# Programming Yellow Book",
                        Gender = "Programming",
                        Page = 224,
                        Author = "Rob Miles",
                        Price = 12M,
                        ReleaseDate = DateTime.Parse("2014-1-6"),
                        InventoryQuantity = 1000,
                        ImageSrc = "/public/image/book/the_csharp_programming_yellow_book.jpg"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}