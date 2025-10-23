# 🎨 PLAN DE MIGRACIÓN FRONTEND - Legacy Web Forms → Clean Architecture MVC

**Proyecto:** MiGente En Línea  
**Fecha:** 2025-10-12  
**Estado:** PLANIFICACIÓN INICIAL  
**Objetivo:** Migrar EXACTO diseño Legacy ASP.NET Web Forms a ASP.NET Core 8.0 MVC

---

## 📊 RESUMEN EJECUTIVO

### Situación Actual

**LEGACY (Web Forms - .NET Framework 4.7.2):**

- **Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/`
- **Patrón:** Master Pages + ASPX + Code-Behind + ViewState
- **Módulos:** 13+ páginas (9 Empleador + 4 Contratista + root pages)
- **Assets:** Bootstrap 5, DevExpress, jQuery, custom CSS/JS
- **Autenticación:** Forms Authentication con cookies

**CLEAN (MVC - .NET 8.0):**

- **Ubicación:** `MiGenteEnLinea.Clean/src/Presentation/MiGenteEnLinea.Web/`
- **Patrón:** MVC (Model-View-Controller) + Razor syntax
- **Backend:** 123 REST API endpoints en `MiGenteEnLinea.API`
- **Autenticación:** JWT tokens (ya implementado en API)
- **Estado:** Scaffold básico existente (1 HomeController + _Layout genérico)

### Estrategia de Migración

1. **Copia 1:1 de Assets** → Preservar EXACTO look & feel
2. **Master Pages → Layouts** → 3 layouts (público, empleador, contratista)
3. **ASPX → Razor CSHTML** → Mantener estructura HTML exacta
4. **Code-Behind → Controllers/ViewModels** → MVC pattern
5. **Web Forms postbacks → AJAX API calls** → Consumir 123 endpoints
6. **Forms Auth → JWT** → Tokens en localStorage

---

## 🏗️ ESTRUCTURA LEGACY IDENTIFICADA

### Master Pages (3 layouts)

```
Platform.Master           → _Layout.cshtml (layout público - Login, Registro, FAQ)
Comunity1.Master          → _LayoutEmpleador.cshtml (dashboard empleador)
ContratistaM.Master       → _LayoutContratista.cshtml (dashboard contratista)
Comunity2.Master          → (dentro de Contratista/, verificar si se usa)
```

### Páginas Root (6 páginas públicas/comunes)

```
Login.aspx                → Views/Auth/Login.cshtml
Registrar.aspx            → Views/Auth/Register.cshtml
Dashboard.aspx            → Views/Home/Dashboard.cshtml
comunidad.aspx            → Views/Home/Comunidad.cshtml
FAQ.aspx                  → Views/Home/FAQ.cshtml
MiSuscripcion.aspx        → Views/Subscription/Index.cshtml
Activar.aspx              → Views/Auth/Activate.cshtml
abogadoVirtual.aspx       → (IGNORAR - bot OpenAI para fase futura)
```

### Módulo Empleador (9 páginas + subcarpeta)

```
Empleador/colaboradores.aspx              → Views/Empleador/Colaboradores.cshtml
Empleador/fichaEmpleado.aspx              → Views/Empleador/FichaEmpleado.cshtml
Empleador/nomina.aspx                     → Views/Empleador/Nomina.cshtml
Empleador/MiPerfilEmpleador.aspx          → Views/Empleador/MiPerfil.cshtml
Empleador/CalificacionDePerfiles.aspx     → Views/Empleador/Calificaciones.cshtml
Empleador/detalleContratacion.aspx        → Views/Empleador/DetalleContratacion.cshtml
Empleador/fichaColaboradorTemporal.aspx   → Views/Empleador/ColaboradorTemporal.cshtml
Empleador/AdquirirPlanEmpleador.aspx      → Views/Empleador/AdquirirPlan.cshtml
Empleador/Checkout.aspx                   → Views/Empleador/Checkout.cshtml
Empleador/Impresion/                      → (templates PDF - verificar uso)
```

### Módulo Contratista (3 páginas)

```
Contratista/index_contratista.aspx        → Views/Contratista/Index.cshtml
Contratista/MisCalificaciones.aspx        → Views/Contratista/MisCalificaciones.cshtml
Contratista/AdquirirPlanContratista.aspx  → Views/Contratista/AdquirirPlan.cshtml
```

### Assets Identificados

```
assets/css/
  ├── animated.css
  └── style.css (PRINCIPAL - custom styles)

assets/js/
  ├── main.js (funciones globales)
  └── registrar.js (lógica registro)

assets/img/
  └── (imágenes de UI - verificar cantidad)

assets/scss/
  └── (código fuente SASS - NO copiar, solo CSS compilado)

assets/vendor/
  ├── apexcharts/
  ├── bootstrap/
  ├── bootstrap-icons/
  ├── boxicons/
  ├── chart.js/
  ├── echarts/
  ├── php-email-form/ (NO NECESARIO - tenemos API)
  ├── quill/
  ├── remixicon/
  ├── simple-datatables/
  └── tinymce/

