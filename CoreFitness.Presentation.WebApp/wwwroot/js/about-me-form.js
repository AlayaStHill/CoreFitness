import { setupFormValidation } from "./formValidation.js";
import { required, minLength, email, phone, imageFile } from "./validators.js";
import { initFileUploadField } from "./fileUploadField.js";

document.addEventListener("DOMContentLoaded", () => {
    const formSelector = ".about-me-form";

    setupFormValidation(formSelector, {
        FirstName: [(v) => minLength(v, 2, "Minimum 2 characters")],
        LastName: [(v) => minLength(v, 2, "Minimum 2 characters")],
        Email: [
            (v) => required(v, "You must enter an email address"),
            (v) => email(v, "Invalid email address"),
        ],
        PhoneNumber: [(v) => phone(v, "Invalid phone number")],
        profileImage: [(v, input) => imageFile(input)],
    });

    initFileUploadField({
        formSelector,
        inputSelector: "#profile-image",
        placeholderSelector: ".file-input__placeholder",
        removeButtonSelector: ".file-input__remove",
        errorSelector: '[data-valmsg-for="profileImage"]',
        emptyText: "Upload Profile image",
    });
});
