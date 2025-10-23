using System.ComponentModel.DataAnnotations;

namespace MiGenteEnLinea.Web.Models.ViewModels
{
    /// <summary>
    /// ViewModel para el formulario de registro de usuario
    /// </summary>
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El tipo de cuenta es requerido")]
        [Display(Name = "Tipo de Cuenta")]
        public int TipoCuenta { get; set; } // 1 = Empleador, 2 = Ofertante/Contratista

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        [StringLength(100, ErrorMessage = "El correo no puede exceder 100 caracteres")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono 1 es requerido")]
        [Phone(ErrorMessage = "El teléfono no es válido")]
        [Display(Name = "Teléfono 1")]
        public string Telefono1 { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El teléfono no es válido")]
        [Display(Name = "Teléfono 2")]
        public string? Telefono2 { get; set; }
    }
}
