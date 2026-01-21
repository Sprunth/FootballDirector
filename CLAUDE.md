# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Football Director is a Football Manager-style game where players take the role of a Director of Football. The distinctive feature is LLM integration that makes NPCs (players, coaches) feel more intelligent and reactive.

## Build Commands

### Frontend (React + Vite)

```bash
cd footballdirector.client
npm install          # Install dependencies
npm run dev          # Start dev server with HMR
npm run build        # Build to ../FootballDirector.Server/wwwroot
npm run lint         # Run ESLint
```

### Backend (.NET 10)

```bash
dotnet build                           # Build all projects
dotnet build --configuration Release   # Release build
dotnet run --project FootballDirector.Server  # Run server only
```

### Full Development

Open `FootballDirector.slnx` in Visual Studio and run with both `FootballDirector.Server` and `footballdirector.client` as startup projects.

## Architecture

### Solution Structure

- **footballdirector.client/** - React SPA frontend (Vite, Tailwind, shadcn/ui pattern). See `footballdirector.client/CLAUDE.md` for frontend-specific guidance.
- **FootballDirector.Server/** - ASP.NET Core web host (controllers, not game logic)
- **FootballDirector.Core/** - Game logic library and LLM services
- **FootballDirector.Contracts/** - Shared DTOs (Person, Footballer, Staff types, Club, Conversation)
- **FootballDirector.Desktop/** - Windows WPF shell with WebView2

### Deployment Modes

1. **Web Development**: Vite dev server + ASP.NET via SPA proxy
2. **Desktop**: WPF app starts embedded ASP.NET server on localhost, loads UI in WebView2
3. **Production Web**: React builds into `wwwroot`, served by ASP.NET

### Key Entry Points

- Frontend: `footballdirector.client/src/main.jsx`
- Server: `FootballDirector.Server/Program.cs`
- Desktop: `FootballDirector.Desktop/App.xaml.cs` (uses `LocalGameHost.cs` for in-process server)

### Build Output Flow

React builds output to `FootballDirector.Server/wwwroot`. Desktop app copies these files at build time.

## Game Design Philosophy

- Player is **Director of Football** (not match-day manager) — focus on squad building, transfers, hiring staff, club strategy
- LLM integration makes NPCs feel intelligent and reactive — conversations, negotiations, personality-driven behavior
- UI aesthetic: **"Fake professional with a twinge of quirky"** — sports broadcast feel, not a toy or spreadsheet
- Fun stories on the edge of believable — the LLM content provides personality, not the UI styling
- Start simple, add complexity only when proven necessary

## Development Notes

- OpenAPI available at `/openapi/v1.json` in Development environment only
- No test framework configured yet
- All C# projects use nullable reference types (`<Nullable>enable</Nullable>`)
- Desktop app auto-selects free port for local server
- This is a one-person project and prototype. Do not overbuild. It is OK to hardcode things, skip abstractions.
- Make choices that are friendly to AI coding agents.
- Use fictional names for all game entities (players, staff, clubs). Do not use real-world football players or teams, even for test data.
- Controllers should be lightweight — complex logic goes in services in the Core project.
