document.addEventListener('DOMContentLoaded', function () {
    const calendarEl = document.getElementById('calendar');
    const hiddenInput = document.getElementById('TimeslotId');
    const feedback = document.getElementById('slot-feedback');
    const submitBtn = document.getElementById('submit-btn');

    const calendar = new FullCalendar.Calendar(calendarEl, createBaseCalendarOptions(window.__timeslotEvents, {
        validRange: function () {
            const start = new Date();
            const end = new Date(start);
            end.setDate(end.getDate() + 28);
            return { start, end };
        },
        eventClick: function (info) {
            calendarEl.querySelectorAll('.fc-event-selected')
                .forEach(el => el.classList.remove('fc-event-selected'));
            info.el.classList.add('fc-event-selected');

            hiddenInput.value = info.event.id;

            const options = { weekday: 'short', month: 'short', day: 'numeric', hour: 'numeric', minute: '2-digit' };
            feedback.textContent = `Selected: ${info.event.start.toLocaleDateString('en-NZ', options)} — ${info.event.title}`;
            feedback.classList.remove('d-none');

            submitBtn.disabled = false;
        }
    }));

    calendar.render();
});