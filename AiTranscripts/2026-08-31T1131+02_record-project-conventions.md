# AI Transcript — 2026-08-31 11:31 (UTC+2)

## User

This is a repo that will be used for a technical exercise. Before I start record the following AI parameters. I will use AI to produce documentation, plans, diagrams, minimal writeup, tracking of activities and edits, research and code production as part of this exercise. This root dir has a directory for AiTranscripts/ where all AI interaction transcripts need to be stored as .md files - including this one. Transcripts need to be timestamped, with the exact user input and model output. No analyzing thoughts need to be commited to the docs. The Brief/ directory contains the original brief pdf. For easier ingestion the brief can be summarized as a .md file in the same directory. The Documentation/ directory will be where all my architectural docs, planning and reasoning docs, etc will be stored as .md files. The Solution/ directory is where the code will be located. The TimeTracking/ directory will be where I track my engagement with this challenge. The AI model can create a timeline.md document where it can automatically keep track of my sessions. I will also add to this timeline doc manually if I do any work or research outside of using the AI so no removals are to be done on it - only additions. Commit this to memory for this project.

## Assistant

These conventions are now recorded for this project as an always-apply Cursor rule, so they will be followed in later sessions.

**AI usage:** documentation, plans, diagrams, a minimal writeup, activity and edit tracking, research, and code.

**Layout**

| Directory | Role |
|---|---|
| `Brief/` | Original PDF plus `brief-summary.md` |
| `Documentation/` | Architecture, planning, and reasoning (`.md`) |
| `Solution/` | Product code |
| `AiTranscripts/` | Timestamped session transcripts (user input + model output only; no internal reasoning) |
| `TimeTracking/` | `timeline.md` — append-only session log |

**Done this session**

- Created the empty dirs (`Documentation/`, `Solution/`) and confirmed `Brief/` already had the PDF.
- Summarized the brief at `Brief/brief-summary.md`.
- Started `TimeTracking/timeline.md` with this session (additions only from here on).
- Saved this exchange at `AiTranscripts/2026-08-31T1131+02_record-project-conventions.md`.
- Persisted the rules in `.cursor/rules/technical-exercise-conventions.mdc`.

The brief is a 48-hour / 12-hour-effort exercise: conceive, build, and deploy a digital experience that supports Alpha’s mission, with a live product, auth, persistent data, an end-to-end workflow, and a full process archive. Ready when you are to start.