Images/
  └── logoMiGene.png + otros (verificar inventario completo)

HtmlTemplates/
  └── (templates HTML - verificar si se usan en frontend)

MailTemplates/
  └── (templates de email - BACKEND, no frontend)

UserControls/
  ├── abogadoBot.ascx (ignorar - OpenAI bot)
  └── otros... (verificar si hay controles reutilizables)
```

---

## 📋 PLAN DE EJECUCIÓN POR FASES

### ✅ FASE 0: PREPARACIÓN Y ANÁLISIS (COMPLETADO)

**Tiempo:** 1 hora  
**Estado:** ✅ COMPLETADO

**Tareas:**

- [x] Analizar estructura Legacy completa
- [x] Listar todos los archivos ASPX y Master Pages
- [x] Inventariar assets (CSS, JS, imágenes, vendor libraries)
- [x] Confirmar estructura Clean existente (MVC pattern)
- [x] Crear este documento de planificación

---

### 🔄 FASE 1: MIGRACIÓN DE ASSETS (2-3 HORAS)

**Objetivo:** Copiar TODOS los assets del Legacy a Clean preservando estructura exacta

**Estado:** ⏳ PENDIENTE  
**Prioridad:** 🔴 CRÍTICA (bloqueante para todas las demás fases)

#### Comandos PowerShell

```powershell
# Variables de rutas
$legacyPath = "C:\Users\ray\OneDrive\Documents\ProyectoMigente\Codigo Fuente Mi Gente\MiGente_Front"
$cleanPath = "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean\src\Presentation\MiGenteEnLinea.Web\wwwroot"

# 1. Copiar CSS (animated.css + style.css)
Copy-Item "$legacyPath\assets\css\*" -Destination "$cleanPath\css\" -Force -Recurse

# 2. Copiar JavaScript (main.js + registrar.js)
Copy-Item "$legacyPath\assets\js\*" -Destination "$cleanPath\js\" -Force -Recurse

# 3. Copiar imágenes de assets
Copy-Item "$legacyPath\assets\img" -Destination "$cleanPath\img" -Force -Recurse

# 4. Copiar imágenes de Images/ (incluye logoMiGene.png)
Copy-Item "$legacyPath\Images" -Destination "$cleanPath\images" -Force -Recurse

# 5. Copiar vendor libraries a lib/vendor (ya existe lib/)
New-Item -ItemType Directory -Path "$cleanPath\lib\vendor" -Force
Copy-Item "$legacyPath\assets\vendor\*" -Destination "$cleanPath\lib\vendor\" -Force -Recurse

# 6. Verificar archivos copiados
Get-ChildItem "$cleanPath\css" -Recurse
Get-ChildItem "$cleanPath\js" -Recurse
Get-ChildItem "$cleanPath\img" -Recurse
Get-ChildItem "$cleanPath\images" -Recurse
Get-ChildItem "$cleanPath\lib\vendor" -Recurse
```

#### Checklist

- [ ] `wwwroot/css/animated.css` copiado
- [ ] `wwwroot/css/style.css` copiado (PRINCIPAL - verificar tamaño)
- [ ] `wwwroot/js/main.js` copiado
- [ ] `wwwroot/js/registrar.js` copiado
- [ ] `wwwroot/img/` copiado (imágenes de assets)
- [ ] `wwwroot/images/logoMiGene.png` copiado
- [ ] `wwwroot/lib/vendor/bootstrap/` copiado
- [ ] `wwwroot/lib/vendor/bootstrap-icons/` copiado
- [ ] `wwwroot/lib/vendor/boxicons/` copiado
- [ ] `wwwroot/lib/vendor/quill/` copiado
- [ ] `wwwroot/lib/vendor/remixicon/` copiado
- [ ] `wwwroot/lib/vendor/simple-datatables/` copiado
- [ ] `wwwroot/lib/vendor/apexcharts/` copiado
- [ ] `wwwroot/lib/vendor/chart.js/` copiado
- [ ] `wwwroot/lib/vendor/echarts/` copiado
- [ ] `wwwroot/lib/vendor/tinymce/` copiado
- [ ] Verificar peso total de assets (< 100MB recomendado)

#### Validación

```powershell
# Test: Verificar que style.css existe y tiene contenido
Get-Content "$cleanPath\css\style.css" | Select-Object -First 10

# Test: Contar archivos copiados
(Get-ChildItem "$cleanPath" -Recurse -File).Count
```

---

### 🔄 FASE 2: CONVERSIÓN DE LAYOUTS (3-4 HORAS)

**Objetivo:** Convertir 3 Master Pages a Razor Layouts manteniendo estructura HTML EXACTA

**Estado:** ⏳ PENDIENTE  
**Prioridad:** 🔴 CRÍTICA (bloqueante para páginas)  
**Dependencia:** FASE 1 completada (assets copiados)

#### 2.1. Layout Público (_Layout.cshtml)

**Fuente:** `Platform.Master`  
**Destino:** `Views/Shared/_Layout.cshtml`

**Conversión ASP.NET → Razor:**

```diff
- <asp:ContentPlaceHolder ID="head" runat="server">
- </asp:ContentPlaceHolder>
+ @RenderSection("Head", required: false)

