using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using UserService.Models;

namespace UserService.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");
            builder.HasKey(x => x.Id);
            builder.Property<string>("Username").IsRequired();
            builder.Property<string>("Email").IsRequired();
            builder.Property<string>("Password").IsRequired();
        }
    }
}
