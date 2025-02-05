﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WorldCupWrapped.Models;

namespace WorldCupWrapped.Data
{
    public class ProjectContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Trophy> Trophies { get; set; }
        public DbSet<TeamTrophy> TeamTrophies { get; set; }
        public DbSet<Stadium> Stadiums { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchReferee> MatchReferees { get; set; }
        public DbSet<User> Users { get; set; }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TeamTrophy>()
                .HasKey(t => new { t.TeamId, t.TrophyId });

            modelBuilder.Entity<TeamTrophy>()
                .HasOne<Team>(tt => tt.Team)
                .WithMany(t => t.TeamsTrophies)
                .HasForeignKey(tt => tt.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamTrophy>()
                .HasOne<Trophy>(tt => tt.Trophy)
                .WithMany(t => t.TeamsTrophies)
                .HasForeignKey(tt => tt.TrophyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchReferee>()
                .HasKey(m => new { m.MatchId, m.RefereeId });

            modelBuilder.Entity<MatchReferee>()
                .HasOne<Match>(mr => mr.Match)
                .WithMany(m => m.MatchesReferees)
                .HasForeignKey(mr => mr.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchReferee>()
                .HasOne<Referee>(mr => mr.Referee)
                .WithMany(r => r.MatchesReferees)
                .HasForeignKey(mr => mr.RefereeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Stadium>()
                .HasOne(s => s.City)
                .WithMany(c => c.Stadiums)
                .HasForeignKey(s => s.CityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Manager>()
                .HasOne(m => m.Team)
                .WithOne(t => t.Manager)
                .HasForeignKey<Team>(t => t.ManagerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Stadium)
                .WithMany(s => s.Matches)
                .HasForeignKey(m => m.StadiumId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.Players)
                .WithOne(p => p.Team)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

    }
}
