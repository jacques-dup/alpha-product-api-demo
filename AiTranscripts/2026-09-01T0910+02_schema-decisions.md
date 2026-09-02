# AI Transcript — 2026-09-01 09:10 (UTC+2)

## User

for 1. translation paradigm. Look at how Brightcove and Vimeo could produce the dubs and subs. Do they offer different URLs for a dub or is it still the same file with a different audio track. I'll go back to see how this was defined in MyAlpha to be sure.

for 2. the product identifier in whatsapp content distribution. That identifier was conceptualized as an easy fot for the manifest. with this API we have the capability to do better. the main issue i foresee is the audience tag with a many-to-many schema. that needs to be solved. what would be the risks of adding a default_audience tag to be used for this identification? country matches are irrelevant as ke and co will be treated the same, we just need an acl map from the country code to lookup in the market table. the language question defers to #1 as it will be influenced by how we can provide the assets

for 3. this is a ui concern, surely. the api can structure the data by any contract or shape we define.

for 4. lets remove all 4 fields from product and the data dictionary. it shows intent but can be removed from scope.

for 5. remove product status. that is a wordpress concept we don't need. Also remove market status as out of scope for this round.

for 6. remove them from the dictionary.

for 7. asset_market exact matches are intended. you can have a global course with a market specific asset that you would want to filter out. product_item sharing is the exception to the rule, so if 2 courses want the same video but as different episodes, we'll add 2 different product item entries. Acknowledge the tag category casing. we'll normalize the inputs and put restriction on the UI at first to only enable a few hard coded options.

## Assistant

Brightcove and Vimeo both treat dubs as extra audio tracks on one video for playback (same title/embed; player switches tracks). Subs are text tracks, not a second video. Download is different: Brightcove social/muxed MP4 always uses the source muxed audio, so a dub does not get its own MP4 URL from the same title. Vimeo can download an alternate audio track as its own file URL, not a second muxed video. WCD is download-first MP4, so a dub for that consumer is a distinct download URL (and today a distinct hosted file), not a track switch. Do not model dub as a new product; `provider_asset_id` + `language_code` + `download_url` is enough until MyAlpha is checked.

`default_audience` not added. Main risks: two sources of truth vs `product_tag`; default missing or not in the M2M set; "default" reads as UI fallback not identity; a product can only appear under one WCD audience key unless you also query tags. Cleaner alternative: constrain to exactly one `audience` tag per product. Country: ACL map caller country → `market.code` (`ke`/`co` equivalent for match). Language waits on #1.

Grouping left as free-text; presentation is API/UI.

Removed from product and dictionary: slug, sequence, legacy_ref, published_at. Removed product.status and product_market status (kept launched_on). Removed dictionary-only extras (views; is_active except language; asset sequence/mime_type/legacy_ref). Captured exact asset_market match, unshared product_item, normalised tag category with hard-coded UI options.
