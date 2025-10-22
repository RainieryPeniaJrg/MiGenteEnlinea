using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MiGenteEnLinea.Application.Features.Authentication.Commands.UpdateProfileExtended;
using MiGenteEnLinea.Domain.Entities.Seguridad;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;
using Xunit;

namespace MiGenteEnLinea.Infrastructure.Tests.Authentication;

public class UpdateProfileExtendedCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdatePerfilAndPerfilesInfo_WhenExtendedDataProvided()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MiGenteDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var userId = Guid.NewGuid().ToString();
        using var context = new MiGenteDbContext(options);

        var perfil = Perfile.CrearPerfilEmpleador(
            userId,
            "Nombre original",
            "Apellido original",
            "original@email.com",
            "8090000000",
            "8090000001",
            "usuario-original");

        context.Perfiles.Add(perfil);
        await context.SaveChangesAsync();

        var perfilInfo = PerfilesInfo.CrearPerfilPersonaFisica(
            userId,
            "00112233445",
            direccion: "Direccion original",
            presentacion: "Presentacion original");
        perfilInfo.AsociarAPerfil(perfil.PerfilId);

        context.PerfilesInfos.Add(perfilInfo);
        await context.SaveChangesAsync();

        var handler = new UpdateProfileExtendedCommandHandler(
            context,
            NullLogger<UpdateProfileExtendedCommandHandler>.Instance);

        var command = new UpdateProfileExtendedCommand
        {
            UserId = userId,
            Nombre = "Nombre nuevo",
            Apellido = "Apellido nuevo",
            Email = "nuevo@email.com",
            Telefono1 = "8091234567",
            Telefono2 = string.Empty,
            Usuario = "usuario-nuevo",
            Identificacion = "55667788990",
            TipoIdentificacion = 2,
            NombreComercial = "Empresa Nueva SRL",
            Direccion = string.Empty,
            Presentacion = string.Empty,
            CedulaGerente = "01234567890"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        var updatedPerfil = await context.Perfiles
            .AsNoTracking()
            .FirstAsync(p => p.UserId == userId);

        updatedPerfil.Nombre.Should().Be("Nombre nuevo");
        updatedPerfil.Apellido.Should().Be("Apellido nuevo");
        updatedPerfil.Email.Should().Be("nuevo@email.com");
        updatedPerfil.Telefono1.Should().Be("8091234567");
        updatedPerfil.Telefono2.Should().Be(string.Empty);
        updatedPerfil.Usuario.Should().Be("usuario-nuevo");

        var updatedInfo = await context.PerfilesInfos
            .AsNoTracking()
            .FirstAsync(pi => pi.UserId == userId);

        updatedInfo.Identificacion.Should().Be("55667788990");
        updatedInfo.TipoIdentificacion.Should().Be(2);
        updatedInfo.NombreComercial.Should().Be("Empresa Nueva SRL");
        updatedInfo.Direccion.Should().Be(string.Empty);
        updatedInfo.Presentacion.Should().Be(string.Empty);
        updatedInfo.CedulaGerente.Should().Be("01234567890");
    }
}
