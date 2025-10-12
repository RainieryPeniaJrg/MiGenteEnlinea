using MiGenteEnLinea.Domain.Common;
using MiGenteEnLinea.Domain.Events.Pagos;

namespace MiGenteEnLinea.Domain.Entities.Pagos;

/// <summary>
/// Representa la configuración de la pasarela de pagos (Cardnet) para procesar transacciones.
/// Incluye URLs, credenciales de comercio, y modo de operación (test/producción).
/// </summary>
public sealed class PaymentGateway : AggregateRoot
{
    /// <summary>
    /// Identificador único de la configuración de la pasarela.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Indica si la pasarela está en modo de prueba (test) o producción.
    /// true = Test, false = Producción
    /// </summary>
    public bool ModoTest { get; private set; }

    /// <summary>
    /// URL del endpoint de producción para procesar pagos.
    /// </summary>
    public string UrlProduccion { get; private set; } = string.Empty;

    /// <summary>
    /// URL del endpoint de prueba para procesar pagos.
    /// </summary>
    public string UrlTest { get; private set; } = string.Empty;

    /// <summary>
    /// Identificador del comercio asignado por Cardnet.
    /// </summary>
    public string MerchantId { get; private set; } = string.Empty;

    /// <summary>
    /// Identificador de la terminal asignada por Cardnet.
    /// </summary>
    public string TerminalId { get; private set; } = string.Empty;

    /// <summary>
    /// Indica si la configuración está activa y disponible para uso.
    /// </summary>
    public bool Activa { get; private set; }

    // Constructor privado para EF Core
    private PaymentGateway() { }

    /// <summary>
    /// Crea una nueva configuración de pasarela de pagos.
    /// </summary>
    /// <param name="urlProduccion">URL del endpoint de producción</param>
    /// <param name="urlTest">URL del endpoint de prueba</param>
    /// <param name="merchantId">ID del comercio</param>
    /// <param name="terminalId">ID de la terminal</param>
    /// <param name="modoTest">Si inicia en modo test</param>
    /// <returns>Nueva instancia de PaymentGateway</returns>
    /// <exception cref="ArgumentException">Si los parámetros no son válidos</exception>
    public static PaymentGateway Create(
        string urlProduccion,
        string urlTest,
        string merchantId,
        string terminalId,
        bool modoTest = true)
    {
        if (string.IsNullOrWhiteSpace(urlProduccion))
            throw new ArgumentException("La URL de producción es requerida", nameof(urlProduccion));

        if (string.IsNullOrWhiteSpace(urlTest))
            throw new ArgumentException("La URL de test es requerida", nameof(urlTest));

        if (string.IsNullOrWhiteSpace(merchantId))
            throw new ArgumentException("El Merchant ID es requerido", nameof(merchantId));

        if (string.IsNullOrWhiteSpace(terminalId))
            throw new ArgumentException("El Terminal ID es requerido", nameof(terminalId));

        if (urlProduccion.Length > 150)
            throw new ArgumentException("La URL de producción no puede exceder 150 caracteres", nameof(urlProduccion));

        if (urlTest.Length > 150)
            throw new ArgumentException("La URL de test no puede exceder 150 caracteres", nameof(urlTest));

        if (merchantId.Length > 20)
            throw new ArgumentException("El Merchant ID no puede exceder 20 caracteres", nameof(merchantId));

        if (terminalId.Length > 20)
            throw new ArgumentException("El Terminal ID no puede exceder 20 caracteres", nameof(terminalId));

        // Validar que sean URLs válidas
        if (!Uri.TryCreate(urlProduccion, UriKind.Absolute, out _))
            throw new ArgumentException("La URL de producción no es válida", nameof(urlProduccion));

        if (!Uri.TryCreate(urlTest, UriKind.Absolute, out _))
            throw new ArgumentException("La URL de test no es válida", nameof(urlTest));

        var gateway = new PaymentGateway
        {
            UrlProduccion = urlProduccion.Trim(),
            UrlTest = urlTest.Trim(),
            MerchantId = merchantId.Trim(),
            TerminalId = terminalId.Trim(),
            ModoTest = modoTest,
            Activa = true
        };

        gateway.RaiseDomainEvent(new PaymentGatewayCreadoEvent(
            gateway.Id,
            gateway.MerchantId,
            gateway.ModoTest));

        return gateway;
    }

