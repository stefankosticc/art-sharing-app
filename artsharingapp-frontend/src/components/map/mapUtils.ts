export const MAP_PIN_WRAPPER_CLASS = "map-pin-wrapper";

const HTML_ESCAPES: Record<string, string> = {
  "&": "&amp;",
  "<": "&lt;",
  ">": "&gt;",
  '"': "&quot;",
  "'": "&#39;",
};

export const escapeHtml = (value: string): string =>
  value.replace(/[&<>"']/g, (char) => HTML_ESCAPES[char]);
