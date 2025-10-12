# 🔒 Política de Seguridad

## Versiones Soportadas

Actualmente estamos dando soporte de seguridad a las siguientes versiones:

| Versión | Soportada          | Estado              |
| ------- | ------------------ | ------------------- |
| 2.0.x   | ✅ Si              | En desarrollo activo |
| 1.x.x   | ⚠️ Soporte limitado | Legacy (Web Forms)  |

## 🚨 Reportar una Vulnerabilidad

La seguridad de MiGente En Línea es nuestra máxima prioridad. Si descubres una vulnerabilidad de seguridad, por favor sigue estos pasos:

### 1. Reporte Confidencial (Recomendado para vulnerabilidades críticas)

**NO** abras un issue público si la vulnerabilidad es crítica.

Envía un correo a: **rainierypenajrg@gmail.com** con:
- Descripción detallada de la vulnerabilidad
- Pasos para reproducir
- Impacto potencial
- Versión afectada
- Cualquier información adicional relevante

### 2. Tiempo de Respuesta

- **Acuse de recibo**: Dentro de 48 horas
- **Evaluación inicial**: Dentro de 5 días hábiles
- **Plan de remediación**: Depende de la severidad
  - Crítico: 24-72 horas
  - Alto: 1-2 semanas
  - Medio: 2-4 semanas
  - Bajo: Siguiente sprint

### 3. Divulgación Coordinada

Seguimos el principio de **divulgación responsable**:
- Trabajaremos contigo para entender y validar el reporte
- Desarrollaremos y probaremos el fix
- Coordinaremos la divulgación pública
- Te acreditaremos en el security advisory (si lo deseas)

## 🎯 Vulnerabilidades Conocidas

Actualmente estamos trabajando activamente en remediar las siguientes vulnerabilidades identificadas en la auditoría de Septiembre 2025:

### 🔴 Críticas (En remediación activa)

1. **SQL Injection** - Multiple instances
   - **Estado**: 🔄 En progreso
   - **ETA**: Sprint 1 (Semanas 1-2)
   - **Mitigación temporal**: Validación de inputs en capa de presentación

2. **Plain Text Passwords**
   - **Estado**: 🔄 En progreso
   - **ETA**: Sprint 1 (Semana 1)
   - **Mitigación temporal**: Limitar acceso a base de datos

3. **Missing Authentication**
   - **Estado**: 📋 Planificado
   - **ETA**: Sprint 1 (Semana 2)
   - **Mitigación temporal**: Rate limiting en endpoints críticos

4. **Information Disclosure**
   - **Estado**: 🔄 En progreso
   - **ETA**: Sprint 1 (Semana 2)
   - **Mitigación temporal**: Logging configurado para no exponer detalles

5. **Hardcoded Credentials**
   - **Estado**: ✅ Resuelto parcialmente
   - **Nota**: Credentials movidas a configuración, próximo paso: Azure Key Vault

### 🟡 Altas (Planificadas)

6. **Permissive CORS** - En evaluación para Sprint 2
7. **No Rate Limiting** - Parcialmente implementado
8. **Missing Input Validation** - En desarrollo (FluentValidation)
9. **No Audit Logging** - En desarrollo (Serilog)
10. **Insecure Session Management** - Planificado para migración JWT

## 🛡️ Mejores Prácticas de Seguridad

Si contribuyes al proyecto, asegúrate de seguir estas prácticas:

### Código Seguro

```csharp
// ✅ HACER: Usar Entity Framework con parametrización
var user = await _context.Users
    .Where(u => u.Username == username)
    .FirstOrDefaultAsync();

// ❌ NUNCA: Concatenar strings en SQL
var query = $"SELECT * FROM Users WHERE Username = '{username}'";
```

### Autenticación y Autorización

```csharp
// ✅ HACER: Proteger endpoints
[Authorize(Roles = "Empleador,Contratista")]
[HttpGet]
public async Task<IActionResult> GetSensitiveData() { }

// ❌ NUNCA: Dejar endpoints sin protección
[HttpGet]
public async Task<IActionResult> GetSensitiveData() { }
```

### Manejo de Contraseñas

```csharp
// ✅ HACER: Hashear con BCrypt
string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

// ❌ NUNCA: Guardar en texto plano
usuario.Password = password;
```

### Manejo de Errores

```csharp
// ✅ HACER: Ocultar detalles técnicos
catch (Exception ex)
{
    _logger.LogError(ex, "Error processing request");
    return StatusCode(500, new { message = "An error occurred" });
}

// ❌ NUNCA: Exponer stack traces
catch (Exception ex)
{
    return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
}
```

## 🔍 Auditorías de Seguridad

### Auditoría Manual
- **Frecuencia**: Antes de cada release mayor
- **Última auditoría**: Septiembre 2025
- **Próxima auditoría**: Marzo 2026

### Análisis Automatizado
- **Herramientas**: SonarQube, OWASP Dependency Check
- **Frecuencia**: En cada PR (GitHub Actions)
- **Umbrales**: Zero critical, max 5 high severity

### Penetration Testing
- **Frecuencia**: Anual
- **Última prueba**: Pendiente
- **Próxima prueba**: Q2 2026

## 📋 Checklist de Seguridad para Releases

Antes de cada release, verificar:

- [ ] Todas las vulnerabilidades críticas resueltas
- [ ] Scan de seguridad automatizado pasando
- [ ] Dependencias actualizadas (sin CVEs conocidos)
- [ ] Secrets rotados si es necesario
- [ ] Logs de seguridad revisados
- [ ] Backup de base de datos realizado
- [ ] Plan de rollback documentado
- [ ] Comunicación de cambios de seguridad a stakeholders

## 🆘 Incidente de Seguridad

Si detectas un incidente de seguridad activo:

1. **Notificar inmediatamente**: rainierypenajrg@gmail.com
2. **Documentar**: Captura evidencia sin comprometer más el sistema
3. **Contener**: Si es posible, toma medidas para limitar el impacto
4. **NO**: Elimines logs o evidencia
5. **Esperar**: Instrucciones del equipo de seguridad

## 📚 Recursos de Seguridad

- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [CWE Top 25](https://cwe.mitre.org/top25/)
- [Microsoft Security Best Practices](https://docs.microsoft.com/en-us/security/)
- [NIST Cybersecurity Framework](https://www.nist.gov/cyberframework)

## 🏆 Hall of Fame

Agradecimientos a investigadores de seguridad que han reportado vulnerabilidades responsablemente:

<!-- Los nombres serán agregados aquí con su consentimiento -->

## 📧 Contacto

Para consultas relacionadas con seguridad:
- **Email**: rainierypenajrg@gmail.com
- **Respuesta esperada**: 48 horas

---

**Última actualización**: Octubre 2025  
**Próxima revisión**: Enero 2026
