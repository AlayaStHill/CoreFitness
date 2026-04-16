document.addEventListener("DOMContentLoaded", () => {
  const form = document.querySelector("#remove-account-form");
  const trigger = document.querySelector("#remove-account-trigger");
  const modal = document.querySelector("#remove-account-modal");
  const closeButtons = document.querySelectorAll("[data-close-modal]");
  const confirmButton = document.querySelector("#confirm-remove-account");
  const confirmationInput = document.querySelector("#delete-confirmation");
  const errorMessage = document.querySelector("#remove-account-error");

  if (
    !form ||
    !trigger ||
    !modal ||
    !confirmButton ||
    !confirmationInput ||
    !errorMessage
  ) {
    return;
  }

  const REQUIRED_TEXT = "DELETE";
  let lastFocusedElement = null;

  const setValidationState = () => {
    const value = confirmationInput.value.trim();
    const isValid = value === REQUIRED_TEXT;

    confirmButton.disabled = !isValid;

    if (!value) {
      errorMessage.textContent = "";
      return;
    }

    errorMessage.textContent = isValid
      ? ""
      : `Please type ${REQUIRED_TEXT} exactly to continue.`;
  };

  const openModal = () => {
    lastFocusedElement = document.activeElement;
    modal.hidden = false;
    document.body.style.overflow = "hidden";
    confirmationInput.value = "";
    errorMessage.textContent = "";
    setValidationState();
    confirmationInput.focus();
  };

  const closeModal = () => {
    modal.hidden = true;
    document.body.style.overflow = "";

    if (lastFocusedElement instanceof HTMLElement) {
      lastFocusedElement.focus();
    }
  };

  trigger.addEventListener("click", openModal);

  closeButtons.forEach((button) => {
    button.addEventListener("click", closeModal);
  });

  document.addEventListener("keydown", (event) => {
    if (event.key === "Escape" && !modal.hidden) {
      closeModal();
    }
  });

  confirmationInput.addEventListener("input", setValidationState);

  confirmButton.addEventListener("click", () => {
    confirmButton.disabled = true;
    confirmButton.textContent = "Deleting...";

    // Frontend-only flow. In MVC this submits the POST form.
    form.submit();
  });
});
