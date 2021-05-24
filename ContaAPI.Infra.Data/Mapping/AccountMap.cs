using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ContaAPI.Domain.Entities;
using System;

namespace ContaAPI.Infra.Data.Mapping
{
    public class AccountMap : IEntityTypeConfiguration<AccountEntity>
    {
        public void Configure(EntityTypeBuilder<AccountEntity> builder)
        {
            builder.ToTable("Account");

            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Balance)
                .HasConversion(prop => prop.ToDecimal(), prop => prop)
                .IsRequired()
                .HasColumnName("Balance")
                .HasColumnType("decimal(20,2)");

            builder.HasIndex(prop => prop.UserId)
                .IsUnique();

            builder.Property(prop => prop.UserId)
                .IsRequired()
                .HasColumnName("UserId");
        }
    }
}
