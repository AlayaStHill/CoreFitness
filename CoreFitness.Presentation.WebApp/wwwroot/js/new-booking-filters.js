const initNewBookingFilters = () => {
    const filterButtons = Array.from(document.querySelectorAll(".class-filters .filter-button[data-filter]"));
    const classItems = Array.from(document.querySelectorAll(".class-list .class-item[data-category]"));
    const emptyFilterItem = document.querySelector(".class-list .class-item--empty-filter");

    if (filterButtons.length === 0 || classItems.length === 0)
        return;

    const normalize = (value) => (value ?? "").trim().toLowerCase();

    const applyFilter = (filterValue) => {
        let visibleCount = 0;
        const normalizedFilter = normalize(filterValue);

        for (const classItem of classItems) {
            const category = normalize(classItem.dataset.category);
            const isVisible = normalizedFilter === "all" || category === normalizedFilter;
            classItem.hidden = !isVisible;

            if (isVisible) {
                classItem.style.removeProperty("display");
            } else {
                classItem.style.setProperty("display", "none", "important");
            }

            if (isVisible)
                visibleCount++;
        }

        if (emptyFilterItem) {
            const showEmpty = visibleCount === 0;
            emptyFilterItem.hidden = !showEmpty;

            if (showEmpty) {
                emptyFilterItem.style.removeProperty("display");
            } else {
                emptyFilterItem.style.setProperty("display", "none", "important");
            }
        }
    };

    for (const button of filterButtons) {
        button.addEventListener("click", () => {
            for (const currentButton of filterButtons) {
                const isActive = currentButton === button;
                currentButton.classList.toggle("active", isActive);
                currentButton.setAttribute("aria-selected", isActive ? "true" : "false");
            }

            applyFilter(normalize(button.dataset.filter) || "all");
        });
    }

    applyFilter("all");
};

if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", initNewBookingFilters);
} else {
    initNewBookingFilters();
}
