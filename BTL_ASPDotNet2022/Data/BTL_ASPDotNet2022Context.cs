using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BTL_ASPDotNet2022.Models;
using System.Runtime.ConstrainedExecution;

namespace BTL_ASPDotNet2022.Data
{
    public class BTL_ASPDotNet2022Context : DbContext
    {
        public BTL_ASPDotNet2022Context (DbContextOptions<BTL_ASPDotNet2022Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PK of Order_Details table ( Composite key) 
            modelBuilder.Entity<Order_Details>()
                .HasKey(c => new { c.OrderId, c.BookId });
        }

        public DbSet<BTL_ASPDotNet2022.Models.Account> Account { get; set; } = default!;

        public DbSet<BTL_ASPDotNet2022.Models.Book> Book { get; set; }

        public DbSet<BTL_ASPDotNet2022.Models.Order> Order { get; set; }
        
        public DbSet<BTL_ASPDotNet2022.Models.Order_Details> Order_Details { get; set; }
        public DbSet<BTL_ASPDotNet2022.Models.Order_Processing> Order_Processing { get; set; }

    }
}
