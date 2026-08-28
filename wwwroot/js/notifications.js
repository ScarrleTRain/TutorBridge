document.addEventListener('DOMContentLoaded', function () {
    var toggle = document.getElementById('notificationToggle');
    if (!toggle) return; // not signed in — nothing was rendered

    var dropdown = document.getElementById('notificationDropdown');
    var tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');

    toggle.addEventListener('shown.bs.dropdown', function () {
        dropdown.innerHTML = '<div class="p-2 text-muted small">Loading…</div>';

        fetch('/Notifications/Unread')
            .then(function (response) { return response.text(); })
            .then(function (html) {
                dropdown.innerHTML = html;
                bindMarkReadButtons();
            });
    });

    function bindMarkReadButtons() {
        dropdown.querySelectorAll('.mark-read-icon').forEach(function (button) {
            button.addEventListener('click', function (event) {
                event.preventDefault();
                event.stopPropagation();

                var id = button.dataset.id;
                var item = button.closest('.notification-item');

                fetch('/Notifications/MarkRead/' + id, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                    body: '__RequestVerificationToken=' + encodeURIComponent(tokenInput.value)
                }).then(function (response) {
                    if (!response.ok) return;

                    item.remove();
                    decrementBadge();

                    if (!dropdown.querySelector('.notification-item')) {
                        dropdown.innerHTML = '<div class="p-2 text-muted small">No new notifications.</div>';
                    }
                });
            });
        });
    }

    function decrementBadge() {
        var badge = document.querySelector('#notificationToggle .badge');
        if (!badge) return;

        var next = (parseInt(badge.textContent, 10) || 0) - 1;
        next <= 0 ? badge.remove() : (badge.textContent = next);
    }
});