// GolBet.Entities/AppUser.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace GolBet.Entities;

/// <summary>
/// Application user. Extends IdentityUser with GolBet-specific data.
/// Note: cannot inherit AuditableEntity (C# single inheritance);
/// Identity brings its own string Id.
/// </summary>
public class AppUser : IdentityUser
{
    [MaxLength(100)]
    public string FullName { get; set; } = null!;

    /// <summary>Virtual currency balance (FutCoins).</summary>
    public decimal Balance { get; set; }

    public ICollection<Bet> Bets { get; set; } = new List<Bet>();
}
