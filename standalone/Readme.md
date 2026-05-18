# Standalone

Opis
- `standalone` to warstwa prezentacji — samodzielna aplikacja desktopowa stanowiąca główny interfejs użytkownika systemu. Jej głównym zadaniem jest prezentacja danych, zbieranie działań użytkownika i delegowanie operacji do warstwy domenowej (`shared`) lub do zewnętrznych serwisów (`api`).

Zadania i odpowiedzialności
-
- `Views` — definicje interfejsu (XAML/AXAML). Przykłady: `MainWindow.axaml`, `StartWindow.axaml`.
- `ViewModels` — logika prezentacji i stan widoków. Powinny zawierać komendy, właściwości powiadamiające i minimalną logikę UI.
- `App.axaml(.cs)` / `Program.cs` — inicjalizacja aplikacji, konfiguracja DI i rejestracja usług na starcie.
- `ViewLocator.cs` — mechanizm mapowania widok ↔ viewmodel (ułatwia automatyczne podpinanie).
- Integracje z `shared` — serwisy i modele pobierane z biblioteki współdzielonej.
