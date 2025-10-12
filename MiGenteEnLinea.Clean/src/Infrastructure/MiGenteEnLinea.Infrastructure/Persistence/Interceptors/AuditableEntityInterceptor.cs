using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Interceptor que actualiza automáticamente los campos de auditoría
/// antes de guardar cambios en la base de datos.
/// </summary>
public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;

    public AuditableEntityInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext? context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker.Entries<AuditableEntity>();
        var currentUserId = _currentUserService.UserId ?? "System";
        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = currentUserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = currentUserId;
                    break;
            }
        }
    }
}

/// <summary>
/// Servicio para obtener el usuario actual del contexto HTTP
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// ID del usuario autenticado (null si no está autenticado)
    /// </summary>
    string? UserId { get; }

    /// <summary>
    /// Email del usuario autenticado
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Indica si hay un usuario autenticado
    /// </summary>
    bool IsAuthenticated { get; }
}