- <asp:ContentPlaceHolder ID="ContentPlaceHolder1" runat="server">
- </asp:ContentPlaceHolder>
+ @RenderBody()

- <asp:Image runat="server" Width="50%" ImageUrl="~/Images/logoMiGene.png" />
+ <img src="~/images/logoMiGene.png" alt="Mi Gente Logo" style="width: 50%;" />

- <link href="~/assets/css/style.css" rel="stylesheet">
+ <link href="~/css/style.css" rel="stylesheet">

- <link href="~/assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
+ <link href="~/lib/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
```

**Estructura a preservar:**

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewData["Title"] - Mi Gente en Línea</title>
    
    <!-- Favicons -->
    <link href="~/images/favicon.png" rel="icon">
    <link href="~/images/apple-touch-icon.png" rel="apple-touch-icon">
    
    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,300i,400,400i,600,600i,700,700i|Nunito:300,300i,400,400i,600,600i,700,700i|Poppins:300,300i,400,400i,500,500i,600,600i,700,700i" rel="stylesheet">
    
    <!-- Vendor CSS -->
    <link href="~/lib/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="~/lib/vendor/bootstrap-icons/bootstrap-icons.css" rel="stylesheet">
    <link href="~/lib/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
    <link href="~/lib/vendor/quill/quill.snow.css" rel="stylesheet">
    <link href="~/lib/vendor/quill/quill.bubble.css" rel="stylesheet">
    <link href="~/lib/vendor/remixicon/remixicon.css" rel="stylesheet">
    <link href="~/lib/vendor/simple-datatables/style.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet">
    
    <!-- Main CSS -->
    <link href="~/css/style.css" rel="stylesheet">
    
    @RenderSection("Head", required: false)
</head>
<body>
    <!-- Header -->
    <header id="header" class="header fixed-top d-flex align-items-center">
        <div class="d-flex align-items-center justify-content-between">
            <a href="/Home/Comunidad" class="logo d-flex align-items-center mt-3 mb-2">
                <img src="~/images/logoMiGene.png" alt="Mi Gente Logo" style="width: 50%;" />
            </a>
            <i class="bi bi-list toggle-sidebar-btn"></i>
        </div>
        
        <nav class="header-nav ms-auto">
            <ul class="d-flex align-items-center">
                <!-- User profile dropdown -->
                <li class="nav-item dropdown pe-3">
                    <a class="nav-link nav-profile d-flex align-items-center pe-0" href="#" data-bs-toggle="dropdown">
                        <span class="d-none d-md-block dropdown-toggle ps-2" id="lbAcceso">Usuario</span>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow profile">
                        <li><a class="dropdown-item" href="/Empleador/MiPerfil"><i class="bi bi-person"></i> Mi Perfil</a></li>
                        <li><hr class="dropdown-divider"></li>
                        <li><a class="dropdown-item" href="/Home/FAQ"><i class="bi bi-question-circle"></i> FAQ</a></li>
                        <li><hr class="dropdown-divider"></li>
                        <li><a class="dropdown-item" href="/Auth/Logout"><i class="bi bi-box-arrow-right"></i> Cerrar Sesión</a></li>
                    </ul>
                </li>
            </ul>
        </nav>
    </header>
    
    <!-- Main Content -->
    @RenderBody()
    
    <!-- Footer -->
    <footer id="footer" class="footer">
        <div class="copyright">
            &copy; Copyright <strong><span>Mi Gente en Línea</span></strong>. Todos los derechos reservados
        </div>
    </footer>
    
    <!-- Vendor JS -->
    <script src="~/lib/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="~/lib/vendor/quill/quill.min.js"></script>
    <script src="~/lib/vendor/simple-datatables/simple-datatables.js"></script>
    <script src="~/lib/vendor/tinymce/tinymce.min.js"></script>
    <script src="~/lib/vendor/apexcharts/apexcharts.min.js"></script>
    <script src="~/lib/vendor/chart.js/chart.umd.js"></script>
    <script src="~/lib/vendor/echarts/echarts.min.js"></script>
    
    <!-- Main JS -->
    <script src="~/js/main.js"></script>
    
    @RenderSection("Scripts", required: false)
</body>
</html>
```

**Checklist:**

- [ ] Leer `Platform.Master` completo (247 líneas)
- [ ] Convertir `<asp:ContentPlaceHolder>` a `@RenderBody()` y `@RenderSection()`
- [ ] Actualizar rutas: `~/assets/` → `~/css/`, `~/lib/vendor/`, `~/images/`
- [ ] Convertir controles ASP.NET (`<asp:Image>`) a HTML estándar
- [ ] Preservar estructura header (logo, nav, dropdown usuario)
- [ ] Verificar footer
- [ ] Incluir todos los vendor JS en orden correcto
- [ ] Probar layout cargando en navegador

#### 2.2. Layout Empleador (_LayoutEmpleador.cshtml)

