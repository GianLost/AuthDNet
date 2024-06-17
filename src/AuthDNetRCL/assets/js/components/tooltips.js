var tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
var tooltipList = [];

tooltipTriggerList.forEach(function (tooltipTriggerEl) {
    var tooltip = new bootstrap.Tooltip(tooltipTriggerEl);
    tooltipList.push(tooltip);

    // Adicione um evento de clique para ocultar o tooltip ao clicar
    tooltipTriggerEl.addEventListener('mouseleave', function () {
        tooltip.hide();
    });
});