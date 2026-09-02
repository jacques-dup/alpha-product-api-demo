# AI Transcript — 2026-09-01 09:26 (UTC+2)

## User

For added context of the legacy service: MyAlpha never names “contextualization.” It uses three different graphs, and dub/sub live on episode videos, not on the product.
1. Different course (Youth, Prison, Marriage)
Separate product posts. Related only by coursequestion tags. That is the closest thing to a contextualized offering.
2. Same product, different country
Not a new product. Country sites shadow-override field groups of the global post (parent_relationships: plan, hero, promo, copy, …). Resolve order is global → country override → language.
3. Same product, different language of copy
RTML language_relationships: another product post, parent_id = English primary. rtml_language picks it. A product reference is always (id, blog_id, language).

dubs are stored as a translated resource post with its own primary video id and a unique vimeo url. brightcove uses language flags when requesting the video by id. Brightcove is the newer system that will eventually replace vimeo, so lets focus on using that pattern.

So a "Contextualization" (different video and audio content) is a new product. Thought the themes might be the same it will be a completely new version.
A translation can be a sub/dub of the English version. We won't be making new product entries for "English AFS with French subs/dubs"

Consider this and give me feedback as to waht is viable to be within scope for this exercise.

## Assistant

(See chat: viability of MyAlpha graphs vs WCD POC. Dossier not edited.)