**Fuente:** `Comunity1.Master`  
**Destino:** `Views/Shared/_LayoutEmpleador.cshtml`

**Diferencias esperadas vs Layout público:**

- Sidebar izquierdo con menú Empleador
- Enlaces específicos: Colaboradores, Nómina, Calificaciones
- Validación de rol "Empleador" (implementar con JWT claims)

**Checklist:**

- [ ] Leer `Comunity1.Master` completo
- [ ] Heredar estructura base de `_Layout.cshtml`
- [ ] Agregar sidebar izquierdo con navegación Empleador
- [ ] Convertir menú items: colaboradores.aspx → /Empleador/Colaboradores
- [ ] Implementar check de rol (solo Empleador puede acceder)
- [ ] Probar cargando en navegador

#### 2.3. Layout Contratista (_LayoutContratista.cshtml)

**Fuente:** `ContratistaM.Master`  
**Destino:** `Views/Shared/_LayoutContratista.cshtml`

**Diferencias esperadas:**

- Sidebar con menú Contratista
- Enlaces: Mis Calificaciones, Adquirir Plan
- Validación de rol "Contratista"

**Checklist:**

- [ ] Leer `ContratistaM.Master` completo
- [ ] Heredar estructura base
- [ ] Agregar sidebar con navegación Contratista
- [ ] Convertir menú items
- [ ] Implementar check de rol
- [ ] Probar

#### 2.4. ViewStart Configuration

**Crear:** `Views/_ViewStart.cshtml`

```csharp
@{
    // Determinar layout basado en área/controlador
    var area = ViewContext.RouteData.Values["area"]?.ToString();
    var controller = ViewContext.RouteData.Values["controller"]?.ToString();
    
    if (controller == "Empleador")
    {
        Layout = "_LayoutEmpleador";
    }
    else if (controller == "Contratista")
    {
        Layout = "_LayoutContratista";
    }
    else
    {
        Layout = "_Layout";
    }
}
```

---

### 🔄 FASE 3: AUTENTICACIÓN (4-6 HORAS)

**Objetivo:** Migrar Login y Registro + integración con API JWT

**Estado:** ⏳ PENDIENTE  
**Prioridad:** 🔴 CRÍTICA (bloqueante para módulos protegidos)  
**Dependencia:** FASE 1 y FASE 2 completadas

#### 3.1. Controller AuthController.cs

**Crear:** `Controllers/AuthController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace MiGenteEnLinea.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("API");
            var content = new StringContent(
                JsonSerializer.Serialize(new { model.Email, model.Password }),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                
                // Guardar token en cookie segura
                Response.Cookies.Append("jwt_token", result.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(8)
                });

                // Redirigir según tipo de usuario
                if (result.TipoUsuario == "Empleador")
                    return RedirectToAction("Colaboradores", "Empleador");
                else
                    return RedirectToAction("Index", "Contratista");
            }

            ModelState.AddModelError("", "Credenciales inválidas");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("API");
            var content = new StringContent(
                JsonSerializer.Serialize(model),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/api/Auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Registro exitoso. Revise su correo para activar su cuenta.";
                return RedirectToAction("Login");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", error);
            return View(model);
        }

        [HttpGet]
        public IActionResult Activate(int userId, string email)
        {
            // Llamar a API /api/Auth/activate
            // Mostrar mensaje de éxito
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_token");
            return RedirectToAction("Login");
        }
    }
}
```

#### 3.2. View Login.cshtml

**Fuente:** `Login.aspx`  
**Crear:** `Views/Auth/Login.cshtml`

**Estructura HTML exacta del Legacy:**

- Leer `Login.aspx` completo
- Copiar HTML preservando clases CSS
- Reemplazar controles ASP.NET (`<asp:TextBox>`) con HTML estándar (`<input>`)
- Agregar `@model LoginViewModel`
- Usar `asp-` tag helpers para formulario

**Checklist:**

- [ ] Leer `Login.aspx` completo
- [ ] Crear `Models/ViewModels/LoginViewModel.cs`
- [ ] Copiar HTML del formulario
- [ ] Convertir `<asp:TextBox>` a `<input asp-for="Email">`
- [ ] Mantener estilos y clases Bootstrap
- [ ] Agregar validación client-side (jQuery Validate)
- [ ] Probar: formulario se ve igual que Legacy

#### 3.3. View Register.cshtml

**Fuente:** `Registrar.aspx`  
**Crear:** `Views/Auth/Register.cshtml`

**Checklist:**

- [ ] Leer `Registrar.aspx` completo
- [ ] Crear `Models/ViewModels/RegisterViewModel.cs`
- [ ] Copiar HTML del formulario (probablemente multi-step)
- [ ] Verificar si usa registrar.js (lógica client-side)
- [ ] Convertir controles ASP.NET a HTML
- [ ] Preservar wizard/steps si existe
- [ ] Probar

#### 3.4. Configurar HttpClientFactory

**Editar:** `Program.cs`

```csharp
// Agregar después de builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5015"); // URL de la API
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Agregar autenticación con cookies
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });
```

