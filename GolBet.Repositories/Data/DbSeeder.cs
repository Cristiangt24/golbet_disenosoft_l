// GolBet.Repositories/Data/DbSeeder.cs
using GolBet.Entities;
using GolBet.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GolBet.Repositories.Data;

public static class DbSeeder
{
    // Role names as constants: a typo won't compile instead of failing silently
    public const string AdminRole = "Admin";
    public const string BettorRole = "Apostador";

    public static async Task SeedAsync(
        AppDbContext context,
        RoleManager<IdentityRole> roleManager,
        UserManager<AppUser> userManager)
    {
        // Applies any pending migration (creates the DB if it does not exist)
        await context.Database.MigrateAsync();

        await SeedDomainAsync(context);
        await SeedIdentityAsync(roleManager, userManager);
    }

    // ---- Domain data: teams and matches (Module 3) ----
    private static async Task SeedDomainAsync(AppDbContext context)
    {
        if (await context.Teams.AnyAsync()) return;   // idempotence guard

        // ---- Teams ----
        var teams = new List<Team>
        {
            new() {
                Name = "Atlético Nacional",
                City = "Medellín",
                CrestUrl = "https://placehold.co/80x80/006633/ffffff?text=NAC"
                //CrestUrl = "https://upload.wikimedia.org/wikipedia/commons/a/ae/Atletico_Nacional_Logo.svg?utm_source=es.wikipedia.org&utm_campaign=index&utm_content=original"
            },
            new() { Name = "Independiente Medellín",  City = "Medellín",     CrestUrl = "https://placehold.co/80x80/cc0000/ffffff?text=DIM" },
            new() { Name = "Millonarios",             City = "Bogotá",       CrestUrl = "https://placehold.co/80x80/003399/ffffff?text=MIL" },
            new() { Name = "Independiente Santa Fe",  City = "Bogotá",       CrestUrl = "https://placehold.co/80x80/cc0000/ffffff?text=SFE" },
            new() { Name = "América de Cali",         City = "Cali",         CrestUrl = "https://placehold.co/80x80/e60000/ffffff?text=AME" },
            new() { Name = "Deportivo Cali",          City = "Cali",         CrestUrl = "https://placehold.co/80x80/00794d/ffffff?text=CAL" },
            new() { Name = "Junior de Barranquilla",  City = "Barranquilla", CrestUrl = "https://placehold.co/80x80/d40026/ffffff?text=JUN" },
            new() { Name = "Once Caldas",             City = "Manizales",    CrestUrl = "https://placehold.co/80x80/ffffff/000000?text=ONC" }
        };

        context.Teams.AddRange(teams);
        await context.SaveChangesAsync();   // CreatedDate stamped automatically

        // ---- Matches ----
        var today = DateTime.UtcNow.Date;

        var matches = new List<Match>
        {
            // Scheduled: open for betting
            new()
            {
                HomeTeamId = teams[0].Id, AwayTeamId = teams[1].Id,    // clásico paisa
                Date = today.AddDays(3).AddHours(20),
                Status = MatchStatus.Scheduled,
                HomeOdds = 2.10m,
                DrawOdds = 3.20m,
                AwayOdds = 3.60m
            },
            new()
            {
                HomeTeamId = teams[2].Id, AwayTeamId = teams[3].Id,    // clásico capitalino
                Date = today.AddDays(5).AddHours(18),
                Status = MatchStatus.Scheduled,
                HomeOdds = 2.45m, DrawOdds = 3.00m, AwayOdds = 2.95m
            },
            new()
            {
                HomeTeamId = teams[4].Id, AwayTeamId = teams[5].Id,    // clásico vallecaucano
                Date = today.AddDays(7).AddHours(19),
                Status = MatchStatus.Scheduled,
                HomeOdds = 2.30m, DrawOdds = 3.10m, AwayOdds = 3.15m
            },
            new()
            {
                HomeTeamId = teams[6].Id, AwayTeamId = teams[7].Id,
                Date = today.AddDays(10).AddHours(16),
                Status = MatchStatus.Scheduled,
                HomeOdds = 1.85m, DrawOdds = 3.40m, AwayOdds = 4.20m
            },

            // InProgress: betting closed, no result yet
            new()
            {
                HomeTeamId = teams[3].Id, AwayTeamId = teams[6].Id,
                Date = DateTime.UtcNow.AddHours(-1),
                Status = MatchStatus.InProgress,
                HomeOdds = 2.60m, DrawOdds = 3.05m, AwayOdds = 2.80m
            },

            // Finished: has a final score
            new()
            {
                HomeTeamId = teams[1].Id, AwayTeamId = teams[2].Id,
                Date = today.AddDays(-4).AddHours(20),
                Status = MatchStatus.Finished,
                HomeGoals = 2, AwayGoals = 1,
                HomeOdds = 2.75m, DrawOdds = 3.10m, AwayOdds = 2.70m
            }
        };

        context.Matches.AddRange(matches);
        await context.SaveChangesAsync();
    }

    // ---- Identity data: roles and admin user (Module 7) ----
    private static async Task SeedIdentityAsync(
        RoleManager<IdentityRole> roleManager,
        UserManager<AppUser> userManager)
    {
        // Roles (idempotent)
        foreach (var role in new[] { AdminRole, BettorRole })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Admin user (idempotent)
        if (await userManager.FindByEmailAsync("admin@golbet.com") is null)
        {
            var admin = new AppUser
            {
                UserName = "admin@golbet.com",
                Email = "admin@golbet.com",
                FullName = "Administrador GolBet",
                Balance = 0m,              // admins manage, they do not bet
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Admin123*");
            await userManager.AddToRoleAsync(admin, AdminRole);
        }
    }
}
