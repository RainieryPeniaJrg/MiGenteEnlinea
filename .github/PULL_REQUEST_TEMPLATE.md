## 📋 Descripción

<!-- Proporciona una descripción clara y concisa de los cambios -->

## 🎯 Tipo de Cambio

<!-- Marca con 'x' el tipo de cambio que aplica -->

- [ ] 🐛 Bug fix (cambio que corrige un issue)
- [ ] ✨ New feature (cambio que agrega funcionalidad)
- [ ] 💥 Breaking change (fix o feature que causaría que funcionalidad existente no funcione como se esperaba)
- [ ] 🔒 Security fix (cambio que corrige una vulnerabilidad de seguridad)
- [ ] 📝 Documentation (actualización de documentación)
- [ ] 🎨 UI/UX (cambios en interfaz de usuario)
- [ ] ⚡ Performance (mejora de rendimiento)
- [ ] ♻️ Refactor (cambio que ni corrige ni agrega funcionalidad)
- [ ] ✅ Test (agregar o corregir tests)
- [ ] 🔧 Chore (cambios en build, configuración, dependencias)

## 🔗 Issues Relacionados

<!-- Menciona los issues que este PR cierra o está relacionado -->

Closes #
Related to #

## 📸 Screenshots (si aplica)

<!-- Agrega screenshots si hay cambios visuales -->

## 🧪 Cómo Ha Sido Probado?

<!-- Describe las pruebas que ejecutaste para verificar tus cambios -->

- [ ] Unit Tests
- [ ] Integration Tests
- [ ] Manual Testing
- [ ] Testing en ambiente de desarrollo

**Escenarios de Prueba:**
- 
- 

## ✅ Checklist de Seguridad

<!-- Verifica que tu código cumple con los estándares de seguridad -->

- [ ] **No hay SQL Injection**: Uso Entity Framework o consultas parametrizadas
- [ ] **Contraseñas protegidas**: Passwords hasheados con BCrypt (work factor 12+)
- [ ] **Autenticación/Autorización**: Endpoints protegidos con `[Authorize]`
- [ ] **Validación de Input**: FluentValidation implementado
- [ ] **Manejo de Errores**: No expone información sensible en errores
- [ ] **Logging**: Eventos de seguridad registrados apropiadamente
- [ ] **No hay secretos**: Credenciales en configuración segura (no hardcoded)

## 🏗️ Checklist de Arquitectura

<!-- Verifica que tu código sigue los principios de Clean Architecture -->

- [ ] Sigue principios de Clean Architecture
- [ ] Usa Dependency Injection apropiadamente
- [ ] Entidades de dominio encapsuladas correctamente
- [ ] Separación de concerns mantenida
- [ ] Interfaces usadas para abstracción
- [ ] Cumple con principios SOLID

## 📋 Checklist General

<!-- Verificaciones generales antes de merge -->

- [ ] Mi código sigue las guías de estilo del proyecto
- [ ] He realizado self-review de mi código
- [ ] He comentado mi código, especialmente en áreas difíciles de entender
- [ ] He actualizado la documentación correspondiente
- [ ] Mis cambios no generan nuevos warnings
- [ ] He agregado tests que prueban que mi fix es efectivo o que mi feature funciona
- [ ] Tests unitarios nuevos y existentes pasan localmente
- [ ] He actualizado el CHANGELOG.md
- [ ] He verificado que no hay conflictos con la rama principal

## 🔄 Migración de Base de Datos

<!-- Si este PR incluye cambios en la base de datos -->

- [ ] Migration script incluida
- [ ] Rollback script incluida
- [ ] Script probado localmente
- [ ] Backup recomendado antes de aplicar
- [ ] Downtime esperado: **[especificar]**

## 📚 Documentación Actualizada

<!-- Marca qué documentación fue actualizada -->

- [ ] README.md
- [ ] CHANGELOG.md
- [ ] API Documentation
- [ ] Wiki
- [ ] Copilot Instructions
- [ ] Inline code comments

## 🎭 Impacto en Módulos

<!-- Indica qué módulos son afectados por este PR -->

**Módulos Modificados:**
- [ ] Autenticación
- [ ] Empleadores
- [ ] Contratistas
- [ ] Nómina
- [ ] Pagos (Cardnet)
- [ ] Suscripciones
- [ ] Abogado Virtual
- [ ] Reportes/PDFs
- [ ] Dashboard
- [ ] Base de Datos
- [ ] Infraestructura

## 🚀 Deployment Notes

<!-- Instrucciones especiales para deployment -->

**Pasos especiales requeridos para deployment:**
1. 
2. 

**Variables de ambiente nuevas o modificadas:**
- 

**Dependencias nuevas:**
- 

## 👥 Reviewers

<!-- Menciona a quiénes debe revisar este PR -->

@RainieryPeniaJrg 

## 📝 Notas Adicionales

<!-- Cualquier información adicional que los reviewers deban saber -->

---

**Por favor, asegúrate de que el PR:**
- ✅ Pasa todos los checks de CI/CD
- ✅ Ha sido probado exhaustivamente
- ✅ Sigue los estándares de seguridad del proyecto
- ✅ No introduce regresiones
- ✅ Tiene una descripción clara y completa
