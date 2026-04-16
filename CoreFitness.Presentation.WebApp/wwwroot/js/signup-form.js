document.addEventListener("DOMContentLoaded", () => {

    const form = document.querySelector("#signup-form");
    if (!form) return;
    const input = form.querySelector('input[name="Email"]');

    function getValidationResult(value) {
        if (!value)
        // object-literal/anonymt objekt
        return { status: "invalid", message: "You must enter an email address" };

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        const isCorrect = emailRegex.test(value);

        return isCorrect
            ? { status: "valid", message: "" }
            : { status: "invalid", message: "Invalid email address" };
    }

    function showError(input, errorMessage) {
        const errorSpan = form.querySelector(`[data-valmsg-for="${input.name}"]`);

        if (errorSpan) {
            errorSpan.textContent = errorMessage;
        }

        input.classList.toggle("input-error", Boolean(errorMessage));
    }


    // blir true efter blur/submit
    let touched = false;
    function validateAndDisplay(input, eventType) {
        const value = input.value.trim();
        const result = getValidationResult(value);

        // Innan blur/submit: visa inga fel, men rensa om det råkar ligga kvar något
        if (!touched) {
            showError(input, "");
            return result.status === "valid";
        }

        // Efter blur/submit: visa fel tills det blir valid
        if (result.status === "valid") {
            showError(input, "");
            return true;
        }

        showError(input, result.message);
        return false;

    }

    input.addEventListener("input", () => validateAndDisplay(input, "input"));

    input.addEventListener("blur", () => {
        touched = true;
        validateAndDisplay(input);
    });

    form.addEventListener("submit", (event) => {
        touched = true; // gör att submit beter sig som blur när det gäller att visa felmeddelanden

        const isValid = validateAndDisplay(input, "submit");

        if (!isValid) {
            event.preventDefault();
        }
    });
});