using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MiGenteEnLinea.Web.Models.ViewModels;
using MiGenteEnLinea.Web.Services;
using System.Security.Claims;

namespace MiGenteEnLinea.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiService _apiService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IApiService apiService, ILogger<AuthController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        /// <summary>
        /// GET: /Auth/Login
        /// </summary>
        [HttpGet]
        public IActionResult Login(string? email, string? pass)
        {
            // Si ya está autenticado, redirigir al dashboard apropiado
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToDashboard();
            }

            // Si vienen parámetros en URL (login automático desde email)
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(pass))
            {
                var model = new LoginViewModel
                {
                    Email = email,
                    Password = pass
                };
                return View(model);
            }

            return View(new LoginViewModel());
        }

        /// <summary>
        /// POST: /Auth/Login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var response = await _apiService.LoginAsync(model.Email, model.Password);

                if (!response.Success || response.Data == null)
                {
                    // Determinar el tipo de error basado en el mensaje
                    var errorMessage = response.Message ?? "Error al iniciar sesión";
                    
                    if (errorMessage.Contains("inactiv") || errorMessage.Contains("activ"))
                    {
                        TempData["ErrorType"] = "inactive";
                        TempData["ErrorMessage"] = "Este perfil aún no se encuentra activo. Favor revise su email";
                    }
                    else if (errorMessage.Contains("no existe") || errorMessage.Contains("not found"))
                    {
                        TempData["ErrorType"] = "notfound";
                        TempData["ErrorMessage"] = "Los datos suministrados no están asociados a ninguna cuenta existente";
                    }
                    else
                    {
                        TempData["ErrorType"] = "incorrect";
                        TempData["ErrorMessage"] = "Usuario o contraseña incorrectos";
                    }

                    return View(model);
                }

                // Login exitoso - crear cookie de autenticación
                var user = response.Data.User;
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Nombre),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("tipo", user.Tipo),
                    new Claim("planID", user.PlanId.ToString()),
                    new Claim("token", response.Data.Token)
                };

                if (user.VencimientoPlan.HasValue)
                {
                    claims.Add(new Claim("vencimientoPlan", user.VencimientoPlan.Value.ToString("o")));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe 
                        ? DateTimeOffset.UtcNow.AddDays(30) 
                        : DateTimeOffset.UtcNow.AddHours(8)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                // Guardar JWT en sesión para llamadas a la API
                HttpContext.Session.SetString("JwtToken", response.Data.Token);
                HttpContext.Session.SetString("RefreshToken", response.Data.RefreshToken);

                _logger.LogInformation("Usuario {Email} inició sesión exitosamente", model.Email);

                // Redirigir según tipo de usuario
                return RedirectToDashboard();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al procesar login para {Email}", model.Email);
                TempData["ErrorType"] = "error";
                TempData["ErrorMessage"] = "Ha ocurrido un error inesperado. Por favor intente nuevamente.";
                return View(model);
            }
        }

        /// <summary>
        /// POST: /Auth/ForgotPassword (AJAX)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Email inválido" });
            }

            try
            {
                var response = await _apiService.ForgotPasswordAsync(model.Email);

                if (response.Success)
                {
                    return Json(new 
                    { 
                        success = true, 
                        message = "Te hemos enviado un correo electrónico para restablecer tu acceso" 
                    });
                }
                else
                {
                    return Json(new 
                    { 
                        success = false, 
                        message = response.Message ?? "Los datos suministrados no están asociados a ninguna cuenta existente" 
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar forgot password para {Email}", model.Email);
                return Json(new { success = false, message = "Error al procesar la solicitud" });
            }
        }

        /// <summary>
        /// GET: /Auth/Logout
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            
            _logger.LogInformation("Usuario cerró sesión");
            
            return RedirectToAction(nameof(Login));
        }

        /// <summary>
        /// GET: /Auth/Register
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            // Si ya está autenticado, redirigir al dashboard
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToDashboard();
            }

            return View(new RegisterViewModel());
        }

        /// <summary>
        /// POST: /Auth/Register
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Validar si el email ya existe
                var validateResponse = await _apiService.ValidateEmailAsync(model.Email);

                if (validateResponse.Success && validateResponse.Data != null)
                {
                    // Email existe
                    if (validateResponse.Data.IsActive)
                    {
                        // Email ya está activado
                        TempData["ErrorType"] = "exists";
                        TempData["ErrorMessage"] = "Esta dirección de correo ya fue registrada";
                        return View(model);
                    }
                    else
                    {
                        // Email existe pero no está activado - ofrecer reenvío
                        TempData["ShowResend"] = true;
                        TempData["UserId"] = validateResponse.Data.UserId;
                        TempData["Email"] = validateResponse.Data.Email;
                        TempData["ErrorType"] = "inactive_exists";
                        TempData["ErrorMessage"] = "Esta dirección de correo ya fue registrada pero no está activada";
                        return View(model);
                    }
                }

                // Email no existe - proceder con registro
                var registerRequest = new RegisterRequest
                {
                    TipoCuenta = model.TipoCuenta,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Email = model.Email,
                    Telefono1 = model.Telefono1,
                    Telefono2 = model.Telefono2
                };

                var registerResponse = await _apiService.RegisterAsync(registerRequest);

                if (registerResponse.Success)
                {
                    TempData["SuccessMessage"] = registerResponse.Message;
                    _logger.LogInformation("Usuario {Email} registrado exitosamente", model.Email);
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    TempData["ErrorType"] = "error";
                    TempData["ErrorMessage"] = registerResponse.Message ?? "Error al registrar usuario";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar usuario {Email}", model.Email);
                TempData["ErrorType"] = "error";
                TempData["ErrorMessage"] = "Ha ocurrido un error inesperado. Por favor intente nuevamente.";
                return View(model);
            }
        }

        /// <summary>
        /// POST: /Auth/ResendActivation (AJAX)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ResendActivation([FromBody] ResendActivationRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Email))
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            try
            {
                var response = await _apiService.ResendActivationEmailAsync(request.UserId, request.Email);

                if (response.Success)
                {
                    return Json(new { success = true, message = response.Message });
                }
                else
                {
                    return Json(new { success = false, message = response.Message ?? "Error al enviar correo" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al reenviar correo de activación");
                return Json(new { success = false, message = "Error al procesar la solicitud" });
            }
        }

        /// <summary>
        /// Redirige al dashboard apropiado según el tipo de usuario
        /// </summary>
        private IActionResult RedirectToDashboard()
        {
            var userType = User.FindFirst("tipo")?.Value;

            if (userType == "1") // Empleador
            {
                return RedirectToAction("Comunidad", "Home");
            }
            else if (userType == "2") // Contratista
            {
                return RedirectToAction("Index", "Contratista");
            }

            // Fallback
            return RedirectToAction("Index", "Home");
        }
    }

    // Request DTO para reenvío de activación
    public class ResendActivationRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
