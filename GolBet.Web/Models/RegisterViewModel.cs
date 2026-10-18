// GolBet.Web/Models/RegisterViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace GolBet.Web.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "El nombre completo es obligatorio")]
    [MaxLength(100)]
    [Display(Name = "Nombre completo")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Debe ser un correo válido")]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
    [Display(Name = "Confirmar contraseña")]
    public string ConfirmPassword { get; set; } = null!;
}

