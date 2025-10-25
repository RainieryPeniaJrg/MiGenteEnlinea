# 🚀 LOTE 2: USER MANAGEMENT GAPS - PLAN DE IMPLEMENTACIÓN

**Fecha:** 24 de Octubre 2025, 20:30  
**Duración Estimada:** 18 horas  
**Prioridad:** 🔴 CRÍTICA  
**Estado:** 📋 ANÁLISIS COMPLETADO → IMPLEMENTACIÓN INICIADA  

---

## 📊 RESUMEN EJECUTIVO

### Análisis del Legacy (LoginService.asmx.cs)

**Métodos identificados:** 13 métodos totales

**✅ YA IMPLEMENTADOS en Clean (5/13):**

1. ✅ `login()` → `POST /api/auth/login` (LoginCommand)
2. ✅ `obtenerPerfil()` → `GET /api/auth/perfil/{userID}` (GetPerfilQuery)
3. ✅ `obtenerPerfilByEmail()` → `GET /api/auth/perfil/by-email` (GetPerfilByEmailQuery)
4. ✅ `obtenerCredenciales()` → `GET /api/auth/credenciales/{userID}` (GetCredencialesQuery)
5. ✅ `validarCorreo()` → `GET /api/auth/validate-email` (ValidarCorreoQuery)

**❌ GAPS IDENTIFICADOS (8/13):**

1. ❌ `borrarUsuario()` - Eliminar cuenta de usuario
2. ❌ `actualizarPerfil()` - Actualizar perfil con perfilesInfo + Cuentas
3. ❌ `actualizarPerfil1()` - Actualizar solo Cuentas
4. ❌ `agregarPerfilInfo()` - Agregar información de perfil extendida
5. ❌ `getPerfilByID()` - Obtener perfil por cuentaID
6. ❌ `getPerfilInfo()` - Obtener VPerfiles por GUID userID
7. ❌ **Password change flow** (no existe en Legacy - inferir de UI)
8. ❌ **Password recovery flow** (no existe en Legacy - inferir de UI)

### Funcionalidades NO CUBIERTAS en Legacy (Requeridas)

**Basado en estándares de autenticación moderna:**

- ❌ Cambio de contraseña con verificación de contraseña antigua
- ❌ Recuperación de contraseña por email
- ❌ Confirmación de cuenta por email token
- ❌ Refresh token para JWT (ya parcialmente implementado)

---

## 🎯 PLAN DE IMPLEMENTACIÓN (8 SUB-LOTES)

### SUB-LOTE 2.1: DeleteUser Command (2 horas)

**Legacy:** `borrarUsuario(string userID, int credencialID)`

**Implementar:**

1. **Command:** `Application/Features/Authentication/Commands/DeleteUser/`
   - `DeleteUserCommand.cs`
   - `DeleteUserCommandHandler.cs`
   - `DeleteUserCommandValidator.cs`

2. **Lógica de negocio:**
   - Validar que el usuario existe
   - Validar que el usuario actual tiene permiso (mismo userID o Admin)
   - **SOFT DELETE**: Marcar como inactivo en lugar de borrar físicamente
   - Remover sesiones activas (invalidate JWT refresh tokens)
   - Log de auditoría

3. **Endpoint:** `DELETE /api/auth/users/{userID}`

**Código Legacy:**
```csharp
public void borrarUsuario(string userID, int credencialID)
{
    using (var db = new migenteEntities())
    {
        var result = db.Credenciales.Where(a => a.userID == userID && a.id == credencialID).FirstOrDefault();
        db.Credenciales.Remove(result); // ⚠️ HARD DELETE - Cambiar a SOFT
        db.SaveChanges();
    }
}
```

**Mejoras en Clean:**
- ✅ Soft delete en lugar de hard delete
- ✅ Validación de permisos
- ✅ Cascade soft delete a entidades relacionadas
- ✅ Auditoría completa

---

### SUB-LOTE 2.2: UpdateProfile Command (3 horas)

**Legacy:** `actualizarPerfil(perfilesInfo info, Cuentas cuenta)` + `actualizarPerfil1(Cuentas cuenta)`

**Implementar:**

