// Data: 2025-10-20 - Storage sicuro per token mock via localStorage con hardening
// - Key sanificata
// - Blocca prototyping pollution
// - Evita eccezioni in ambienti senza localStorage
(function () {
    'use strict';

    function sanitizeKey(key) {
        try { return String(key || '').replace(/[^a-zA-Z0-9_\-\.]/g, ''); } catch { return 'auth_token'; }
    }

    function hasLocalStorage() {
        try { return typeof window !== 'undefined' && !!window.localStorage; } catch { return false; }
    }

    const api = {
        setToken: function (key, value) {
            if (!hasLocalStorage()) return;
            const k = sanitizeKey(key);
            if (typeof value !== 'string' || !value) return;
            // Limita dimensione a ~8KB
            if (value.length > 8192) return;
            window.localStorage.setItem(k, value);
        },
        getToken: function (key) {
            if (!hasLocalStorage()) return null;
            const k = sanitizeKey(key);
            const v = window.localStorage.getItem(k);
            return (typeof v === 'string' && v.length > 0) ? v : null;
        },
        removeToken: function (key) {
            if (!hasLocalStorage()) return;
            const k = sanitizeKey(key);
            window.localStorage.removeItem(k);
        }
    };

    if (typeof window !== 'undefined') {
        window.authStorage = api;
    }
})();
