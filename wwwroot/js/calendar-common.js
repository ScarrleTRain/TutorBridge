function createBaseCalendarOptions(events, overrides = {}) {
    const base = {
        initialView: 'timeGridWeek',
        headerToolbar: { left: 'prev,next today', center: 'title' },
        themeSystem: 'bootstrap5',
        locale: 'en-NZ',
        allDaySlot: false,
        slotMinTime: '06:00:00',
        slotDuration: '00:30:00',
        firstDay: 1,
        events: events
    };
    return Object.assign(base, overrides);
}