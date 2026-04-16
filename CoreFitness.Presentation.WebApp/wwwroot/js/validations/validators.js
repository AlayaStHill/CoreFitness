export function required(value, message = "This field is required") {
  return value.trim() ? "" : message;
}

export function minLength(value, min, message) {
  if (!value.trim()) return "";
  return value.trim().length >= min ? "" : message;
}

export function email(value, message = "Invalid email address") {
  if (!value.trim()) return "";
  const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return regex.test(value) ? "" : message;
}

export function phone(value, message = "Invalid phone number") {
  if (!value.trim()) return "";
  const regex = /^[0-9+\-\s()]*$/;
  return regex.test(value) ? "" : message;
}

export function imageFile(
  input,
  {
    allowedTypes = ["image/jpeg", "image/png", "image/webp"],
    maxSizeBytes = 2 * 1024 * 1024,
    typeMessage = "Only JPG, PNG or WEBP images are allowed",
    sizeMessage = "Image must be 2 MB or smaller",
  } = {},
) {
  const file = input?.files?.[0];
  if (!file) return "";

  if (!allowedTypes.includes(file.type)) return typeMessage;
  if (file.size > maxSizeBytes) return sizeMessage;

  return "";
}
