using ECommerce.ProductService.Messaging.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ProductService.Messaging.Infrastructure.Persistence.Configurations;

public class MessageInboxConfiguration : IEntityTypeConfiguration<MessageInbox>
{
    public void Configure(EntityTypeBuilder<MessageInbox> builder)
    {
        builder.HasKey(p => p.Id);
    }
}