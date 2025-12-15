using CBWC.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CBWC.Infrastructure.Persistence.Configs;

public sealed class MemberConfiguration
    : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.FirstName)
               .HasMaxLength(100);

        builder.Property(x => x.LastName)
               .HasMaxLength(100);

        builder.Property(x => x.MembershipNumber)
               .HasMaxLength(50);

        builder.Property(x => x.CNIC)
               .HasMaxLength(20);

        builder.Property(x => x.RecordStatus)
               .IsRequired();

        builder.Property(x => x.Created)
               .IsRequired();
    }
}