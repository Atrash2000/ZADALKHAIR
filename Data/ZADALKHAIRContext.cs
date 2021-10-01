using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZADALKHAIR.Models;

namespace ZADALKHAIR.Data
{
    public class ZADALKHAIRContext : DbContext
    {
        public ZADALKHAIRContext (DbContextOptions<ZADALKHAIRContext> options)
            : base(options)
        {
        }

        public DbSet<ZADALKHAIR.Models.Contact> Contact { get; set; }
    }
}