#### 3.5. Middleware de Autenticación

**Agregar en `Program.cs` antes de `app.UseAuthorization()`:**

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

---

### 🔄 FASE 4: MÓDULO EMPLEADOR (10-12 HORAS)

**Objetivo:** Migrar 9 páginas del módulo Empleador

**Estado:** ⏳ PENDIENTE  
**Prioridad:** 🟠 ALTA  
**Dependencia:** FASE 3 completada (autenticación funcionando)

#### Orden de Implementación (por dependencias)

**1. Colaboradores.cshtml (PRIORITARIO)**

- **Fuente:** `Empleador/colaboradores.aspx`
- **Ruta:** `/Empleador/Colaboradores`
- **API:** `GET /api/Empleados`, `POST /api/Empleados`
- **Funcionalidad:** Lista de empleados, agregar nuevo, buscar, filtrar
- **Tiempo:** 2 horas
- **Checklist:**
  - [ ] Leer colaboradores.aspx completo
  - [ ] Crear EmpleadorController.cs
  - [ ] Crear Views/Empleador/Colaboradores.cshtml
  - [ ] Implementar tabla/grid con datos de API
  - [ ] Botón "Agregar Empleado" → modal o nueva página
  - [ ] Preservar diseño exacto (Bootstrap grid/cards)

**2. FichaEmpleado.cshtml**

- **Fuente:** `Empleador/fichaEmpleado.aspx`
- **Ruta:** `/Empleador/FichaEmpleado/{id}`
- **API:** `GET /api/Empleados/{id}`, `PUT /api/Empleados/{id}`
- **Funcionalidad:** Ver/editar detalles de empleado
- **Tiempo:** 1.5 horas

**3. Nomina.cshtml**

- **Fuente:** `Empleador/nomina.aspx`
- **Ruta:** `/Empleador/Nomina`
- **API:** `GET /api/Nominas`, `POST /api/Nominas/procesar`
- **Funcionalidad:** Generar nómina, ver recibos, calcular deducciones TSS
- **Tiempo:** 2 horas

**4. MiPerfil.cshtml**

- **Fuente:** `Empleador/MiPerfilEmpleador.aspx`
- **Ruta:** `/Empleador/MiPerfil`
- **API:** `GET /api/Empleadores/perfil`, `PUT /api/Empleadores/perfil`
- **Funcionalidad:** Ver/editar perfil empleador
- **Tiempo:** 1 hora

**5. Calificaciones.cshtml**

- **Fuente:** `Empleador/CalificacionDePerfiles.aspx`
- **Ruta:** `/Empleador/Calificaciones`
- **API:** `GET /api/Calificaciones`, `POST /api/Calificaciones`
- **Funcionalidad:** Ver y dejar calificaciones a contratistas
- **Tiempo:** 1.5 horas

**6. DetalleContratacion.cshtml**

- **Fuente:** `Empleador/detalleContratacion.aspx`
- **Ruta:** `/Empleador/DetalleContratacion/{id}`
- **API:** `GET /api/Contrataciones/{id}`
- **Funcionalidad:** Ver detalles de contratación de servicios
- **Tiempo:** 1 hora

**7. ColaboradorTemporal.cshtml**

- **Fuente:** `Empleador/fichaColaboradorTemporal.aspx`
- **Ruta:** `/Empleador/ColaboradorTemporal`
- **API:** `POST /api/Empleados/temporal`
- **Funcionalidad:** Registrar empleado temporal/por servicio
- **Tiempo:** 1 hora

**8. AdquirirPlan.cshtml**

- **Fuente:** `Empleador/AdquirirPlanEmpleador.aspx`
- **Ruta:** `/Empleador/AdquirirPlan`
- **API:** `GET /api/Planes/empleador`, `POST /api/Suscripciones`
- **Funcionalidad:** Ver planes disponibles, seleccionar plan
- **Tiempo:** 1.5 horas

**9. Checkout.cshtml (COMPLEJO)**

- **Fuente:** `Empleador/Checkout.aspx`
- **Ruta:** `/Empleador/Checkout`
- **API:** `POST /api/Payments/process-cardnet`
- **Funcionalidad:** Pasarela de pago Cardnet, confirmar compra
- **Tiempo:** 2 horas
- **Nota:** Integración con Cardnet (ya configurado en API User Secrets)

#### Patrón de Migración para Cada Página

```
1. Leer ASPX completo del Legacy
2. Identificar controles y lógica (code-behind .cs)
3. Crear acción en EmpleadorController.cs
4. Crear ViewModel si necesario
5. Crear View .cshtml
6. Copiar HTML preservando estructura
7. Convertir controles ASP.NET a Razor
8. Implementar llamadas AJAX a API
9. Probar visualmente (debe verse igual)
10. Probar funcionalidad (CRUD, validaciones)
```

---

### 🔄 FASE 5: MÓDULO CONTRATISTA (4-6 HORAS)

**Objetivo:** Migrar 3 páginas del módulo Contratista

