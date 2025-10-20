# 🎨 PLAN 6: FRONTEND MIGRATION - Web Forms → ASP.NET Core MVC

**Fecha de Creación:** 2025-01-18  
**Fecha de Revisión:** 2025-10-20  
**Estado:** 🚀 **LISTO PARA EJECUTAR**  
**Estrategia:** ✅ **MIGRACIÓN (NO reimplementación)**  
**Objetivo:** Migrar TODAS las vistas Legacy (ASP.NET Web Forms) a ASP.NET Core MVC manteniendo diseño y lógica EXACTOS

---

## 🎯 PRINCIPIOS FUNDAMENTALES DE MIGRACIÓN

### ⚠️ REGLAS CRÍTICAS

1. **MISMO DISEÑO** - Copiar HTML/CSS exacto del Legacy (pixel-perfect)
2. **MISMA LÓGICA** - Preservar comportamiento 100% idéntico
3. **MISMA SOLUCIÓN** - Agregar proyecto MVC a `MiGenteEnLinea.Clean.sln`
4. **MISMO DEPLOYMENT** - Un solo `dotnet publish`
5. **MISMA TECNOLOGÍA** - ASP.NET actualizado (no React/Angular/Vue)
6. **CONSUMIR API LIMPIA** - Controllers → MediatR (no código duplicado)

### 🚫 NO HACER

- ❌ Reimplementar en React/Angular/Vue
- ❌ Crear solución separada
- ❌ Cambiar diseño visual
- ❌ "Mejorar" la UX sin aprobación
- ❌ Duplicar lógica de negocio en controllers
- ❌ Deployment separado

---

## 📊 ANÁLISIS DEL LEGACY

### Tecnología Actual (A Migrar)

| Componente | Legacy (.NET Framework 4.7.2) | Migrado (.NET 8.0) |
|------------|-------------------------------|---------------------|
| **Framework** | ASP.NET Web Forms | ASP.NET Core MVC 8.0 |
| **Vistas** | `.aspx` + code-behind | `.cshtml` (Razor Pages) |
| **Master Pages** | `.Master` files | `_Layout.cshtml` |
| **UI Components** | DevExpress v23.1 (Web Forms) | DevExpress v23.2 (MVC) |
| **CSS Framework** | Bootstrap 4.x | Bootstrap 5.3 |
| **JavaScript** | jQuery 3.x | jQuery 3.7 (mantener) |
| **Autenticación** | FormsAuthentication + Cookies | ASP.NET Core Identity + JWT |
| **Data Access** | Controllers → Services → EF6 | Controllers → MediatR → EF Core |
| **Deployment** | IIS + Web.config | Kestrel + appsettings.json |

### Páginas Identificadas (18 Total)

#### Módulo Público (5 páginas)

| Legacy (.aspx) | Migrado (MVC) | Prioridad |
|----------------|---------------|-----------|
| `Login.aspx` | `Account/Login.cshtml` | 🔴 CRÍTICA |
| `Registrar.aspx` | `Account/Register.cshtml` | 🔴 CRÍTICA |
| `Activar.aspx` | `Account/Activate.cshtml` | 🔴 CRÍTICA |
| `Dashboard.aspx` | `Home/Index.cshtml` | 🟡 MEDIA |
| `FAQ.aspx` | `Home/Faq.cshtml` | 🟢 BAJA |

#### Módulo Empleador (9 páginas)

| Legacy (.aspx) | Migrado (MVC) | API Consumida | Prioridad |
|----------------|---------------|---------------|-----------|
| `comunidad.aspx` | `Empleador/Dashboard.cshtml` | `/api/dashboard/empleador` | 🔴 CRÍTICA |
| `colaboradores.aspx` | `Empleador/Empleados/Index.cshtml` | `/api/empleados` | 🟠 ALTA |
| `fichaEmpleado.aspx` | `Empleador/Empleados/Details.cshtml` | `/api/empleados/{id}` | 🟠 ALTA |
| `nomina.aspx` | `Empleador/Nominas/Index.cshtml` | `/api/nominas` | 🟠 ALTA |
| `MiPerfilEmpleador.aspx` | `Empleador/Perfil.cshtml` | `/api/empleadores/{id}` | 🟡 MEDIA |
| `AdquirirPlanEmpleador.aspx` | `Empleador/Suscripciones/Planes.cshtml` | `/api/planes` | 🟡 MEDIA |
| `Checkout.aspx` | `Empleador/Suscripciones/Checkout.cshtml` | `/api/pagos` | 🟡 MEDIA |
| `CalificacionDePerfiles.aspx` | `Empleador/Calificaciones.cshtml` | `/api/calificaciones` | 🟢 BAJA |
| `detalleContratacion.aspx` | `Empleador/Contrataciones/Details.cshtml` | `/api/contrataciones/{id}` | 🟢 BAJA |

#### Módulo Contratista (4 páginas)

| Legacy (.aspx) | Migrado (MVC) | API Consumida | Prioridad |
|----------------|---------------|---------------|-----------|
| `index_contratista.aspx` | `Contratista/Dashboard.cshtml` | `/api/dashboard/contratista` | 🔴 CRÍTICA |
| `MisCalificaciones.aspx` | `Contratista/Calificaciones.cshtml` | `/api/calificaciones` | 🟡 MEDIA |
| `AdquirirPlanContratista.aspx` | `Contratista/Suscripciones/Planes.cshtml` | `/api/planes` | 🟡 MEDIA |
| *(Perfil)* | `Contratista/Perfil.cshtml` | `/api/contratistas/{id}` | 🟡 MEDIA |

---

## 🏗️ ARQUITECTURA DE LA MIGRACIÓN

### Estructura de la Solución (Actualizada)

```
MiGenteEnLinea.Clean.sln                    # ← Solución existente
│
├── src/
│   ├── Core/
│   │   ├── MiGenteEnLinea.Domain/          # ✅ YA EXISTE (100%)
│   │   └── MiGenteEnLinea.Application/     # ✅ YA EXISTE (100%)
│   │
│   ├── Infrastructure/
│   │   └── MiGenteEnLinea.Infrastructure/  # ✅ YA EXISTE (100%)
│   │
│   └── Presentation/
│       ├── MiGenteEnLinea.API/             # ✅ YA EXISTE (REST API)
│       │   ├── Controllers/                # REST endpoints
│       │   ├── Program.cs
│       │   └── appsettings.json
│       │
│       └── MiGenteEnLinea.Web/             # ← NUEVO PROYECTO MVC
│           ├── Controllers/                # MVC Controllers
│           │   ├── AccountController.cs    # Login, Register, Activate
│           │   ├── HomeController.cs       # Dashboard, FAQ
│           │   ├── EmpleadorController.cs  # Módulo Empleador
│           │   └── ContratistaController.cs# Módulo Contratista
│           │
│           ├── Views/                      # Razor Views (.cshtml)
│           │   ├── Shared/
│           │   │   ├── _Layout.cshtml      # Platform.Master migrado
│           │   │   ├── _LayoutEmpleador.cshtml  # Comunity1.Master
│           │   │   └── _LayoutContratista.cshtml # ContratistaM.Master
│           │   ├── Account/
│           │   │   ├── Login.cshtml        # Login.aspx migrado
│           │   │   ├── Register.cshtml     # Registrar.aspx migrado
│           │   │   └── Activate.cshtml     # Activar.aspx migrado
│           │   ├── Empleador/
│           │   │   ├── Dashboard.cshtml    # comunidad.aspx migrado
│           │   │   ├── Empleados/
│           │   │   │   ├── Index.cshtml    # colaboradores.aspx
│           │   │   │   └── Details.cshtml  # fichaEmpleado.aspx
│           │   │   └── Nominas/
│           │   │       └── Index.cshtml    # nomina.aspx
│           │   └── Contratista/
│           │       └── Dashboard.cshtml    # index_contratista.aspx
│           │
│           ├── wwwroot/                    # Assets estáticos
│           │   ├── css/                    # ← COPIAR desde Legacy
│           │   ├── js/                     # ← COPIAR desde Legacy
│           │   ├── images/                 # ← COPIAR desde Legacy
│           │   └── lib/                    # Bootstrap, jQuery, DevExpress
│           │
│           ├── Models/                     # ViewModels (DTOs para vistas)
│           │   ├── Account/
│           │   │   ├── LoginViewModel.cs
│           │   │   └── RegisterViewModel.cs
│           │   └── Empleador/
│           │       └── DashboardViewModel.cs
│           │
│           ├── Services/                   # Application Services
│           │   └── ViewModelMapper.cs      # DTO → ViewModel
│           │
│           ├── Program.cs                  # Startup
│           ├── appsettings.json            # Config (misma DB que API)
│           └── MiGenteEnLinea.Web.csproj
│
└── tests/
    └── MiGenteEnLinea.Web.Tests/           # ← NUEVO (Tests de vistas)
```

