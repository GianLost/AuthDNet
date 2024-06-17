import { encryptData } from '../cryptography/dataCrypt.js';
import { applyPhoneMask } from '../../components/masks.js';

// Constantes para as regras de validação
const VALIDATION_RULES = {
    REQUIRED: 'required',
    MIN_LENGTH: 'min-length',
    PATTERN: 'pattern',
    CONFIRM_EMAIL: 'confirm-email',
    CONFIRM_PASSWORD: 'confirm-password',
};

// Constantes para mensagens de erro
const MESSAGES = {
    REQUIRED: 'Este campo é obrigatório !',
    INVALID_FORMAT: 'Campo com formato inválido !',
    EMAIL_MISMATCH: 'Os e-mails são diferentes !',
    PASSWORD_MISMATCH: 'As senhas são diferentes !'
};

// Constantes de chaves de criptografia
const ENCRYPTION_KEYS = {
    KEY: new TextEncoder().encode("character-key0@55YssY??-&&36A9W="),
    IV: new TextEncoder().encode("char-iv1=Key00?#"),
};

// Objeto FormValidator responsável pela validação dos campos do formulário
const FormValidator = {
    // Função para validar um campo dinamicamente
    validateInputDynamically(input) {
        // Obtém as regras de validação do dataset do input
        const validationRules = (input.dataset.validationRules || '').split(',').map(rule => rule.trim());
        const feedbackDiv = input.parentElement.querySelector('.invalid-tooltip');
        let errorMessage = '';

        // Itera sobre as regras de validação e verifica cada uma
        validationRules.some(rule => {
            switch (rule) {
                case VALIDATION_RULES.REQUIRED:
                    if (input.value.trim() === '') {
                        errorMessage = feedbackDiv.textContent.trim() || MESSAGES.REQUIRED;
                    }
                    break;
                case VALIDATION_RULES.MIN_LENGTH:
                    const minLength = parseInt(input.dataset.minLength, 10);
                    if (input.value.trim().length < minLength) {
                        errorMessage = input.dataset.errorMessage || `Requer pelo menos ${minLength} caracteres !`;
                    }
                    break;
                case VALIDATION_RULES.PATTERN:
                    const pattern = new RegExp(input.dataset.pattern);
                    if (!pattern.test(input.value.trim())) {
                        errorMessage = input.dataset.errorMessage || MESSAGES.INVALID_FORMAT;
                    }
                    break;
                case VALIDATION_RULES.CONFIRM_EMAIL:
                    const emailField = document.getElementById(input.dataset.confirmEmailFor);
                    if (emailField && emailField.value.trim() !== '' && input.value.trim() !== emailField.value.trim()) {
                        errorMessage = MESSAGES.EMAIL_MISMATCH;
                    }
                    break;
                case VALIDATION_RULES.CONFIRM_PASSWORD:
                    const passwordField = document.getElementById(input.dataset.confirmPasswordFor);
                    if (passwordField && passwordField.value.trim() !== '' && input.value.trim() !== passwordField.value.trim()) {
                        errorMessage = MESSAGES.PASSWORD_MISMATCH;
                    }
                    break;
            }
            return !!errorMessage;
        });

        // Exibe a mensagem de erro ou sucesso de acordo com o resultado da validação
        if (errorMessage) {
            this.showValidationError(input, feedbackDiv, errorMessage);
        } else {
            this.showValidationSuccess(input, feedbackDiv);
        }
    },

    // Função para mostrar mensagem de erro
    showValidationError(input, feedbackDiv, errorMessage) {
        input.classList.add('is-invalid');
        input.classList.remove('is-valid');
        feedbackDiv.textContent = errorMessage;
        feedbackDiv.style.display = 'block';
    },

    // Função para mostrar validação bem-sucedida
    showValidationSuccess(input, feedbackDiv) {
        input.classList.remove('is-invalid');
        input.classList.add('is-valid');
        feedbackDiv.textContent = '';
        feedbackDiv.style.display = 'none';

        // Adiciona este bloco para remover a div se estiver vazia
        if (feedbackDiv.textContent.trim() === '') {
            feedbackDiv.style.display = 'none';
        }
    },

    // Função para limpar todas as mensagens de erro
    clearErrorMessages(form) {
        form.querySelectorAll('.invalid-tooltip').forEach(div => {
            div.textContent = '';
            div.style.display = 'none';
        });
    }
};

