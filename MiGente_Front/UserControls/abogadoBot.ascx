<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="abogadoBot.ascx.cs" Inherits="MiGente_Front.UserControls.abogadoBot" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v23.1, Version=23.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<!-- Contenedor del chat y del icono flotante -->
<div id="chatContainerWrapper" class="chat-container-wrapper">
    <!-- Icono de chat flotante -->
    <div id="chatIcon" class="chat-icon" onclick="toggleChat()">
        <img src="Images/btnChat2(1).png" alt="Chat" width="200" />
    </div>

    <!-- Chat Bubble Container -->
    <div id="chatContainer" class="chat-bubble-container" style="display:none;" onclick="event.stopPropagation();">
        <div class="chat-bubble">
            <!-- Header -->
            <div class="chat-header">
                <div>
                    <h5>Abogado Virtual</h5>
                    <p class="mb-0">Especialista en Derecho Laboral</p>
                </div>
                <img src="Images/lawyer-avatar.png" alt="Abogado Virtual">
            </div>

            <!-- Messages -->
            <div class="chat-messages" id="chatMessages">
                <!-- Mensajes serán añadidos dinámicamente -->
            </div>

            <!-- Footer -->
            <div class="chat-footer">
                <input type="text" id="txtUserMessage" placeholder="Escribe tu consulta aquí" class="form-control" />
                <button id="btnSendMessage" class="btn btn-primary">Enviar</button>
            </div>
        </div>
    </div>
</div>

<script>
    // Alternar entre mostrar y ocultar el chat
    function toggleChat() {
        const chatContainer = document.getElementById('chatContainer');
        const chatIcon = document.getElementById('chatIcon');

        if (chatContainer.style.display === 'none') {
            chatContainer.style.display = 'block';
            chatIcon.style.display = 'none'; // Ocultar el ícono cuando el chat está visible
        } else {
            chatContainer.style.display = 'none';
            chatIcon.style.display = 'block'; // Mostrar el ícono cuando el chat está oculto
        }
    }

    // Detectar clics fuera del chat y ocultarlo
    document.addEventListener('click', function (event) {
        const chatContainer = document.getElementById('chatContainer');
        const chatIcon = document.getElementById('chatIcon');
        const chatWrapper = document.getElementById('chatContainerWrapper');

        if (!chatWrapper.contains(event.target)) {
            chatContainer.style.display = 'none';
            chatIcon.style.display = 'block';
        }
    });

    // Enviar mensaje al backend y mostrar respuesta
    document.getElementById('btnSendMessage').addEventListener('click', async (event) => {
        event.preventDefault(); // Detiene el comportamiento predeterminado

        const userMessage = document.getElementById('txtUserMessage').value.trim();

        if (userMessage) {
            // Añadir mensaje del usuario
            addMessageToChat(userMessage, 'user');

            try {
                // Llamar al backend
                const botResponse = await getBotResponse(userMessage);

                // Añadir respuesta del bot
                addMessageToChat(botResponse, 'bot');
            } catch (error) {
                addMessageToChat('Hubo un error al procesar tu solicitud. Intenta nuevamente.', 'bot');
            }

            // Limpiar el input
            document.getElementById('txtUserMessage').value = '';
        }
    });

    // Función para obtener respuesta del bot
    function getBotResponse(message) {
        return $.ajax({
            url: '/Services/botService.asmx/GetChatResponse',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({ userMessage: message }),
        })
            .done(function (response) {
                console.log('Respuesta del servidor:', response.d);
                if (response.d) {
                    // Mostrar la respuesta en la interfaz
                    $('#responseContainer').text(response.d);
                } else {
                    console.error('No se recibió una respuesta válida.');
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                console.error('Error en la solicitud AJAX:', textStatus, errorThrown);
            });
    }
    // Añadir mensajes al chat
    function addMessageToChat(message, sender) {
        const chatMessages = document.getElementById('chatMessages');
        const cssClass = sender === 'bot' ? 'chat-bubble bot-message' : 'chat-bubble user-message';

        const messageDiv = document.createElement('div');
        messageDiv.className = cssClass;
        messageDiv.textContent = message;

        chatMessages.appendChild(messageDiv);
        chatMessages.scrollTop = chatMessages.scrollHeight; // Scroll al final
    }
</script>

<style>
       .chat-icon {
        cursor: pointer;
        animation: bounce 2s infinite;
        animation-timing-function: ease-in-out;
    }

  

    @keyframes bounce {
        0%, 100% {
            transform: translateY(0);
        }
        50% {
            transform: translateY(-10px);
        }
    }
    /* Contenedor que envuelve el chat y el ícono flotante */
    .chat-container-wrapper {
        position: fixed;
        bottom: 20px;
        right: 20px;
        z-index: 9999;
    }

    /* Icono de chat flotante */
  

    /* Contenedor del chat flotante */
    .chat-bubble-container {
        width: 320px;
        max-height: 400px;
        box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
        display: none; /* Se oculta por defecto */
    }

    .chat-bubble {
        width: 100%;
        background-color: #ffffff;
        border-radius: 10px;
        padding: 20px;
        display: flex;
        flex-direction: column;
        max-height: 400px;
        overflow-y: auto;
    }

    .chat-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        background-color: #0d6efd;
        color: #fff;
        padding: 10px 20px;
        border-radius: 10px 10px 0 0;
    }

    .chat-header img {
        width: 50px;
        border-radius: 50%;
    }

    .chat-messages {
        max-height: 300px;
        overflow-y: auto;
        margin: 20px 0;
        padding: 10px;
        border: 1px solid #e9ecef;
        border-radius: 10px;
    }

    .bot-message {
        background-color: #e9ecef;
        text-align: left;
        margin-bottom: 10px;
        padding: 10px;
        border-radius: 10px;
    }

    .user-message {
        background-color: #0d6efd;
        color: white;
        text-align: right;
        margin-bottom: 10px;
        padding: 10px;
        border-radius: 10px;
    }

    .chat-footer {
        display: flex;
        align-items: center;
    }

    .chat-footer input {
        flex-grow: 1;
        margin-right: 10px;
    }

    .chat-footer button {
        background-color: #0d6efd;
        color: #fff;
    }
</style>
