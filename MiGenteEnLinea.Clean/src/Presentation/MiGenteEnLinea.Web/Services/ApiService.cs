using System.Text;
using System.Text.Json;
using MiGenteEnLinea.Web.Controllers;

namespace MiGenteEnLinea.Web.Services
{
    /// <summary>
    /// Servicio para integración con la API de MiGente
    /// </summary>
    public interface IApiService
    {
        Task<ApiResponse<LoginResponse>> LoginAsync(string email, string password);
        Task<ApiResponse<object>> ForgotPasswordAsync(string email);
        Task<ApiResponse<ValidateEmailResponse>> ValidateEmailAsync(string email);
        Task<ApiResponse<object>> RegisterAsync(RegisterRequest request);
        Task<ApiResponse<object>> ResendActivationEmailAsync(string userId, string email);
        Task<ApiResponse<object>> ActivateAccountAsync(object request);
        Task<ApiResponse<List<EmpleadoDto>>> GetEmpleadosAsync(string userId, bool activo);
        Task<ApiResponse<List<ContratacionTemporalDto>>> GetContratacionesTemporalesAsync(string userId, int estatus);
        Task<ApiResponse<EmpleadoDto>> GetEmpleadoByIdAsync(string userId, int empleadoID);
        Task<ApiResponse<List<PagoEmpleadoDto>>> GetPagosEmpleadoAsync(int empleadoID);
        Task<ApiResponse<object>> EliminarReciboAsync(int pagoID);
        Task<ApiResponse<ReciboResponseDto>> ProcesarPagoAsync(object request);
        Task<ApiResponse<PerfilEmpleadorDto>> GetPerfilEmpleadorAsync(string userId);
        Task<ApiResponse<object>> ActualizarPerfilAsync(object request);
        Task<ApiResponse<List<CredencialDto>>> GetCredencialesAsync(string userId);
        Task<ApiResponse<object>> CrearCredencialAsync(object request);
        Task<ApiResponse<object>> ResetPasswordAsync(string email);
        Task<ApiResponse<List<CalificacionDto>>> GetMisCalificacionesAsync(string userId);
        Task<ApiResponse<List<PerfilParaCalificarDto>>> GetPerfilesParaCalificarAsync(string userId);
        Task<bool> CalificarPerfilAsync(string userId, object request);
        Task<PerfilConsultaDto?> ConsultarPerfilAsync(string identificacion);
        Task<DetalleContratacionDto?> GetDetalleContratacionAsync(int contratacionID, int detalleID, string userId);
        Task<bool> ActualizarContratacionAsync(string userId, object request);
        Task<PagoContratacionResponseDto?> ProcesarPagoContratacionAsync(string userId, object request);
        Task<bool> AnularReciboContratacionAsync(int pagoID);
        Task<bool> CancelarTrabajoAsync(int contratacionID, int detalleID);
        Task<bool> FinalizarTrabajoAsync(int contratacionID, int detalleID);
        Task<bool> CalificarContratacionAsync(string userId, object request);
        Task<EmpleadorController.FichaColaboradorTemporalViewModel?> GetFichaColaboradorTemporalAsync(int contratacionID, string userId);
        Task<List<TrabajoContratacionDto>> GetTrabajosContratacionAsync(int contratacionID, int estatus, string userId);
        Task<bool> EliminarColaboradorTemporalAsync(int contratacionID, string userId);
        Task<bool> CrearContratacionTemporalAsync(string userId, object request);
        Task<List<PlanDto>> GetPlanesEmpleadorAsync();
        Task<bool> ProcesarPagoSuscripcionAsync(string userId, object request);
        Task<EmpleadorController.SuscripcionInfo?> GetMiSuscripcionAsync(string userId);
        Task<List<EmpleadorController.VentaInfo>> GetHistorialVentasAsync(string userId);
        Task<bool> CancelarSuscripcionAsync(string userId);

