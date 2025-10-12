# MiGente En Línea - AI Coding Instructions

## Project Overview

**MiGente En Línea** is an ASP.NET Web Forms application (.NET Framework 4.7.2) for managing employment relationships in the Dominican Republic. It connects **Empleadores** (employers) and **Contratistas** (contractors/service providers) with subscription-based access and integrated payment processing.

## Architecture & Technology Stack

### Core Framework

- **ASP.NET Web Forms** (.NET Framework 4.7.2)
- **Entity Framework 6** for data access (Database-First approach with EDMX)
- **SQL Server** database (`migenteV2`)
- **IIS Express** for local development (port 44358 with SSL)

### Key Dependencies

- **DevExpress v23.1**: Commercial UI component library (ASPxGridView, Bootstrap controls)
- **iText 8.0.5**: PDF generation (contracts, receipts, payroll documents)
- **Cardnet Payment Gateway**: Dominican payment processor integration
- **OpenAI Integration**: Virtual legal assistant ("abogado virtual")
- **RestSharp 112.1.0**: HTTP client for external API calls
- **Newtonsoft.Json 13.0.3**: JSON serialization

### Authentication & Authorization

- **Forms Authentication** with cookie-based sessions (`~/Login.aspx` as login URL)
- **Two user roles** stored in cookies:
  - `tipo = "1"`: Empleador (Employer) → redirects to `/comunidad.aspx`
  - `tipo = "2"`: Contratista (Contractor) → redirects to `/Contratista/index_contratista.aspx`
- Cookie structure: `login` cookie contains `userID`, `nombre`, `tipo`, `planID`, `vencimientoPlan`, `email`

## Project Structure

### Master Pages (Role-Based Layouts)

- `Platform.Master`: Base layout for public/general pages
- `Comunity1.Master`: Empleador dashboard layout (checks `tipo = "1"`)
- `ContratistaM.Master`: Contratista dashboard layout (checks `tipo = "2"`)
- **Plan enforcement**: Both master pages redirect to subscription purchase if `planID = "0"` or plan is expired

### Key Directories

```
MiGente_Front/
├── Contratista/          # Contractor-specific pages
│   ├── index_contratista.aspx
│   ├── AdquirirPlanContratista.aspx
│   └── MisCalificaciones.aspx
├── Empleador/            # Employer-specific pages
│   ├── colaboradores.aspx
│   ├── nomina.aspx
│   ├── fichaEmpleado.aspx
│   ├── Checkout.aspx
│   └── Impresion/        # Print templates for contracts/receipts
├── Data/                 # Entity Framework models (auto-generated from EDMX)
│   ├── DataModel.edmx
│   └── [Entity classes].cs
├── Services/             # Business logic & API services
│   ├── LoginService.cs
│   ├── EmailService.cs
│   ├── PaymentService.cs
│   ├── BotServices.cs (OpenAI integration)
│   └── *.asmx (SOAP web services)
├── UserControls/         # Reusable ASCX components
├── HtmlTemplates/        # Static HTML content (terms, authorizations)
└── MailTemplates/        # Email templates (HTML)
```

### Database Connection

```xml
<!-- Web.config -->
<connectionStrings>
  <add name="migenteEntities"
       connectionString="metadata=res://*/Data.DataModel.csdl|...;
       provider=System.Data.SqlClient;
       provider connection string='data source=.;initial catalog=migenteV2;
       user id=sa;password=1234;...'"
       providerName="System.Data.EntityClient"/>
</connectionStrings>
```

**Note**: Connection uses SQL Server on localhost (`.`) with hardcoded credentials.

### Payment Integration (Cardnet)

```xml
<appSettings>
  <add key="CardnetMerchantId" value="349000001"/>
  <add key="CardnetApiKey" value="TU_API_KEY"/>
  <add key="CardnetApiUrlSales" value="https://ecommerce.cardnet.com.do/api/payment/transactions/sales"/>
  <add key="CardnetApiUrlIdempotency" value="https://ecommerce.cardnet.com.do/api/payment/idenpotency-keys"/>
</appSettings>
```

## Critical Workflows

### User Registration & Activation

