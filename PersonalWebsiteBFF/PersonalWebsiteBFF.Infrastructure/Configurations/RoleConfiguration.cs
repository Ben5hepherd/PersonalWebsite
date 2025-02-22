using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalWebsiteBFF.Domain.Entities;

namespace PersonalWebsiteBFF.Infrastructure.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Name).HasMaxLength(50).IsRequired();

            builder.HasData(
                new Role { Id = Role.MemberId, Name = Role.Member },
                new Role { Id = Role.AdminId, Name = Role.Admin }
            );
        }
    }
}