### Flujo de Datos (Sin HTTP Overhead)

```
Usuario → Browser
    ↓
[MiGenteEnLinea.Web]
    ↓
EmpleadorController.Dashboard()
    ↓
_mediator.Send(new GetDashboardEmpleadorQuery { EmpleadorId = 123 })
    ↓
[MiGenteEnLinea.Application]
    ↓
GetDashboardEmpleadorQueryHandler
    ↓
[MiGenteEnLinea.Infrastructure]
    ↓
MiGenteDbContext (EF Core)
    ↓
SQL Server
    ↓
Response DTO
    ↓
ViewModel (mapping)
    ↓
Dashboard.cshtml (Razor View)
    ↓
HTML → Browser
```

**✅ VENTAJAS DE ESTE ENFOQUE:**

- No hay overhead HTTP (llamadas internas)
- Reutilización 100% de Application Layer
- Un solo deployment (`dotnet publish`)
- Misma base de datos, misma configuración
- Performance óptimo (sin serialización JSON extra)

---

## 📦 LOTES DE MIGRACIÓN

### 🔐 LOTE 6.1: Setup + Autenticación (8-10 horas)

**Objetivo:** Crear proyecto MVC + migrar login, register, activate

#### Tareas

**1. Crear Proyecto MVC (1 hora)**

```bash
cd MiGenteEnLinea.Clean/src/Presentation
dotnet new mvc -n MiGenteEnLinea.Web -f net8.0
cd MiGenteEnLinea.Web
dotnet add reference ../../Core/MiGenteEnLinea.Application
dotnet add reference ../../Infrastructure/MiGenteEnLinea.Infrastructure
```

**2. Configurar NuGet Packages (30 min)**

```bash
# DevExpress para .NET 8 (MVC)
dotnet add package DevExpress.AspNetCore.Bootstrap -v 23.2.3

# MediatR (ya incluido vía Application)
# AutoMapper (ya incluido vía Application)

# Adicionales para MVC
dotnet add package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation -v 8.0.0
```

**3. Configurar Program.cs (1 hora)**

```csharp
// Program.cs
using MiGenteEnLinea.Application;
using MiGenteEnLinea.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add MVC + Razor runtime compilation (dev only)
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation(); // Hot reload en desarrollo

// Add Application Layer (MediatR, FluentValidation, AutoMapper)
builder.Services.AddApplicationServices();

// Add Infrastructure Layer (DbContext, Repositories, Services)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add Session (para migración gradual desde cookies)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Authentication
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireEmpleadorRole", policy => 
        policy.RequireRole("Empleador"));
    options.AddPolicy("RequireContratistaRole", policy => 
        policy.RequireRole("Contratista"));
});

// Add DevExpress
builder.Services.AddDevExpressControls();

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// DevExpress resources
app.UseDevExpressControls();

// MVC Routes
app.MapControllerRoute(
    name: "empleador",
    pattern: "Empleador/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "Empleador" });

app.MapControllerRoute(
    name: "contratista",
    pattern: "Contratista/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "Contratista" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

**4. Copiar Assets desde Legacy (30 min)**

```bash
# Desde Codigo Fuente Mi Gente/MiGente_Front/
# Copiar a MiGenteEnLinea.Web/wwwroot/

