
using Microsoft.EntityFrameworkCore;
using SmartRefund.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.Infra.Context
{
    [ExcludeFromCodeCoverage]
    public class AppDbContext : DbContext
    {
        public DbSet<Teste> Teste { get; set; }
        public DbSet<TranslatedVisionReceipt> TranslatedVisionReceipt { get; set; }
        public DbSet<RawVisionReceipt> RawVisionReceipt { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
