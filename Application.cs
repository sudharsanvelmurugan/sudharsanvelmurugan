using Microsoft.EntityFrameworkCore;
using sample_task.Models;

namespace sample_task.Data
{
    public class application:DbContext
    {
        public application(DbContextOptions<application> obj) : base(obj) 
        { 

        }
        public DbSet<company>company { get; set; }
        public DbSet<companyuser> companyuser { get; set; }
       public DbSet<orderentry> orderheader { get; set; }

        public DbSet<tradingparty> tradingparty_master { get; set; }
        public DbSet<orderinvoice> orderinvoice { get; set; }
        public DbSet<itemmaster> itemmaster { get; set; }
        public DbSet<itemsection> itemsections { get; set; }
        
    }
}
