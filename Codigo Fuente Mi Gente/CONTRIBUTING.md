# 🤝 Contribuyendo a MiGente En Línea

¡Gracias por tu interés en contribuir a MiGente En Línea! Este documento proporciona pautas para contribuir al proyecto.

## 📋 Tabla de Contenidos

- [Código de Conducta](#código-de-conducta)
- [Cómo Puedo Contribuir](#cómo-puedo-contribuir)
- [Configuración del Entorno de Desarrollo](#configuración-del-entorno-de-desarrollo)
- [Proceso de Pull Request](#proceso-de-pull-request)
- [Guías de Estilo](#guías-de-estilo)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Commit Messages](#commit-messages)

## 📜 Código de Conducta

Este proyecto y todos los participantes están gobernados por nuestro [Código de Conducta](CODE_OF_CONDUCT.md). Al participar, se espera que mantengas este código. Por favor reporta comportamiento inaceptable a rainierypenajrg@gmail.com.

## 🎯 Cómo Puedo Contribuir

### Reportando Bugs

Antes de crear un reporte de bug, por favor revisa la lista de issues existentes para evitar duplicados.

**Cómo reportar un bug:**
1. Usa el template de "Bug Report" al crear un nuevo issue
2. Proporciona un título claro y descriptivo
3. Describe los pasos exactos para reproducir el problema
4. Proporciona ejemplos específicos y screenshots si es posible
5. Describe el comportamiento que observaste y por qué es un problema
6. Explica qué comportamiento esperabas ver
7. Incluye detalles sobre tu configuración (OS, browser, versión, etc.)

### Sugiriendo Mejoras

Las sugerencias de mejoras se gestionan como GitHub issues usando el template de "Feature Request".

**Cómo sugerir una mejora:**
1. Usa el template de "Feature Request"
2. Proporciona un título claro y descriptivo
3. Describe el problema que la mejora resolvería
4. Proporciona una descripción detallada de la solución propuesta
5. Explica el valor de negocio
6. Si es posible, incluye mockups o wireframes

### Reportando Vulnerabilidades de Seguridad

🔒 **IMPORTANTE**: Para vulnerabilidades de seguridad, sigue el proceso descrito en [SECURITY.md](SECURITY.md).

**NO** uses el issue tracker público para reportar vulnerabilidades de seguridad.

## 🛠️ Configuración del Entorno de Desarrollo

### Prerequisitos

- Visual Studio 2019 o superior
- .NET Framework 4.7.2 / .NET Core 8.0 (para nueva arquitectura)
- SQL Server 2016 o superior
- Git
- DevExpress v23.1 license (para legacy Web Forms)

### Setup Inicial

1. **Fork el repositorio**
   ```bash
   # Fork desde GitHub UI
   ```

2. **Clonar tu fork**
   ```bash
   git clone https://github.com/TU_USUARIO/MiGenteEnlinea.git
   cd MiGenteEnlinea
   ```

3. **Agregar upstream remote**
   ```bash
   git remote add upstream https://github.com/RainieryPeniaJrg/MiGenteEnlinea.git
   ```

4. **Configurar base de datos**
   ```sql
   -- Crear base de datos desde backup
   RESTORE DATABASE migenteV2 FROM DISK = 'path/to/backup.bak'
   ```

5. **Configurar Web.config**
   ```bash
   # Copiar el template de configuración
   cp MiGente_Front/Web.config.example MiGente_Front/Web.config
   
   # Editar Web.config con tus credenciales locales
   ```

6. **Restaurar paquetes NuGet**
   ```bash
   nuget restore MiGente.sln
   ```

7. **Compilar la solución**
   ```bash
   msbuild MiGente.sln /p:Configuration=Debug
   ```

### Crear una Rama de Trabajo

```bash
# Asegúrate de estar en main y actualizado
git checkout main
git pull upstream main

# Crea una rama descriptiva
git checkout -b feature/descripcion-corta
# o
git checkout -b fix/nombre-del-bug
# o
git checkout -b security/nombre-vulnerabilidad
```

**Convenciones de nombres de ramas:**
- `feature/` - Nuevas funcionalidades
- `fix/` - Corrección de bugs
- `security/` - Fixes de seguridad
- `refactor/` - Refactoring de código
- `docs/` - Cambios en documentación
- `test/` - Adición de tests
- `chore/` - Tareas de mantenimiento

## 🔄 Proceso de Pull Request

### Antes de Enviar

1. **Asegúrate de que tu código cumple con:**
   - [ ] Estándares de seguridad (ver SECURITY.md)
   - [ ] Guías de estilo del proyecto
   - [ ] Principios de Clean Architecture
   - [ ] No hay vulnerabilidades conocidas

2. **Ejecuta los tests**
   ```bash
   dotnet test
   ```

3. **Verifica que compile sin warnings**
   ```bash
   msbuild MiGente.sln /p:Configuration=Release /p:TreatWarningsAsErrors=true
   ```

### Enviando el PR

1. **Push a tu fork**
   ```bash
   git push origin feature/tu-rama
   ```

2. **Crear Pull Request en GitHub**
   - Usa el template de PR
   - Completa TODAS las secciones del template
   - Asegúrate de que los checks de CI pasen

3. **Describe tus cambios claramente**
   - Qué problema resuelve
   - Cómo lo resuelve
   - Impacto en otros módulos
   - Screenshots si hay cambios visuales

4. **Vincula issues relacionados**
   ```markdown
   Closes #123
   Related to #456
   ```

### Revisión de Código

- Al menos 1 aprobación requerida
- Todos los checks de CI deben pasar
- No debe haber conflictos con main
- Debe cumplir con checklist de seguridad

### Después del Merge

```bash
# Actualiza tu repositorio local
git checkout main
git pull upstream main

# Elimina la rama local
git branch -d feature/tu-rama

# Elimina la rama remota (opcional)
git push origin --delete feature/tu-rama
```

## 🎨 Guías de Estilo

### C# Coding Standards

Seguimos las [C# Coding Conventions de Microsoft](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).

**Principales convenciones:**

```csharp
// ✅ Nombres de clases: PascalCase
public class UsuarioService { }

// ✅ Nombres de métodos: PascalCase
public async Task<Usuario> GetUsuarioAsync(int id) { }

// ✅ Nombres de variables: camelCase
var usuarioActivo = true;

// ✅ Nombres de constantes: PascalCase
public const int MaxLoginAttempts = 5;

// ✅ Nombres de interfaces: IPrefijo
public interface IPasswordHasher { }

// ✅ Async methods: Sufijo Async
public async Task<bool> ValidatePasswordAsync(string password) { }

// ✅ Usa var cuando el tipo es obvio
var usuario = new Usuario();
var lista = GetUsuarios();

// ❌ No usar var cuando no es obvio
// var result = Process(); // Qué es result?
ProcessResult result = Process(); // Claro!
```

### Clean Architecture Principles

```csharp
// ✅ Domain entities - No dependencias externas
public class Usuario
{
    private Usuario() { } // Constructor privado para EF Core
    
    public static Usuario Create(string username, string email)
    {
        // Factory method con validaciones
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username is required");
            
        return new Usuario { Username = username, Email = email };
    }
}

// ✅ Use Cases - Application layer
public class LoginCommand : IRequest<LoginResult>
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IUsuarioRepository _repository;
    private readonly IPasswordHasher _hasher;
    
    // Dependency injection
    public LoginCommandHandler(IUsuarioRepository repository, IPasswordHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }
}
```

### Security Best Practices

```csharp
// ✅ SIEMPRE: Validar inputs
public class CreateUsuarioCommandValidator : AbstractValidator<CreateUsuarioCommand>
{
    public CreateUsuarioCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().Length(3, 50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
    }
}

// ✅ SIEMPRE: Usar Entity Framework para queries
var usuarios = await _context.Usuarios
    .Where(u => u.IsActive && u.Email == email)
    .ToListAsync();

// ❌ NUNCA: Concatenar SQL
// var query = $"SELECT * FROM Usuarios WHERE Email = '{email}'";

// ✅ SIEMPRE: Hash passwords
var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

// ✅ SIEMPRE: Proteger endpoints
[Authorize(Roles = "Admin")]
[HttpGet]
public async Task<IActionResult> GetSensitiveData() { }
```

## 📁 Estructura del Proyecto

```
MiGenteEnLinea/
├── src/
│   ├── Core/
│   │   ├── Domain/              # Entidades, Value Objects
│   │   └── Application/         # Use Cases, DTOs, Validators
│   ├── Infrastructure/          # EF Core, Identity, External APIs
│   └── Presentation/
│       └── API/                 # Controllers, Middleware
├── tests/
│   ├── Domain.Tests/
│   ├── Application.Tests/
│   └── API.Tests/
├── docs/                        # Documentación adicional
└── .github/                     # GitHub config, workflows
```

## 💬 Commit Messages

Seguimos [Conventional Commits](https://www.conventionalcommits.org/).

### Formato

```
<tipo>(<scope>): <descripción corta>

[cuerpo opcional]

[footer opcional]
```

### Tipos

- `feat`: Nueva funcionalidad
- `fix`: Corrección de bug
- `security`: Fix de seguridad
- `refactor`: Refactoring de código
- `perf`: Mejora de performance
- `test`: Adición de tests
- `docs`: Cambios en documentación
- `style`: Cambios de formato (no afectan funcionalidad)
- `chore`: Tareas de mantenimiento
- `ci`: Cambios en CI/CD

### Ejemplos

```bash
# Feature simple
git commit -m "feat(auth): add JWT token refresh"

# Bug fix con descripción
git commit -m "fix(nomina): correct TSS deduction calculation

The previous calculation was not considering the salary cap
for TSS contributions, resulting in incorrect deductions.

Closes #234"

# Security fix
git commit -m "security(sql): prevent SQL injection in login

Replaced string concatenation with parameterized query
using Entity Framework.

BREAKING CHANGE: LoginService signature changed"

# Documentation
git commit -m "docs(readme): update installation instructions"
```

## 🧪 Testing Guidelines

### Unit Tests

```csharp
[Fact]
public async Task LoginCommand_WithValidCredentials_ReturnsSuccess()
{
    // Arrange
    var mockRepo = new Mock<IUsuarioRepository>();
    var mockHasher = new Mock<IPasswordHasher>();
    // ... setup mocks

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeTrue();
}
```

### Integration Tests

```csharp
public class LoginEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Login_WithValidCredentials_Returns200()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new LoginRequest { /* ... */ };

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

## 📊 Definition of Done

Un feature/fix se considera "Done" cuando:

- [ ] Código implementado y funcionando
- [ ] Tests escritos y pasando (min 80% coverage)
- [ ] Documentación actualizada
- [ ] Revisión de código aprobada
- [ ] CI/CD checks pasando
- [ ] No hay vulnerabilidades de seguridad
- [ ] Cumple con checklist de seguridad
- [ ] CHANGELOG actualizado
- [ ] Probado en ambiente de desarrollo

## 🤔 Preguntas?

Si tienes preguntas sobre el proceso de contribución:

1. Revisa la documentación existente
2. Busca en los issues cerrados
3. Abre un issue con la etiqueta "question"
4. Contacta al maintainer: rainierypenajrg@gmail.com

## 📚 Recursos Adicionales

- [Copilot Instructions](.github/copilot-instructions.md)
- [Security Policy](SECURITY.md)
- [Code of Conduct](CODE_OF_CONDUCT.md)
- [Project Wiki](https://github.com/RainieryPeniaJrg/MiGenteEnlinea/wiki)

---

**Gracias por contribuir a MiGente En Línea!** 🎉
