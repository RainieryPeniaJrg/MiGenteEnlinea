using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiGenteEnLinea.Web.Models;
using MiGenteEnLinea.Web.Services;
using Microsoft.AspNetCore.Authorization;

namespace MiGenteEnLinea.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IApiService _apiService;

    public HomeController(ILogger<HomeController> logger, IApiService apiService)
    {
        _logger = logger;
        _apiService = apiService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    #region Dashboard

    [Authorize]
    [HttpGet]
    public IActionResult Dashboard()
    {
        try
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Dashboard muestra datos estáticos/mock según el Legacy
            // En una implementación futura se pueden cargar datos reales desde la API
            var model = new DashboardViewModel
            {
                TotalPagos = 0,
                TotalEmpleados = 0,
                TotalCalificaciones = 0,
                HistorialPagos = new List<PagoHistorialViewModel>
                {
                    new PagoHistorialViewModel { Fecha = new DateTime(2024, 11, 12), Monto = 500.00m, Estado = "Completado" },
                    new PagoHistorialViewModel { Fecha = new DateTime(2024, 11, 11), Monto = 320.00m, Estado = "Completado" }
                }
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar Dashboard");
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    #endregion

    #region Comunidad

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Comunidad(string? criterio, string? ubicacion)
    {
        try
        {
            // Cargar provincias para dropdown de ubicación
            var provincias = await _apiService.GetProvinciasAsync() ?? new List<ProvinciaDto>();
            ViewBag.Provincias = provincias;

            var model = new ComunidadViewModel
            {
                Criterio = criterio,
                Ubicacion = ubicacion,
                Contratistas = new List<ContratistaCardViewModel>()
            };

            // Si hay criterios de búsqueda o no hay nada, cargar contratistas
            if (!string.IsNullOrEmpty(criterio) || !string.IsNullOrEmpty(ubicacion))
            {
                // Búsqueda con criterios
                var contratistas = await _apiService.BuscarContratistasAsync(criterio, ubicacion);
                if (contratistas != null)
                {
                    model.Contratistas = MapearContratistas(contratistas);
                    model.TituloBuscador = "Resultados de Búsqueda";
                }
            }
            else
            {
                // Últimas publicaciones (sin filtros)
                var contratistas = await _apiService.GetUltimosContratistasAsync(20);
                if (contratistas != null)
                {
                    model.Contratistas = MapearContratistas(contratistas);
                    model.TituloBuscador = "Últimas Publicaciones";
                }
            }

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar Comunidad");
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> PerfilContratista(string id)
    {
        try
        {
            var perfil = await _apiService.GetPerfilContratistaPublicoAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }

            var servicios = await _apiService.GetServiciosContratistaAsync(id);

            var model = new PerfilContratistaPublicoViewModel
            {
                UserID = id,
                Nombre = perfil.Nombre,
                Apellido = perfil.Apellido,
                Tipo = perfil.Tipo,
                RazonSocial = perfil.RazonSocial,
                Titulo = perfil.Titulo,
                Presentacion = perfil.Presentacion,
                Email = perfil.Email,
                Experiencia = perfil.Experiencia,
                Telefono1 = perfil.Telefono1,
                Whatsapp1 = perfil.Whatsapp1,
                Telefono2 = perfil.Telefono2,
                Whatsapp2 = perfil.Whatsapp2,
                ImagenURL = perfil.ImagenURL,
                Calificacion = perfil.Calificacion,
                Servicios = servicios?.Select(s => new ServicioContratistaViewModel
                {
                    ServicioID = s.ServicioID,
                    DetalleServicio = s.DetalleServicio
                }).ToList() ?? new List<ServicioContratistaViewModel>()
            };

            return Json(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar perfil de contratista {Id}", id);
            return StatusCode(500, new { success = false, message = "Error al cargar el perfil" });
        }
    }

    private List<ContratistaCardViewModel> MapearContratistas(dynamic contratistas)
    {
        var result = new List<ContratistaCardViewModel>();
        
        foreach (var c in contratistas)
        {
            string nombre = c.Tipo == 1 ? $"{c.Nombre} {c.Apellido}" : c.Nombre;
            string imagenURL = string.IsNullOrEmpty(c.ImagenURL) 
                ? "https://upload.wikimedia.org/wikipedia/commons/7/7c/Profile_avatar_placeholder_large.png" 
                : c.ImagenURL;

            result.Add(new ContratistaCardViewModel
            {
                UserID = c.UserID?.ToString() ?? "",
                Nombre = nombre,
                Titulo = c.Titulo ?? "",
                ImagenURL = imagenURL,
                Calificacion = c.Calificacion,
                TotalRegistros = c.TotalRegistros
            });
        }

        return result;
    }

    #endregion

    #region FAQ

    [HttpGet]
    public IActionResult FAQ()
    {
        // Página estática, no requiere lógica del servidor
        return View();
    }

    #endregion

    #region ViewModels

    public class DashboardViewModel
    {
        public decimal TotalPagos { get; set; }
        public int TotalEmpleados { get; set; }
        public int TotalCalificaciones { get; set; }
        public List<PagoHistorialViewModel> HistorialPagos { get; set; } = new();
    }

    public class PagoHistorialViewModel
    {
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; } = "";
    }

    public class ComunidadViewModel
    {
        public string? Criterio { get; set; }
        public string? Ubicacion { get; set; }
        public string TituloBuscador { get; set; } = "Encuentra al Colaborador Perfecto";
        public List<ContratistaCardViewModel> Contratistas { get; set; } = new();
    }

    public class ContratistaCardViewModel
    {
        public string UserID { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Titulo { get; set; } = "";
        public string ImagenURL { get; set; } = "";
        public decimal Calificacion { get; set; }
        public int TotalRegistros { get; set; }

        public string GetStarsHtml()
        {
            var starsHtml = new System.Text.StringBuilder();
            decimal estrellasLlenas = Math.Floor(Calificacion);
            decimal estrellasMedias = Math.Ceiling(Calificacion - estrellasLlenas);

            for (int i = 1; i <= 5; i++)
            {
                if (i <= estrellasLlenas)
                {
                    starsHtml.Append("<i class='bi bi-star-fill' style='color:darkorange'></i>");
                }
                else if (i <= estrellasLlenas + estrellasMedias)
                {
                    starsHtml.Append("<i class='bi bi-star-half' style='color:darkorange'></i>");
                }
                else
                {
                    starsHtml.Append("<i class='bi bi-star' style='color:darkgray'></i>");
                }
            }

            return starsHtml.ToString();
        }
    }

    public class PerfilContratistaPublicoViewModel
    {
        public string UserID { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public int Tipo { get; set; }
        public string? RazonSocial { get; set; }
        public string Titulo { get; set; } = "";
        public string Presentacion { get; set; } = "";
        public string Email { get; set; } = "";
        public int Experiencia { get; set; }
        public string Telefono1 { get; set; } = "";
        public bool Whatsapp1 { get; set; }
        public string? Telefono2 { get; set; }
        public bool Whatsapp2 { get; set; }
        public string? ImagenURL { get; set; }
        public decimal Calificacion { get; set; }
        public List<ServicioContratistaViewModel> Servicios { get; set; } = new();

        public string GetNombreCompleto()
        {
            return Tipo == 1 ? $"{Nombre} {Apellido}" : RazonSocial ?? Nombre;
        }

        public string GetStarsHtml()
        {
            var starsHtml = new System.Text.StringBuilder();
            decimal estrellasLlenas = Math.Floor(Calificacion);
            decimal estrellasMedias = Math.Ceiling(Calificacion - estrellasLlenas);

            for (int i = 1; i <= 5; i++)
            {
                if (i <= estrellasLlenas)
                {
                    starsHtml.Append("<i class='bi bi-star-fill' style='color:yellow'></i>");
                }
                else if (i <= estrellasLlenas + estrellasMedias)
                {
                    starsHtml.Append("<i class='bi bi-star-half' style='color:yellow'></i>");
                }
                else
                {
                    starsHtml.Append("<i class='bi bi-star' style='color:white'></i>");
                }
            }

            return starsHtml.ToString();
        }
    }

    public class ServicioContratistaViewModel
    {
        public int ServicioID { get; set; }
        public string DetalleServicio { get; set; } = "";
    }

    #endregion
}
