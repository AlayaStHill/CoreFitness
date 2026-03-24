document.addEventListener("DOMContentLoaded", () => {

    const form = document.querySelector(".contact-form");

    const inputs = form.querySelectorAll("input, textarea");

    inputs.forEach(input => {
        /*skriver i fältet - realtidsfeedback*/
        input.addEventListener("input", handleEvent);
        /*lämnar fältet*/
        input.addEventListener("blur", handleEvent);
    });

    function handleEvent(event) {
        const input = event.target;
        const value = input.value.trim();
        const errorSpan = form.querySelector(`[data-valmsg-for="${input.name}"]`);

        const errorMessage = validateField(input.name, value);

        if (errorSpan) {
            errorSpan.textContent = errorMessage;
        }

        /*lägg till klassen om errorMessage inte är tom(true), annars ta bort den(false).*/
        input.classList.toggle("input-error", Boolean(errorMessage));
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
                if (!value) return ""; // inte required

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

    // Validera hela formuläret vid submit
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