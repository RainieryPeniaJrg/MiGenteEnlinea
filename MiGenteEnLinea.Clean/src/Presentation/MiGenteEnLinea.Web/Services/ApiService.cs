using System.Text;
using System.Text.Json;

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
}
