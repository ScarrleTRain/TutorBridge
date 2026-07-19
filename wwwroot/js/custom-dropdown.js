document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.custom-select-dropdown').forEach(initCustomDropdown);
});

function initCustomDropdown(container) {
    const fieldName = container.dataset.field;
    const btn = container.querySelector('button');
    const label = container.querySelector('.dropdown-label');
    const hiddenInput = document.getElementById(`${fieldName}Hidden`);
    const searchInput = container.querySelector('.dropdown-search');
    const noResultsMsg = container.querySelector('.no-results-msg');
    const options = container.querySelectorAll('.custom-option');

    if (hiddenInput.value) {
        const match = [...options].find(o => o.dataset.id === hiddenInput.value);

        if (match) {
            label.textContent = match.dataset.name;
        }
    }

    options.forEach(option => {
        option.addEventListener('click', function (e) {
            e.preventDefault();
            hiddenInput.value = this.dataset.id;
            label.textContent = this.dataset.name;
            searchInput.value = '';
            filterOptions('');
            bootstrap.Dropdown.getOrCreateInstance(btn).hide();

            hiddenInput.dispatchEvent(new Event('change', { bubbles: true }));
        });
    });

    function filterOptions(query) {
        const lowerQuery = query.trim().toLowerCase();
        let visibleCount = 0;

        options.forEach(option => {
            const matches = option.dataset.name.toLowerCase().includes(lowerQuery)
                || option.dataset.id.toLowerCase().includes(lowerQuery);
            option.closest('li').classList.toggle('d-none', !matches);
            if (matches) visibleCount++;
        });

        noResultsMsg.classList.toggle('d-none', visibleCount !== 0);
    }

    searchInput.addEventListener('input', function () {
        filterOptions(this.value);
    });

    btn.addEventListener('shown.bs.dropdown', () => searchInput.focus());
    btn.addEventListener('hidden.bs.dropdown', () => {
        searchInput.value = '';
        filterOptions('');
    });

    searchInput.addEventListener('click', e => e.stopPropagation());
}