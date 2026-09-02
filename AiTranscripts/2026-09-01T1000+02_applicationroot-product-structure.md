# AI Transcript — 2026-09-01 10:00 (UTC+2)

## User

I've removed the naming of the modules to not include Driven.* in project name and renamed the solution to be "product" instead of catalog" I've also renamed the postgres adapter to be ProductStore. This last will be a repository. Also name the entrypoint to ApplicationRoot to fit an established convention. there will not be 2 host processes. There will be one ApplicationRoot that is the main process. the BFF and web api can be added as their own services. Here is another example. This repository has it nested twice, so don;t get caught up on that. But inside this repo is a Profile Directory that has the ApplicationRoot for the profile service set up and the web api added as an adapter. This should inspire how I want htis project to be structured

## Assistant

Updated the dossier to one process (`Product.ApplicationRoot`) with `Product.Bff` and `Product.WebApi` as adapter services, `Product.ProductStore` as the Postgres repository, and `product` / `product-portal` under `Solution/`. Added a Cursor rule for Core Services Profile as a structure reference (no Driven/Driver names, no inner-host reverse proxy). No Solution code yet.
