
using Microsoft.EntityFrameworkCore;
using SmartRefund.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace SmartRefund.Infra.Context
{
    [ExcludeFromCodeCoverage]
    public class AppDbContext : DbContext
    {
        public DbSet<TranslatedVisionReceipt> TranslatedVisionReceipt { get; set; }
        public DbSet<RawVisionReceipt> RawVisionReceipt { get; set; }
        public DbSet<InternalReceipt> InternalReceipt { get; set; }
        public DbSet<ReceiptEventSource> ReceiptEventSource { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
