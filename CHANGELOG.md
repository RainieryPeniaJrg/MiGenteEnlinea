# Changelog

Todos los cambios notables a este proyecto serán documentados en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/),
y este proyecto adhiere a [Semantic Versioning](https://semver.org/lang/es/).

## [Unreleased]

### 🔄 En Progreso
- Migración a Clean Architecture (Onion Architecture)
- Migración de Database-First a Code-First Entity Framework Core
- Implementación de JWT Authentication con refresh tokens
- Sistema de hashing de contraseñas con BCrypt
- FluentValidation para validación de inputs
- Serilog para logging y auditoría
- Rate limiting para endpoints críticos

### 🔒 Security
- Remediación de vulnerabilidades críticas identificadas en auditoría Sept 2025
  - SQL Injection en múltiples controladores
  - Plain text passwords en base de datos
  - Missing authentication en endpoints críticos
  - Information disclosure en error messages
  - Hardcoded credentials en configuración

## [1.0.0] - 2025-10-12

### ✨ Added
- Configuración profesional de GitHub
  - Issue templates (Bug Report, Feature Request, Security Vulnerability)
  - Pull Request template con checklist completo
  - GitHub Actions workflows (CI/CD básico)
  - CONTRIBUTING.md con guías de contribución
  - CODE_OF_CONDUCT.md
  - SECURITY.md con política de seguridad
  - Copilot instructions mejoradas con plan de migración
- README.md profesional con documentación completa
- CHANGELOG.md para tracking de versiones
- .gitignore configurado para .NET Framework
- Web.config.example como template de configuración
- LICENSE (GPL) del repositorio

### 📝 Documentation
- Documentación completa de arquitectura actual
- Plan de migración de 6 semanas detallado
- Guías de seguridad para AI agents
- Ejemplos de código para security fixes
- Roadmap visual del proyecto
- Instrucciones de instalación y deployment

### 🏗️ Infrastructure
- Repositorio Git inicializado
- Configuración de GitHub profesional
- Estructura de proyecto lista para migración
- Templates para issues y PRs

## [Legacy - Web Forms] - Anterior a 2025-10-12

### Funcionalidades Existentes

#### Autenticación y Usuarios
- Login con Forms Authentication
- Registro de usuarios (Empleadores y Contratistas)
- Sistema de roles (tipo: 1=Empleador, 2=Contratista)
- Activación de cuenta por email
- Gestión de perfiles de usuario

#### Módulo de Empleadores
- Dashboard de empleador
- Gestión de empleados
  - Alta, baja, modificación de empleados
  - Fichas de empleado completas
  - Contratos de trabajo
- Sistema de nómina
  - Generación de nómina
  - Cálculo de deducciones TSS
  - Recibos de pago
  - Exportación a PDF
- Gestión de colaboradores
- Reportes y estadísticas

#### Módulo de Contratistas
- Dashboard de contratista
- Perfil profesional
- Sistema de calificaciones
- Historial de trabajos
- Búsqueda de oportunidades

#### Sistema de Suscripciones
- Planes para Empleadores
- Planes para Contratistas
- Proceso de checkout
- Integración con Cardnet (pasarela de pago RD)
- Gestión de vencimientos
- Renovación de suscripciones

#### Integraciones Externas
- Cardnet Payment Gateway
  - Procesamiento de pagos
  - Manejo de transacciones
  - Idempotency keys
- OpenAI Integration
  - Abogado Virtual
  - Asistencia legal automatizada
- iText PDF Generation
  - Contratos de trabajo
  - Recibos de nómina
  - Reportes

#### Documentación y Plantillas
- Templates HTML para contratos
- Templates de email
- Plantillas de impresión
- Términos y condiciones
- Formularios de autorización

### Tecnologías Utilizadas (Legacy)
- ASP.NET Web Forms (.NET Framework 4.7.2)
- Entity Framework 6 (Database-First)
- SQL Server (migenteV2 database)
- DevExpress v23.1 (UI Components)
- iText 8.0.5 (PDF Generation)
- RestSharp 112.1.0 (HTTP Client)
- Newtonsoft.Json 13.0.3 (JSON)
- SweetAlert2 (User Notifications)
- Bootstrap 5 (UI Framework)

### Vulnerabilidades Conocidas (Legacy)
⚠️ **CRÍTICO - Requiere remediación inmediata**
- SQL Injection en múltiples servicios
- Contraseñas en texto plano
- Falta de autenticación en endpoints
- Exposición de información sensible en errores
- Credenciales hardcodeadas
- CORS permisivo
- Sin rate limiting
- Sin validación sistemática de inputs
- Sin logging de auditoría
- Gestión insegura de sesiones
- Sin protección CSRF
- Sin enforcement de HTTPS
- Política de contraseñas débil
- Sin versionado de API
- Superficie de ataque amplia (arquitectura monolítica)

---

## Tipos de Cambios

- `Added` - Para nuevas funcionalidades
- `Changed` - Para cambios en funcionalidad existente
- `Deprecated` - Para funcionalidades que serán removidas
- `Removed` - Para funcionalidades removidas
- `Fixed` - Para bugs corregidos
- `Security` - Para vulnerabilidades corregidas

## Semantic Versioning

Este proyecto sigue Semantic Versioning (MAJOR.MINOR.PATCH):

- **MAJOR**: Cambios incompatibles con versiones anteriores
- **MINOR**: Nuevas funcionalidades compatibles con versiones anteriores
- **PATCH**: Correcciones de bugs compatibles con versiones anteriores

---

**Nota**: Las versiones anteriores al sistema de versionado formal están documentadas como "Legacy".

[Unreleased]: https://github.com/RainieryPeniaJrg/MiGenteEnlinea/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/RainieryPeniaJrg/MiGenteEnlinea/releases/tag/v1.0.0
