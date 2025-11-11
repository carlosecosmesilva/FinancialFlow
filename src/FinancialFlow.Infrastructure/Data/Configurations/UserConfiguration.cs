using FinancialFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialFlow.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            // Email como Value Object (owned entity)
            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Address)
                     .HasColumnName("email")
                     .HasMaxLength(320)
                     .IsRequired();

                // Índice único no email (no mesmo table do owner)
                email.HasIndex(e => e.Address)
                     .IsUnique()
                     .HasDatabaseName("ix_users_email");
            });

            builder.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
        }
    }
}