document.addEventListener('DOMContentLoaded', function () {
    const calendarEl = document.getElementById('calendar');
    const hiddenInput = document.getElementById('TimeslotId');
    const feedback = document.getElementById('slot-feedback');
    const submitBtn = document.getElementById('submit-btn');

    const calendar = new FullCalendar.Calendar(calendarEl, createBaseCalendarOptions(window.__timeslotEvents, {
        eventClick: function (info) {
            const id = info.event.id;
            const editUrl = `${window.__timeslotUrls.edit}/${id}`;
            const detailsUrl = `${window.__timeslotUrls.details}/${id}`;
            const deleteUrl = `${window.__timeslotUrls.delete}/${id}`;

            activeTippy = tippy(info.el, {
                appendTo: () => document.body,
                content: `
                    <div class="d-flex gap-2 p-1">
                        <a href="${editUrl}" class="text-decoration-none">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                        <a href="${detailsUrl}" class="text-decoration-none">
                            <i class="bi bi-file-text"></i>
                        </a>
                        <a href="${deleteUrl}" class="text-decoration-none">
                            <i class="bi bi-trash3"></i>
                        </a>
                    </div>
                    `,
                allowHTML: true,
                interactive: true,
                trigger: 'manual',
                placement: 'top',
                arrow: false,
                zIndex: 999,
                theme: 'light',
                offset: [0, 5],
                onHidden: (instance) => instance.destroy(),
            });

            activeTippy.show();
        },
        eventDidMount: function (info) {
            const isBooked = info.event._def.extendedProps.isBooked;
            info.el.classList.add(isBooked ? 'bg-danger' : 'bg-success', 'bg-opacity-75');
        },
        eventContent: function (arg) {
            return {
                html: ` <div class="d-flex align-items-center gap-1 px-1" style="height:100%;">
                            <i class="bi bi-person-fill small"></i>
                            <span class="small">${arg.event.title}</span>
                        </div>`
            };
        }
    }));

    function resizeCalendar() {
        const calendarEl = document.getElementById("calendar");
        const top = calendarEl.getBoundingClientRect().top;
        const bottom = calendarEl.getBoundingClientRect().bottom;

        calendar.setOption('height', window.innerHeight - top - bottom);
    }

    window.addEventListener('resize', resizeCalendar);

    resizeCalendar();
    calendar.render();
});