cp -r assets/css/* wwwroot/css/
cp -r assets/js/* wwwroot/js/
cp -r Images/* wwwroot/images/
```

**5. Migrar Master Pages a Layouts (2 horas)**

**_Layout.cshtml** (Platform.Master migrado)

```html
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - MiGente En Línea</title>
    
    <!-- Bootstrap 5 -->
    <link href="~/lib/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    
    <!-- DevExpress CSS -->
    <link href="~/lib/devextreme/dist/css/dx.light.css" rel="stylesheet" />
    
    <!-- Custom CSS (desde Legacy) -->
    <link href="~/css/site.css" rel="stylesheet" />
    
    @await RenderSectionAsync("Styles", required: false)
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-lg navbar-light bg-light">
            <div class="container">
                <a class="navbar-brand" href="/">
                    <img src="~/images/logo.png" alt="MiGente" height="40" />
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ms-auto">
                        @if (User.Identity?.IsAuthenticated == true)
                        {
                            <li class="nav-item">
                                <span class="nav-link">Hola, @User.Identity.Name</span>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Account" asp-action="Logout">Cerrar Sesión</a>
                            </li>
                        }
                        else
                        {
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Account" asp-action="Login">Iniciar Sesión</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Account" asp-action="Register">Registrarse</a>
                            </li>
                        }
                    </ul>
                </div>
            </div>
        </nav>
    </header>
    
    <main role="main">
        @RenderBody()
    </main>
    
    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2025 - MiGente En Línea - <a asp-controller="Home" asp-action="Privacy">Privacidad</a>
        </div>
    </footer>
    
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/lib/devextreme/dist/js/dx.all.js"></script>
    <script src="~/js/site.js"></script>
    
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

**_LayoutEmpleador.cshtml** (Comunity1.Master migrado)

```html
@{
    Layout = "_Layout";
}

<div class="container-fluid">
    <div class="row">
        <!-- Sidebar (desde Comunity1.Master) -->
        <nav class="col-md-2 d-none d-md-block bg-light sidebar">
            <div class="sidebar-sticky">
                <ul class="nav flex-column">
                    <li class="nav-item">
                        <a class="nav-link @(ViewContext.RouteData.Values["Action"]?.ToString() == "Dashboard" ? "active" : "")" 
                           asp-area="Empleador" asp-controller="Dashboard" asp-action="Index">
                            <i class="fas fa-home"></i> Dashboard
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" asp-area="Empleador" asp-controller="Empleados" asp-action="Index">
                            <i class="fas fa-users"></i> Colaboradores
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" asp-area="Empleador" asp-controller="Nominas" asp-action="Index">
                            <i class="fas fa-money-bill-wave"></i> Nómina
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" asp-area="Empleador" asp-controller="Perfil" asp-action="Index">
                            <i class="fas fa-user"></i> Mi Perfil
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" asp-area="Empleador" asp-controller="Suscripciones" asp-action="Index">
                            <i class="fas fa-credit-card"></i> Mi Suscripción
                        </a>
                    </li>
                </ul>
            </div>
        </nav>

        <!-- Main content -->
        <main role="main" class="col-md-10 ml-sm-auto px-4">
            @RenderBody()
        </main>
    </div>
</div>

@section Scripts {
    @RenderSection("Scripts", required: false)
}
```

**6. Migrar Login.aspx → AccountController.cs + Login.cshtml (2 horas)**

**AccountController.cs**

```csharp
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using MiGenteEnLinea.Application.Features.Authentication.Commands.Login;
using MiGenteEnLinea.Web.Models.Account;

namespace MiGenteEnLinea.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IMediator mediator, ILogger<AccountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // LÓGICA EXACTA DEL LEGACY LoginService.login()
                var command = new LoginCommand
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var result = await _mediator.Send(command);

                if (result.Codigo == 2) // Success (mismo código que Legacy)
                {
                    // Crear claims (misma info que cookies del Legacy)
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                        new Claim(ClaimTypes.Email, result.Email),
                        new Claim(ClaimTypes.Name, result.Nombre),
                        new Claim(ClaimTypes.Role, result.Tipo), // "Empleador" o "Contratista"
                        new Claim("PlanID", result.PlanID.ToString()),
                        new Claim("VencimientoPlan", result.VencimientoPlan?.ToString("yyyy-MM-dd") ?? "")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                    };

                    await HttpContext.SignInAsync("CookieAuth", 
                        new ClaimsPrincipal(claimsIdentity), 
                        authProperties);

                    _logger.LogInformation("Usuario {Email} inició sesión exitosamente", model.Email);

                    // Redirigir según tipo (MISMA LÓGICA QUE LEGACY)
                    if (result.Tipo == "1") // Empleador
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Empleador" });
                    }
                    else if (result.Tipo == "2") // Contratista
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Contratista" });
                    }

                    return Redirect(returnUrl ?? "/");
                }
                else if (result.Codigo == 0) // Credenciales inválidas
                {
                    ModelState.AddModelError(string.Empty, "Correo electrónico o contraseña incorrectos");
                }
                else if (result.Codigo == -1) // Usuario inactivo
                {
                    ModelState.AddModelError(string.Empty, "Usuario inactivo. Revise su correo para activar su cuenta.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al iniciar sesión. Intente nuevamente.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en login para usuario {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "Error del servidor. Intente más tarde.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            _logger.LogInformation("Usuario cerró sesión");
            return RedirectToAction("Login");
        }
    }
}
```

**Models/Account/LoginViewModel.cs**

```csharp
using System.ComponentModel.DataAnnotations;

namespace MiGenteEnLinea.Web.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }
}
```

**Views/Account/Login.cshtml** (copiar HTML EXACTO de Login.aspx)

```html
@model MiGenteEnLinea.Web.Models.Account.LoginViewModel
@{
    ViewData["Title"] = "Iniciar Sesión";
    Layout = "_Layout";
}

<!-- COPIAR HTML EXACTO DE Login.aspx aquí -->
<!-- Mantener clases CSS, estructura, imágenes, todo igual -->

<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card shadow">
                <div class="card-body">
                    <h2 class="card-title text-center mb-4">Iniciar Sesión</h2>
                    
                    @if (!string.IsNullOrEmpty(ViewBag.Message))
                    {
                        <div class="alert alert-info">@ViewBag.Message</div>
                    }
                    
                    <form asp-controller="Account" asp-action="Login" method="post">
                        <div asp-validation-summary="All" class="text-danger"></div>
                        
                        <div class="form-group mb-3">
                            <label asp-for="Email"></label>
                            <input asp-for="Email" class="form-control" placeholder="correo@ejemplo.com" />
                            <span asp-validation-for="Email" class="text-danger"></span>
                        </div>
                        
                        <div class="form-group mb-3">
                            <label asp-for="Password"></label>
                            <input asp-for="Password" class="form-control" placeholder="********" />
                            <span asp-validation-for="Password" class="text-danger"></span>
                        </div>
                        
                        <div class="form-check mb-3">
                            <input asp-for="RememberMe" class="form-check-input" />
                            <label asp-for="RememberMe" class="form-check-label"></label>
                        </div>
                        
                        <button type="submit" class="btn btn-primary w-100">Iniciar Sesión</button>
                    </form>
                    
                    <div class="text-center mt-3">
                        <a asp-action="ForgotPassword">¿Olvidaste tu contraseña?</a>
                    </div>
                    <div class="text-center mt-2">
                        <span>¿No tienes cuenta?</span>
                        <a asp-action="Register">Regístrate aquí</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

**7. Migrar Register.aspx → Register.cshtml (2 horas)**

- Copiar HTML exacto
- Controller action con MediatR → RegisterCommand
- ViewModels con DataAnnotations
- Validación cliente + servidor (misma que Legacy)

---

### 📊 LOTE 6.2: Dashboard Empleador (10-12 horas)

**Objetivo:** Migrar `comunidad.aspx` → `Empleador/Dashboard/Index.cshtml`

#### Tareas

**1. Crear Area Empleador**

```bash
mkdir -p Areas/Empleador/Controllers
mkdir -p Areas/Empleador/Views/Dashboard
```

**2. DashboardController.cs**

```csharp
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiGenteEnLinea.Application.Features.Dashboard.Queries.GetDashboardEmpleador;
using MiGenteEnLinea.Web.Models.Empleador;

namespace MiGenteEnLinea.Web.Areas.Empleador.Controllers
{
    [Area("Empleador")]
    [Authorize(Policy = "RequireEmpleadorRole")]
    public class DashboardController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IMediator mediator, ILogger<DashboardController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<IActionResult> Index(DateTime? fechaReferencia = null)
        {
            try
            {
                // Obtener ID del empleador desde claims
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return RedirectToAction("Login", "Account", new { area = "" });
                }

                // TODO: Obtener EmpleadorId desde UserId (agregar query si no existe)
                // Por ahora asumimos que userId == empleadorId
                var empleadorId = userId;

                var query = new GetDashboardEmpleadorQuery
                {
                    EmpleadorId = empleadorId,
                    FechaReferencia = fechaReferencia ?? DateTime.Today
                };

                var dashboard = await _mediator.Send(query);

                // Mapear a ViewModel (agregar propiedades de UI si es necesario)
                var viewModel = new DashboardEmpleadorViewModel
                {
                    Metricas = dashboard.Metricas,
                    Nomina = dashboard.Nomina,
                    Empleados = dashboard.Empleados,
                    ChartEvolucionNomina = dashboard.ChartEvolucionNomina,
                    ChartDeducciones = dashboard.ChartDeducciones,
                    ChartDistribucionEmpleados = dashboard.ChartDistribucionEmpleados,
                    TopEmpleadosPorSalario = dashboard.TopEmpleadosPorSalario,
                    EmpleadosRecientes = dashboard.EmpleadosRecientes
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar dashboard del empleador");
                TempData["ErrorMessage"] = "Error al cargar el dashboard. Intente nuevamente.";
                return View(new DashboardEmpleadorViewModel());
            }
        }
    }
}
```

**3. Index.cshtml** (copiar HTML de comunidad.aspx)

```html
@model MiGenteEnLinea.Web.Models.Empleador.DashboardEmpleadorViewModel
@{
    ViewData["Title"] = "Dashboard - Empleador";
    Layout = "~/Views/Shared/_LayoutEmpleador.cshtml";
}

<!-- COPIAR HTML EXACTO DE comunidad.aspx -->
<!-- Reemplazar controles DevExpress Web Forms por DevExpress MVC -->

<div class="dashboard-empleador">
    <h1 class="mb-4">Bienvenido, @User.Identity?.Name</h1>
    
    @if (TempData["ErrorMessage"] != null)
    {
        <div class="alert alert-danger">@TempData["ErrorMessage"]</div>
    }
    
    <!-- Métricas Cards -->
    <div class="row mb-4">
        <div class="col-md-3">
            <div class="card bg-primary text-white">
                <div class="card-body">
                    <h5 class="card-title">Total Empleados</h5>
                    <h2>@Model.Metricas.TotalEmpleadosActivos</h2>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card bg-success text-white">
                <div class="card-body">
                    <h5 class="card-title">Nómina Actual</h5>
                    <h2>$@Model.Metricas.NominaMesActual.ToString("N2")</h2>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card bg-info text-white">
                <div class="card-body">
                    <h5 class="card-title">Promedio Salario</h5>
                    <h2>$@Model.Metricas.PromedioSalario.ToString("N2")</h2>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card bg-warning text-white">
                <div class="card-body">
                    <h5 class="card-title">Deducciones Total</h5>
                    <h2>$@Model.Metricas.DeduccionesTotales.ToString("N2")</h2>
                </div>
            </div>
        </div>
    </div>
    
    <!-- Charts (DevExpress Charts para MVC) -->
    <div class="row mb-4">
        <div class="col-md-6">
            <div class="card">
                <div class="card-header">
                    <h5>Evolución Nómina (Últimos 6 Meses)</h5>
                </div>
                <div class="card-body">
                    @* DevExpress Chart - Línea *@
                    @Html.DevExpress().Chart(settings => {
                        settings.Name = "chartEvolucionNomina";
                        settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        settings.Height = 300;
                        
                        settings.DataSource = Model.ChartEvolucionNomina;
                        
                        settings.SeriesDataMember = "Mes";
                        settings.ArgumentField = "Mes";
                        
                        settings.Series.Add(series => {
                            series.ValueField = "Total";
                            series.SeriesType = DevExpress.XtraCharts.SeriesType.Line;
                        });
                    }).GetHtml()
                </div>
            </div>
        </div>
        
        <div class="col-md-6">
            <div class="card">
                <div class="card-header">
                    <h5>Distribución Deducciones</h5>
                </div>
                <div class="card-body">
                    @* DevExpress Chart - Barras *@
                    @Html.DevExpress().Chart(settings => {
                        settings.Name = "chartDeducciones";
                        settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        settings.Height = 300;
                        
                        settings.DataSource = Model.ChartDeducciones;
                        
                        settings.SeriesTemplate.ArgumentDataMember = "Tipo";
                        settings.SeriesTemplate.ValueDataMembersSerializable = "Monto";
                        settings.SeriesTemplate.SeriesDataMember = "Mes";
                        
                        settings.SeriesTemplate.View = new DevExpress.XtraCharts.Web.StackedBarSeriesView();
                    }).GetHtml()
                </div>
            </div>
        </div>
    </div>
    
    <!-- Tabla Empleados Recientes (DevExpress GridView) -->
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                    <h5>Empleados Recientes</h5>
                </div>
                <div class="card-body">
                    @Html.DevExpress().GridView(settings => {
                        settings.Name = "gridEmpleadosRecientes";
                        settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        
                        settings.DataSource = Model.EmpleadosRecientes;
                        
                        settings.Columns.Add("NombreCompleto", "Nombre");
                        settings.Columns.Add("Cargo", "Cargo");
                        settings.Columns.Add(column => {
                            column.FieldName = "Salario";
                            column.Caption = "Salario";
                            column.PropertiesEdit.DisplayFormatString = "c2";
                        });
                        settings.Columns.Add(column => {
                            column.FieldName = "FechaIngreso";
                            column.Caption = "Fecha Ingreso";
                            column.PropertiesEdit.DisplayFormatString = "dd/MM/yyyy";
                        });
                        
                        settings.Settings.ShowFooter = true;
                        settings.Settings.ShowFilterRow = true;
                        settings.SettingsPager.PageSize = 10;
                    }).GetHtml()
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <script>
        // JavaScript desde comunidad.aspx (si existe)
    </script>
}
```

---

### 👥 LOTE 6.3: Gestión de Empleados (12-15 horas)

**Migrar:**

- `colaboradores.aspx` → `Empleador/Empleados/Index.cshtml`
- `fichaEmpleado.aspx` → `Empleador/Empleados/Details.cshtml` + `Edit.cshtml`

**Controllers:** EmpleadosController con acciones Index, Details, Create, Edit, Delete  
**Lógica:** Todos los métodos llaman a MediatR commands/queries  
**DevExpress:** ASPxGridView Web Forms → DevExpress.Mvc.GridView

---

### 💰 LOTE 6.4: Gestión de Nómina (12-15 horas)

**Migrar:**

- `nomina.aspx` → `Empleador/Nominas/Index.cshtml`
- Procesar nómina, generar PDFs, enviar emails, exportar CSV

**Componentes DevExpress:**

- GridView para recibos
- Button con callbacks AJAX
- Export to CSV directo desde GridView

---

### 📦 LOTE 6.5: Módulo Contratista (8-10 horas)

**Migrar:**

- `index_contratista.aspx` → `Contratista/Dashboard/Index.cshtml`
- `MisCalificaciones.aspx` → `Contratista/Calificaciones/Index.cshtml`

---

### 🔄 LOTE 6.6: Suscripciones y Checkout (8-10 horas)

**Migrar:**

- `AdquirirPlanEmpleador.aspx` → `Empleador/Suscripciones/Planes.cshtml`
- `Checkout.aspx` → `Empleador/Suscripciones/Checkout.cshtml`
- Integración Cardnet (mantener lógica exacta)

---

### 🧪 LOTE 6.7: Testing y Deployment (6-8 horas)

1. Unit tests para Controllers
2. Integration tests (UI + API)
3. Manual testing completo (cada página vs Legacy)
4. Configurar CI/CD para deployment conjunto
5. Migración gradual (proxy URLs Legacy → MVC hasta 100%)

---

## 🚀 ESTRATEGIA DE DEPLOYMENT

### Opción A: Proxy Gradual (RECOMENDADO)

```
Usuario → Browser
    ↓
IIS/Nginx (Reverse Proxy)
    ↓
/Empleador/Dashboard → MiGenteEnLinea.Web (MVC) ✅ MIGRADO
/colaboradores.aspx → Legacy (Web Forms) ⏳ PENDIENTE
    ↓
Mismo SQL Server, misma DB
```

**Ventajas:**

- Migración incremental sin downtime
- Rollback inmediato si hay problemas
- Testing en producción página por página

### Opción B: Big Bang (Deploy todo junto)

```bash
# Build ambos proyectos
dotnet publish MiGenteEnLinea.Web -c Release -o ./publish/web
dotnet publish MiGenteEnLinea.API -c Release -o ./publish/api

# Deploy
# - Web en IIS en /migente-web
# - API en IIS en /migente-api
# - O unificar en mismo host con rutas diferentes
```

---

## ✅ CHECKLIST DE INICIO (AHORA SÍ CORRECTO)

### Setup Inicial (2 horas)

- [ ] Crear proyecto MVC: `dotnet new mvc -n MiGenteEnLinea.Web`
- [ ] Agregar a solución: `dotnet sln add src/Presentation/MiGenteEnLinea.Web`
- [ ] Agregar referencias: Application + Infrastructure
- [ ] Instalar DevExpress.AspNetCore.Bootstrap v23.2
- [ ] Configurar Program.cs (MediatR, Auth, DevExpress)
- [ ] Copiar assets (CSS, JS, Images) desde Legacy
- [ ] Crear _Layout.cshtml base
- ⚠️ Ecosystem más limitado

**Opción C: Next.js + TypeScript (OVERKILL?)**

- ✅ SSR/SSG para SEO
- ✅ React-based
- ✅ Best practices out-of-the-box
- ⚠️ Más complejo de configurar
- ⚠️ Innecesario si no requiere SEO

**RECOMENDACIÓN:** **React + TypeScript + Vite**

**Razones:**

1. Balance perfecto entre performance y DX (Developer Experience)
2. DevExpress React Components (dx-react-grid, dx-react-scheduler)
3. Vite build tool (ultra-rápido)
4. Type safety con TypeScript
5. Ecosystem rico (React Query, Zustand, React Router)

---

## 🏗️ ARQUITECTURA PROPUESTA

### Estructura del Proyecto Frontend

```
migente-frontend/
├── public/
│   ├── assets/
│   │   ├── img/          # Imágenes migradas de Legacy
│   │   ├── icons/        # Iconos
│   │   └── fonts/        # Fuentes personalizadas
│   └── favicon.ico
│
├── src/
│   ├── app/              # Configuración de la aplicación
│   │   ├── App.tsx       # Componente raíz
│   │   ├── Router.tsx    # Configuración de rutas
│   │   └── theme.ts      # Tema global (colores, tipografía)
│   │
│   ├── features/         # Módulos por dominio (Feature-First)
│   │   ├── auth/
│   │   │   ├── components/
│   │   │   │   ├── LoginForm.tsx
│   │   │   │   ├── RegisterForm.tsx
│   │   │   │   └── ActivationPage.tsx
│   │   │   ├── hooks/
│   │   │   │   ├── useAuth.ts
│   │   │   │   └── useLogin.ts
│   │   │   ├── services/
│   │   │   │   └── authService.ts
│   │   │   ├── store/
│   │   │   │   └── authStore.ts
│   │   │   └── types/
│   │   │       └── auth.types.ts
│   │   │
│   │   ├── empleador/
│   │   │   ├── components/
│   │   │   │   ├── Dashboard.tsx
│   │   │   │   ├── ColaboradoresList.tsx
│   │   │   │   ├── FichaEmpleado.tsx
│   │   │   │   ├── NominaGrid.tsx
│   │   │   │   ├── DetalleContratacion.tsx
│   │   │   │   ├── Checkout.tsx
│   │   │   │   └── MiPerfil.tsx
│   │   │   ├── hooks/
│   │   │   ├── services/
│   │   │   └── types/
│   │   │
│   │   ├── contratista/
│   │   │   ├── components/
│   │   │   │   ├── Dashboard.tsx
│   │   │   │   ├── MisCalificaciones.tsx
│   │   │   │   ├── MiPerfil.tsx
│   │   │   │   └── Checkout.tsx
│   │   │   ├── hooks/
│   │   │   ├── services/
│   │   │   └── types/
│   │   │
│   │   ├── common/       # Features compartidos
│   │   │   ├── components/
│   │   │   │   ├── Dashboard/
│   │   │   │   ├── FAQ.tsx
│   │   │   │   └── PublicLayout.tsx
│   │   │   └── hooks/
│   │   │
│   │   └── calificaciones/
│   │       ├── components/
│   │       │   ├── CalificacionForm.tsx
│   │       │   └── CalificacionesList.tsx
│   │       └── hooks/
│   │
│   ├── shared/           # Código compartido entre features
│   │   ├── components/   # Componentes reutilizables
│   │   │   ├── ui/       # UI primitives
│   │   │   │   ├── Button.tsx
│   │   │   │   ├── Input.tsx
│   │   │   │   ├── Card.tsx
│   │   │   │   ├── Modal.tsx
│   │   │   │   ├── Table.tsx
│   │   │   │   └── Spinner.tsx
│   │   │   ├── layout/
│   │   │   │   ├── Header.tsx
│   │   │   │   ├── Sidebar.tsx
│   │   │   │   ├── Footer.tsx
│   │   │   │   └── MainLayout.tsx
│   │   │   └── forms/
│   │   │       ├── FormInput.tsx
│   │   │       ├── FormSelect.tsx
│   │   │       ├── FormDatePicker.tsx
│   │   │       └── FormCheckbox.tsx
│   │   │
│   │   ├── hooks/        # Custom hooks reutilizables
│   │   │   ├── useApi.ts
│   │   │   ├── useDebounce.ts
│   │   │   ├── usePagination.ts
│   │   │   └── useLocalStorage.ts
│   │   │
│   │   ├── utils/        # Utilidades
│   │   │   ├── formatters.ts
│   │   │   ├── validators.ts
│   │   │   ├── dateHelpers.ts
│   │   │   └── currencyHelpers.ts
│   │   │
│   │   ├── services/     # Servicios globales
│   │   │   ├── api.ts    # Axios instance configurado
│   │   │   └── storage.ts
│   │   │
│   │   └── types/        # Types globales
│   │       ├── global.types.ts
│   │       └── api.types.ts
│   │
│   ├── assets/           # Assets locales (no públicos)
│   │   ├── styles/
│   │   │   ├── global.css
│   │   │   ├── variables.css
│   │   │   └── utilities.css
│   │   └── images/       # Imágenes importadas en componentes
│   │
│   ├── main.tsx          # Entry point
│   └── vite-env.d.ts     # Vite types
│
├── .env.development      # Variables de entorno (desarrollo)
├── .env.production       # Variables de entorno (producción)
├── .eslintrc.cjs         # ESLint config
├── .prettierrc           # Prettier config
├── tsconfig.json         # TypeScript config
├── vite.config.ts        # Vite config
└── package.json          # Dependencies
```

---

## 📦 DEPENDENCIAS DEL PROYECTO

### Core Dependencies

```json
{
  "dependencies": {
    "react": "^18.2.0",
    "react-dom": "^18.2.0",
    "react-router-dom": "^6.20.0",
    "typescript": "^5.3.0",
    
    "axios": "^1.6.2",
    "react-query": "^3.39.3",
    "zustand": "^4.4.7",
    
    "react-hook-form": "^7.48.2",
    "zod": "^3.22.4",
    "@hookform/resolvers": "^3.3.2",
    
    "devextreme": "^23.2.3",
    "devextreme-react": "^23.2.3",
    
    "date-fns": "^2.30.0",
    "clsx": "^2.0.0",
    "react-hot-toast": "^2.4.1"
  },
  "devDependencies": {
    "@types/react": "^18.2.43",
    "@types/react-dom": "^18.2.17",
    "@vitejs/plugin-react": "^4.2.1",
    "vite": "^5.0.8",
    
    "eslint": "^8.55.0",
    "prettier": "^3.1.1",
    "tailwindcss": "^3.3.6",
    "autoprefixer": "^10.4.16",
    "postcss": "^8.4.32"
  }
}
```

### Justificación de Dependencias

**React Query:**

- Manejo de estado del servidor (cache, refetch, mutations)
- Sincronización automática con backend
- Optimistic updates

**Zustand:**

- State management global (auth, theme, etc.)
- Más simple que Redux
- TypeScript-first

**React Hook Form + Zod:**

- Validación de formularios type-safe
- Performance (uncontrolled inputs)
- Integración con TypeScript

**DevExpress React:**

- Grids complejos (nómina, colaboradores)
- Schedulers (calendario de contrataciones)
- Charts (dashboard)

**date-fns:**

- Manipulación de fechas
- Formateo localizado (español)
- Tree-shakeable

**Tailwind CSS:**

- Utility-first CSS
- Responsive design fácil
- Customización vía config

---

## 🎯 LOTES DE IMPLEMENTACIÓN

### LOTE 6.1: Setup & Infrastructure 🔴 CRÍTICA

**Prioridad:** 🔴 **CRÍTICA - FUNDACIÓN**  
**Estimación:** 2-3 días (16-24 horas)  
**Estado:** ❌ NO INICIADO

#### Tareas de Implementación

**FASE 1: Proyecto Vite + React + TypeScript (4 horas)**

```bash
# Crear proyecto con Vite
npm create vite@latest migente-frontend -- --template react-ts

# Instalar dependencias
cd migente-frontend
npm install

# Instalar dependencias adicionales
npm install react-router-dom axios react-query zustand
npm install react-hook-form zod @hookform/resolvers
npm install devextreme devextreme-react
npm install date-fns clsx react-hot-toast

# Dev dependencies
npm install -D tailwindcss postcss autoprefixer
npm install -D eslint prettier eslint-config-prettier
npx tailwindcss init -p
```

**FASE 2: Configuración (4 horas)**

1. **Tailwind CSS Setup**

   ```css
   /* src/assets/styles/global.css */
   @tailwind base;
   @tailwind components;
   @tailwind utilities;
   
   /* Variables de colores del Legacy */
   :root {
     --primary: #007bff;    /* Azul principal */
     --secondary: #6c757d;  /* Gris */
     --success: #28a745;    /* Verde */
     --danger: #dc3545;     /* Rojo */
     --warning: #ffc107;    /* Amarillo */
     --info: #17a2b8;       /* Cyan */
   }
   ```

2. **Axios Instance**

   ```typescript
   // src/shared/services/api.ts
   import axios from 'axios';
   
   const api = axios.create({
     baseURL: import.meta.env.VITE_API_URL,
     timeout: 10000,
   });
   
   api.interceptors.request.use((config) => {
     const token = localStorage.getItem('token');
     if (token) {
       config.headers.Authorization = `Bearer ${token}`;
     }
     return config;
   });
   
   api.interceptors.response.use(
     (response) => response,
     (error) => {
       if (error.response?.status === 401) {
         // Redirect to login
         window.location.href = '/login';
       }
       return Promise.reject(error);
     }
   );
   
   export default api;
   ```

3. **React Query Setup**

   ```typescript
   // src/app/App.tsx
   import { QueryClient, QueryClientProvider } from 'react-query';
   
   const queryClient = new QueryClient({
     defaultOptions: {
       queries: {
         retry: 1,
         refetchOnWindowFocus: false,
         staleTime: 5 * 60 * 1000, // 5 minutes
       },
     },
   });
   
   function App() {
     return (
       <QueryClientProvider client={queryClient}>
         <Router />
       </QueryClientProvider>
     );
   }
   ```

4. **Environment Variables**

   ```env
   # .env.development
   VITE_API_URL=http://localhost:5015/api
   VITE_APP_NAME=MiGente En Línea
   
   # .env.production
   VITE_API_URL=https://api.migenteonlinea.com/api
   VITE_APP_NAME=MiGente En Línea
   ```

**FASE 3: UI Components Library (6-8 horas)**

1. **Button Component**

   ```typescript
   // src/shared/components/ui/Button.tsx
   import clsx from 'clsx';
   
   interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
     variant?: 'primary' | 'secondary' | 'success' | 'danger';
     size?: 'sm' | 'md' | 'lg';
     isLoading?: boolean;
   }
   
   export function Button({ 
     variant = 'primary', 
     size = 'md', 
     isLoading,
     children,
     className,
     ...props 
   }: ButtonProps) {
     return (
       <button
         className={clsx(
           'rounded font-medium transition-colors',
           {
             'bg-blue-600 text-white hover:bg-blue-700': variant === 'primary',
             'bg-gray-200 text-gray-800 hover:bg-gray-300': variant === 'secondary',
             'px-3 py-1.5 text-sm': size === 'sm',
             'px-4 py-2': size === 'md',
             'px-6 py-3 text-lg': size === 'lg',
             'opacity-50 cursor-not-allowed': isLoading,
           },
           className
         )}
         disabled={isLoading}
         {...props}
       >
         {isLoading ? 'Cargando...' : children}
       </button>
     );
   }
   ```

2. **Input Component**
3. **Card Component**
4. **Modal Component**
5. **Table Component**
6. **Spinner Component**

**FASE 4: Layout Components (4-6 horas)**

1. **MainLayout (con Header, Sidebar, Footer)**
2. **PublicLayout (sin Sidebar)**
3. **Responsive Sidebar**
4. **User Dropdown Menu**

#### Archivos a Crear (Total: ~40 archivos, ~2,500 líneas)

**Configuración:**

- `vite.config.ts`
- `tsconfig.json`
- `tailwind.config.js`
- `.eslintrc.cjs`
- `.prettierrc`
- `.env.development`
- `.env.production`

**App Setup:**

- `src/main.tsx`
- `src/app/App.tsx`
- `src/app/Router.tsx`
- `src/app/theme.ts`

**Shared Components (UI):**

- `Button.tsx`, `Input.tsx`, `Card.tsx`, `Modal.tsx`, `Table.tsx`, `Spinner.tsx` (6 archivos)

**Shared Components (Forms):**

- `FormInput.tsx`, `FormSelect.tsx`, `FormDatePicker.tsx`, `FormCheckbox.tsx` (4 archivos)

**Shared Components (Layout):**

- `Header.tsx`, `Sidebar.tsx`, `Footer.tsx`, `MainLayout.tsx`, `PublicLayout.tsx` (5 archivos)

**Services:**

- `api.ts`, `storage.ts` (2 archivos)

**Hooks:**

- `useApi.ts`, `useDebounce.ts`, `usePagination.ts`, `useLocalStorage.ts` (4 archivos)

**Utils:**

- `formatters.ts`, `validators.ts`, `dateHelpers.ts`, `currencyHelpers.ts` (4 archivos)

**Types:**

- `global.types.ts`, `api.types.ts` (2 archivos)

**Styles:**

- `global.css`, `variables.css`, `utilities.css` (3 archivos)

#### Métricas de Éxito

- ✅ Vite dev server corre sin errores
- ✅ Build de producción exitoso
- ✅ TypeScript sin errores
- ✅ ESLint sin errores
- ✅ 6 UI components funcionales
- ✅ Layout responsive (mobile, tablet, desktop)
- ✅ Axios interceptors funcionando
- ✅ React Query configurado correctamente

---

### LOTE 6.2: Authentication Module 🔴 CRÍTICA

**Prioridad:** 🔴 **CRÍTICA - BLOQUEANTE**  
**Estimación:** 2-3 días (16-24 horas)  
**Estado:** ❌ NO INICIADO

#### Páginas Legacy a Migrar

1. `/Login.aspx` - Login de usuarios
2. `/Registrar.aspx` - Registro de nuevos usuarios
3. `/Activar.aspx` - Activación de cuenta por email

#### Tareas de Implementación

**FASE 1: Auth Store & Service (4 horas)**

1. **Auth Store (Zustand)**

   ```typescript
   // src/features/auth/store/authStore.ts
   import create from 'zustand';
   import { persist } from 'zustand/middleware';
   
   interface User {
     id: string;
     email: string;
     nombre: string;
     tipo: 'Empleador' | 'Contratista';
     planId?: string;
   }
   
   interface AuthState {
     user: User | null;
     token: string | null;
     isAuthenticated: boolean;
     login: (email: string, password: string) => Promise<void>;
     logout: () => void;
     register: (data: RegisterData) => Promise<void>;
   }
   
   export const useAuthStore = create<AuthState>()(
     persist(
       (set) => ({
         user: null,
         token: null,
         isAuthenticated: false,
         
         login: async (email, password) => {
           const response = await authService.login(email, password);
           set({ 
             user: response.user, 
             token: response.token, 
             isAuthenticated: true 
           });
           localStorage.setItem('token', response.token);
         },
         
         logout: () => {
           set({ user: null, token: null, isAuthenticated: false });
           localStorage.removeItem('token');
         },
         
         register: async (data) => {
           await authService.register(data);
         },
       }),
       { name: 'auth-storage' }
     )
   );
   ```

2. **Auth Service**

   ```typescript
   // src/features/auth/services/authService.ts
   import api from '@/shared/services/api';
   
   export const authService = {
     login: async (email: string, password: string) => {
       const response = await api.post('/auth/login', { email, password });
       return response.data;
     },
     
     register: async (data: RegisterData) => {
       const response = await api.post('/auth/register', data);
       return response.data;
     },
     
     activate: async (userId: string, email: string) => {
       const response = await api.post('/auth/activate', { userId, email });
       return response.data;
     },
   };
   ```

**FASE 2: Login Page (4 horas)**

1. **LoginForm Component**

   ```typescript
   // src/features/auth/components/LoginForm.tsx
   import { useForm } from 'react-hook-form';
   import { zodResolver } from '@hookform/resolvers/zod';
   import { z } from 'zod';
   import { useAuthStore } from '../store/authStore';
   
   const loginSchema = z.object({
     email: z.string().email('Email inválido'),
     password: z.string().min(6, 'Mínimo 6 caracteres'),
   });
   
   type LoginFormData = z.infer<typeof loginSchema>;
   
   export function LoginForm() {
     const { register, handleSubmit, formState: { errors } } = useForm<LoginFormData>({
       resolver: zodResolver(loginSchema),
     });
     
     const login = useAuthStore((state) => state.login);
     const [isLoading, setIsLoading] = useState(false);
     
     const onSubmit = async (data: LoginFormData) => {
       setIsLoading(true);
       try {
         await login(data.email, data.password);
         navigate('/dashboard');
       } catch (error) {
         toast.error('Credenciales inválidas');
       } finally {
         setIsLoading(false);
       }
     };
     
     return (
       <form onSubmit={handleSubmit(onSubmit)}>
         <FormInput
           label="Email"
           {...register('email')}
           error={errors.email?.message}
         />
         <FormInput
           label="Contraseña"
           type="password"
           {...register('password')}
           error={errors.password?.message}
         />
         <Button type="submit" isLoading={isLoading}>
           Iniciar Sesión
         </Button>
       </form>
     );
   }
   ```

2. **LoginPage Container**
   - Layout con imagen de fondo (igual que Legacy)
   - Logo de MiGente
   - Link a registro
   - Link a "Olvidé mi contraseña"

**FASE 3: Register Page (6 horas)**

1. **RegisterForm Component (Multi-step)**
   - Step 1: Tipo de usuario (Empleador / Contratista)
   - Step 2: Datos personales
   - Step 3: Credenciales (email, password)
   - Step 4: Confirmación

2. **Form Validation con Zod**
   - Email único (validar en backend)
   - Password: min 8 chars, uppercase, lowercase, number
   - RNC/Cédula válido (formato dominicano)

**FASE 4: Activation Page (2 horas)**

1. **ActivationPage Component**
   - Leer `userId` y `email` de URL params
   - Llamar API `POST /auth/activate`
   - Mostrar mensaje de éxito/error
   - Redirect a login

**FASE 5: Protected Routes (2 horas)**

1. **ProtectedRoute Component**

   ```typescript
   // src/app/ProtectedRoute.tsx
   import { Navigate } from 'react-router-dom';
   import { useAuthStore } from '@/features/auth/store/authStore';
   
   export function ProtectedRoute({ children }: { children: React.ReactNode }) {
     const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
     
     if (!isAuthenticated) {
       return <Navigate to="/login" replace />;
     }
     
     return <>{children}</>;
   }
   ```

2. **RoleBasedRoute Component**
   - Validar rol de usuario
   - Redirect si no tiene permisos

#### Archivos a Crear (Total: ~20 archivos, ~1,500 líneas)

**Store:**

- `authStore.ts` (~100 líneas)

**Services:**

- `authService.ts` (~80 líneas)

**Hooks:**

- `useAuth.ts`, `useLogin.ts`, `useRegister.ts` (3 archivos, ~150 líneas)

**Components:**

- `LoginForm.tsx` (~150 líneas)
- `LoginPage.tsx` (~80 líneas)
- `RegisterForm.tsx` (~250 líneas) - Multi-step
- `RegisterPage.tsx` (~100 líneas)
- `ActivationPage.tsx` (~80 líneas)

**Utils:**

- `authValidators.ts` (~60 líneas)

**Types:**

- `auth.types.ts` (~50 líneas)

**Routing:**

- `ProtectedRoute.tsx` (~50 líneas)
- `RoleBasedRoute.tsx` (~60 líneas)

**Tests:**

- `LoginForm.test.tsx`, `RegisterForm.test.tsx`, `authStore.test.ts` (3 archivos, ~300 líneas)

#### Métricas de Éxito

- ✅ Login funciona correctamente
- ✅ Registro funciona (multi-step)
- ✅ Email de activación se envía
- ✅ Activación funciona
- ✅ Token JWT se guarda en localStorage
- ✅ Protected routes funcionan
- ✅ Redirect después de login correcto
- ✅ Diseño idéntico al Legacy

---

### LOTE 6.3: Empleador Module 🔴 ALTA

**Prioridad:** 🔴 **ALTA - FUNCIONALIDAD CORE**  
**Estimación:** 5-6 días (40-48 horas)  
**Estado:** ❌ NO INICIADO

#### Páginas Legacy a Migrar (9 páginas)

1. `/comunidad.aspx` - Dashboard del empleador
2. `/Empleador/colaboradores.aspx` - Lista de empleados y contratistas
3. `/Empleador/fichaEmpleado.aspx` - Ficha de empleado (CRUD)
4. `/Empleador/fichaColaboradorTemporal.aspx` - Crear contratación
5. `/Empleador/detalleContratacion.aspx` - Detalle de contratación
6. `/Empleador/nomina.aspx` - Procesar nómina
7. `/Empleador/CalificacionDePerfiles.aspx` - Calificar contratista
8. `/Empleador/MiPerfilEmpleador.aspx` - Perfil del empleador
9. `/Empleador/Checkout.aspx` - Compra de suscripción

#### Tareas de Implementación

**FASE 1: Dashboard (1 día, 4 archivos, ~300 líneas)**

1. **Dashboard Component**
   - Cards con métricas (empleados activos, nómina mensual, contrataciones)
   - Gráficos (Chart.js o Recharts)
   - Tabla de contrataciones recientes
   - Notificaciones (calificaciones pendientes, suscripción por vencer)

2. **useEmpleadorDashboard Hook**

   ```typescript
   export function useEmpleadorDashboard(empleadorId: string) {
     return useQuery(['empleador-dashboard', empleadorId], () =>
       api.get(`/dashboard/empleador/${empleadorId}`).then(res => res.data)
     );
   }
   ```

**FASE 2: Colaboradores (Lista de Empleados) (1 día, 6 archivos, ~500 líneas)**

1. **ColaboradoresList Component**
   - DevExpress Grid (igual que Legacy)
   - Filtros (nombre, estado, tipo)
   - Búsqueda
   - Acciones: Ver, Editar, Desactivar
   - Paginación

2. **ColaboradoresPage Container**

**FASE 3: Ficha de Empleado (CRUD) (1.5 días, 8 archivos, ~700 líneas)**

1. **FichaEmpleadoForm Component**
   - Formulario multi-sección (Datos personales, Salario, Deducciones)
   - Validación con Zod
   - Integration con API `/api/empleados`

2. **CreateEmpleadoPage**
3. **EditEmpleadoPage**

**FASE 4: Contrataciones (2 días, 10 archivos, ~900 líneas)**

1. **FichaColaboradorTemporalForm** - Crear contratación
2. **DetalleContratacionPage** - Ver y gestionar contratación
3. **ChangeStatusModal** - Cambiar estado de contratación

**FASE 5: Nómina (1 día, 6 archivos, ~600 líneas)**

1. **NominaGrid Component** (DevExpress Grid)
2. **ProcessNominaModal** - Procesar nómina en lote
3. **useProcessNomina Hook**

**FASE 6: Calificación de Perfiles (4 horas, 3 archivos, ~250 líneas)**

1. **CalificacionForm Component** (Rating 1-5 estrellas + comentario)
2. **CalificacionModal**

**FASE 7: Perfil & Checkout (1 día, 6 archivos, ~500 líneas)**

1. **MiPerfilEmpleadorPage** - Editar perfil
2. **CheckoutPage** - Compra de suscripción (Cardnet payment)

#### Archivos a Crear (Total: ~45 archivos, ~3,750 líneas)

**Pages:**

- Dashboard, Colaboradores, FichaEmpleado, FichaColaboradorTemporal, DetalleContratacion, Nomina, CalificacionPerfiles, MiPerfil, Checkout (9 archivos)

**Components:**

- ~30 componentes (grids, forms, modals)

**Hooks:**

- ~15 hooks (useEmpleados, useContrataciones, useNomina, etc.)

**Services:**

- empleadosService.ts, contratacionesService.ts, nominaService.ts (3 archivos)

**Types:**

- empleador.types.ts, empleado.types.ts, contratacion.types.ts (3 archivos)

#### Métricas de Éxito

- ✅ Dashboard carga métricas correctamente
- ✅ CRUD de empleados funciona
- ✅ Crear contratación funciona
- ✅ Cambiar estado de contratación funciona
- ✅ Procesar nómina funciona (individual y lote)
- ✅ Calificar contratista funciona
- ✅ Checkout funciona con Cardnet
- ✅ Diseño idéntico al Legacy (DevExpress grids)

---

### LOTE 6.4: Contratista Module 🔴 ALTA

**Prioridad:** 🔴 **ALTA**  
**Estimación:** 2-3 días (16-24 horas)  
**Estado:** ❌ NO INICIADO

#### Páginas Legacy a Migrar (4 páginas)

1. `/Contratista/index_contratista.aspx` - Dashboard del contratista
2. `/Contratista/MisCalificaciones.aspx` - Ver calificaciones recibidas
3. `/Contratista/MiPerfilContratista.aspx` - Perfil del contratista (supuesto)
4. `/Contratista/AdquirirPlanContratista.aspx` - Compra de suscripción

#### Tareas de Implementación

**FASE 1: Dashboard (1 día, 4 archivos, ~300 líneas)**

1. **ContratistaDashboard Component**
   - Métricas (calificaciones, contrataciones activas, ingresos)
   - Gráficos (contrataciones por mes)
   - Lista de contrataciones activas
   - Notificaciones

**FASE 2: Mis Calificaciones (1 día, 4 archivos, ~300 líneas)**

1. **MisCalificacionesList Component**
   - DevExpress Grid
   - Promedio de calificaciones visible
   - Distribución por estrellas (1★-5★)
   - Paginación

**FASE 3: Perfil & Checkout (1 día, 6 archivos, ~500 líneas)**

1. **MiPerfilContratistaPage**
2. **CheckoutPage** (reutilizar de Empleador)

#### Archivos a Crear (Total: ~14 archivos, ~1,100 líneas)

**Pages:**

- Dashboard, MisCalificaciones, MiPerfil, Checkout (4 archivos)

**Components:**

- ~8 componentes

**Hooks:**

- ~5 hooks

**Services:**

- contr atistasService.ts

**Types:**

- contratista.types.ts

#### Métricas de Éxito

- ✅ Dashboard de contratista funciona
- ✅ Ver calificaciones funciona
- ✅ Editar perfil funciona
- ✅ Checkout funciona
- ✅ Diseño idéntico al Legacy

---

### LOTE 6.5: Shared Pages & Final Polish 🟡 MEDIA

**Prioridad:** 🟡 **MEDIA**  
**Estimación:** 1-2 días (8-16 horas)  
**Estado:** ❌ NO INICIADO

#### Páginas Legacy a Migrar

1. `/FAQ.aspx` - Preguntas frecuentes
2. `/Dashboard.aspx` - Dashboard público (si aplica)
3. `/abogadoVirtual.aspx` - Chat con bot (OPCIONAL)

#### Tareas de Implementación

**FASE 1: FAQ Page (2 horas)**

1. **FAQPage Component**
   - Accordion con preguntas/respuestas
   - Búsqueda de preguntas
   - Categorías

**FASE 2: Public Dashboard (2 horas)**

1. **PublicDashboard Component** (si aplica)

**FASE 3: Bot Chat (4 horas - OPCIONAL)**

1. **BotChatPage Component**
   - Chat interface
   - Integration con OpenAI API
   - Historial de conversaciones

**FASE 4: Final Polish (2-4 horas)**

1. Revisar consistencia visual
2. Optimizar performance (lazy loading, code splitting)
3. Accessibility (a11y) - ARIA labels, keyboard navigation
4. SEO - Meta tags, Open Graph
5. Error boundaries
6. 404 page
7. Loading states

#### Archivos a Crear (Total: ~10 archivos, ~600 líneas)

**Pages:**

- FAQ, PublicDashboard, BotChat, NotFound (4 archivos)

**Components:**

- ~6 componentes

#### Métricas de Éxito

- ✅ FAQ funciona
- ✅ Performance score > 90 (Lighthouse)
- ✅ Accessibility score > 90
- ✅ No errores de consola
- ✅ Responsive en mobile/tablet/desktop

---

## 📊 MÉTRICAS DEL PLAN 6

### Resumen de LOTEs

| LOTE | Nombre | Prioridad | Estimación | Archivos | Líneas | Estado |
|------|--------|-----------|-----------|----------|--------|--------|
| 6.1 | Setup & Infrastructure | 🔴 CRÍTICA | 2-3 días | ~40 | ~2,500 | ❌ |
| 6.2 | Authentication | 🔴 CRÍTICA | 2-3 días | ~20 | ~1,500 | ❌ |
| 6.3 | Empleador Module | 🔴 ALTA | 5-6 días | ~45 | ~3,750 | ❌ |
| 6.4 | Contratista Module | 🔴 ALTA | 2-3 días | ~14 | ~1,100 | ❌ |
| 6.5 | Shared & Polish | 🟡 MEDIA | 1-2 días | ~10 | ~600 | ❌ |

**Total:**

- **Tiempo:** 12-17 días (~96-136 horas)
- **Archivos:** ~129 archivos
- **Líneas:** ~9,450 líneas
- **LOTEs:** 5

### Priorización Recomendada

#### Sprint 1 (Semana 1-2): Fundación

1. **LOTE 6.1: Setup & Infrastructure** (2-3 días)
2. **LOTE 6.2: Authentication** (2-3 días)

**Total Sprint 1:** 4-6 días

#### Sprint 2 (Semana 3-4): Empleador

3. **LOTE 6.3: Empleador Module** (5-6 días)

**Total Sprint 2:** 5-6 días

#### Sprint 3 (Semana 5-6): Contratista & Polish

4. **LOTE 6.4: Contratista Module** (2-3 días)
5. **LOTE 6.5: Shared & Polish** (1-2 días)

**Total Sprint 3:** 3-5 días

---

## 🎯 CHECKLIST DE VALIDACIÓN

### Por Cada LOTE

- [ ] ✅ Componentes creados según plan
- [ ] ✅ TypeScript sin errores
- [ ] ✅ ESLint sin errores
- [ ] ✅ Tests unitarios escritos y pasando
- [ ] ✅ Integration con API funciona
- [ ] ✅ Diseño responsive (mobile, tablet, desktop)
- [ ] ✅ Diseño idéntico al Legacy (colores, tipografía, espaciado)
- [ ] ✅ Documentación `LOTE_X_COMPLETADO.md` creada
- [ ] ✅ Commit con mensaje descriptivo
- [ ] ✅ PR creado y revisado

### Validación Final del PLAN 6

- [ ] ✅ Todas las 18 páginas Legacy migradas
- [ ] ✅ 100% paridad visual con Legacy
- [ ] ✅ 100% paridad funcional con Legacy
- [ ] ✅ Performance score > 90 (Lighthouse)
- [ ] ✅ Accessibility score > 90
- [ ] ✅ SEO optimizado
- [ ] ✅ Responsive en todos los dispositivos
- [ ] ✅ Testing E2E con Playwright/Cypress
- [ ] ✅ Documentación de componentes (Storybook)
- [ ] ✅ Deployment a staging successful
- [ ] ✅ User Acceptance Testing (UAT) aprobado
- [ ] ✅ Reporte final PLAN_6_COMPLETADO_100.md

---

## 🚀 DEPLOYMENT STRATEGY

### Environments

1. **Development** (`localhost:5173`)
   - Hot reload
   - API: `http://localhost:5015/api`

