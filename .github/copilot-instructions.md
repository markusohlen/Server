# Copilot Instructions

## Project Overview

This is a .NET 10 Blazor WebAssembly application using **Radzen Blazor** components for the UI.

## Styling

- **Do not use Bootstrap.** The project has removed Bootstrap entirely.
- Use **Radzen Blazor** components and their built-in styling.
- Override Radzen theming via CSS custom properties (e.g. `--rz-primary`, `--rz-text-font-family`) in `Presentation/wwwroot/css/app.css`.
- Use **component-scoped CSS** (`.razor.css` files) for per-component styles instead of Bootstrap utility classes.
- Use semantic class names in scoped CSS (e.g. `.page-title`, `.section-title`) rather than utility classes.

## Component Structure

- Use **code-behind files** (`.razor.cs`) for all component logic. Do not use inline `@code` blocks in `.razor` files.
- Use `[Inject]` attribute on properties in code-behind files instead of `@inject` directives in `.razor` files.
- Keep `.razor` files focused on markup only.
- Each component should have up to three files:
  - `Component.razor` — markup
  - `Component.razor.cs` — code-behind (partial class)
  - `Component.razor.css` — scoped styles
