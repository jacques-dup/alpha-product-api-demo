-- Product catalog schema (dossier section 2.7).
-- Country-market ACL is application code, not a table.
-- Audience is many-to-many; there is no unique-audience constraint.
-- Re-runnable: drops catalog tables in public, then recreates them.

BEGIN;

-- UUID defaults use core gen_random_uuid() (PostgreSQL 13+). Azure Flexible Server
-- does not allow CREATE EXTENSION pgcrypto unless it is added to azure.extensions.

DROP TABLE IF EXISTS
    asset_market,
    asset,
    product_item,
    product_tag,
    product_market,
    product,
    product_family,
    tag,
    market,
    language
CASCADE;

CREATE TABLE language (
    code      text    PRIMARY KEY,
    is_active boolean NOT NULL DEFAULT true
);

CREATE TABLE market (
    code text PRIMARY KEY,
    kind text NOT NULL,
    name text NOT NULL,
    CONSTRAINT market_kind_check CHECK (kind IN ('country', 'region'))
);

CREATE TABLE product_family (
    id       uuid    PRIMARY KEY DEFAULT gen_random_uuid(),
    code     text    NOT NULL,
    name     text    NOT NULL,
    summary  text    NULL,
    sequence integer NOT NULL,
    CONSTRAINT product_family_code_key UNIQUE (code)
);

CREATE TABLE product (
    id                uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    family_id         uuid NOT NULL REFERENCES product_family (id) ON DELETE RESTRICT,
    code              text NOT NULL,
    title             text NOT NULL,
    summary           text NULL,
    description       text NULL,
    content_language  text NOT NULL REFERENCES language (code) ON DELETE RESTRICT,
    CONSTRAINT product_code_key UNIQUE (code)
);

CREATE TABLE tag (
    id        uuid    PRIMARY KEY DEFAULT gen_random_uuid(),
    category  text    NOT NULL,
    code      text    NOT NULL,
    name      text    NOT NULL,
    is_public boolean NOT NULL DEFAULT true,
    sequence  integer NOT NULL,
    CONSTRAINT tag_category_lowercase_check CHECK (category = lower(category)),
    CONSTRAINT tag_category_code_key UNIQUE (category, code)
);

CREATE TABLE product_tag (
    product_id uuid NOT NULL REFERENCES product (id) ON DELETE CASCADE,
    tag_id     uuid NOT NULL REFERENCES tag (id) ON DELETE RESTRICT,
    PRIMARY KEY (product_id, tag_id)
);

CREATE TABLE product_market (
    product_id  uuid NOT NULL REFERENCES product (id) ON DELETE CASCADE,
    market_code text NOT NULL REFERENCES market (code) ON DELETE RESTRICT,
    launched_on date NULL,
    PRIMARY KEY (product_id, market_code)
);

CREATE TABLE product_item (
    id          uuid    PRIMARY KEY DEFAULT gen_random_uuid(),
    product_id  uuid    NOT NULL REFERENCES product (id) ON DELETE CASCADE,
    kind        text    NOT NULL,
    code        text    NOT NULL,
    sequence    integer NOT NULL,
    title       text    NOT NULL,
    summary     text    NULL,
    grouping    text    NULL,
    is_optional boolean NOT NULL DEFAULT false,
    CONSTRAINT product_item_kind_check CHECK (kind IN ('episode', 'training')),
    CONSTRAINT product_item_product_id_code_key UNIQUE (product_id, code),
    CONSTRAINT product_item_product_id_kind_sequence_key UNIQUE (product_id, kind, sequence),
    CONSTRAINT product_item_id_product_id_key UNIQUE (id, product_id)
);

CREATE TABLE asset (
    id                uuid    PRIMARY KEY DEFAULT gen_random_uuid(),
    product_id        uuid    NOT NULL REFERENCES product (id) ON DELETE CASCADE,
    item_id           uuid    NULL,
    role              text    NOT NULL,
    kind              text    NOT NULL,
    language_code     text    NULL REFERENCES language (code) ON DELETE RESTRICT,
    title             text    NULL,
    group_code        text    NULL,
    provider          text    NOT NULL,
    provider_asset_id text    NULL,
    stream_url        text    NULL,
    download_url      text    NULL,
    allow_stream      boolean NOT NULL DEFAULT false,
    allow_download    boolean NOT NULL DEFAULT false,
    duration_seconds  integer NULL,
    file_size_bytes   bigint  NULL,
    CONSTRAINT asset_role_check CHECK (
        role IN (
            'main_video',
            'supporting',
            'material',
            'thumbnail',
            'promo_video',
            'promo_banner',
            'hero_image'
        )
    ),
    CONSTRAINT asset_kind_check CHECK (kind IN ('video', 'document', 'image', 'audio', 'link')),
    CONSTRAINT asset_provider_check CHECK (provider IN ('brightcove', 'vimeo', 'url')),
    CONSTRAINT asset_item_product_fkey FOREIGN KEY (item_id, product_id)
        REFERENCES product_item (id, product_id)
        ON DELETE CASCADE
);

CREATE TABLE asset_market (
    asset_id    uuid NOT NULL REFERENCES asset (id) ON DELETE CASCADE,
    market_code text NOT NULL REFERENCES market (code) ON DELETE RESTRICT,
    PRIMARY KEY (asset_id, market_code)
);

COMMIT;
