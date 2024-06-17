$(document).ready(function () {
    const $alert = $('.alert');
    const $closeButton = $('.close-alert');

    // Show alert with transition
    setTimeout(() => {
        $alert.addClass('show');
    }, 100);

    // Close alert with transition
    $closeButton.on('click', function () {
        $alert.addClass('hide').removeClass('show');

        // Remove alert from DOM after transition
        setTimeout(() => {
            $alert.hide();
        }, 500);
    });
});