1. **Command:** `Application/Features/Authentication/Commands/UpdateProfile/`
   - `UpdateProfileCommand.cs`
   - `UpdateProfileCommandHandler.cs`
   - `UpdateProfileCommandValidator.cs`

2. **DTOs:**
   - `UpdateProfileDto` (nombre, apellido, teléfono, dirección, etc.)
   - `UpdateProfileExtendedDto` (incluye perfilesInfo)

3. **Lógica de negocio:**
   - Validar que el usuario existe
   - Validar permiso (mismo userID o Admin)
   - Actualizar Cuenta entity
   - Si hay perfilesInfo, actualizar/crear PerfilInfo entity
   - Validar email único si se cambia
   - Auditoría

4. **Endpoint:** `PUT /api/auth/profile`

**Código Legacy:**
```csharp
public bool actualizarPerfil(perfilesInfo info, Cuentas cuenta)
{
    using (var db = new migenteEntities())
    {
        db.Entry(info).State = System.Data.Entity.EntityState.Modified; // ⚠️ Uso de Entry()
        db.SaveChanges();
    }
    using (var db1 = new migenteEntities()) // ⚠️ 2 DbContexts - ineficiente
    {
        db1.Entry(cuenta).State = System.Data.Entity.EntityState.Modified;
        db1.SaveChanges();
    }
    return true;
}
```

**Mejoras en Clean:**
- ✅ Un solo DbContext con transacción
- ✅ Validación completa con FluentValidation
- ✅ Repository pattern
- ✅ Auditoría automática con interceptor

---

### SUB-LOTE 2.3: GetProfileById Query (1 hora)

**Legacy:** `getPerfilByID(int cuentaID)` + `getPerfilInfo(Guid userID)`

**Implementar:**

1. **Query:** `Application/Features/Authentication/Queries/GetProfileById/`
   - `GetProfileByIdQuery.cs`
   - `GetProfileByIdQueryHandler.cs`

2. **Lógica:**
   - Query con Include de relaciones (Credencial, PerfilInfo, Suscripcion)
   - Mapeo a DTO completo
   - Cache opcional

3. **Endpoint:** `GET /api/auth/profile/{id}`

**Código Legacy:**
```csharp
public Cuentas getPerfilByID(int cuentaID)
{
    using (var db = new migenteEntities())
    {
        return db.Cuentas.Where(x => x.cuentaID == cuentaID).FirstOrDefault(); // ⚠️ Sin Include
    }
}
```

---

### SUB-LOTE 2.4: AddProfileInfo Command (1 hora)

**Legacy:** `agregarPerfilInfo(perfilesInfo info)`

**Implementar:**

1. **Command:** `Application/Features/Authentication/Commands/AddProfileInfo/`
   - `AddProfileInfoCommand.cs`
   - `AddProfileInfoCommandHandler.cs`
   - `AddProfileInfoCommandValidator.cs`

2. **Lógica:**
   - Validar que Cuenta existe
   - Verificar que no existe PerfilInfo duplicado
   - Crear PerfilInfo entity
   - Auditoría

3. **Endpoint:** `POST /api/auth/profile-info`

---

### SUB-LOTE 2.5: ChangePassword Command (3 horas)

**⚠️ NO EXISTE EN LEGACY - Implementar desde cero**

**Implementar:**

1. **Command:** `Application/Features/Authentication/Commands/ChangePassword/`
   - `ChangePasswordCommand.cs`
   - `ChangePasswordCommandHandler.cs`
   - `ChangePasswordCommandValidator.cs`

2. **Request DTO:**
```csharp
public record ChangePasswordCommand : IRequest<Result>
{
    public string UserID { get; init; }
    public string CurrentPassword { get; init; }
    public string NewPassword { get; init; }
    public string ConfirmNewPassword { get; init; }
}
```

3. **Lógica de negocio:**
   - Validar usuario autenticado
   - Verificar contraseña actual con BCrypt.Verify()
   - Validar nueva contraseña (complejidad, no igual a la anterior)
   - Hash con BCrypt (work factor 12)
   - Actualizar Credencial.password
   - Invalidar refresh tokens (forzar re-login)
   - Enviar email de notificación
   - Auditoría

