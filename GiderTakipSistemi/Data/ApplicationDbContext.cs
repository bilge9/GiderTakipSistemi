using Microsoft.EntityFrameworkCore;
using GiderTakipSistemi.Models;

namespace GiderTakipSistemi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    }
}