1. User registers via `Registrar.aspx` → creates `Credenciales` + `Ofertantes`/`Contratistas` record
2. Activation email sent with URL: `activarperfil.aspx?userID={id}&email={email}`
3. User activates account → sets `Activo = true` in database
4. First login redirects to subscription purchase if no plan

### Subscription Management

- Plans stored in `Planes_empleadores` / `Planes_Contratistas` tables
- Subscription data in `Suscripciones` table (with `FechaVencimiento`)
- Master pages enforce active subscription before page access
- Checkout flow: `AdquirirPlan*.aspx` → `Checkout.aspx` → Cardnet payment → update subscription

### Payroll & Document Generation

- Employers create employees in `Empleados` table
- Payroll generation creates `Empleador_Recibos_Header` + `Empleador_Recibos_Detalle`
- TSS (social security) deductions calculated via `Deducciones_TSS` table
- PDF generation using iText: contracts (`ContratoPersonaFisica.html`), receipts in `Empleador/Impresion/`

## Development Conventions

### Code-Behind Pattern

All `.aspx` pages follow the standard Web Forms pattern:

```csharp
namespace MiGente_Front
{
    public partial class PageName : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { /* initialization */ }
        }
    }
}
```

### Service Layer Pattern

Services are instantiated in code-behind, not via dependency injection:

```csharp
LoginService service = new LoginService();
var result = service.login(username, password);
```

### SweetAlert for User Feedback

All user messages use SweetAlert2 via `ClientScript.RegisterStartupScript`:

```csharp
string script = @"<script>
    Swal.fire({
        title: 'Título',
        text: 'Mensaje',
        icon: 'success|error|warning|info',
        confirmButtonText: 'Aceptar'
    });
</script>";
ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);
```

### Session & Cookie Management

- Session cleared on logout: `Session.Clear(); Session.Abandon();`
- Forms authentication: `FormsAuthentication.SignOut();`
- Cookie access: `HttpCookie myCookie = Request.Cookies["login"];`

## Build & Run

### Prerequisites

- Visual Studio 2017+ (solution targets VS 17.6)
- IIS Express configured
- SQL Server with `migenteV2` database
- DevExpress v23.1 license (commercial component)

### Build Configuration

```bash
# Debug build
msbuild MiGente.sln /p:Configuration=Debug

# Publish to Azure/IIS (Web Deploy configured in Properties/PublishProfiles/)
```

### Local Development URL

- **HTTPS**: `https://localhost:44358/`
- **Start page**: `Login.aspx`

## Important Notes for AI Agents

### Do NOT Modify

- Entity Framework EDMX and auto-generated model classes in `Data/`
- DevExpress control configurations (proprietary markup)
- Payment gateway integration endpoints
- Database connection strings without explicit approval

### External Dependencies Reference

- **ClassLibrary CSharp.dll**: External utility library at `..\..\Utility_Suite\Utility_POS\Utility_POS\bin\Debug\` (not in repository)
- DevExpress assemblies: Requires valid license for development

### Security Considerations

- Passwords stored as plain text in database (needs improvement)
- SQL credentials hardcoded in Web.config
- Cookie-based authentication without encryption
- No CSRF protection on forms

### Testing Strategy

- No unit tests currently exist in solution
- Manual testing required for all changes
- Test with both user types (Empleador and Contratista)
- Verify subscription enforcement on protected pages

## Domain-Specific Terms (Dominican Context)

- **TSS**: Tesorería de la Seguridad Social (Social Security Treasury)
- **RNC/Cédula**: Tax ID / National ID numbers
- **Padrón**: National registry/database
- **Recibo de pago**: Payment receipt
- **Nómina**: Payroll
- **Colaborador**: Employee/collaborator

## Quick Reference: Key Files

- `Login.aspx.cs`: Authentication entry point
- `Comunity1.Master.cs`: Empleador session/plan validation
- `ContratistaM.Master.cs`: Contratista session/plan validation
- `Web.config`: All configuration (DB, APIs, DevExpress)
- `NumeroEnLetras.cs`: Number-to-words conversion (for legal documents)

---

_Last updated: 2025-10-12_
_For questions about business logic or specific features, consult the project owner before making assumptions._
