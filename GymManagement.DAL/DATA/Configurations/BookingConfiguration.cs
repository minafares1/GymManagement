using GymManagement.DAL.DATA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.DATA.Configurations
{
    internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
           builder.Ignore(b => b.Id);

            builder.HasKey(b => new { b.MemberId, b.SessionId });

            builder.Property(b => b.CreatedAt)
                   .HasColumnName("BookingDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(b => b.Session)
                 .WithMany(s => s.SessionMembers)
                     .HasForeignKey(b => b.SessionId)
                     .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