2. **Staging** (`staging.migenteonlinea.com`)
   - API: `https://api-staging.migenteonlinea.com/api`
   - Testing environment

3. **Production** (`www.migenteonlinea.com`)
   - API: `https://api.migenteonlinea.com/api`
   - CDN para assets
   - Performance monitoring

### Build & Deploy

```bash
# Build para producción
npm run build

# Preview de build local
npm run preview

# Deploy a staging (Azure Static Web Apps / Netlify)
npm run deploy:staging

# Deploy a producción
npm run deploy:production
```

### CI/CD Pipeline (GitHub Actions)

```yaml
# .github/workflows/deploy.yml
name: Deploy Frontend

on:
  push:
    branches: [main, develop]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-node@v3
        with:
          node-version: '18'
      
      - run: npm ci
      - run: npm run lint
      - run: npm run type-check
      - run: npm run test
      - run: npm run build
      
      - name: Deploy to Azure
        uses: Azure/static-web-apps-deploy@v1
        with:
          azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
          repo_token: ${{ secrets.GITHUB_TOKEN }}
          action: "upload"
          app_location: "/"
          output_location: "dist"
```

---

## 📚 PRÓXIMOS PASOS

### Acción Inmediata (HOY)

```bash
# 1. Crear proyecto Vite
npm create vite@latest migente-frontend -- --template react-ts

# 2. Instalar dependencias
cd migente-frontend
npm install react-router-dom axios react-query zustand
npm install react-hook-form zod @hookform/resolvers
npm install devextreme devextreme-react
npm install date-fns clsx react-hot-toast

# 3. Setup Tailwind CSS
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p

# 4. Configurar estructura de carpetas
mkdir -p src/{app,features,shared}
mkdir -p src/shared/{components,hooks,services,utils,types}
mkdir -p src/assets/styles

# 5. Crear archivos base (App.tsx, Router.tsx, etc.)
```

