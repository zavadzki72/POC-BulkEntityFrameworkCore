using BulkMerge.App.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BulkMerge.App.Infra.Maps
{
    public class TeamMap : IEntityTypeConfiguration<TeamEntity>
    {
        public void Configure(EntityTypeBuilder<TeamEntity> builder)
        {
            builder.ToTable("team");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Initials)
                .HasMaxLength(5)
                .IsRequired();

            builder.Property(x => x.Country)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.StadiumId)
                .IsRequired();

            builder.HasOne(x => x.Stadium)
                .WithOne(x => x.Team)
                .HasForeignKey<TeamEntity>(x => x.StadiumId);
        }
    }
}