4. **Validaciones:**
   - CurrentPassword requerido
   - NewPassword: min 8 caracteres, 1 mayúscula, 1 minúscula, 1 número, 1 especial
   - NewPassword != CurrentPassword
   - NewPassword == ConfirmNewPassword

5. **Endpoint:** `POST /api/auth/change-password`

---

### SUB-LOTE 2.6: ForgotPassword Command (4 horas)

**⚠️ NO EXISTE EN LEGACY - Implementar desde cero**

**Implementar:**

1. **Commands:** `Application/Features/Authentication/Commands/PasswordReset/`
   - `ForgotPasswordCommand.cs` (solicitar reset)
   - `ForgotPasswordCommandHandler.cs`
   - `ResetPasswordCommand.cs` (confirmar reset con token)
   - `ResetPasswordCommandHandler.cs`

2. **Flow:**

**Paso 1: Usuario solicita reset**
```
User → POST /api/auth/forgot-password (email)
↓
Generate secure token (GUID + hash)
↓
Save token in PasswordResetTokens table (expira en 1 hora)
↓
Send email with reset link: /reset-password?token=XXX
↓
Return success (sin revelar si email existe - security)
```

**Paso 2: Usuario confirma reset**
```
User → POST /api/auth/reset-password (token, newPassword)
↓
Validate token (exists, not expired, not used)
↓
Hash new password con BCrypt
↓
Update Credencial.password
↓
Mark token as used
↓
Invalidate all refresh tokens
↓
Send confirmation email
```

3. **Tabla nueva requerida:**
```sql
CREATE TABLE PasswordResetTokens (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserID NVARCHAR(450) NOT NULL,
    Token NVARCHAR(256) NOT NULL UNIQUE,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ExpiresAt DATETIME2 NOT NULL,
    UsedAt DATETIME2 NULL,
    IsUsed BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (UserID) REFERENCES Credenciales(userID)
);
```

4. **Endpoints:**
   - `POST /api/auth/forgot-password` (solicitar)
   - `POST /api/auth/reset-password` (confirmar)

---

### SUB-LOTE 2.7: ConfirmAccount Command (2 horas)

**⚠️ Existe parcialmente en Legacy (activarperfil.aspx)**

**Implementar:**

1. **Command:** `Application/Features/Authentication/Commands/ConfirmAccount/`
   - `ConfirmAccountCommand.cs`
   - `ConfirmAccountCommandHandler.cs`

2. **Flow:**
```
Registration → Send email con token
↓
User clicks link: /confirm-account?token=XXX
↓
POST /api/auth/confirm-account (token)
↓
Validate token
↓
Set Credencial.activo = true
↓
Send welcome email
```

3. **Endpoint:** `POST /api/auth/confirm-account`

---

### SUB-LOTE 2.8: Testing & Integration (2 horas)

**Implementar:**

1. **Unit Tests:**
   - DeleteUserCommandHandlerTests
   - UpdateProfileCommandHandlerTests
   - ChangePasswordCommandHandlerTests
   - ForgotPasswordCommandHandlerTests
   - ResetPasswordCommandHandlerTests
   - ConfirmAccountCommandHandlerTests

2. **Integration Tests:**
   - Full flow testing con TestServer
   - Email sending validation (mock SMTP)
   - Token expiration testing
   - Security testing (unauthorized access)

3. **Swagger Testing:**
   - Test todos los endpoints nuevos
   - Validar responses
   - Documentar en `LOTE_2_USER_MANAGEMENT_COMPLETADO.md`

---

## 📋 CHECKLIST DE IMPLEMENTACIÓN

### Preparación (30 min)
- [ ] Crear carpetas en `Application/Features/Authentication/Commands/`
- [ ] Crear `PasswordResetTokens` table migration
- [ ] Actualizar `IApplicationDbContext` con nuevo DbSet

### Commands (12 horas)
- [ ] SUB-LOTE 2.1: DeleteUserCommand (2h)
- [ ] SUB-LOTE 2.2: UpdateProfileCommand (3h)
- [ ] SUB-LOTE 2.4: AddProfileInfoCommand (1h)
- [ ] SUB-LOTE 2.5: ChangePasswordCommand (3h)
- [ ] SUB-LOTE 2.6: ForgotPassword + ResetPassword (4h)
- [ ] SUB-LOTE 2.7: ConfirmAccountCommand (2h)

