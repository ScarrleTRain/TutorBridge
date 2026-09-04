document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.checklist-multiselect').forEach(initChecklistMultiSelect);
});

function initChecklistMultiSelect(container) {
    const searchInput = container.querySelector('.checklist-search');
    const noResultsMsg = container.querySelector('.no-results-msg');
    const options = container.querySelectorAll('.checklist-option');

    function filterOptions(query) {
        const lowerQuery = query.trim().toLowerCase();
        let visibleCount = 0;

        options.forEach(option => {
            const text = option.querySelector('.checklist-item-text').textContent.toLowerCase();
            const matches = text.includes(lowerQuery);
            option.classList.toggle('d-none', !matches);
            if (matches) visibleCount++;
        });

        noResultsMsg.classList.toggle('d-none', visibleCount !== 0);
    }

    searchInput.addEventListener('input', function () {
        filterOptions(this.value);
    });

    searchInput.addEventListener('click', e => e.stopPropagation());
}
