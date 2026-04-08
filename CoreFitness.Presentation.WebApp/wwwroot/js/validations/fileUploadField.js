export function initFileUploadField({
  formSelector,
  inputSelector,
  placeholderSelector,
  removeButtonSelector,
  errorSelector,
  emptyText = "Upload file",
}) {
  const form = document.querySelector(formSelector);
  if (!form) return;

  const fileInput = form.querySelector(inputSelector);
  const filePlaceholder = form.querySelector(placeholderSelector);
  const fileRemoveButton = form.querySelector(removeButtonSelector);
  const fileErrorMessage = errorSelector
    ? form.querySelector(errorSelector)
    : null;

  if (!fileInput || !filePlaceholder || !fileRemoveButton) return;

  const updateFileSelectionUi = () => {
    const selectedFile = fileInput.files?.[0];

    if (!selectedFile) {
      filePlaceholder.textContent = emptyText;
      filePlaceholder.classList.remove("is-selected");
      fileRemoveButton.hidden = true;
      return;
    }

    filePlaceholder.textContent = selectedFile.name;
    filePlaceholder.classList.add("is-selected");
    fileRemoveButton.hidden = false;
  };

  fileInput.addEventListener("change", () => {
    updateFileSelectionUi();
  });

  fileRemoveButton.addEventListener("click", (event) => {
    event.preventDefault();
    event.stopPropagation();

    fileInput.value = "";
    fileInput.classList.remove("input-error");
    if (fileErrorMessage) fileErrorMessage.textContent = "";

    updateFileSelectionUi();
  });

  updateFileSelectionUi();
}
