import { setupFormValidation } from "./formValidation.js";
import { required, minLength, email, phone } from "./validators.js";

document.addEventListener("DOMContentLoaded", () => {
  setupFormValidation(".about-me-form", {
    FirstName: [
      (v) => required(v, "You must enter a first name"),
      (v) => minLength(v, 2, "Minimum 2 characters"),
    ],
    LastName: [
      (v) => required(v, "You must enter a last name"),
      (v) => minLength(v, 2, "Minimum 2 characters"),
    ],
    Email: [
      (v) => required(v, "You must enter an email address"),
      (v) => email(v, "Invalid email address"),
    ],
    PhoneNumber: [(v) => phone(v, "Invalid phone number")],
  });
});
