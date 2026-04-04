export function setupFormValidation(selectedForm, rules) {
  const form = document.querySelector(selectedForm);
  if (!form) return;

  const inputs = form.querySelectorAll("input:not([type='file']), textarea");

  inputs.forEach((input) => {
    input.addEventListener("input", () => validateField(input));
    input.addEventListener("blur", () => {
      input.dataset.touched = "true";
      validateField(input);
    });
  });

  function validateField(input) {
    const errorSpan = form.querySelector(`[data-valmsg-for="${input.name}"]`);

    const errorMessage = getErrorMessage(input);

    if (!input.dataset.touched) {
      if (errorSpan) errorSpan.textContent = "";
      input.classList.remove("input-error");
      return true;
    }

    if (errorSpan) errorSpan.textContent = errorMessage;
    input.classList.toggle("input-error", Boolean(errorMessage));

    return !errorMessage;
  }

  function getErrorMessage(input) {
    const value = input.value.trim();
    const fieldRules = rules[input.name];

    if (!fieldRules) return "";

    for (const rule of fieldRules) {
      const error = rule(value);
      if (error) return error;
    }

    return "";
  }

  form.addEventListener("submit", (event) => {
    let allValid = true;

    inputs.forEach((input) => {
      input.dataset.touched = "true";

      const isValid = validateField(input);
      if (!isValid) allValid = false;
    });

    if (!allValid) {
      event.preventDefault();
    }
  });
}
