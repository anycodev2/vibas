# API

- `api` to lekka warstwa serwerowa, której zadaniem jest udostępnianie danych i operacji przez HTTP dla klientów (web, desktop, inne).

Co zawiera
- `Program.cs` — konfiguracja aplikacji i rejestracja kontrolerów.
- `Controllers` — kontrolery obsługujące endpointy.
- Modele używane w odpowiedziach/żądaniach — jeśli to konieczne, pochodzą z `shared`.

Jak działa (w skrócie)
- Każdy kontroler mapuje zestaw endpointów HTTP do operacji aplikacji.
- `api` przyjmuje żądanie → mapuje/potwierdza dane → deleguje do logiki lub mapuje modele z `shared` → zwraca odpowiedź.
