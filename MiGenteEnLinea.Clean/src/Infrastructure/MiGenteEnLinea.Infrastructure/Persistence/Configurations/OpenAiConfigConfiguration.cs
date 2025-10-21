using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Configuracion;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuración de entidad OpenAiConfig
/// Tabla: OpenAi_Config
/// 
/// **SECURITY WARNING:**
/// Esta tabla contiene API keys sensibles. En un escenario ideal, esta configuración
/// debería estar en appsettings.json o Azure Key Vault, no en base de datos.
/// 
/// Esta configuración se mantiene por compatibilidad con Legacy.
/// </summary>
public class OpenAiConfigConfiguration : IEntityTypeConfiguration<OpenAiConfig>
{
    public void Configure(EntityTypeBuilder<OpenAiConfig> builder)
    {
        // Tabla
        builder.ToTable("OpenAi_Config");

        // Primary Key
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // Propiedades
        builder.Property(e => e.OpenAIApiKey)
            .HasColumnName("OpenAIApiKey")
            .HasMaxLength(500)
            .IsRequired(false); // Legacy permite null

        builder.Property(e => e.OpenAIApiUrl)
            .HasColumnName("OpenAIApiUrl")
            .HasMaxLength(500)
            .IsRequired(false); // Legacy permite null

        // Comentarios de documentación (si tu SQL Server lo soporta)
        builder.HasComment("Configuración del bot OpenAI para el 'abogado virtual'. ⚠️ Contiene información sensible.");
    }
}
