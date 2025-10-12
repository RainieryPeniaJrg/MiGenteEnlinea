<%@ Page Title="" Language="C#" MasterPageFile="~/Platform.Master" Async="true" AutoEventWireup="true" CodeBehind="abogadoVirtual.aspx.cs" Inherits="MiGente_Front.abogadoVirtual" %>

<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body {
    background-color: #f8f9fa;
    color: #343a40;
    font-family: Arial, sans-serif;
}

/* Hero section */
.hero-section {
    background: url('https://legalsolutions.thomsonreuters.co.uk/content/dam/openweb/images/uk-legal-solutions/stock/Hero/45637676-1280x518.jpg.transform/hero-s/q90/image.jpg') no-repeat center center;
    background-size: cover;
    color: white;
    padding: 100px 0;
    text-align: center;
}

.hero-section h1 {
    font-size: 3rem;
    font-weight: bold;
}

.hero-section p {
    font-size: 1.25rem;
    margin-top: 20px;
}

/* Chatbox Styling */
.chat-container {
    background-color: #ffffff;
    border-radius: 10px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    margin-top: 50px;
    padding: 20px;
    width: 100%;
    max-width: 900px;
    margin-left: auto;
    margin-right: auto;
    display: flex;
    flex-direction: column;
}

.chat-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 2px solid #f0f0f0;
    padding-bottom: 10px;
    margin-bottom: 15px;
}

.chat-header h5 {
    margin: 0;
}

.chat-header p {
    margin: 0;
    font-size: 0.9rem;
    color: #6c757d;
}

.chat-header img {
    width: 40px;
    height: 40px;
    border-radius: 50%;
}