        // Contratista methods
        Task<dynamic?> GetPerfilContratistaAsync(string userId);
        Task<ApiResponse<object>> ActualizarPerfilContratistaAsync(string userId, object request);
        Task<ApiResponse<object>> UploadAvatarAsync(string userId, IFormFile avatarFile);
        Task<List<ServicioContratistaDto>?> GetServiciosContratistaAsync(string userId);
        Task<ApiResponse<object>> AddServicioAsync(string userId, string detalleServicio);
        Task<ApiResponse<object>> DeleteServicioAsync(int servicioID);
        Task<List<SectorDto>?> GetSectoresAsync();
        Task<List<ProvinciaDto>?> GetProvinciasAsync();
        Task<List<CalificacionContratistaDto>?> GetCalificacionesContratistaAsync(string userId);
        Task<List<PlanDto>?> GetPlanesContratistaAsync();
        Task<ApiResponse<object>> ToggleProfileStatusAsync(string userId, bool activate);

        // Comunidad methods (búsqueda pública de contratistas)
        Task<List<dynamic>?> GetUltimosContratistasAsync(int cantidad = 20);
        Task<List<dynamic>?> BuscarContratistasAsync(string? criterio, string? ubicacion);
        Task<dynamic?> GetPerfilContratistaPublicoAsync(string userId);
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(string email, string password)
        {
            try
            {
                var request = new
                {
                    email = email.Trim().ToLower(),
                    password = password.Trim()
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("/api/auth/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, _jsonOptions);
                    return new ApiResponse<LoginResponse>
                    {
                        Success = true,
                        Data = loginResponse
                    };
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, _jsonOptions);
                    return new ApiResponse<LoginResponse>
                    {
                        Success = false,
                        Message = errorResponse?.Message ?? "Error al iniciar sesión"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al llamar a la API de login");
                return new ApiResponse<LoginResponse>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor"
                };
            }
        }

        public async Task<ApiResponse<object>> ForgotPasswordAsync(string email)
        {
            try
            {
                var request = new { email = email.Trim().ToLower() };

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("/api/auth/forgot-password", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Se ha enviado un correo para restablecer tu contraseña"
                    };
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, _jsonOptions);
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = errorResponse?.Message ?? "Error al enviar solicitud"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al llamar a la API de forgot password");
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor"
                };
            }
        }

