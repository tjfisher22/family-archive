using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FamilyArchive.Infrastructure.Data;

public class FamilyArchiveDbContext : DbContext
{
    public FamilyArchiveDbContext(DbContextOptions<FamilyArchiveDbContext> options)
        : base(options) { }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<MemberName> MemberNames => Set<MemberName>();
    public DbSet<Family> Families => Set<Family>();
    public DbSet<Clan> Clans => Set<Clan>();
    public DbSet<MemberRelationship> MemberRelationships => Set<MemberRelationship>();
    public DbSet<MemberPartnership> MemberPartnerships => Set<MemberPartnership>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Member <-> MemberName (1-to-many) - use backing field
        modelBuilder.Entity<Member>()
            .HasMany(m => m.Names)
            .WithOne(n => n.Member)
            .HasForeignKey(n => n.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Member>()
            .Navigation(m => m.Names)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Member <-> Family (many-to-1)
        modelBuilder.Entity<Member>()
            .HasOne(m => m.Family)
            .WithMany(f => f.Members)
            .HasForeignKey(m => m.FamilyId)
            .OnDelete(DeleteBehavior.SetNull);

        // Member <-> Clan (many-to-1)
        modelBuilder.Entity<Member>()
            .HasOne(m => m.Clan)
            .WithMany(c => c.Members)
            .HasForeignKey(m => m.ClanId)
            .OnDelete(DeleteBehavior.SetNull);

        // Member <-> MemberRelationship (1-to-many, as child)
        modelBuilder.Entity<MemberRelationship>()
            .HasOne(r => r.Member)
            .WithMany(m => m.Relationships)
            .HasForeignKey(r => r.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        // Member <-> MemberRelationship (1-to-many, as parent)
        modelBuilder.Entity<MemberRelationship>()
            .HasOne(r => r.RelatedMember)
            .WithMany(m => m.ParentRelationships)
            .HasForeignKey(r => r.RelatedMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Member <-> MemberPartnership (1-to-many, as member)
        modelBuilder.Entity<MemberPartnership>()
            .HasOne(p => p.Member)
            .WithMany(m => m.Partnerships)
            .HasForeignKey(p => p.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        // Member <-> MemberPartnership (1-to-many, as partner)
        modelBuilder.Entity<MemberPartnership>()
            .HasOne(p => p.Partner)
            .WithMany(m => m.PartnerPartnerships)
            .HasForeignKey(p => p.PartnerId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}
