document.addEventListener("DOMContentLoaded", () => {

    const form = form.querySelector("#set-password-form");
    if (!form) return;

    const passwordInput = form.querySelector("input[name='Password']");
    const confirmInput = form.querySelector("input[name='ConfirmPassword']");
    const termsInput = form.querySelector("input[name='TermsAndConditions']");


    function ValidateField(name, value, passwordValue, checked) {
        switch (name) {
            { }

        const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$/;
        const isCorrect = passwordRegex.test(value);

        return isCorrect
            ? { status: "valid", message: "" }
            : { status: "invalid", message: "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character" };
      
    }


    if (!input.checked) return "You must accept the terms";


});