        public async Task<ApiResponse<ValidateEmailResponse>> ValidateEmailAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/auth/validate-email?email={Uri.EscapeDataString(email.Trim().ToLower())}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var validateResponse = JsonSerializer.Deserialize<ValidateEmailResponse>(responseContent, _jsonOptions);
                    return new ApiResponse<ValidateEmailResponse>
                    {
                        Success = true,
                        Data = validateResponse
                    };
                }
                else
                {
                    return new ApiResponse<ValidateEmailResponse>
                    {
                        Success = false,
                        Message = "Email no encontrado"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar email");
                return new ApiResponse<ValidateEmailResponse>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor"
                };
            }
        }

        public async Task<ApiResponse<object>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("/api/auth/register", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Su perfil se ha registrado correctamente, verifique su correo electrónico para completar el proceso"
                    };
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, _jsonOptions);
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = errorResponse?.Message ?? "Error al registrar usuario"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario");
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor"
                };
            }
        }

        public async Task<ApiResponse<object>> ResendActivationEmailAsync(string userId, string email)
        {
            try
            {
                var request = new { userId, email = email.Trim().ToLower() };

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("/api/auth/resend-activation", content);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Se ha enviado nuevamente el correo de activación"
                    };
                }
                else
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error al enviar correo"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al reenviar correo de activación");
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor"
                };
            }
        }

        public async Task<ApiResponse<object>> ActivateAccountAsync(object request)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("/api/auth/activate", content);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Cuenta activada correctamente"
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = errorContent ?? "Error al activar la cuenta"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar cuenta");
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor"
                };
            }
        }

        public async Task<ApiResponse<List<EmpleadoDto>>> GetEmpleadosAsync(string userId, bool activo)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/empleados?userId={userId}&activo={activo}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var empleados = JsonSerializer.Deserialize<List<EmpleadoDto>>(responseContent, _jsonOptions);
                    return new ApiResponse<List<EmpleadoDto>>
                    {
                        Success = true,
                        Data = empleados ?? new List<EmpleadoDto>()
                    };
                }
                else
                {
                    return new ApiResponse<List<EmpleadoDto>>
                    {
                        Success = false,
                        Message = "Error al obtener empleados",
                        Data = new List<EmpleadoDto>()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleados para userId: {UserId}", userId);
                return new ApiResponse<List<EmpleadoDto>>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor",
                    Data = new List<EmpleadoDto>()
                };
            }
        }

        public async Task<ApiResponse<List<ContratacionTemporalDto>>> GetContratacionesTemporalesAsync(string userId, int estatus)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/contrataciones?userId={userId}&estatus={estatus}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var contrataciones = JsonSerializer.Deserialize<List<ContratacionTemporalDto>>(responseContent, _jsonOptions);
                    return new ApiResponse<List<ContratacionTemporalDto>>
                    {
                        Success = true,
                        Data = contrataciones ?? new List<ContratacionTemporalDto>()
                    };
                }
                else
                {
                    return new ApiResponse<List<ContratacionTemporalDto>>
                    {
                        Success = false,
                        Message = "Error al obtener contrataciones",
                        Data = new List<ContratacionTemporalDto>()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener contrataciones temporales para userId: {UserId}", userId);
                return new ApiResponse<List<ContratacionTemporalDto>>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor",
                    Data = new List<ContratacionTemporalDto>()
                };
            }
        }

        public async Task<ApiResponse<EmpleadoDto>> GetEmpleadoByIdAsync(string userId, int empleadoID)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/empleados/{empleadoID}?userId={userId}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var empleado = JsonSerializer.Deserialize<EmpleadoDto>(responseContent, _jsonOptions);
                    return new ApiResponse<EmpleadoDto>
                    {
                        Success = true,
                        Data = empleado
                    };
                }
                else
                {
                    return new ApiResponse<EmpleadoDto>
                    {
                        Success = false,
                        Message = "Empleado no encontrado"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleado {EmpleadoID} para userId: {UserId}", empleadoID, userId);
                return new ApiResponse<EmpleadoDto>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor"
                };
            }
        }

        public async Task<ApiResponse<List<PagoEmpleadoDto>>> GetPagosEmpleadoAsync(int empleadoID)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/pagos/empleado/{empleadoID}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var pagos = JsonSerializer.Deserialize<List<PagoEmpleadoDto>>(responseContent, _jsonOptions);
                    return new ApiResponse<List<PagoEmpleadoDto>>
                    {
                        Success = true,
                        Data = pagos ?? new List<PagoEmpleadoDto>()
                    };
                }
                else
                {
                    return new ApiResponse<List<PagoEmpleadoDto>>
                    {
                        Success = false,
                        Message = "Error al obtener pagos",
                        Data = new List<PagoEmpleadoDto>()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pagos para empleado {EmpleadoID}", empleadoID);
                return new ApiResponse<List<PagoEmpleadoDto>>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor",
                    Data = new List<PagoEmpleadoDto>()
                };
            }
        }

        public async Task<ApiResponse<object>> EliminarReciboAsync(int pagoID)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/pagos/{pagoID}");

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Recibo eliminado correctamente"
                    };
                }
                else
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error al eliminar recibo"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar recibo {PagoID}", pagoID);
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor"
                };
            }
        }

        public async Task<ApiResponse<ReciboResponseDto>> ProcesarPagoAsync(object request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(request, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/nomina/procesar-pago", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var recibo = JsonSerializer.Deserialize<ReciboResponseDto>(responseContent, _jsonOptions);
                    return new ApiResponse<ReciboResponseDto>
                    {
                        Success = true,
                        Message = "Pago procesado correctamente",
                        Data = recibo
                    };
                }
                else
                {
                    return new ApiResponse<ReciboResponseDto>
                    {
                        Success = false,
                        Message = "Error al procesar el pago"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar pago");
                return new ApiResponse<ReciboResponseDto>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor"
                };
            }
        }

        public async Task<ApiResponse<PerfilEmpleadorDto>> GetPerfilEmpleadorAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/perfil/empleador?userId={userId}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var perfil = JsonSerializer.Deserialize<PerfilEmpleadorDto>(responseContent, _jsonOptions);
                    return new ApiResponse<PerfilEmpleadorDto>
                    {
                        Success = true,
                        Data = perfil
                    };
                }
                else
                {
                    return new ApiResponse<PerfilEmpleadorDto>
                    {
                        Success = false,
                        Message = "Perfil no encontrado"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfil empleador para userId: {UserId}", userId);
                return new ApiResponse<PerfilEmpleadorDto>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor"
                };
            }
        }

        public async Task<ApiResponse<object>> ActualizarPerfilAsync(object request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(request, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("/api/perfil/empleador", content);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Perfil actualizado correctamente"
                    };
                }
                else
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error al actualizar perfil"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar perfil");
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor"
                };
            }
        }

        public async Task<ApiResponse<List<CredencialDto>>> GetCredencialesAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/credenciales?userId={userId}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var credenciales = JsonSerializer.Deserialize<List<CredencialDto>>(responseContent, _jsonOptions);
                    return new ApiResponse<List<CredencialDto>>
                    {
                        Success = true,
                        Data = credenciales ?? new List<CredencialDto>()
                    };
                }
                else
                {
                    return new ApiResponse<List<CredencialDto>>
                    {
                        Success = false,
                        Message = "Error al obtener credenciales",
                        Data = new List<CredencialDto>()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener credenciales para userId: {UserId}", userId);
                return new ApiResponse<List<CredencialDto>>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor",
                    Data = new List<CredencialDto>()
                };
            }
        }

        public async Task<ApiResponse<object>> CrearCredencialAsync(object request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(request, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/credenciales", content);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Credencial creada correctamente"
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = errorContent.Contains("ya existe") ? "El correo ya está registrado" : "Error al crear credencial"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear credencial");
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor"
                };
            }
        }

        public async Task<ApiResponse<object>> ResetPasswordAsync(string email)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { email }, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/auth/reset-password", content);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Email de recuperación enviado correctamente"
                    };
                }
                else
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Error al enviar email"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar email de recuperación para {Email}", email);
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor"
                };
            }
        }

        public async Task<ApiResponse<List<CalificacionDto>>> GetMisCalificacionesAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/calificaciones?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var calificaciones = JsonSerializer.Deserialize<List<CalificacionDto>>(responseContent, _jsonOptions);

                    return new ApiResponse<List<CalificacionDto>>
                    {
                        Success = true,
                        Data = calificaciones ?? new List<CalificacionDto>()
                    };
                }
                else
                {
                    return new ApiResponse<List<CalificacionDto>>
                    {
                        Success = false,
                        Message = "Error al obtener calificaciones"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener calificaciones del usuario {UserId}", userId);
                return new ApiResponse<List<CalificacionDto>>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor",
                    Data = new List<CalificacionDto>()
                };
            }
        }

        public async Task<ApiResponse<List<PerfilParaCalificarDto>>> GetPerfilesParaCalificarAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/perfiles/calificar?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var perfiles = JsonSerializer.Deserialize<List<PerfilParaCalificarDto>>(responseContent, _jsonOptions);

                    return new ApiResponse<List<PerfilParaCalificarDto>>
                    {
                        Success = true,
                        Data = perfiles ?? new List<PerfilParaCalificarDto>()
                    };
                }
                else
                {
                    return new ApiResponse<List<PerfilParaCalificarDto>>
                    {
                        Success = false,
                        Message = "Error al obtener perfiles para calificar"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfiles para calificar del usuario {UserId}", userId);
                return new ApiResponse<List<PerfilParaCalificarDto>>
                {
                    Success = false,
                    Message = "Error de conexión con el servidor",
                    Data = new List<PerfilParaCalificarDto>()
                };
            }
        }

        public async Task<bool> CalificarPerfilAsync(string userId, object request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new 
                { 
                    userId, 
                    calificacion = request 
                }, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/calificaciones", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // Si la API devuelve 400, es porque no cumple validación de 2 meses
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return false;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calificar perfil");
                return false;
            }
        }

        public async Task<PerfilConsultaDto?> ConsultarPerfilAsync(string identificacion)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/calificaciones/consultar?identificacion={identificacion}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var perfil = JsonSerializer.Deserialize<PerfilConsultaDto>(responseContent, _jsonOptions);
                    return perfil;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar perfil {Identificacion}", identificacion);
                return null;
            }
        }

        public async Task<DetalleContratacionDto?> GetDetalleContratacionAsync(int contratacionID, int detalleID, string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/contrataciones/{contratacionID}/detalle/{detalleID}?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var detalle = JsonSerializer.Deserialize<DetalleContratacionDto>(responseContent, _jsonOptions);
                    return detalle;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalle de contratación {ContratacionID}/{DetalleID}", contratacionID, detalleID);
                return null;
            }
        }

        public async Task<bool> ActualizarContratacionAsync(string userId, object request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { userId, contratacion = request }, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("/api/contrataciones/actualizar", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar contratación");
                return false;
            }
        }

        public async Task<PagoContratacionResponseDto?> ProcesarPagoContratacionAsync(string userId, object request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { userId, pago = request }, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/contrataciones/procesar-pago", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var resultado = JsonSerializer.Deserialize<PagoContratacionResponseDto>(responseContent, _jsonOptions);
                    return resultado;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar pago de contratación");
                return null;
            }
        }

        public async Task<bool> AnularReciboContratacionAsync(int pagoID)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/contrataciones/recibo/{pagoID}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al anular recibo de contratación {PagoID}", pagoID);
                return false;
            }
        }

        public async Task<bool> CancelarTrabajoAsync(int contratacionID, int detalleID)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { contratacionID, detalleID }, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("/api/contrataciones/cancelar", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar trabajo {ContratacionID}/{DetalleID}", contratacionID, detalleID);
                return false;
            }
        }

        public async Task<bool> FinalizarTrabajoAsync(int contratacionID, int detalleID)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { contratacionID, detalleID }, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("/api/contrataciones/finalizar", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al finalizar trabajo {ContratacionID}/{DetalleID}", contratacionID, detalleID);
                return false;
            }
        }

        public async Task<bool> CalificarContratacionAsync(string userId, object request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { userId, calificacion = request }, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/contrataciones/calificar", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calificar contratación");
                return false;
            }
        }

        public async Task<EmpleadorController.FichaColaboradorTemporalViewModel?> GetFichaColaboradorTemporalAsync(int contratacionID, string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/contrataciones/ficha/{contratacionID}?userId={userId}");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("No se encontró la ficha de colaborador temporal: {ContratacionID}", contratacionID);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<EmpleadorController.FichaColaboradorTemporalViewModel>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ficha de colaborador temporal");
                return null;
            }
        }

        public async Task<List<TrabajoContratacionDto>> GetTrabajosContratacionAsync(int contratacionID, int estatus, string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/contrataciones/{contratacionID}/trabajos?estatus={estatus}&userId={userId}");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("No se pudieron obtener trabajos de contratación");
                    return new List<TrabajoContratacionDto>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var trabajos = JsonSerializer.Deserialize<List<TrabajoContratacionDto>>(content, _jsonOptions);
                return trabajos ?? new List<TrabajoContratacionDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener trabajos de contratación");
                return new List<TrabajoContratacionDto>();
            }
        }

        public async Task<bool> EliminarColaboradorTemporalAsync(int contratacionID, string userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/contrataciones/{contratacionID}?userId={userId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar colaborador temporal");
                return false;
            }
        }

        public async Task<bool> CrearContratacionTemporalAsync(string userId, object request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { userId, contratacion = request }, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/contrataciones/temporal", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear contratación temporal");
                return false;
            }
        }

        public async Task<List<PlanDto>> GetPlanesEmpleadorAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/planes/empleador");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var planes = JsonSerializer.Deserialize<List<PlanDto>>(content, _jsonOptions);
                    return planes ?? new List<PlanDto>();
                }

                _logger.LogWarning("Error al obtener planes: {StatusCode}", response.StatusCode);
                return new List<PlanDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener planes de empleador");
                return new List<PlanDto>();
            }
        }

        public async Task<bool> ProcesarPagoSuscripcionAsync(string userId, object request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { userId, pago = request }, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/suscripciones/procesar-pago", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar pago de suscripción");
                return false;
            }
        }

        public async Task<EmpleadorController.SuscripcionInfo?> GetMiSuscripcionAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/suscripciones/mi-suscripcion?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var suscripcion = JsonSerializer.Deserialize<EmpleadorController.SuscripcionInfo>(content, _jsonOptions);
                    return suscripcion;
                }

                _logger.LogWarning("No se encontró suscripción para usuario {UserId}", userId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener suscripción del usuario");
                return null;
            }
        }

        public async Task<List<EmpleadorController.VentaInfo>> GetHistorialVentasAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/suscripciones/historial-ventas?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ventas = JsonSerializer.Deserialize<List<EmpleadorController.VentaInfo>>(content, _jsonOptions);
                    return ventas ?? new List<EmpleadorController.VentaInfo>();
                }

                _logger.LogWarning("No se encontró historial de ventas para usuario {UserId}", userId);
                return new List<EmpleadorController.VentaInfo>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial de ventas");
                return new List<EmpleadorController.VentaInfo>();
            }
        }

        public async Task<bool> CancelarSuscripcionAsync(string userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/suscripciones/cancelar?userId={userId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar suscripción");
                return false;
            }
        }

        #region Contratista Methods

        public async Task<dynamic?> GetPerfilContratistaAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/contratistas/perfil?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var perfil = JsonSerializer.Deserialize<dynamic>(content, _jsonOptions);
                    return perfil;
                }

                _logger.LogWarning("No se encontró perfil de contratista para usuario {UserId}", userId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfil de contratista");
                return null;
            }
        }

        public async Task<ApiResponse<object>> ActualizarPerfilContratistaAsync(string userId, object request)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { userId, perfil = request }, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("/api/contratistas/perfil", content);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object> { Success = true };
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error al actualizar perfil: {errorContent}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar perfil de contratista");
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error al actualizar el perfil"
                };
            }
        }

        public async Task<ApiResponse<object>> UploadAvatarAsync(string userId, IFormFile avatarFile)
        {
            try
            {
                using var formData = new MultipartFormDataContent();
                using var fileStream = avatarFile.OpenReadStream();
                using var fileContent = new StreamContent(fileStream);

                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(avatarFile.ContentType);
                formData.Add(fileContent, "avatarFile", avatarFile.FileName);
                formData.Add(new StringContent(userId), "userId");

                var response = await _httpClient.PostAsync("/api/contratistas/avatar", formData);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<dynamic>(content, _jsonOptions);
                    return new ApiResponse<object>
                    {
                        Success = true,
                        Data = result
                    };
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error al subir avatar: {errorContent}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir avatar de contratista");
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error al subir la imagen"
                };
            }
        }

        public async Task<List<ServicioContratistaDto>?> GetServiciosContratistaAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/contratistas/servicios?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var servicios = JsonSerializer.Deserialize<List<ServicioContratistaDto>>(content, _jsonOptions);
                    return servicios ?? new List<ServicioContratistaDto>();
                }

                _logger.LogWarning("No se encontraron servicios para contratista {UserId}", userId);
                return new List<ServicioContratistaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener servicios del contratista");
                return new List<ServicioContratistaDto>();
            }
        }

        public async Task<ApiResponse<object>> AddServicioAsync(string userId, string detalleServicio)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { userId, detalleServicio }, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/contratistas/servicios", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<dynamic>(responseContent, _jsonOptions);
                    return new ApiResponse<object>
                    {
                        Success = true,
                        Data = result
                    };
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error al agregar servicio: {errorContent}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar servicio");
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error al agregar el servicio"
                };
            }
        }

        public async Task<ApiResponse<object>> DeleteServicioAsync(int servicioID)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/contratistas/servicios/{servicioID}");

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object> { Success = true };
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error al eliminar servicio: {errorContent}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar servicio");
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error al eliminar el servicio"
                };
            }
        }

        public async Task<List<SectorDto>?> GetSectoresAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/catalogos/sectores");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var sectores = JsonSerializer.Deserialize<List<SectorDto>>(content, _jsonOptions);
                    return sectores ?? new List<SectorDto>();
                }

                _logger.LogWarning("No se encontraron sectores");
                return new List<SectorDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener sectores");
                return new List<SectorDto>();
            }
        }

        public async Task<List<ProvinciaDto>?> GetProvinciasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/catalogos/provincias");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var provincias = JsonSerializer.Deserialize<List<ProvinciaDto>>(content, _jsonOptions);
                    return provincias ?? new List<ProvinciaDto>();
                }

                _logger.LogWarning("No se encontraron provincias");
                return new List<ProvinciaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener provincias");
                return new List<ProvinciaDto>();
            }
        }

        public async Task<List<CalificacionContratistaDto>?> GetCalificacionesContratistaAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/calificaciones/contratista?userId={userId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var calificaciones = JsonSerializer.Deserialize<List<CalificacionContratistaDto>>(content, _jsonOptions);
                    return calificaciones ?? new List<CalificacionContratistaDto>();
                }

                _logger.LogWarning("No se encontraron calificaciones para contratista {UserId}", userId);
                return new List<CalificacionContratistaDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener calificaciones del contratista");
                return new List<CalificacionContratistaDto>();
            }
        }

        public async Task<List<PlanDto>?> GetPlanesContratistaAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/planes/contratista");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var planes = JsonSerializer.Deserialize<List<PlanDto>>(content, _jsonOptions);
                    return planes ?? new List<PlanDto>();
                }

                _logger.LogWarning("No se encontraron planes para contratistas");
                return new List<PlanDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener planes de contratista");
                return new List<PlanDto>();
            }
        }

        public async Task<ApiResponse<object>> ToggleProfileStatusAsync(string userId, bool activate)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { userId, activate }, _jsonOptions);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/contratistas/toggle-status", content);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object> { Success = true };
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error al cambiar estado del perfil: {errorContent}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado del perfil");
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error al cambiar el estado del perfil"
                };
            }
        }

        #endregion

        #region Comunidad Methods

        public async Task<List<dynamic>?> GetUltimosContratistasAsync(int cantidad = 20)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/comunidad/ultimos?cantidad={cantidad}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var contratistas = JsonSerializer.Deserialize<List<dynamic>>(content, _jsonOptions);
                    return contratistas;
                }

                _logger.LogWarning("No se encontraron contratistas");
                return new List<dynamic>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener últimos contratistas");
                return new List<dynamic>();
            }
        }

        public async Task<List<dynamic>?> BuscarContratistasAsync(string? criterio, string? ubicacion)
        {
            try
            {
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(criterio))
                    queryParams.Add($"criterio={Uri.EscapeDataString(criterio)}");
                if (!string.IsNullOrEmpty(ubicacion))
                    queryParams.Add($"ubicacion={Uri.EscapeDataString(ubicacion)}");

                var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
                var response = await _httpClient.GetAsync($"/api/comunidad/buscar{query}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var contratistas = JsonSerializer.Deserialize<List<dynamic>>(content, _jsonOptions);
                    return contratistas;
                }

                _logger.LogWarning("No se encontraron contratistas con los criterios proporcionados");
                return new List<dynamic>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar contratistas");
                return new List<dynamic>();
            }
        }

        public async Task<dynamic?> GetPerfilContratistaPublicoAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/comunidad/perfil/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var perfil = JsonSerializer.Deserialize<dynamic>(content, _jsonOptions);
                    return perfil;
                }

                _logger.LogWarning("No se encontró el perfil del contratista {UserId}", userId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfil público del contratista {UserId}", userId);
                return null;
            }
        }

        #endregion
    }

    // DTOs
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UserInfo User { get; set; } = new();
    }

    public class UserInfo
    {
        public int UserId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // "1" = Empleador, "2" = Contratista
        public int PlanId { get; set; }
        public DateTime? VencimientoPlan { get; set; }
    }

    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
    }

    public class ValidateEmailResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class RegisterRequest
    {
        public int TipoCuenta { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono1 { get; set; } = string.Empty;
        public string? Telefono2 { get; set; }
    }

    public class EmpleadoDto
    {
        public int EmpleadoID { get; set; }
        public DateTime? FechaInicio { get; set; }
        public string? Identificacion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public decimal? Salario { get; set; }
        public int? DiasPago { get; set; }
        public string? Foto { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string? Direccion { get; set; }
        public string? Provincia { get; set; }
        public string? Municipio { get; set; }
        public string? ContactoEmergencia { get; set; }
        public string? TelefonoEmergencia { get; set; }
        public string? Telefono1 { get; set; }
        public string? Telefono2 { get; set; }
        public string? PeriodoPago { get; set; }
        public bool TSS { get; set; }
    }

    public class ContratacionTemporalDto
    {
        public int ContratacionID { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string? Identificacion { get; set; }
        public string? RNC { get; set; }
        public string? NombreComercial { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Telefono1 { get; set; }
        public string? Telefono2 { get; set; }
        public string? Foto { get; set; }
    }

    public class PagoEmpleadoDto
    {
        public int PagoID { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaPago { get; set; }
        public string Tipo { get; set; } = string.Empty; // "Salario", "Regalia"
        public decimal Total { get; set; }
    }

    public class ReciboResponseDto
    {
        public int ReciboID { get; set; }
        public string UrlPdf { get; set; } = string.Empty;
    }

    public class PerfilEmpleadorDto
    {
        public int CuentaID { get; set; }
        public int TipoIdentificacion { get; set; } // 1=Persona Física, 2=Empresa
        public string Identificacion { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono1 { get; set; }
        public string? Telefono2 { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string? NombreComercial { get; set; }
        public string? NombreGerente { get; set; }
        public string? ApellidoGerente { get; set; }
        public string? DireccionGerente { get; set; }
    }

    public class CredencialDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class CalificacionDto
    {
        public int CalificacionID { get; set; }
        public DateTime Fecha { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int Puntualidad { get; set; }
        public int Cumplimiento { get; set; }
        public int Conocimientos { get; set; }
        public int Recomendacion { get; set; }
    }

    public class PerfilParaCalificarDto
    {
        public string Identificacion { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int Tipo { get; set; } // 1=Persona Física, 2=Empresa
    }

    public class PerfilConsultaDto
    {
        public string Identificacion { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public decimal Puntualidad { get; set; }
        public decimal Cumplimiento { get; set; }
        public decimal Conocimientos { get; set; }
        public decimal Recomendacion { get; set; }
        public int TotalCalificaciones { get; set; }
    }

    public class DetalleContratacionDto
    {
        public int ContratacionID { get; set; }
        public int DetalleID { get; set; }
        public string NombreContratista { get; set; } = string.Empty;
        public string DescripcionCorta { get; set; } = string.Empty;
        public string DescripcionAmpliada { get; set; } = string.Empty;
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaConclusion { get; set; }
        public decimal MontoAcordado { get; set; }
        public string EsquemaPagos { get; set; } = string.Empty;
        public int Estatus { get; set; }
        public decimal PagosRealizados { get; set; }
        public decimal MontoPendiente { get; set; }
        public List<PagoContratacionDto> Pagos { get; set; } = new();
        public bool Calificado { get; set; }
        public int Puntualidad { get; set; }
        public int Cumplimiento { get; set; }
        public int Conocimientos { get; set; }
        public int Recomendacion { get; set; }
    }

    public class PagoContratacionDto
    {
        public int PagoID { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string ConceptoPago { get; set; } = string.Empty;
    }

    public class PagoContratacionResponseDto
    {
        public int PagoID { get; set; }
        public string UrlPdf { get; set; } = string.Empty;
    }

    public class TrabajoContratacionDto
    {
        public int DetalleID { get; set; }
        public int ContratacionID { get; set; }
        public string DescripcionCorta { get; set; } = string.Empty;
        public string DescripcionAmpliada { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public decimal MontoAcordado { get; set; }
        public string EsquemaPagos { get; set; } = string.Empty;
        public int Estatus { get; set; }
    }

    public class PlanDto
    {
        public int PlanID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int MaxEmpleados { get; set; }
        public int MaxUsuarios { get; set; }
        public string Caracteristicas { get; set; } = string.Empty; // Separadas por |
        public int Duracion { get; set; } // En meses
    }

    // Contratista DTOs
    public class ServicioContratistaDto
    {
        public int ServicioID { get; set; }
        public string DetalleServicio { get; set; } = string.Empty;
        public int ContratistaID { get; set; }
    }

    public class SectorDto
    {
        public int SectorID { get; set; }
        public string Sector { get; set; } = string.Empty;
    }

    public class ProvinciaDto
    {
        public int ProvinciaID { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class CalificacionContratistaDto
    {
        public int CalificacionID { get; set; }
        public DateTime Fecha { get; set; }
        public string? NombreCalificador { get; set; }
        public int Puntualidad { get; set; }
        public int Conocimientos { get; set; }
        public int Cumplimiento { get; set; }
        public int Recomendacion { get; set; }
    }
}