### Queries (1 hora)
- [ ] SUB-LOTE 2.3: GetProfileByIdQuery (1h)

### Testing (2 horas)
- [ ] SUB-LOTE 2.8: Unit tests (1h)
- [ ] SUB-LOTE 2.8: Integration tests (1h)

### Integración (2.5 horas)
- [ ] AuthController endpoints (30 min)
- [ ] DI registration (15 min)
- [ ] Swagger documentation (15 min)
- [ ] Testing completo (1h)
- [ ] Documentación final (30 min)

---

## 🚀 ORDEN DE EJECUCIÓN RECOMENDADO

**FASE 1 (5 horas):** Commands básicos
1. DeleteUserCommand (2h)
2. UpdateProfileCommand (3h)
3. AddProfileInfoCommand (1h) - puede ser paralelo

**FASE 2 (4 horas):** Password management
4. ChangePasswordCommand (3h)
5. ForgotPassword + ResetPassword (4h) - incluye migration

**FASE 3 (3 horas):** Account activation + Queries
6. ConfirmAccountCommand (2h)
7. GetProfileByIdQuery (1h)

**FASE 4 (2 horas):** Testing
8. Tests unitarios + integración

**FASE 5 (4 horas):** Integración + Validación
9. Controller endpoints
10. Swagger testing
11. Documentación

---

## ⚠️ DEPENDENCIAS CRÍTICAS

### Nuevas Tablas Requeridas
- ✅ `PasswordResetTokens` (SUB-LOTE 2.6)
- ✅ `EmailConfirmationTokens` (SUB-LOTE 2.7) - si no existe

### Servicios Externos Requeridos
- ✅ `IEmailService` - Ya existe en Infrastructure
- ❌ **SMTP configurado** - Verificar appsettings.json

### NuGet Packages Requeridos
- ✅ BCrypt.Net-Next 4.0.3 - Ya instalado
- ✅ FluentValidation 11.9.0 - Ya instalado
- ✅ MediatR 12.2.0 - Ya instalado

---

## 📊 MÉTRICAS DE ÉXITO

### Funcionalidad
- ✅ 8 Commands implementados y compilando
- ✅ 1 Query implementada
- ✅ 0 errores de compilación
- ✅ 100% coverage en tests unitarios (8 test classes)

### Seguridad
- ✅ Passwords hasheados con BCrypt work factor 12
- ✅ Tokens con expiración (1 hora)
- ✅ Validación de permisos en todos los endpoints
- ✅ Auditoría de cambios de password

### Performance
- ✅ Password reset flow <5s
- ✅ Profile update <2s
- ✅ Email sending asíncrono (no bloquea request)

---

## 🎯 SIGUIENTE PASO

**Ejecutar:** SUB-LOTE 2.1 (DeleteUserCommand)

**Comando para comenzar:**
```bash
# Crear estructura de carpetas
mkdir -p src/Core/MiGenteEnLinea.Application/Features/Authentication/Commands/DeleteUser
mkdir -p src/Core/MiGenteEnLinea.Application/Features/Authentication/Commands/UpdateProfile
mkdir -p src/Core/MiGenteEnLinea.Application/Features/Authentication/Commands/AddProfileInfo
mkdir -p src/Core/MiGenteEnLinea.Application/Features/Authentication/Commands/ChangePassword
mkdir -p src/Core/MiGenteEnLinea.Application/Features/Authentication/Commands/PasswordReset
mkdir -p src/Core/MiGenteEnLinea.Application/Features/Authentication/Commands/ConfirmAccount
mkdir -p src/Core/MiGenteEnLinea.Application/Features/Authentication/Queries/GetProfileById
```

**Después de crear carpetas:**
- Implementar DeleteUserCommand.cs
- Implementar DeleteUserCommandHandler.cs
- Implementar DeleteUserCommandValidator.cs
- Agregar endpoint en AuthController
- Probar en Swagger
- Documentar en LOTE_2_SUB_1_COMPLETADO.md
