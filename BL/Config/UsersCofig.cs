using BL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BL.Config
{
    public class UsersCofig : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {

            builder.Property(u => u.UserName)
                 .HasMaxLength(15)
                 .IsRequired();

            builder.Property(u => u.Password)
                .IsRequired();


            builder.HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId);
        }
    }
}