// Objeto FormHandler responsável pela manipulação e submissão do formulário
const FormHandler = {
    // Função para lidar com a submissão do formulário
    async handleFormSubmit(event) {
        event.preventDefault();

        const form = event.target;
        const inputs = form.querySelectorAll('input:not([type=hidden])');

        let firstInvalidInput = null;
        let isFormValid = true;

        // Valida cada input dinamicamente
        inputs.forEach(input => {
            FormValidator.validateInputDynamically(input);
            if (input.classList.contains('is-invalid') && !firstInvalidInput) {
                firstInvalidInput = input;
                isFormValid = false;
            }
        });

        // Se algum campo estiver inválido, impede a submissão e foca no primeiro campo inválido
        if (!isFormValid) {
            firstInvalidInput.focus();
        } else {
            // Se o formulário for válido, criptografa os dados e envia
            let formData = {};
            inputs.forEach(input => {
                formData[input.name] = input.value;
            });

            // Criptografa os dados do formulário
            const encryptedData = await encryptData(formData, ENCRYPTION_KEYS.KEY, ENCRYPTION_KEYS.IV);

            // Cria um campo de input oculto para armazenar os dados criptografados
            let encryptedInput = form.querySelector('input[name="userEncrypted"]');
            if (!encryptedInput) {
                encryptedInput = document.createElement('input');
                encryptedInput.type = 'hidden';
                encryptedInput.name = 'userEncrypted';
                form.appendChild(encryptedInput);
            }
            encryptedInput.value = encryptedData;

            // Desabilita os campos originais para não enviar dados não criptografados
            inputs.forEach(input => input.disabled = true);

            // Submete o formulário
            form.submit();
        }
    },

    // Função para inicializar a validação de um formulário
    initializeFormValidation(form) {
        form.addEventListener('submit', this.handleFormSubmit.bind(this));
        form.querySelectorAll('input').forEach(input => {
            input.addEventListener('input', () => FormValidator.validateInputDynamically(input));
            input.addEventListener('focus', () => {
                FormValidator.clearErrorMessages(form);
                const feedbackDiv = input.parentElement.querySelector('.invalid-tooltip');
                FormValidator.validateInputDynamically(input);
                if (input.classList.contains('is-invalid')) {
                    FormValidator.showValidationError(input, feedbackDiv, feedbackDiv.textContent);
                }
            });
            // Aplica máscara de telefone se o tipo do input for 'tel'
            if (input.getAttribute('type') === 'tel') {
                input.addEventListener('input', applyPhoneMask);
            }
        });
    },

    // Função para inicializar a validação de todos os formulários na página
    initialize() {

        // Esconde todas as divs .invalid-tooltip vazias ao carregar a página
        document.querySelectorAll('.invalid-tooltip').forEach(div => {
            if (div.textContent.trim() === '') {
                div.style.display = 'none';
                div.classList.add('hidden');
            }
        });

        document.querySelectorAll('.needs-validation').forEach(form => {
            this.initializeFormValidation(form);
        });

        // Adiciona eventos de input aos campos de e-mail e confirmar e-mail para validar dinamicamente
        document.querySelectorAll('input[type="email"]').forEach(input => {
            const confirmInput = document.getElementById(input.dataset.confirmEmailFor);
            if (confirmInput) {
                input.addEventListener('input', () => {
                    if (confirmInput.value.trim() !== '') {
                        FormValidator.validateInputDynamically(confirmInput);
                    }
                });
                confirmInput.addEventListener('input', () => {
                    if (input.value.trim() !== '') {
                        FormValidator.validateInputDynamically(input);
                    }
                });
            }
        });

        // Adiciona eventos de input aos campos de senha e confirmar senha para validar dinamicamente
        document.querySelectorAll('input[type="password"]').forEach(input => {
            const confirmInput = document.getElementById(input.dataset.confirmPasswordFor);
            if (confirmInput) {
                input.addEventListener('input', () => {
                    if (confirmInput.value.trim() !== '') {
                        FormValidator.validateInputDynamically(confirmInput);
                    }
                });
                confirmInput.addEventListener('input', () => {
                    if (input.value.trim() !== '') {
                        FormValidator.validateInputDynamically(input);
                    }
                });
            }
        });
    }
};

// Inicializa a validação de todos os formulários quando o DOM estiver totalmente carregado
document.addEventListener('DOMContentLoaded', () => {
    FormHandler.initialize();
});