### Esta Semana

1. **Día 1-3:** LOTE 6.1 - Setup & Infrastructure ✅
2. **Día 4-6:** LOTE 6.2 - Authentication ✅
3. **Día 7:** Review & Testing 🔄

### Próximas Semanas

- **Semana 2-3:** LOTE 6.3 - Empleador Module
- **Semana 4:** LOTE 6.4 - Contratista Module
- **Semana 5:** LOTE 6.5 - Shared & Polish
- **Semana 6:** UAT, Deployment, Go-live

---

## 🎉 CONCLUSIÓN

**PLAN 6** cubre la migración COMPLETA del frontend Legacy (ASP.NET Web Forms) a frontend moderno (React + TypeScript). Al completar este plan:

- ✅ **100% paridad visual** con sistema Legacy
- ✅ **100% paridad funcional** con sistema Legacy
- ✅ **UI/UX mejorado** (responsive, performance, accessibility)
- ✅ **Modern stack** (React, TypeScript, Vite, Tailwind CSS)
- ✅ **Type-safe** (TypeScript end-to-end)
- ✅ **Testeable** (React Testing Library, Playwright)
- ✅ **Maintainable** (Feature-first architecture, componentes reutilizables)
- ✅ **Production-ready** (CI/CD, deployment automatizado)

**Ventajas del Nuevo Frontend:**

1. **Performance:** ~50% más rápido que Web Forms
2. **Developer Experience:** Hot reload, TypeScript, modern tooling
3. **Maintenance:** Componentes reutilizables, código modular
4. **Scalability:** Fácil agregar nuevas features
5. **Mobile-first:** Responsive design out-of-the-box
6. **SEO:** Meta tags, Open Graph, mejor indexación

**Estado Post-PLAN 6:** Sistema MiGente En Línea **100% moderno**, frontend + backend completamente migrado a Clean Architecture.

---

**Elaborado por:** GitHub Copilot AI Agent  
**Fecha:** 2025-01-18  
**Versión:** 1.0  
**Plan Anterior:** PLAN 5 - Backend Gap Closure  
**Plan Siguiente:** Testing & Go-Live
