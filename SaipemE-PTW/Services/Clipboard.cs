//using System.Threading.Tasks;

//namespace SaipemE_PTW
//{
//    // Data: 2025-10-16 - Helper Clipboard per Blazor WASM: fallback no-op per evitare errori di compilazione
//    // Nota: Per copia reale usare JS interop (navigator.clipboard). Questo wrapper evita riferimenti mancanti.
//    public static class Clipboard
//    {
//        public static IClipboard Default { get; } = new NoopClipboard();

//        public interface IClipboard
//        {
//            Task SetTextAsync(string text);
//        }

//        private sealed class NoopClipboard : IClipboard
//        {
//            public Task SetTextAsync(string text)
//            {
//                // Data: 2025-10-16 - No-op sicuro. Implementare via JS interop nella UI quando necessario
//                return Task.CompletedTask;
//            }
//        }
//    }
//}
