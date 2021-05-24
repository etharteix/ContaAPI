using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ContaAPI.Domain.Entities;
using System;

namespace ContaAPI.Infra.Data.Mapping
{
    public class HistoricMap : IEntityTypeConfiguration<HistoricEntity>
    {
        public void Configure(EntityTypeBuilder<HistoricEntity> builder)
        {
            builder.ToTable("Historic");

            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.MovementType)
                .HasConversion(prop => prop.ToString(), prop => prop)
                .IsRequired()
                .HasColumnName("MovementType")
                .HasColumnType("varchar(8)");

            builder.Property(prop => prop.MovementDate)
                .IsRequired()
                .HasColumnName("MovementDate")
                .HasColumnType("datetime");

            builder.Property(prop => prop.MovementValue)
                .IsRequired()
                .HasColumnName("MovementValue")
                .HasColumnType("decimal(20,2)");

            builder.Property(prop => prop.UserId)
                .IsRequired()
                .HasColumnName("UserId");
        }
    }
}
