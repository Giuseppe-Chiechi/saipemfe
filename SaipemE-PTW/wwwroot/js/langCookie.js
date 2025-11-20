"use strict";
// Data: 2025-10-08 - Utility JS per lingua preferita e gestione cookie sicura
// - Rileva navigator.languages / navigator.language (user agent)
// - Sanifica il valore lingua per evitare injection
// - Gestisce cookie con SameSite=Lax, Path=/ e Secure (se HTTPS)

function sanitizeLang(lang) {
  try {
    if (typeof lang !== "string") return "en";
    const trimmed = lang.trim();
    // Consenti pattern tipo: it, it-IT, en-US, zh-Hans-CN (semplificato)
    const re = /^[a-zA-Z]{2,3}(-[a-zA-Z0-9]{2,8})*$/;
    return re.test(trimmed) ? trimmed : "en";
  } catch {
    return "en";
  }
}

function getPreferredLanguage() {
  try {
    const navLang = Array.isArray(navigator.languages) && navigator.languages.length > 0
      ? navigator.languages[0]
      : (navigator.language || "en");
    return sanitizeLang(navLang);
  } catch {
    return "en";
  }
}

function setCookie(name, value, days = 365) {
  try {
    if (typeof name !== "string" || !name) return;
    const safeName = name.replace(/[^a-zA-Z0-9_\-\.]/g, "");
    const safeVal = encodeURIComponent(String(value ?? ""));
    const maxAge = Math.max(0, Math.floor(days * 24 * 60 * 60));
    const isHttps = typeof location !== "undefined" && location.protocol === "https:";
    const parts = [
      `${safeName}=${safeVal}`,
      `Max-Age=${maxAge}`,
      "Path=/",
      "SameSite=Lax",
    ];
    if (isHttps) parts.push("Secure");
    document.cookie = parts.join("; ");
  } catch {
    // no-op
  }
}

function getCookie(name) {
  try {
    if (typeof name !== "string" || !name) return "";
    const safeName = name.replace(/[^a-zA-Z0-9_\-\.]/g, "");
    const all = document.cookie ? document.cookie.split(/;\s*/) : [];
    for (const c of all) {
      const idx = c.indexOf("=");
      const k = idx > -1 ? c.substring(0, idx) : c;
      const v = idx > -1 ? c.substring(idx + 1) : "";
      if (k === safeName) return decodeURIComponent(v);
    }
    return "";
  } catch {
    return "";
  }
}

function deleteCookie(name) {
  try {
    if (typeof name !== "string" || !name) return;
    const safeName = name.replace(/[^a-zA-Z0-9_\-\.]/g, "");
    const isHttps = typeof location !== "undefined" && location.protocol === "https:";
    const parts = [
      `${safeName}=`,
      "Max-Age=0",
      "Path=/",
      "SameSite=Lax",
    ];
    if (isHttps) parts.push("Secure");
    document.cookie = parts.join("; ");
  } catch {
    // no-op
  }
}

// Data: 2025-10-09 - Esposizione globale per interop Blazor (le funzioni sono esportate come ES module)
// Blazor IJSRuntime.InvokeAsync("<identifier>") cerca su window. Mappiamo quindi le funzioni su window.
if (typeof window !== "undefined") {
  window.getPreferredLanguage = getPreferredLanguage;
  window.setCookie = setCookie;
  window.getCookie = getCookie;
  window.deleteCookie = deleteCookie;
}
