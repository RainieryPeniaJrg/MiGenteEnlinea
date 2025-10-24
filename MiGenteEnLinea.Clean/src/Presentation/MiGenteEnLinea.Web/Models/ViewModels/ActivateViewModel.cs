using System.ComponentModel.DataAnnotations;

namespace MiGenteEnLinea.Web.Models.ViewModels
{
    public class ActivateViewModel
    {
        [Required(ErrorMessage = "El ID de usuario es requerido")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe confirmar su contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public bool ResetPass { get; set; }
    }
}
