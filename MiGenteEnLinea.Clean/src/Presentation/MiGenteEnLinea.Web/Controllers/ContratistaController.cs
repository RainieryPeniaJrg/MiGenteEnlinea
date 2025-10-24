using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiGenteEnLinea.Web.Services;
using System.Security.Claims;

namespace MiGenteEnLinea.Web.Controllers
{
    [Authorize(Roles = "Contratista")]
    public class ContratistaController : Controller
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ContratistaController> _logger;
        private readonly IWebHostEnvironment _env;

        public ContratistaController(
            IApiService apiService,
            ILogger<ContratistaController> logger,
            IWebHostEnvironment env)
        {
            _apiService = apiService;
            _logger = logger;
            _env = env;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("Usuario no autenticado");
        }

        #region Index - Profile Management

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = GetUserId();
                var perfil = await _apiService.GetPerfilContratistaAsync(userId);

                if (perfil == null)
                {
                    // Si no existe perfil, crear uno vacío
                    var model = new PerfilContratistaViewModel
                    {
                        UserID = userId,
                        Tipo = 1, // Default: Persona Física
                        Activo = false,
                        Rating = 0,
                        NumeroCalificaciones = 0,
                        Servicios = new List<ServicioViewModel>()
                    };

                    // Cargar catálogos
                    await LoadCatalogos();

                    return View(model);
                }

                var viewModel = MapToViewModel(perfil);

                // Cargar servicios
                var servicios = await _apiService.GetServiciosContratistaAsync(userId);
                viewModel.Servicios = servicios?.Select(s => new ServicioViewModel
                {
                    ServicioID = s.ServicioID,
                    DetalleServicio = s.DetalleServicio
                }).ToList() ?? new List<ServicioViewModel>();

                // Cargar catálogos
                await LoadCatalogos();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando perfil del contratista");
                TempData["ErrorMessage"] = "Error al cargar el perfil. Intente nuevamente.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                var userId = GetUserId();

                var result = await _apiService.ActualizarPerfilContratistaAsync(userId, request);

