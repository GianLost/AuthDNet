function styleLabel(label, hasValue) {
    if (hasValue) {
        label.style.top = '0.05rem';
        label.style.left = '0.6rem';
        label.style.fontSize = '0.8rem';
        label.style.fontWeight = '500';
        label.style.padding = '0.35rem';
        label.style.backgroundColor = '#5c5c5c';
        label.style.borderRadius = '3px';
        label.style.transition = 'all 0.13s linear';
    } else {
        label.style.top = '50%';
        label.style.left = '1rem';
        label.style.fontSize = '1.1rem';
        label.style.fontWeight = 'normal';
        label.style.backgroundColor = 'transparent';
        label.style.padding = '0';
    }
}

function updateLabelPositions() {
    document.querySelectorAll('.form-group input').forEach(input => {
        const label = input.nextElementSibling;
        styleLabel(label, input.value.trim() !== '');
    });
}

function handleFocus(event) {
    const label = event.target.nextElementSibling;
    styleLabel(label, true);
}

function handleBlur(event) {
    const input = event.target;
    const label = input.nextElementSibling;
    styleLabel(label, input.value.trim() !== '');
}

// Adiciona os eventos de foco e blur aos inputs
document.querySelectorAll('.form-group input').forEach(input => {
    input.addEventListener('focus', handleFocus);
    input.addEventListener('blur', handleBlur);
});

// Atualiza a posição dos labels após o carregamento da página
window.addEventListener('load', updateLabelPositions);