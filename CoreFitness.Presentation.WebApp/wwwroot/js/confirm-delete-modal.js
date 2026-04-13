document.addEventListener("DOMContentLoaded", () => {
    const modal = document.querySelector(".modal");
    const overlay = document.querySelector(".modal-overlay"); 

    const cancelButton = modal?.querySelector("[data-cancel-delete]");
    const confirmButton = modal?.querySelector("[data-confirm-delete]");

    let sessionIdToDelete = null;

    if (!modal || !overlay || !cancelButton || !confirmButton) return;

    // Öppna modal
    document.addEventListener("click", (e) => {
        /*Om klicket skedde i en delete-knapp --> hitta den knappen (närmsta element med klassen .js-delete-trigger)*/
        const deleteButton = e.target.closest(".js-delete-trigger");
        // om klicket inte var på en delete-knapp, avsluta funktionen
        if (!deleteButton) return;

        // hämta id för sessionen som ska tas bort
        sessionIdToDelete = deleteButton.getAttribute("data-session-id");

        modal.hidden = false;
        overlay.hidden = false;
    });

    // CONFIRM -> sätt HTMX-url
    confirmButton.addEventListener("click", () => {
        if (!sessionIdToDelete) return;

        const deleteUrl = `/admin/workoutsessions/delete/${sessionIdToDelete}`;
        // sätter hx-post på knappen så att HTMX vet vart den ska skicka POST-förfrågan när knappen klickas
        confirmButton.setAttribute("hx-post", deleteUrl);

        closeModal();
    });

    function closeModal() {
        modal.hidden = true;
        overlay.hidden = true;
        sessionIdToDelete = null;
    }

    cancelButton.addEventListener("click", closeModal);
    // klick utanför rutan stänger modal
    overlay.addEventListener("click", closeModal);

    // ESC stänger modal
    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape") {
            closeModal();
        }
    });
});





