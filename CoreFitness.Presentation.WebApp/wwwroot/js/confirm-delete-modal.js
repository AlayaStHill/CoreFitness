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
        if (!sessionIdToDelete) return;

        const deleteUrl = `/admin/workoutsessions/delete/${sessionIdToDelete}`;
        // sätter hx-post på knappen så att HTMX vet vart den ska skicka POST-förfrågan när knappen klickas
        confirmButton.setAttribute("hx-post", deleteUrl);

        //CSS-selector-string som pekar på tabellraden med ett specifikt id
        const rowSelector = `#session-${sessionIdToDelete}`;
        // detta element ska uppdateras
        confirmButton.setAttribute("hx-target", rowSelector);

        // läs om så att HTMX binder de nya hx-attributen
        if (window.htmx) {
            htmx.process(confirmButton);
        }

        // reset error
        const errorBox = modal.querySelector(".modal__error");
        if (errorBox) {
            errorBox.textContent = "";
            errorBox.hidden = true;
        }

        modal.hidden = false;
        overlay.hidden = false;
    });

    // så att modalen stängs efter requesten skickats
    document.body.addEventListener("htmx:afterRequest", (e) => {
        if (e.target === confirmButton && e.detail.successful) {
            closeModal();
        }
    });

    // visar errormeddelande i modalen om något gick fel med requesten
    document.body.addEventListener("htmx:responseError", (e) => {
        // kör bara om eventet kommer från delete-knappen. Eftersom HTMX triggar events för alla requests på sidan
        if (e.target !== confirmButton) return;

        // hitta error-elementet
        const errorBox = modal.querySelector(".modal__error");
        if (!errorBox) return;

        errorBox.textContent = e.detail.xhr.responseText;
        errorBox.hidden = false;
    });

    function closeModal() {
        modal.hidden = true;
        overlay.hidden = true;
        sessionIdToDelete = null;
        confirmButton.removeAttribute("hx-post");
        confirmButton.removeAttribute("hx-target");
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