                if (result.Success)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando perfil del contratista");
                return Json(new { success = false, message = "Error al actualizar el perfil" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile avatarFile)
        {
            try
            {
                if (avatarFile == null || avatarFile.Length == 0)
                {
                    return Json(new { success = false, message = "No se seleccionó ningún archivo" });
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(avatarFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    return Json(new { success = false, message = "Tipo de archivo no permitido. Use JPG, PNG o GIF." });
                }

                // Validate file size (max 5MB)
                if (avatarFile.Length > 5 * 1024 * 1024)
                {
                    return Json(new { success = false, message = "El archivo es muy grande. Tamaño máximo: 5MB" });
                }

                var userId = GetUserId();

                var result = await _apiService.UploadAvatarAsync(userId, avatarFile);

                if (result.Success)
                {
                    // Return success - the frontend will reload the page to show the new avatar
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subiendo avatar del contratista");
                return Json(new { success = false, message = "Error al subir la imagen" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleProfileStatus([FromBody] ToggleStatusRequest request)
        {
            try
            {
                var userId = GetUserId();

                var result = await _apiService.ToggleProfileStatusAsync(userId, request.Activate);

                if (result.Success)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cambiando estado del perfil");
                return Json(new { success = false, message = "Error al cambiar el estado del perfil" });
            }
        }

        private async Task LoadCatalogos()
        {
            try
            {
                // Cargar sectores
                var sectores = await _apiService.GetSectoresAsync();
                ViewBag.Sectores = sectores?.Select(s => new SelectListItem
                {
                    Value = s.Sector,
                    Text = s.Sector
                }).ToList() ?? new List<SelectListItem>();

                // Cargar provincias
                var provincias = await _apiService.GetProvinciasAsync();
                ViewBag.Provincias = provincias?.Select(p => new SelectListItem
                {
                    Value = p.Nombre,
                    Text = p.Nombre
                }).ToList() ?? new List<SelectListItem>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando catálogos");
                ViewBag.Sectores = new List<SelectListItem>();
                ViewBag.Provincias = new List<SelectListItem>();
            }
        }

        private PerfilContratistaViewModel MapToViewModel(dynamic perfil)
        {
            return new PerfilContratistaViewModel
            {
                ContratistaID = perfil.ContratistaID,
                UserID = perfil.UserID,
                Titulo = perfil.Titulo ?? string.Empty,
                Sector = perfil.Sector ?? string.Empty,
                Presentacion = perfil.Presentacion ?? string.Empty,
                Email = perfil.Email ?? string.Empty,
                Experiencia = perfil.Experiencia ?? 0,
                Provincia = perfil.Provincia ?? string.Empty,
                Tipo = perfil.Tipo,
                Identificacion = perfil.Identificacion ?? string.Empty,
                Nombre = perfil.Nombre ?? string.Empty,
                Apellido = perfil.Apellido ?? string.Empty,
                RazonSocial = perfil.Tipo == 2 ? perfil.Nombre : string.Empty,
                Telefono1 = perfil.Telefono1 ?? string.Empty,
                Telefono2 = perfil.Telefono2 ?? string.Empty,
                Whatsapp1 = perfil.Whatsapp1 ?? false,
                Whatsapp2 = perfil.Whatsapp2 ?? false,
                ImagenURL = perfil.ImagenURL,
                Rating = perfil.Rating ?? 0,
                NumeroCalificaciones = perfil.NumeroCalificaciones ?? 0,
                Activo = perfil.Activo ?? false,
                Servicios = new List<ServicioViewModel>()
            };
        }

        #endregion

        #region Servicios - Service Catalog

        [HttpPost]
        public async Task<IActionResult> AddServicio([FromBody] AddServicioRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.DetalleServicio))
                {
                    return Json(new { success = false, message = "Debe ingresar la descripción del servicio" });
                }

                var userId = GetUserId();

                var result = await _apiService.AddServicioAsync(userId, request.DetalleServicio);

                if (result.Success)
                {
                    // Return success - generate a temporary ID (the real ID will come from the API reload)
                    return Json(new { success = true, servicioID = DateTime.Now.Ticks });
                }

                return Json(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error agregando servicio");
                return Json(new { success = false, message = "Error al agregar el servicio" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteServicio([FromBody] DeleteServicioRequest request)
        {
            try
            {
                var result = await _apiService.DeleteServicioAsync(request.ServicioID);

                if (result.Success)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando servicio");
                return Json(new { success = false, message = "Error al eliminar el servicio" });
            }
        }

        #endregion

        #region Calificaciones - Ratings

        [HttpGet]
        public async Task<IActionResult> MisCalificaciones()
        {
            try
            {
                var userId = GetUserId();

                // Obtener perfil para el encabezado
                var perfil = await _apiService.GetPerfilContratistaAsync(userId);

                var viewModel = new MisCalificacionesViewModel
                {
                    Rating = perfil?.Rating ?? 0,
                    NumeroCalificaciones = perfil?.NumeroCalificaciones ?? 0
                };

                // Obtener calificaciones
                var calificaciones = await _apiService.GetCalificacionesContratistaAsync(userId);

                viewModel.Calificaciones = calificaciones?.Select(c => new CalificacionViewModel
                {
                    CalificacionID = c.CalificacionID,
                    Fecha = c.Fecha,
                    NombreCalificador = c.NombreCalificador ?? "Anónimo",
                    Puntualidad = c.Puntualidad,
                    Conocimientos = c.Conocimientos,
                    Cumplimiento = c.Cumplimiento,
                    Recomendacion = c.Recomendacion,
                    PromedioEstrellas = (c.Puntualidad + c.Conocimientos + c.Cumplimiento + c.Recomendacion) / 4.0m
                }).OrderByDescending(c => c.Fecha).ToList() ?? new List<CalificacionViewModel>();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando calificaciones del contratista");
                TempData["ErrorMessage"] = "Error al cargar las calificaciones. Intente nuevamente.";
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region Suscripciones - Subscription Plans

        [HttpGet]
        public async Task<IActionResult> AdquirirPlan()
        {
            try
            {
                var planes = await _apiService.GetPlanesContratistaAsync();

                var viewModel = new AdquirirPlanViewModel
                {
                    Planes = planes?.Select(p => new PlanViewModel
                    {
                        PlanID = p.PlanID,
                        Nombre = p.Nombre ?? "Plan",
                        Precio = p.Precio,
                        Descripcion = p.Caracteristicas ?? string.Empty,
                        Duracion = p.Duracion,
                        Caracteristicas = ParseCaracteristicas(p.Caracteristicas)
                    }).ToList() ?? new List<PlanViewModel>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cargando planes para contratista");
                TempData["ErrorMessage"] = "Error al cargar los planes. Intente nuevamente.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarPago([FromBody] ProcesarPagoRequest request)
        {
            try
            {
                var userId = GetUserId();

                var result = await _apiService.ProcesarPagoSuscripcionAsync(userId, request);

                if (result)
                {
                    return Json(new { success = true, transactionId = Guid.NewGuid().ToString() });
                }

                return Json(new { success = false, message = "No se pudo procesar el pago" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando pago de suscripción");
                return Json(new { success = false, message = "Error al procesar el pago. Intente nuevamente." });
            }
        }

        private List<string> ParseCaracteristicas(string? descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                return new List<string>
                {
                    "1 Administrador",
                    "1 Registro de Perfil Profesional",
                    "Consultas Ilimitadas",
                    "Registro de Portafolio",
                    "Adquisición de Reputación por Rating",
                    "12 Meses de Data Histórica"
                };
            }

            return descripcion
                .Split(new[] { '\n', '\r', '|', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .Where(c => !string.IsNullOrEmpty(c))
                .ToList();
        }

        #endregion

        #region ViewModels

        public class PerfilContratistaViewModel
        {
            public int ContratistaID { get; set; }
            public string UserID { get; set; } = string.Empty;
            public string Titulo { get; set; } = string.Empty;
            public string Sector { get; set; } = string.Empty;
            public string Presentacion { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public int Experiencia { get; set; }
            public string Provincia { get; set; } = string.Empty;
            public int Tipo { get; set; } // 1=Persona Física, 2=Empresa
            public string Identificacion { get; set; } = string.Empty;
            public string Nombre { get; set; } = string.Empty;
            public string Apellido { get; set; } = string.Empty;
            public string RazonSocial { get; set; } = string.Empty;
            public string Telefono1 { get; set; } = string.Empty;
            public string Telefono2 { get; set; } = string.Empty;
            public bool Whatsapp1 { get; set; }
            public bool Whatsapp2 { get; set; }
            public string? ImagenURL { get; set; }
            public decimal Rating { get; set; }
            public int NumeroCalificaciones { get; set; }
            public bool Activo { get; set; }
            public List<ServicioViewModel> Servicios { get; set; } = new();
        }

        public class ServicioViewModel
        {
            public int ServicioID { get; set; }
            public string DetalleServicio { get; set; } = string.Empty;
        }

        public class MisCalificacionesViewModel
        {
            public decimal Rating { get; set; }
            public int NumeroCalificaciones { get; set; }
            public List<CalificacionViewModel> Calificaciones { get; set; } = new();
        }

        public class CalificacionViewModel
        {
            public int CalificacionID { get; set; }
            public DateTime Fecha { get; set; }
            public string NombreCalificador { get; set; } = string.Empty;
            public int Puntualidad { get; set; }
            public int Conocimientos { get; set; }
            public int Cumplimiento { get; set; }
            public int Recomendacion { get; set; }
            public decimal PromedioEstrellas { get; set; }

            public string GetStarsHtml(int rating)
            {
                var stars = string.Empty;
                for (int i = 1; i <= 5; i++)
                {
                    stars += i <= rating
                        ? "<i class='fas fa-star text-warning'></i>"
                        : "<i class='far fa-star text-secondary'></i>";
                }
                return stars;
            }
        }

        public class AdquirirPlanViewModel
        {
            public List<PlanViewModel> Planes { get; set; } = new();
        }

        public class PlanViewModel
        {
            public int PlanID { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public decimal Precio { get; set; }
            public string Descripcion { get; set; } = string.Empty;
            public int Duracion { get; set; } // Meses
            public List<string> Caracteristicas { get; set; } = new();
        }

        public class UpdateProfileRequest
        {
            public string Titulo { get; set; } = string.Empty;
            public string Sector { get; set; } = string.Empty;
            public string Presentacion { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public int Experiencia { get; set; }
            public string Provincia { get; set; } = string.Empty;
            public int Tipo { get; set; }
            public string Identificacion { get; set; } = string.Empty;
            public string? Nombre { get; set; }
            public string? Apellido { get; set; }
            public string? RazonSocial { get; set; }
            public string Telefono1 { get; set; } = string.Empty;
            public string Telefono2 { get; set; } = string.Empty;
            public bool Whatsapp1 { get; set; }
            public bool Whatsapp2 { get; set; }
        }

        public class AddServicioRequest
        {
            public string DetalleServicio { get; set; } = string.Empty;
        }

        public class DeleteServicioRequest
        {
            public int ServicioID { get; set; }
        }

        public class ToggleStatusRequest
        {
            public bool Activate { get; set; }
        }

        public class ProcesarPagoRequest
        {
            public int PlanID { get; set; }
            public string CardNumber { get; set; } = string.Empty;
            public string CardHolderName { get; set; } = string.Empty;
            public string ExpiryDate { get; set; } = string.Empty;
            public string CVV { get; set; } = string.Empty;
        }

        #endregion
    }
}