    /// <summary>
    /// Actualiza las URLs de la pasarela de pagos.
    /// </summary>
    public void ActualizarUrls(string urlProduccion, string urlTest)
    {
        if (string.IsNullOrWhiteSpace(urlProduccion))
            throw new ArgumentException("La URL de producción es requerida", nameof(urlProduccion));

        if (string.IsNullOrWhiteSpace(urlTest))
            throw new ArgumentException("La URL de test es requerida", nameof(urlTest));

        if (urlProduccion.Length > 150)
            throw new ArgumentException("La URL de producción no puede exceder 150 caracteres", nameof(urlProduccion));

        if (urlTest.Length > 150)
            throw new ArgumentException("La URL de test no puede exceder 150 caracteres", nameof(urlTest));

        if (!Uri.TryCreate(urlProduccion, UriKind.Absolute, out _))
            throw new ArgumentException("La URL de producción no es válida", nameof(urlProduccion));

        if (!Uri.TryCreate(urlTest, UriKind.Absolute, out _))
            throw new ArgumentException("La URL de test no es válida", nameof(urlTest));

        UrlProduccion = urlProduccion.Trim();
        UrlTest = urlTest.Trim();
    }

    /// <summary>
    /// Actualiza las credenciales del comercio.
    /// </summary>
    public void ActualizarCredenciales(string merchantId, string terminalId)
    {
        if (string.IsNullOrWhiteSpace(merchantId))
            throw new ArgumentException("El Merchant ID es requerido", nameof(merchantId));

        if (string.IsNullOrWhiteSpace(terminalId))
            throw new ArgumentException("El Terminal ID es requerido", nameof(terminalId));

        if (merchantId.Length > 20)
            throw new ArgumentException("El Merchant ID no puede exceder 20 caracteres", nameof(merchantId));

        if (terminalId.Length > 20)
            throw new ArgumentException("El Terminal ID no puede exceder 20 caracteres", nameof(terminalId));

        MerchantId = merchantId.Trim();
        TerminalId = terminalId.Trim();

        RaiseDomainEvent(new PaymentGatewayCredencialesActualizadasEvent(Id, MerchantId, TerminalId));
    }

    /// <summary>
    /// Cambia al modo de prueba (test).
    /// </summary>
    public void CambiarAModoTest()
    {
        if (ModoTest)
            throw new InvalidOperationException("La pasarela ya está en modo test");

        ModoTest = true;
        RaiseDomainEvent(new PaymentGatewayModoTestActivadoEvent(Id));
    }

    /// <summary>
    /// Cambia al modo de producción.
    /// </summary>
    public void CambiarAModoProduccion()
    {
        if (!ModoTest)
            throw new InvalidOperationException("La pasarela ya está en modo producción");

        ModoTest = false;
        RaiseDomainEvent(new PaymentGatewayModoProduccionActivadoEvent(Id, MerchantId));
    }

    /// <summary>
    /// Activa la configuración de la pasarela.
    /// </summary>
    public void Activar()
    {
        if (Activa)
            throw new InvalidOperationException("La pasarela ya está activa");

        Activa = true;
    }

    /// <summary>
    /// Desactiva la configuración de la pasarela (no se podrán procesar pagos).
    /// </summary>
    public void Desactivar()
    {
        if (!Activa)
            throw new InvalidOperationException("La pasarela ya está desactivada");

        Activa = false;
        RaiseDomainEvent(new PaymentGatewayDesactivadoEvent(Id));
    }

    /// <summary>
    /// Obtiene la URL activa según el modo actual (test o producción).
    /// </summary>
    public string ObtenerUrlActiva() => ModoTest ? UrlTest : UrlProduccion;

    /// <summary>
    /// Obtiene el modo de operación como texto.
    /// </summary>
    public string ObtenerModoTexto() => ModoTest ? "Modo Test" : "Modo Producción";

    /// <summary>
    /// Verifica si la pasarela está lista para procesar pagos.
    /// </summary>
    public bool EstaListaParaProcesar()
    {
        return Activa
            && !string.IsNullOrWhiteSpace(MerchantId)
            && !string.IsNullOrWhiteSpace(TerminalId)
            && !string.IsNullOrWhiteSpace(ObtenerUrlActiva());
    }

    /// <summary>
    /// Obtiene un resumen de la configuración.
    /// </summary>
    public string ObtenerResumen()
    {
        var estado = Activa ? "Activa" : "Inactiva";
        var modo = ObtenerModoTexto();
        return $"Gateway {Id} - {modo} - {estado} | Merchant: {MerchantId} | Terminal: {TerminalId}";
    }
}
