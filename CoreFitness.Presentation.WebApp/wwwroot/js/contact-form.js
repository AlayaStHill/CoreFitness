document.addEventListener("DOMContentLoaded", () => {

    const form = document.querySelector(".contact-form");
    if (!form) return;
    const inputs = form.querySelectorAll("input, textarea");

    // blir true efter blur/submit
    let touched = false;

    inputs.forEach(input => {
        input.addEventListener("input", validateAndDisplay);
        input.addEventListener("blur", (event) => {
            touched = true;
            validateAndDisplay(event);
        });
    });

    function validateAndDisplay(event) {
        const input = event.target;
        const value = input.value.trim();
        // hitta rätt errorspan
        const errorSpan =
            form.querySelector(`[data-valmsg-for="${input.name}"]`) ||
            form.querySelector(`#${input.id}__validationMessage`);

        const errorMessage = getValidationResult(input.name, value);
        const isValid = !errorMessage;

        // Innan blur/submit: visa inga errors / rensa om något ligger kvar
        if (!touched) {
            if (errorSpan) errorSpan.textContent = "";
            input.classList.remove("input-error");
            return;
        }

        // Efter blur/submit: visa errors tills giltig
        if (isValid) {
            if (errorSpan) errorSpan.textContent = "";
            input.classList.remove("input-error");
            return;
        }

        if (errorSpan) errorSpan.textContent = errorMessage;
        input.classList.add("input-error"); 
    }

    function getValidationResult(name, value) {
        switch (name) {

            case "FirstName":
                if (!value) return "You must enter a first name";
                if (value.length < 2) return "Minimum 2 characters";
                return "";

            case "LastName":
                if (!value) return "You must enter a last name";
                if (value.length < 2) return "Minimum 2 characters";
                return "";

            case "Email":
                if (!value) return "You must enter an email address";

                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!emailRegex.test(value)) return "Invalid email address";
                return "";

            case "PhoneNumber":
                if (!value) return "";

                const phoneRegex = /^[0-9+\-\s()]*$/;
                if (!phoneRegex.test(value)) return "Invalid phone number";
                return "";

            case "Message":
                if (!value) return "You must enter a message";
                if (value.length < 5) return "Minimum 5 characters";
                return "";

            default:
                return "";
        }
    }

    form.addEventListener("submit", (event) => {
        touched = true;
        let allValid = true;

        inputs.forEach(input => {
            const value = input.value.trim();
            const errorMessage = getValidationResult(input.name, value);

            const errorSpan =
                form.querySelector(`[data-valmsg-for="${input.name}"]`) ||
                form.querySelector(`#${input.id}__validationMessage`);

            if (errorSpan) {
                errorSpan.textContent = errorMessage;
            }

            input.classList.toggle("input-error", Boolean(errorMessage));

            if (errorMessage) {
                allValid = false;
            }
        });

        if (!allValid) {
            event.preventDefault();
        }
    });
});