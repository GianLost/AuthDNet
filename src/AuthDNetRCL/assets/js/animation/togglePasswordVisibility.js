import { loginPasswordInput, loginTogglePasswordButton } from "../constants/constantsComponent.js";

/**
 * "function togglePasswordVisibility()" é a função responsável por alterar a visibilidade dos campos de senha nos formulários em que é aplicada.
 * - Utiliza os eventos "mousedown" e "mouseup" para exibir e ocultar os caractéres conforme o ícone de exibição é pressionado e/ou solto;
 */

function togglePasswordVisibility() {
    // Seleciona todos os botões de alternância de senha com a classe 'toggle-password'
    const toggleButtons = $(loginTogglePasswordButton);

    // Adiciona os eventos a cada botão de alternância de senha
    toggleButtons.on('mousedown mouseup mouseleave', function (event) {
        // Seleciona o botão de alternância
        const toggleButton = $(this);
        // Encontra o campo de senha relacionado ao botão
        const passwordInput = toggleButton.siblings(loginPasswordInput);
        // Encontra o ícone do botão
        const passwordIcon = toggleButton;

        // Verifica o tipo de evento
        if (event.type === 'mousedown') {
            // Quando o botão é pressionado, mostra a senha
            passwordInput.attr('type', 'text');
            // Atualiza o ícone para o modo "ver"
            passwordIcon.removeClass('bi-eye-slash-fill').addClass('bi-eye-fill');
        } else if (event.type === 'mouseup' || event.type === 'mouseleave') {
            // Quando o botão é solto ou o mouse sai, oculta a senha
            passwordInput.attr('type', 'password');
            // Atualiza o ícone para o modo "ocultar"
            passwordIcon.removeClass('bi-eye-fill').addClass('bi-eye-slash-fill');
        }
    });
}

/**
 * Função que é executada quando o documento HTML foi completamente carregado e está pronto para manipulação.
 * Neste momento, a visibilidade da senha é ativada, permitindo ao usuário alternar entre exibir e ocultar a senha.
 * Isso é feito chamando a função `togglePasswordVisibility` com os elementos relevantes do formulário.
 */

$(document).ready(function () {
    // Ativa a funcionalidade de alternância de visibilidade da senha
    togglePasswordVisibility();
});