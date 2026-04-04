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
