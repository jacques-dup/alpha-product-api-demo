# AI Transcript — 2026-08-31 21:03 (UTC+2)

## User

side note. my markdown preview displayes most mermaid charts but not the journey ones. they do show up in the mermaid plugin's interface. ensure that the syntax is correct for these to display in the .md preview. let me know if the syntax is correct according to the mermaid spec. do not change the charts to a different type without verification

## Assistant

The journey syntax is correct against the Mermaid spec. Charts were not changed.

Spec form (https://mermaid.js.org/syntax/userJourney.html):

```
journey
    title ...
    section ...
      Task name: <score 1-5>: Actor
      Task name: <score>: Actor1, Actor2
```

Both dossier journeys match that: `journey` keyword, `title`, `section`s, tasks as `name: 5: actors`, no extra colons in task names. That is why the Mermaid plugin renders them.

Markdown preview showing flowcharts but not journeys is not a syntax error in these blocks. The preview engine is almost certainly not running the user-journey diagram (limited diagram set, older bundle, or a conflict with another Mermaid extension). A flowchart with an em dash in this same file still previews, so the titles' dashes are not the failure.

I did not convert them to another type. If you want them visible in `.md` preview without the plugin, that would mean changing type (for example to flowchart) after you confirm you want that trade-off.
