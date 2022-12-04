using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Manager.Infra.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn().HasColumnType("bigint");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(80).HasColumnType("varchar(80)").HasColumnName("Name");
            builder.Property(x => x.Email).IsRequired().HasMaxLength(180).HasColumnType("varchar(180)").HasColumnName("Email");
            builder.Property(x => x.Password).IsRequired().HasMaxLength(30).HasColumnType("varchar(30)").HasColumnName("Password");
        }
    }
}