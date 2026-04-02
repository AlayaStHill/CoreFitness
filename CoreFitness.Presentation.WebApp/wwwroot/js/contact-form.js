document.addEventListener("DOMContentLoaded", () => {

    const form = document.querySelector(".contact-form");
    if (!form) return;
    const inputs = form.querySelectorAll("input, textarea");

    // state för email
    let emailHasBeenValid = false;

    inputs.forEach(input => {
        input.addEventListener("input", handleEvent);
        input.addEventListener("blur", handleEvent);
    });

    function handleEvent(event) {
        const input = event.target;
        const value = input.value.trim();
        const errorSpan = form.querySelector(`[data-valmsg-for="${input.name}"]`);
        const isBlur = event.type === "blur";

        // SPECIALLOGIK för email 
        if (input.name === "Email") {

            const errorMessage = validateField("Email", value);
            const isValid = !errorMessage;

            // har det varit giltigt någon gång?
            if (isValid) {
                emailHasBeenValid = true;
            }

            // visa fel: vid blur om ogiltigt eller input går från giltigt --> ogiltigt 
            const shouldShowError = (isBlur && !isValid) || (emailHasBeenValid && !isValid);

            if (errorSpan) {
                errorSpan.textContent = shouldShowError ? errorMessage : "";
            }

            input.classList.toggle("input-error", shouldShowError);

            return;
        }

        // resten av fälten
        const errorMessage = validateField(input.name, value);
        const isValid = !errorMessage;

        // rensa fel direkt om giltigt
        if (isValid) {
            if (errorSpan) {
                errorSpan.textContent = "";
            }

            input.classList.remove("input-error");
            return;
        }

        // visa fel annars
        if (errorSpan) {
            errorSpan.textContent = errorMessage;
        }

        input.classList.add("input-error");
    }

    function validateField(name, value) {
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

        let isValid = true;

        inputs.forEach(input => {
            const value = input.value.trim();
            const errorMessage = validateField(input.name, value);

            const errorSpan = form.querySelector(`[data-valmsg-for="${input.name}"]`);

            if (errorSpan) {
                errorSpan.textContent = errorMessage;
            }

            input.classList.toggle("input-error", Boolean(errorMessage));

            if (errorMessage) {
                isValid = false;
            }
        });

        if (!isValid) {
            event.preventDefault();
        }
    });
});