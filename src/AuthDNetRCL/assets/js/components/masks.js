// Função para aplicar máscara em campos de telefone
export function applyPhoneMask(event) {
    const input = event.target;
    let phoneNumber = input.value.replace(/\D/g, '');

    // Aplica a máscara (XX) XXXXX-XXXX
    phoneNumber = phoneNumber.replace(/(\d{2})(\d)/, '($1) $2');
    phoneNumber = phoneNumber.replace(/(\d{4,5})(\d{4})$/, '$1-$2');

    input.value = phoneNumber;
}

// Seleciona todos os inputs do tipo 'tel'
const phoneInputs = document.querySelectorAll('input[type="tel"]');

// Adiciona o evento de input a cada um deles
phoneInputs.forEach(input => {
    input.addEventListener('input', applyPhoneMask);
});