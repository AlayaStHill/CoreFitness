document.addEventListener("DOMContentLoaded", () => {

    const form = document.querySelector("#set-password-form");
    if (!form) return;

    const passwordInput = form.querySelector("input[name='Password']");
    const confirmInput = form.querySelector("input[name='ConfirmPassword']");
    const termsInput = form.querySelector("input[name='TermsAndConditions']");


    function getValidationResult(name, value, passwordValue, checked) {
        switch (name) {
            case "Password": 
                if (!value) 
                    return { status: "invalid", message: "You must enter a password" }; 
                
                const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$/;
                const isCorrect = passwordRegex.test(value);

                return isCorrect
                    ? { status: "valid", message: "" }
                    : { status: "invalid", message: "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character" };

            case "ConfirmPassword":
                if (!value)
                    return { status: "invalid", message: "You must confirm your password" };

                const isMatching = value === passwordValue;
                return isMatching
                    ? { status: "valid", message: "" }
                    : { status: "invalid", message: "Passwords do not match" };

            case "TermsAndConditions":
                return checked
                ? { status: "valid", message: "" }
                : { status: "invalid", message: "You must accept the terms" };

            default:
                return { status: "valid", message: "" };
        }
    }

    function showError(input, errorMessage) {
        const errorSpan = form.querySelector(`[data-valmsg-for="${input.name}"]`);

        if (errorSpan) {
            errorSpan.textContent = errorMessage;
        }

        input.classList.toggle("input-error", Boolean(errorMessage));
    }


    let touched = false;
    function validateAndDisplay(input) {
        const value = input.value.trim();
        const passwordValue = passwordInput.value.trim();

        const result = getValidationResult(input.name, value, passwordValue, termsInput.checked);

        // Innan blur/submit: visa inga fel (och rensa om något ligger kvar)
        if (!touched) {
            showError(input, "");
            return result.status === "valid";
        }

        // Efter blur/submit: visa fel tills giltigt
        if (result.status === "valid") {
            showError(input, "");
            return true;
        }

        showError(input, result.message);
        return false;
    }


    // INPUT EVENTS
    passwordInput.addEventListener("input", () => {
        validateAndDisplay(passwordInput);
        validateAndDisplay(confirmInput);
    });

    confirmInput.addEventListener("input", () => {
        validateAndDisplay(confirmInput);
    });

    termsInput.addEventListener("change", () => {
        touched = true;
        validateAndDisplay(termsInput);
    });

    passwordInput.addEventListener("blur", () => {
        touched = true;
        validateAndDisplay(passwordInput);
    });

    confirmInput.addEventListener("blur", () => {
        touched = true;
        validateAndDisplay(confirmInput);
    });


    form.addEventListener("submit", (event) => {

        touched = true;

        const isPasswordValid = validateAndDisplay(passwordInput);
        const isConfirmValid = validateAndDisplay(confirmInput);
        const isTermsValid = validateAndDisplay(termsInput);

        if (!isPasswordValid || !isConfirmValid || !isTermsValid) {
            event.preventDefault();
        }
    });
});
