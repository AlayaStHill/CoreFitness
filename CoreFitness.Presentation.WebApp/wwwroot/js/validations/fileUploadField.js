export function initFileUploadField({
  formSelector,
  inputSelector,
  placeholderSelector,
  removeButtonSelector,
  errorSelector,
  imageUrlSelector,
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
  const imageUrlInput = imageUrlSelector ? form.querySelector(imageUrlSelector) : null;

  if (!fileInput || !filePlaceholder || !fileRemoveButton) return;

  const imageUrlValue = imageUrlInput?.value?.trim() ?? "";
  let hasExistingFile =
    imageUrlValue.length > 0 ||
    fileInput.dataset.hasExistingFile === "True" ||
    fileInput.dataset.hasExistingFile === "true";

  const existingFileLabelFromData = fileInput.dataset.existingFileLabel?.trim();
  const existingFileLabel =
    existingFileLabelFromData && existingFileLabelFromData.length > 0
      ? existingFileLabelFromData
      : (imageUrlValue.split("/").pop()?.split("?")[0] ?? "Current profile image");

  const updateFileSelectionUi = () => {
    const selectedFile = fileInput.files?.[0];

    if (selectedFile) {
      filePlaceholder.textContent = selectedFile.name;
      filePlaceholder.classList.add("is-selected");
      fileRemoveButton.hidden = false;
      return;
    }

    if (hasExistingFile) {
      filePlaceholder.textContent = existingFileLabel;
      filePlaceholder.classList.add("is-selected");
      fileRemoveButton.hidden = false;
      return;
    }

    filePlaceholder.textContent = emptyText;
    filePlaceholder.classList.remove("is-selected");
    fileRemoveButton.hidden = true;
  };

  fileInput.addEventListener("change", () => {
    hasExistingFile = false;
    updateFileSelectionUi();
  });

  fileRemoveButton.addEventListener("click", (event) => {
    event.preventDefault();
    event.stopPropagation();

    fileInput.value = "";
    hasExistingFile = false;

    if (imageUrlInput) imageUrlInput.value = "";
    fileInput.classList.remove("input-error");
    if (fileErrorMessage) fileErrorMessage.textContent = "";

    updateFileSelectionUi();
  });

  updateFileSelectionUi();
}
