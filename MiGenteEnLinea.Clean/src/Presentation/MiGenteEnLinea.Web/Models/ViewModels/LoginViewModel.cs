using System.ComponentModel.DataAnnotations;

namespace MiGenteEnLinea.Web.Models.ViewModels
{
    /// <summary>
    /// ViewModel para el formulario de inicio de sesión
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// ViewModel para recuperación de contraseña
    /// </summary>
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; } = string.Empty;
    }
}
