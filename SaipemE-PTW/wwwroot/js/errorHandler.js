// Data: 2025-10-16 - Gestore errori JavaScript globale: intercetta errori ed inoltra a console in modo strutturato
(function () {
  'use strict';
  // Protezione: evitare re-definizione iniettando flag su window - Data: 2025-10-16
  if (window.__saipem_error_handler_installed__) return;
  window.__saipem_error_handler_installed__ = true;

  // Data: 2025-10-16 - Buffer sicuro per errori JS da mostrare in UI
  const MAX_ITEMS = 200;
  const errors = [];
  function pushError(item) {
    try {
      errors.push(item);
      while (errors.length > MAX_ITEMS) errors.shift();
    } catch { /* no-op */ }
  }

  function sanitize(str) {
    try {
      if (typeof str !== 'string') return '';
      return str.replace(/[\n\r\t]/g, ' ').slice(0, 5000);
    } catch { return ''; }
  }

  // Data: 2025-10-16 - Invio errori a console in modo strutturato (Serilog BrowserConsole li mostrerà)
  function logClientError(payload) {
    try {
      const msg = `[JS-ERROR] ${payload.message} | src=${payload.source} | line=${payload.lineno} col=${payload.colno} | id=${payload.id}`;
      console.error(msg, payload.stack ? { stack: payload.stack } : {});
      pushError({
        id: payload.id,
        message: payload.message,
        source: payload.source,
        line: payload.lineno,
        col: payload.colno
      });
    } catch { /* no-op */ }
  }

  window.addEventListener('error', function (event) {
    try {
      const id = (self.crypto && crypto.randomUUID) ? crypto.randomUUID().replace(/-/g, '') : Date.now().toString(36);
      const payload = {
        id,
        message: sanitize(event.message || 'Script error'),
        source: sanitize(event.filename || ''),
        lineno: event.lineno || 0,
        colno: event.colno || 0,
        stack: event.error && event.error.stack ? sanitize(String(event.error.stack)) : ''
      };
      logClientError(payload);
    } catch { /* no-op */ }
  }, true);

  window.addEventListener('unhandledrejection', function (event) {
    try {
      const id = (self.crypto && crypto.randomUUID) ? crypto.randomUUID().replace(/-/g, '') : Date.now().toString(36);
      const reason = event.reason ? (event.reason.message || String(event.reason)) : 'Unhandled rejection';
      const stack = event.reason && event.reason.stack ? String(event.reason.stack) : '';
      const payload = {
        id,
        message: sanitize(reason),
        source: 'promise',
        lineno: 0,
        colno: 0,
        stack: sanitize(stack)
      };
      logClientError(payload);
    } catch { /* no-op */ }
  }, true);

  // Data: 2025-10-16 - API esposte per UI Blazor: get/clear e trigger test
  window.saipemErrors = {
    get: function () { return errors.slice(); },
    clear: function () { errors.length = 0; },
    triggerReferenceError: function () { /* genera ReferenceError intenzionale */
      // eslint-disable-next-line no-undef
      nonEsisto();
    },
    triggerPromiseRejection: function () {
      // Rigetta intenzionalmente una Promise
      Promise.reject(new Error('Rejected for test'));
    },
    download: function (text, fileName) {
      try {
        const blob = new Blob([text || ''], { type: 'text/plain;charset=utf-8' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = fileName || 'client-log.txt';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(url);
      } catch { /* no-op */ }
    }
  };
})();
