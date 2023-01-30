using Bulk.App.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulk.App.Infra.Maps
{
    public class StadiumMap : IEntityTypeConfiguration<StadiumEntity>
    {
        public void Configure(EntityTypeBuilder<StadiumEntity> builder)
        {
            builder.ToTable("stadium");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Capacity)
                .IsRequired();

            builder.Property(x => x.Nickname)
                .IsRequired();

            builder.HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
