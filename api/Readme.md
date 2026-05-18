# API

- `api` is a lightweight server layer whose task is to expose data and operations over HTTP for clients (web, desktop, others).

What it contains
- `Program.cs` — application configuration and controller registration.
- `Controllers` — controllers handling endpoints.
- Models used in responses/requests — if necessary, they come from `shared`.

How it works (in brief)
- Each controller maps a set of HTTP endpoints to application operations.
- `api` accepts a request → maps/validates data → delegates to logic or maps models from `shared` → returns a response.
