function makeSortableTable(tableId, storageKey) {
    function applySort(key, asc) {
        const th = document.querySelector(`#${tableId} th[data-sort="${key}"]`);
        if (!th) return;

        const tbody = document.querySelector(`#${tableId} tbody`);
        const rows = Array.from(tbody.querySelectorAll('tr'));

        rows.sort((a, b) => {
            let valA = a.dataset[key] ?? '';
            let valB = b.dataset[key] ?? '';

            if (key === 'id') {
                return asc ? Number(valA) - Number(valB) : Number(valB) - Number(valA);
            }
            if (key === 'time') {
                return asc ? new Date(valA) - new Date(valB) : new Date(valB) - new Date(valA);
            }
            return asc ? valA.localeCompare(valB) : valB.localeCompare(valA);
        });

        rows.forEach(row => tbody.appendChild(row));

        document.querySelectorAll(`#${tableId} th[data-sort]`).forEach(h => {
            h.dataset.dir = '';
            h.querySelector('i').className = 'bi bi-arrow-down-up sort-icon opacity-50';
        });
        th.dataset.dir = asc ? 'asc' : 'desc';
        th.querySelector('i').className = (asc ? 'bi bi-arrow-up' : 'bi bi-arrow-down') + ' sort-icon opacity-50';
    }

    document.querySelectorAll(`#${tableId} th[data-sort]`).forEach(th => {
        th.addEventListener('click', () => {
            const key = th.dataset.sort;
            const asc = th.dataset.dir !== 'asc';
            applySort(key, asc);
            localStorage.setItem(storageKey, JSON.stringify({ key, asc }));
        });
    });

    const saved = localStorage.getItem(storageKey);
    if (saved) {
        try {
            const { key, asc } = JSON.parse(saved);
            applySort(key, asc);
        } catch { /* ignore corrupt value */ }
    }
}