.disclaimer {
    margin-top: 20px;
    padding: 15px;
    background-color: #f8f9fa;
    border-radius: 10px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.disclaimer h5 {
    margin-top: 0;
}

.chat-messages {
    max-height: 300px;
    overflow-y: auto;
    padding: 10px;
    border-bottom: 1px solid #f0f0f0;
}

.chat-footer {
    display: flex;
    align-items: center;
}

.chat-footer input {
    width: 80%;
    padding: 10px;
    border-radius: 5px;
    border: 1px solid #ccc;
}

.chat-footer button {
    width: 15%;
    padding: 10px;
    margin-left: 5px;
    background-color: #007bff;
    color: white;
    border: none;
    border-radius: 5px;
}

.chat-footer button:hover {
    background-color: #0056b3;
}

/* Flexbox Layout for Disclaimer */
.chat-header, .disclaimer {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
}

.chat-header {
    margin-bottom: 20px;
}

.lawyer-avatar {
    width: 50px;
    height: 50px;
    border-radius: 50%;
}
.disclaimer {
    display: flex;
    flex-direction: column; /* Coloca todos los elementos en una sola columna */
    gap: 1rem; /* Espacio entre cada párrafo */
}

.disclaimer h5 {
    margin-bottom: 10px; /* Ajusta el espacio debajo del título */
}

.disclaimer p {
    margin: 0;
    padding: 0;
}
.message-lawyer {
    background-color: #f1f1f1; /* Fondo gris claro */
    padding: 10px;
    margin: 5px 0;
    border-radius: 8px;
    max-width: 70%;
    margin-left: 20px;
}

.message-user {
    background-color: #007bff; /* Fondo bg-primary */
    color: white;
    padding: 10px;
    margin: 5px 0;
    border-radius: 8px;
    max-width: 70%;
    margin-right: 20px;
    text-align: right;
}
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager runat="server" ID="sm"></asp:ScriptManager>
    <div class="pagetitle">
        <h1>Abogado Virtual</h1>
        <nav>
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Consultas en Línea</a></li>
            </ol>
        </nav>
    </div>

    <!-- Hero Section -->
    <div class="hero-section">
        <h1>Consulta con tu abogado virtual</h1>
        <p>Especialista en Derecho Laboral</p>
    </div>

    <!-- Feature Section -->
     <section class="feature-section">
        <div class="container">
            <div class="row text-center">
                <div class="col-md-4">
                    <div class="card feature-card">
                        <div class="card-body">
                            <div class="feature-icon">
                                <i class="bi bi-briefcase"></i>
                            </div>
                            <h5 class="feature-title">Asesoría Legal al Instante</h5>
                            <p class="feature-description">Obtén respuestas rápidas y confiables sobre temas laborales de la República Dominicana.</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card feature-card">
                        <div class="card-body">
                            <div class="feature-icon">
                                <i class="bi bi-chat-dots"></i>
                            </div>
                            <h5 class="feature-title">Conversaciones Simples</h5>
                            <p class="feature-description">Interactúa con nuestro abogado virtual mediante preguntas sencillas y obtén soluciones claras.</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card feature-card">
                        <div class="card-body">
                            <div class="feature-icon">
                                <i class="bi bi-file-earmark-text"></i>
                            </div>
                            <h5 class="feature-title">Cumplimiento Legal Garantizado</h5>
                            <p class="feature-description">Recibe orientación sobre el Código de Trabajo y la Ley 16-92 con seguridad y fiabilidad.</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- Chatbox Section -->
   <div class="chat-container">
    <!-- Header -->
    <div class="chat-header">
        <div class="header-left">
            <h5>Abogado Virtual</h5>
            <p class="mb-0">Especialista en Derecho Laboral</p>
        </div>
        <img src="Images/lawyer-avatar.png" alt="Abogado Virtual" class="lawyer-avatar">
    </div>

    <!-- Disclaimer -->
 
       <div class="disclaimer">
    <h5>Aviso Legal</h5>
    <p style="font-size:smaller">El <strong>Abogado Virtual</strong> proporcionado en este sitio web es una herramienta automatizada diseñada para ofrecer orientación general en temas legales relacionados con el Derecho Laboral. Los consejos proporcionados por este servicio no deben considerarse como asesoría legal personalizada ni reemplazan la consulta con un abogado licenciado.</p>
    
    <p style="font-size:smaller"><strong>Aviso Importante:</strong> Cualquier interacción a través de este chat no establece una relación abogado-cliente. Si tienes un caso específico o necesitas asesoría detallada, te recomendamos que contactes a un abogado especializado en la materia.</p>

    <p style="font-size:smaller">Este servicio está diseñado para ayudar a resolver dudas generales, pero no debe usarse para resolver problemas legales complejos o urgentes. Los resultados proporcionados no garantizan el éxito en tu caso, y la información aquí brindada es de carácter informativo y general.</p>

    <p style="font-size:smaller">Al utilizar el <strong>Abogado Virtual</strong>, aceptas que no se crea una relación legal formal entre tú y el proveedor de este servicio, y que la información que compartas en este chat no es confidencial ni privilegiada.</p>
</div>

    <!-- Messages -->
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="chat-messages" enableviewstate="true" id="chatMessages" runat="server">
            <!-- Mensajes serán añadidos dinámicamente -->
            <!-- Ejemplo de cómo se puede añadir un mensaje del abogado y del usuario -->
           
         
        </div>

        <!-- Loading Animation -->
        <asp:UpdateProgress AssociatedUpdatePanelID="UpdatePanel1" runat="server">
            <ProgressTemplate>
                <div id="row">
                    <label>Espera un momento...</label>
                    <img width="60" src="https://media.tenor.com/53JWSqJt16QAAAAM/waiting-texting.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <!-- Footer -->
        <div class="chat-footer row">
            <div class="col col-md-8 col-12">
                <dx:BootstrapTextBox ID="txtUserMessage" ClientIDMode="AutoID" ClientEnabled="true" runat="server" NullText="Escribe tu consulta aquí"></dx:BootstrapTextBox>
            </div>
            <div class="col col-md-4 col-12">
                <asp:Button ID="btnSendMessage" runat="server" Text="Enviar" CssClass="btn btn-primary" OnClick="btnSendMessage_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
</div>

</asp:Content>
