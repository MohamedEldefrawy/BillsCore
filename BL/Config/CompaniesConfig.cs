using BL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BL.Config
{
    public class CompaniesConfig : IEntityTypeConfiguration<Companies>
    {

        public void Configure(EntityTypeBuilder<Companies> builder)
        {
        }
    }
}