**Estado:** ⏳ PENDIENTE  
**Prioridad:** 🟠 ALTA  
**Dependencia:** FASE 3 completada

#### Páginas a Migrar

**1. Index.cshtml (Dashboard Contratista)**

- **Fuente:** `Contratista/index_contratista.aspx`
- **Ruta:** `/Contratista/Index`
- **API:** `GET /api/Contratistas/dashboard`
- **Funcionalidad:** Dashboard principal contratista, estadísticas, notificaciones
- **Tiempo:** 2 horas

**2. MisCalificaciones.cshtml**

- **Fuente:** `Contratista/MisCalificaciones.aspx`
- **Ruta:** `/Contratista/MisCalificaciones`
- **API:** `GET /api/Calificaciones/contratista/{id}`
- **Funcionalidad:** Ver calificaciones recibidas
- **Tiempo:** 1.5 horas

**3. AdquirirPlan.cshtml**

- **Fuente:** `Contratista/AdquirirPlanContratista.aspx`
- **Ruta:** `/Contratista/AdquirirPlan`
- **API:** `GET /api/Planes/contratista`, `POST /api/Suscripciones`
- **Funcionalidad:** Ver y adquirir planes para contratista
- **Tiempo:** 1.5 horas

#### Checklist General

- [ ] Crear ContratistaController.cs
- [ ] Verificar si Comunity2.Master se usa (dentro de carpeta Contratista)
- [ ] Migrar 3 páginas siguiendo patrón estándar
- [ ] Probar flujo completo: Login como Contratista → Index → Calificaciones → Adquirir Plan

---

### 🔄 FASE 6: PÁGINAS ROOT/COMUNES (4-6 HORAS)

**Objetivo:** Migrar páginas públicas y comunes

**Estado:** ⏳ PENDIENTE  
**Prioridad:** 🟡 MEDIA

#### Páginas a Migrar

**1. Dashboard.cshtml**

- **Fuente:** `Dashboard.aspx`
- **Ruta:** `/Home/Dashboard`
- **Funcionalidad:** Dashboard genérico (verificar si se usa)

**2. Comunidad.cshtml**

- **Fuente:** `comunidad.aspx`
- **Ruta:** `/Home/Comunidad`
- **Funcionalidad:** Página de comunidad/landing después de login

**3. FAQ.cshtml**

- **Fuente:** `FAQ.aspx`
- **Ruta:** `/Home/FAQ`
- **Funcionalidad:** Preguntas frecuentes (contenido estático)

**4. MiSuscripcion.cshtml**

- **Fuente:** `MiSuscripcion.aspx`
- **Ruta:** `/Subscription/Index`
- **API:** `GET /api/Suscripciones/mi-suscripcion`
- **Funcionalidad:** Ver estado de suscripción actual

**5. Activar.cshtml**

- **Fuente:** `Activar.aspx` (probablemente redirecciona a activarperfil.aspx)
- **Ruta:** `/Auth/Activate?userId={id}&email={email}`
- **API:** `POST /api/Auth/activate`
- **Funcionalidad:** Activar cuenta desde email

---

### 🔄 FASE 7: INTEGRACIÓN API Y JAVASCRIPT (6-8 HORAS)

**Objetivo:** Crear capa de servicios JavaScript para consumir API

**Estado:** ⏳ PENDIENTE  
**Prioridad:** 🟠 ALTA (para funcionalidad completa)

#### 7.1. API Service Layer (JavaScript)

**Crear:** `wwwroot/js/services/api-service.js`

```javascript
class ApiService {
    constructor() {
        this.baseUrl = 'http://localhost:5015/api';
        this.token = this.getToken();
    }

    getToken() {
        // Leer de cookie o localStorage
        return document.cookie
            .split('; ')
            .find(row => row.startsWith('jwt_token='))
            ?.split('=')[1];
    }

    async request(url, options = {}) {
        const headers = {
            'Content-Type': 'application/json',
            ...options.headers
        };

        if (this.token) {
            headers['Authorization'] = `Bearer ${this.token}`;
        }

        try {
            const response = await fetch(`${this.baseUrl}${url}`, {
                ...options,
                headers
            });

            if (response.status === 401) {
                // Token expirado, redirigir a login
                window.location.href = '/Auth/Login';
                return;
            }

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}: ${response.statusText}`);
            }

            return await response.json();
        } catch (error) {
            console.error('API Error:', error);
            throw error;
        }
    }

    // Métodos específicos
    async getEmpleados() {
        return this.request('/Empleados');
    }

    async getEmpleadoById(id) {
        return this.request(`/Empleados/${id}`);
    }

    async createEmpleado(data) {
        return this.request('/Empleados', {
            method: 'POST',
            body: JSON.stringify(data)
        });
    }

    // ... más métodos para cada endpoint
}

// Instancia global
window.apiService = new ApiService();
```

#### 7.2. Reemplazar Postbacks por AJAX

**Patrón Legacy (Web Forms):**

```aspx
<asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
```

**Patrón Clean (MVC + AJAX):**

```html
<button id="btnGuardar" class="btn btn-primary">Guardar</button>

