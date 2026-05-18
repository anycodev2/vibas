# Standalone

Description
- `standalone` is the presentation layer — a standalone desktop application that serves as the main user interface of the system. Its primary task is to present data, collect user actions and delegate operations to the domain layer (`shared`) or to external services (`api`).

Tasks and responsibilities
-
- `Views` — UI definitions (XAML/AXAML). Examples: `MainWindow.axaml`, `StartWindow.axaml`.
- `ViewModels` — presentation logic and view state. They should contain commands, notifyable properties and minimal UI logic.
- `App.axaml(.cs)` / `Program.cs` — application initialization, DI configuration and service registration at startup.
- `ViewLocator.cs` — mechanism for mapping view ↔ viewmodel (helps automatic wiring).
- Integrations with `shared` — services and models fetched from the shared library.
