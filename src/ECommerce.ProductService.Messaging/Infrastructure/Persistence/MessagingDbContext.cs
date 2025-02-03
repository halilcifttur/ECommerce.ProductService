using ECommerce.ProductService.Messaging.Domain.Entities;
using ECommerce.ProductService.Messaging.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ProductService.Messaging.Infrastructure.Persistence;

public class MessagingDbContext : DbContext
{
    public MessagingDbContext(DbContextOptions<MessagingDbContext> options) : base(options) { }

    public DbSet<MessageInbox> MessageInboxes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MessageInboxConfiguration());
    }
}