<script>
$('#btnGuardar').click(async function() {
    const data = {
        nombre: $('#txtNombre').val(),
        apellido: $('#txtApellido').val()
        // ...
    };

    try {
        const result = await apiService.createEmpleado(data);
        Swal.fire('Éxito', 'Empleado guardado correctamente', 'success');
        // Recargar lista o redirigir
    } catch (error) {
        Swal.fire('Error', error.message, 'error');
    }
});
</script>
```

#### 7.3. Adaptar main.js y registrar.js

- [ ] Verificar si main.js tiene lógica específica del Legacy
- [ ] Adaptar referencias a controles ASP.NET (IDs cambian)
- [ ] Preservar funciones globales (toggle sidebar, etc.)
- [ ] Adaptar registrar.js para funcionar con nuevo formulario

---

### 🔄 FASE 8: TESTING Y VALIDACIÓN (4-6 HORAS)

**Objetivo:** Probar todo el sistema migrado

**Estado:** ⏳ PENDIENTE  
**Prioridad:** 🔴 CRÍTICA (antes de producción)

#### 8.1. Visual Testing

- [ ] Comparar cada página con Legacy lado a lado
- [ ] Verificar colores, fuentes, espaciados (debe ser IDÉNTICO)
- [ ] Probar responsive design (mobile, tablet, desktop)
- [ ] Verificar imágenes cargan correctamente
- [ ] Probar en Chrome, Firefox, Edge

#### 8.2. Functional Testing

- [ ] **Autenticación:**
  - [ ] Registro nuevo usuario
  - [ ] Login con credenciales válidas
  - [ ] Login con credenciales inválidas
  - [ ] Logout
  - [ ] Activación de cuenta desde email

- [ ] **Empleador:**
  - [ ] Login como Empleador
  - [ ] Ver lista de colaboradores
  - [ ] Agregar nuevo empleado
  - [ ] Editar empleado existente
  - [ ] Generar nómina
  - [ ] Ver recibos de pago
  - [ ] Calificar contratista
  - [ ] Adquirir plan
  - [ ] Checkout con Cardnet (TEST mode)

- [ ] **Contratista:**
  - [ ] Login como Contratista
  - [ ] Ver dashboard
  - [ ] Ver mis calificaciones
  - [ ] Adquirir plan

#### 8.3. Integration Testing

- [ ] Probar TODAS las llamadas a API (123 endpoints)
- [ ] Verificar JWT se envía en headers
- [ ] Probar refresh token si implementado
- [ ] Verificar manejo de errores (400, 401, 404, 500)
- [ ] Probar validaciones (client-side y server-side)

#### 8.4. Performance Testing

- [ ] Medir tiempo de carga de páginas
- [ ] Verificar tamaño de assets (CSS, JS, imágenes < 10MB total)
- [ ] Probar con DevTools (Network, Performance)
- [ ] Verificar no hay memory leaks (JavaScript)

---

## 📊 INVENTARIO COMPLETO DE ASSETS

### CSS Files (2 archivos)

```
assets/css/animated.css    → wwwroot/css/animated.css
assets/css/style.css       → wwwroot/css/style.css (PRINCIPAL)
```

### JavaScript Files (2+ archivos)

```
assets/js/main.js         → wwwroot/js/main.js (funciones globales)
assets/js/registrar.js    → wwwroot/js/registrar.js (lógica registro)
```

### Vendor Libraries (11 carpetas)

```
assets/vendor/apexcharts/         → wwwroot/lib/vendor/apexcharts/
assets/vendor/bootstrap/          → wwwroot/lib/vendor/bootstrap/
assets/vendor/bootstrap-icons/    → wwwroot/lib/vendor/bootstrap-icons/
assets/vendor/boxicons/           → wwwroot/lib/vendor/boxicons/
assets/vendor/chart.js/           → wwwroot/lib/vendor/chart.js/
assets/vendor/echarts/            → wwwroot/lib/vendor/echarts/
assets/vendor/php-email-form/     → (NO COPIAR - no necesario)
assets/vendor/quill/              → wwwroot/lib/vendor/quill/
assets/vendor/remixicon/          → wwwroot/lib/vendor/remixicon/
assets/vendor/simple-datatables/  → wwwroot/lib/vendor/simple-datatables/
assets/vendor/tinymce/            → wwwroot/lib/vendor/tinymce/
```

### Images

```
assets/img/               → wwwroot/img/ (verificar inventario completo)
Images/logoMiGene.png     → wwwroot/images/logoMiGene.png (CRÍTICO)
Images/...                → wwwroot/images/... (resto de imágenes)
```

### Templates y Otros

```
HtmlTemplates/            → (verificar si se usan en frontend)
MailTemplates/            → (BACKEND - no copiar, API maneja emails)
UserControls/             → (analizar, convertir a Partial Views si necesario)
```

---

## 🎯 CRITERIOS DE ÉXITO

### Visual (Diseño)

- ✅ Cada página se ve IDÉNTICA al Legacy (pixel-perfect no obligatorio, pero muy similar)
- ✅ Colores, fuentes, espaciados preservados
- ✅ Logo y imágenes cargan correctamente
- ✅ Responsive design funciona en mobile/tablet/desktop
- ✅ Animaciones CSS preservadas (animated.css)

### Funcional

- ✅ Autenticación JWT funciona (login, logout, refresh)
- ✅ Roles (Empleador/Contratista) se respetan
- ✅ CRUD de empleados funciona (Create, Read, Update, Delete via API)
- ✅ Nómina se genera correctamente
- ✅ Calificaciones se guardan
- ✅ Suscripciones se pueden adquirir
- ✅ Cardnet TEST mode funciona (checkout)

### Técnico

- ✅ 0 errores de compilación
- ✅ 0 errores de JavaScript en consola
- ✅ 100% de assets cargan (0 404)
- ✅ API responde correctamente (0 errores 500)
- ✅ Validaciones client-side y server-side funcionan
- ✅ Manejo de errores apropiado (mensajes amigables)

---

## 📝 NOTAS IMPORTANTES

### Exclusiones (NO Migrar)

- ❌ `abogadoVirtual.aspx` - Bot OpenAI (fase futura)
- ❌ `UserControls/abogadoBot.ascx` - Control del bot
- ❌ `assets/vendor/php-email-form/` - No necesario (tenemos API)
- ❌ `assets/scss/` - Código fuente SASS (solo copiar CSS compilado)
- ❌ `MailTemplates/` - Templates backend (API maneja)

### Tecnologías Clave

- **Frontend:** ASP.NET Core 8.0 MVC + Razor
- **Backend API:** 123 endpoints REST (ya completado 100%)
- **CSS Framework:** Bootstrap 5
- **JavaScript:** jQuery (del Legacy, mantener para compatibilidad)
- **Charts:** ApexCharts, Chart.js, ECharts
- **Datatables:** Simple DataTables
- **Editor:** TinyMCE, Quill
- **Icons:** Bootstrap Icons, Boxicons, Remix Icons, Font Awesome
- **Autenticación:** JWT (tokens en cookies HttpOnly)

### Mejoras Permitidas (Sin Romper Compatibilidad Visual)

- ✅ Mejorar performance (lazy loading, minificación)
- ✅ Agregar cache de assets
- ✅ Implementar Service Workers (PWA) - opcional
- ✅ Mejorar accesibilidad (ARIA labels)
- ✅ Agregar unit tests (backend ya existe)

### Riesgos y Mitigaciones

| Riesgo | Impacto | Mitigación |
|--------|---------|------------|
| Assets no cargan (rutas incorrectas) | 🔴 ALTO | Verificar cada ruta manualmente, usar ~ para rutas relativas |
| JavaScript Legacy no funciona en MVC | 🔴 ALTO | Adaptar IDs de controles, usar `data-*` attributes |
| ViewState perdido (Web Forms feature) | 🟡 MEDIO | Reemplazar con AJAX + API (mejor solución) |
| DevExpress controls no migran | 🟡 MEDIO | Reemplazar con Bootstrap + JavaScript equivalent |
| Cardnet integration falla | 🟠 ALTO | Ya configurado en API, probar en TEST mode primero |
| Performance pobre (muchos assets) | 🟢 BAJO | Minificar, bundling, CDN |

---

## 📅 ESTIMACIÓN DE TIEMPO TOTAL

| Fase | Tiempo Estimado | Prioridad |
|------|-----------------|-----------|
| **FASE 1:** Assets | 2-3 horas | 🔴 CRÍTICA |
| **FASE 2:** Layouts | 3-4 horas | 🔴 CRÍTICA |
| **FASE 3:** Autenticación | 4-6 horas | 🔴 CRÍTICA |
| **FASE 4:** Módulo Empleador | 10-12 horas | 🟠 ALTA |
| **FASE 5:** Módulo Contratista | 4-6 horas | 🟠 ALTA |
| **FASE 6:** Páginas Root | 4-6 horas | 🟡 MEDIA |
| **FASE 7:** API Integration | 6-8 horas | 🟠 ALTA |
| **FASE 8:** Testing | 4-6 horas | 🔴 CRÍTICA |
| **TOTAL** | **37-51 horas** | (1-2 semanas full-time) |

---

## ✅ PRÓXIMO PASO INMEDIATO

**ACCIÓN:** Comenzar FASE 1 (Migración de Assets)

**Comando:**

```powershell
# Copiar assets del Legacy a Clean
cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean\src\Presentation\MiGenteEnLinea.Web\wwwroot"

# Ejecutar comandos de copia (ver sección FASE 1)
```

**Validación:**

```powershell
# Verificar archivos copiados
Get-ChildItem "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean\src\Presentation\MiGenteEnLinea.Web\wwwroot" -Recurse | Measure-Object -Property Length -Sum
```

**Tiempo estimado:** 2-3 horas

---

_Documento creado: 2025-10-12_  
_Última actualización: 2025-10-12_  
_Estado: FASE 0 COMPLETADO, listo para FASE 1_
