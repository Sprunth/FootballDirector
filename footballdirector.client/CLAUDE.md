# Frontend Agent Instructions

This file provides guidance for AI agents working on the Football Director web client.

## UI/UX Philosophy

**"Fake professional with a twinge of quirky"**

- Sports broadcast aesthetic — deep slate backgrounds, amber/gold accents
- Professional enough that players feel like they're in a Director of Football role
- NOT a spreadsheet or actual job application — keep it lighthearted
- Personality and humor come from LLM-generated content (conversations, backstories), not from wild UI colors or excessive styling
- The game generates fun stories on the edge of believable — UI should support that tone

**Visual Guidelines:**
- Tight border radius: 4-8px max (defined in `@theme`), not bubbly/toy-like
- Primary color: amber/gold (trophy, achievement feel)
- Accent: teal for positive states
- Semantic colors: green=success, amber=warning, red=danger (used in stat bars)

## Tech Stack

- **React 19** with Vite
- **Tailwind CSS v4** with `@tailwindcss/vite` plugin
- **shadcn/ui pattern**: Components in `src/components/ui/` are fully owned (not a dependency)
- **lucide-react** for icons
- **react-router-dom** for routing

## Project Structure

```
src/
├── api/
│   └── api.js              # All API fetch functions
├── components/
│   ├── ui/                 # Base components (Card, Badge, StatBar, etc.)
│   │   └── index.js        # Exports all UI components
│   └── shared/
│       └── Layout.jsx      # App shell with nav sidebar
├── pages/                  # Route components
│   ├── ClubOverview.jsx
│   ├── Squad.jsx
│   ├── Staff.jsx
│   └── Inbox.jsx
├── lib/
│   └── utils.js            # cn() helper for class merging
└── index.css               # Tailwind + theme configuration
```

## Component Patterns

### Adding UI Components

Follow shadcn/ui conventions:
1. Create component in `src/components/ui/`
2. Use `cn()` from `@/lib/utils` for conditional classes
3. Use `class-variance-authority` for variants
4. Export from `src/components/ui/index.js`

```jsx
// Example: components/ui/button.jsx
import { cva } from "class-variance-authority";
import { cn } from "@/lib/utils";

const buttonVariants = cva("base-classes", {
    variants: {
        variant: { default: "...", secondary: "..." },
        size: { default: "...", sm: "..." },
    },
    defaultVariants: { variant: "default", size: "default" },
});

export function Button({ className, variant, size, ...props }) {
    return <button className={cn(buttonVariants({ variant, size }), className)} {...props} />;
}
```

### Theme Customization

All theme tokens are in `src/index.css` under `@theme`. Modify there, not in components:

```css
@theme {
    --color-primary: oklch(0.75 0.15 75);  /* Amber/gold */
    --radius-md: 0.25rem;                   /* Tight corners */
}
```

## Pages Overview

| Page | Route | Purpose |
|------|-------|---------|
| Club Overview | `/` | Dashboard: club info, finances, quick links to other pages |
| Squad | `/squad` | Player list + detail panel (stats, personality, backstory) |
| Staff | `/staff` | Staff list + detail panel (role-specific stats, personality) |
| Inbox | `/inbox` | NPC-initiated conversations, message threads |

## API Layer

All API functions in `src/api/api.js`. Pattern:

```js
export async function getSomething() {
    const response = await fetch('/api/something');
    if (!response.ok) throw new Error(`Failed: ${response.statusText}`);
    return response.json();
}
```

### Current Endpoints

- `GET /api/club` — Club overview with finances and counts
- `GET /api/footballers` — All players
- `GET /api/footballers/{id}` — Single player
- `GET /api/staff?role=` — All staff (optional role filter)
- `GET /api/staff/{id}` — Single staff member
- `GET /api/inbox` — NPC-initiated conversations
- `GET /api/conversation/{id}` — Full conversation with messages
- `GET /api/person/{personId}/conversations` — All conversations with a person

## Data Model Notes

### Naming Conventions
- `footballer` = individual player (singular)
- `squad` = future concept for player groupings (first team, U21, etc.) — not yet implemented
- Don't rename `footballer` to `player` — too generic

### Staff Types
The API returns a unified `StaffMember` object with nullable role-specific attributes:
- Coach: attacking, defending, goalkeeping, tactics, communication, specialization
- Manager: tactics, manManagement, motivation, mediaHandling
- Scout: judgingAbility, judgingPotential
- Physio: injuryPrevention, recovery
- ChiefExecutive: businessAcumen, negotiation
- ClubOwner: wealth, ambition

Check for `!= null` before displaying role-specific stats.

### Conversations
- Same `Conversation` model used everywhere
- `initiatedByNpc: true` → shows in Inbox
- `initiatedByNpc: false` → player started it (shown in person detail)
- Both views use the same conversation interface

## Deferred Features (Not Yet Built)

- News/events feed
- Transfer market / scouting
- Match day view
- Fixtures calendar
- POST endpoints (mutations, sending messages, etc.)

## Development Notes

- Path alias: `@/` maps to `src/` (configured in vite.config.js and jsconfig.json)
- Build output goes to `../FootballDirector.Server/wwwroot`
- Dev server proxies `/api` to the .NET backend
- Use `npm run dev` for development with HMR
