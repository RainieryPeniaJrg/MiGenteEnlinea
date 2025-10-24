using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiGenteEnLinea.Web.Services;
using System.Security.Claims;

namespace MiGenteEnLinea.Web.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly ILogger<SubscriptionController> _logger;
        private readonly IApiService _apiService;

        public SubscriptionController(ILogger<SubscriptionController> logger, IApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Auth");
                }

                // Obtener suscripción actual
                var suscripcion = await _apiService.GetMiSuscripcionAsync(userId);
                
                // Obtener historial de ventas
                var historico = await _apiService.GetHistorialVentasAsync(userId);

                var model = new MiSuscripcionViewModel
                {
                    PlanActual = suscripcion?.NombrePlan ?? "Sin plan activo",
                    FechaInicio = suscripcion?.FechaInicio ?? DateTime.Now,
                    ProximoPago = suscripcion?.ProximoPago,
                    HistoricoFacturacion = historico?.Select(v => new FacturaViewModel
                    {
                        Fecha = v.Fecha,
                        NombrePlan = v.NombrePlan,
                        Precio = v.Precio,
                        Tarjeta = v.TarjetaEnmascarada ?? "N/A"
                    }).ToList() ?? new List<FacturaViewModel>()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar suscripción del usuario");
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelarSuscripcion()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { success = false, message = "Usuario no autenticado" });
                }

                var result = await _apiService.CancelarSuscripcionAsync(userId);

                if (result)
                {
                    return Json(new { success = true, message = "Suscripción cancelada correctamente" });
                }

                return Json(new { success = false, message = "No se pudo cancelar la suscripción" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar suscripción");
                return Json(new { success = false, message = "Error al procesar la solicitud" });
            }
        }

        #region ViewModels

        public class MiSuscripcionViewModel
        {
            public string PlanActual { get; set; } = "";
            public DateTime FechaInicio { get; set; }
            public DateTime? ProximoPago { get; set; }
            public List<FacturaViewModel> HistoricoFacturacion { get; set; } = new();
        }

        public class FacturaViewModel
        {
            public DateTime Fecha { get; set; }
            public string NombrePlan { get; set; } = "";
            public decimal Precio { get; set; }
            public string Tarjeta { get; set; } = "";
        }

        #endregion
    }
}
