document.addEventListener("DOMContentLoaded", () => {

    const form = document.querySelector("#signup-form");
    if (!form) return;
    const input = form.querySelector('input[name="Email"]');

    function validateField(value) {
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

    let touched = false;
    let hasBeenValid = false;
    let prevStatus = "invalid";
    function validateInput(input) {
        const value = input.value.trim();
        const result = validateField(value);

        const becameInvalidAfterValid = hasBeenValid && prevStatus === "valid" && result.status === "invalid";
        const leftInvalid = touched && result.status === "invalid";

        // visa fel endast om man går från giltigt till ogiltigt under input, eller man har lämnat fältet ogiltigt
        if (becameInvalidAfterValid || leftInvalid)
        {
            showError(input, result.message);
        } else if (result.status === "valid" && touched) {
            showError(input, "");
        }

        // sparar nuvarande status, så att becameInvalid fungerar korrekt när det blir ogiltigt nästa gång 
        prevStatus = result.status;

        // om status är exakt valid- returnera true, pending och invalid = false
        return result.status === "valid";
    }

    input.addEventListener("input", () => validateInput(input));

    input.addEventListener("blur", () => {
        touched = true;
        validateInput(input);
    });

    form.addEventListener("submit", (event) => {
        touched = true; // gör att submit beter sig som blur när det gäller att visa felmeddelanden

        const isValid = validateInput(input);

        if (!isValid) {
            event.preventDefault();
        }
    });
});