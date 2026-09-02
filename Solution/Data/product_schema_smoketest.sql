-- Schema smoke test for product_schema.sql (dossier section 2.7).
-- Run after applying product_schema.sql:
--   psql "$DATABASE_URL" -v ON_ERROR_STOP=1 -f product_schema.sql
--   psql "$DATABASE_URL" -v ON_ERROR_STOP=1 -f product_schema_smoketest.sql
-- Raises an exception if the catalog does not match the intended model.
-- Insert tests are deleted afterwards; they leave no rows.

CREATE TEMP TABLE schema_check (
    name   text PRIMARY KEY,
    ok     boolean NOT NULL,
    detail text NOT NULL
);

CREATE TEMP TABLE expected_table (
    table_name text PRIMARY KEY
);

INSERT INTO expected_table (table_name) VALUES
    ('language'),
    ('market'),
    ('product_family'),
    ('product'),
    ('tag'),
    ('product_tag'),
    ('product_market'),
    ('product_item'),
    ('asset'),
    ('asset_market');

CREATE TEMP TABLE expected_column (
    table_name   text NOT NULL,
    column_name  text NOT NULL,
    udt_name     text NOT NULL,
    is_nullable  text NOT NULL,
    PRIMARY KEY (table_name, column_name)
);

INSERT INTO expected_column (table_name, column_name, udt_name, is_nullable) VALUES
    ('language', 'code', 'text', 'NO'),
    ('language', 'is_active', 'bool', 'NO'),
    ('market', 'code', 'text', 'NO'),
    ('market', 'kind', 'text', 'NO'),
    ('market', 'name', 'text', 'NO'),
    ('product_family', 'id', 'uuid', 'NO'),
    ('product_family', 'code', 'text', 'NO'),
    ('product_family', 'name', 'text', 'NO'),
    ('product_family', 'summary', 'text', 'YES'),
    ('product_family', 'sequence', 'int4', 'NO'),
    ('product', 'id', 'uuid', 'NO'),
    ('product', 'family_id', 'uuid', 'NO'),
    ('product', 'code', 'text', 'NO'),
    ('product', 'title', 'text', 'NO'),
    ('product', 'summary', 'text', 'YES'),
    ('product', 'description', 'text', 'YES'),
    ('product', 'content_language', 'text', 'NO'),
    ('tag', 'id', 'uuid', 'NO'),
    ('tag', 'category', 'text', 'NO'),
    ('tag', 'code', 'text', 'NO'),
    ('tag', 'name', 'text', 'NO'),
    ('tag', 'is_public', 'bool', 'NO'),
    ('tag', 'sequence', 'int4', 'NO'),
    ('product_tag', 'product_id', 'uuid', 'NO'),
    ('product_tag', 'tag_id', 'uuid', 'NO'),
    ('product_market', 'product_id', 'uuid', 'NO'),
    ('product_market', 'market_code', 'text', 'NO'),
    ('product_market', 'launched_on', 'date', 'YES'),
    ('product_item', 'id', 'uuid', 'NO'),
    ('product_item', 'product_id', 'uuid', 'NO'),
    ('product_item', 'kind', 'text', 'NO'),
    ('product_item', 'code', 'text', 'NO'),
    ('product_item', 'sequence', 'int4', 'NO'),
    ('product_item', 'title', 'text', 'NO'),
    ('product_item', 'summary', 'text', 'YES'),
    ('product_item', 'grouping', 'text', 'YES'),
    ('product_item', 'is_optional', 'bool', 'NO'),
    ('asset', 'id', 'uuid', 'NO'),
    ('asset', 'product_id', 'uuid', 'NO'),
    ('asset', 'item_id', 'uuid', 'YES'),
    ('asset', 'role', 'text', 'NO'),
    ('asset', 'kind', 'text', 'NO'),
    ('asset', 'language_code', 'text', 'YES'),
    ('asset', 'title', 'text', 'YES'),
    ('asset', 'group_code', 'text', 'YES'),
    ('asset', 'provider', 'text', 'NO'),
    ('asset', 'provider_asset_id', 'text', 'YES'),
    ('asset', 'stream_url', 'text', 'YES'),
    ('asset', 'download_url', 'text', 'YES'),
    ('asset', 'allow_stream', 'bool', 'NO'),
    ('asset', 'allow_download', 'bool', 'NO'),
    ('asset', 'duration_seconds', 'int4', 'YES'),
    ('asset', 'file_size_bytes', 'int8', 'YES'),
    ('asset_market', 'asset_id', 'uuid', 'NO'),
    ('asset_market', 'market_code', 'text', 'NO');

CREATE TEMP TABLE expected_pk (
    table_name text PRIMARY KEY,
    columns    text[] NOT NULL
);

INSERT INTO expected_pk (table_name, columns) VALUES
    ('language', ARRAY['code']),
    ('market', ARRAY['code']),
    ('product_family', ARRAY['id']),
    ('product', ARRAY['id']),
    ('tag', ARRAY['id']),
    ('product_tag', ARRAY['product_id', 'tag_id']),
    ('product_market', ARRAY['product_id', 'market_code']),
    ('product_item', ARRAY['id']),
    ('asset', ARRAY['id']),
    ('asset_market', ARRAY['asset_id', 'market_code']);

CREATE TEMP TABLE expected_unique (
    table_name text NOT NULL,
    columns    text[] NOT NULL
);

INSERT INTO expected_unique (table_name, columns) VALUES
    ('product_family', ARRAY['code']),
    ('product', ARRAY['code']),
    ('tag', ARRAY['category', 'code']),
    ('product_item', ARRAY['product_id', 'code']),
    ('product_item', ARRAY['product_id', 'kind', 'sequence']),
    ('product_item', ARRAY['id', 'product_id']);

CREATE TEMP TABLE expected_fk (
    table_name     text NOT NULL,
    columns        text[] NOT NULL,
    foreign_table  text NOT NULL,
    foreign_columns text[] NOT NULL
);

INSERT INTO expected_fk (table_name, columns, foreign_table, foreign_columns) VALUES
    ('product', ARRAY['family_id'], 'product_family', ARRAY['id']),
    ('product', ARRAY['content_language'], 'language', ARRAY['code']),
    ('product_tag', ARRAY['product_id'], 'product', ARRAY['id']),
    ('product_tag', ARRAY['tag_id'], 'tag', ARRAY['id']),
    ('product_market', ARRAY['product_id'], 'product', ARRAY['id']),
    ('product_market', ARRAY['market_code'], 'market', ARRAY['code']),
    ('product_item', ARRAY['product_id'], 'product', ARRAY['id']),
    ('asset', ARRAY['product_id'], 'product', ARRAY['id']),
    ('asset', ARRAY['language_code'], 'language', ARRAY['code']),
    ('asset', ARRAY['item_id', 'product_id'], 'product_item', ARRAY['id', 'product_id']),
    ('asset_market', ARRAY['asset_id'], 'asset', ARRAY['id']),
    ('asset_market', ARRAY['market_code'], 'market', ARRAY['code']);

CREATE OR REPLACE FUNCTION pg_temp.index_columns(rel regclass, indkey int2vector)
RETURNS text[] LANGUAGE sql STABLE AS $$
    SELECT coalesce(array_agg(a.attname::text ORDER BY x.ord), ARRAY[]::text[])
    FROM unnest(indkey) WITH ORDINALITY AS x(attnum, ord)
    JOIN pg_attribute a ON a.attrelid = rel AND a.attnum = x.attnum;
$$;

CREATE OR REPLACE FUNCTION pg_temp.has_unique(tbl text, cols text[])
RETURNS boolean LANGUAGE sql STABLE AS $$
    SELECT EXISTS (
        SELECT 1
        FROM pg_index i
        JOIN pg_class c ON c.oid = i.indrelid
        JOIN pg_namespace n ON n.oid = c.relnamespace
        WHERE n.nspname = 'public'
          AND c.relname = tbl
          AND i.indisunique
          AND pg_temp.index_columns(c.oid, i.indkey) = cols
    );
$$;

CREATE OR REPLACE FUNCTION pg_temp.has_fk(src_table text, src_cols text[], dst_table text, dst_cols text[])
RETURNS boolean LANGUAGE sql STABLE AS $$
    SELECT EXISTS (
        SELECT 1
        FROM pg_constraint c
        JOIN pg_class src ON src.oid = c.conrelid
        JOIN pg_class dst ON dst.oid = c.confrelid
        JOIN pg_namespace ns ON ns.oid = src.relnamespace
        JOIN pg_namespace nd ON nd.oid = dst.relnamespace
        WHERE c.contype = 'f'
          AND ns.nspname = 'public'
          AND nd.nspname = 'public'
          AND src.relname = src_table
          AND dst.relname = dst_table
          AND (
              SELECT array_agg(a.attname::text ORDER BY x.ord)
              FROM unnest(c.conkey) WITH ORDINALITY AS x(attnum, ord)
              JOIN pg_attribute a ON a.attrelid = src.oid AND a.attnum = x.attnum
          ) = src_cols
          AND (
              SELECT array_agg(a.attname::text ORDER BY x.ord)
              FROM unnest(c.confkey) WITH ORDINALITY AS x(attnum, ord)
              JOIN pg_attribute a ON a.attrelid = dst.oid AND a.attnum = x.attnum
          ) = dst_cols
    );
$$;

INSERT INTO schema_check (name, ok, detail)
SELECT
    'tables.present.' || e.table_name,
    EXISTS (
        SELECT 1
        FROM information_schema.tables t
        WHERE t.table_schema = 'public'
          AND t.table_name = e.table_name
          AND t.table_type = 'BASE TABLE'
    ),
    CASE
        WHEN EXISTS (
            SELECT 1 FROM information_schema.tables t
            WHERE t.table_schema = 'public' AND t.table_name = e.table_name
        ) THEN 'present'
        ELSE 'missing table'
    END
FROM expected_table e;

INSERT INTO schema_check (name, ok, detail)
SELECT
    'tables.unexpected_acl_or_variant',
    NOT EXISTS (
        SELECT 1
        FROM information_schema.tables t
        WHERE t.table_schema = 'public'
          AND t.table_name IN (
              'country_market_acl',
              'country_acl',
              'variant',
              'product_variant',
              'default_audience'
          )
    ),
    coalesce(
        (
            SELECT string_agg(t.table_name, ', ' ORDER BY t.table_name)
            FROM information_schema.tables t
            WHERE t.table_schema = 'public'
              AND t.table_name IN (
                  'country_market_acl',
                  'country_acl',
                  'variant',
                  'product_variant',
                  'default_audience'
              )
        ),
        'none'
    );

INSERT INTO schema_check (name, ok, detail)
SELECT
    'columns.' || e.table_name || '.' || e.column_name,
    EXISTS (
        SELECT 1
        FROM information_schema.columns c
        WHERE c.table_schema = 'public'
          AND c.table_name = e.table_name
          AND c.column_name = e.column_name
          AND c.udt_name = e.udt_name
          AND c.is_nullable = e.is_nullable
    ),
    coalesce(
        (
            SELECT format('found %s nullable=%s', c.udt_name, c.is_nullable)
            FROM information_schema.columns c
            WHERE c.table_schema = 'public'
              AND c.table_name = e.table_name
              AND c.column_name = e.column_name
        ),
        'missing column'
    )
FROM expected_column e;

INSERT INTO schema_check (name, ok, detail)
SELECT
    'columns.extra.' || c.table_name || '.' || c.column_name,
    false,
    'column not in dossier section 2.7'
FROM information_schema.columns c
JOIN expected_table e ON e.table_name = c.table_name
WHERE c.table_schema = 'public'
  AND NOT EXISTS (
      SELECT 1
      FROM expected_column x
      WHERE x.table_name = c.table_name
        AND x.column_name = c.column_name
  );

INSERT INTO schema_check (name, ok, detail)
SELECT
    'pk.' || e.table_name,
    pg_temp.has_unique(e.table_name, e.columns)
        AND EXISTS (
            SELECT 1
            FROM pg_index i
            JOIN pg_class c ON c.oid = i.indrelid
            JOIN pg_namespace n ON n.oid = c.relnamespace
            WHERE n.nspname = 'public'
              AND c.relname = e.table_name
              AND i.indisprimary
              AND pg_temp.index_columns(c.oid, i.indkey) = e.columns
        ),
    array_to_string(e.columns, ',')
FROM expected_pk e;

INSERT INTO schema_check (name, ok, detail)
SELECT
    'unique.' || e.table_name || '.' || array_to_string(e.columns, '_'),
    pg_temp.has_unique(e.table_name, e.columns),
    array_to_string(e.columns, ',')
FROM expected_unique e;

INSERT INTO schema_check (name, ok, detail)
SELECT
    'fk.' || e.table_name || '.' || array_to_string(e.columns, '_') || '->' || e.foreign_table,
    pg_temp.has_fk(e.table_name, e.columns, e.foreign_table, e.foreign_columns),
    array_to_string(e.columns, ',') || ' -> ' || e.foreign_table || '(' || array_to_string(e.foreign_columns, ',') || ')'
FROM expected_fk e;

INSERT INTO schema_check (name, ok, detail)
SELECT
    'no_unique_audience_on_product',
    NOT EXISTS (
        SELECT 1
        FROM pg_index i
        JOIN pg_class c ON c.oid = i.indrelid
        JOIN pg_namespace n ON n.oid = c.relnamespace
        WHERE n.nspname = 'public'
          AND c.relname = 'product_tag'
          AND i.indisunique
          AND NOT i.indisprimary
          AND pg_temp.index_columns(c.oid, i.indkey) = ARRAY['product_id']
    ),
    'product_tag must allow more than one tag (including audience) per product';

INSERT INTO schema_check (name, ok, detail)
SELECT
    'check.market.kind',
    EXISTS (
        SELECT 1
        FROM pg_constraint c
        JOIN pg_class t ON t.oid = c.conrelid
        JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'public'
          AND t.relname = 'market'
          AND c.contype = 'c'
          AND pg_get_constraintdef(c.oid) ILIKE '%country%'
          AND pg_get_constraintdef(c.oid) ILIKE '%region%'
    ),
    'kind in (country, region)';

INSERT INTO schema_check (name, ok, detail)
SELECT
    'check.product_item.kind',
    EXISTS (
        SELECT 1
        FROM pg_constraint c
        JOIN pg_class t ON t.oid = c.conrelid
        JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'public'
          AND t.relname = 'product_item'
          AND c.contype = 'c'
          AND pg_get_constraintdef(c.oid) ILIKE '%episode%'
          AND pg_get_constraintdef(c.oid) ILIKE '%training%'
    ),
    'kind in (episode, training)';

INSERT INTO schema_check (name, ok, detail)
SELECT
    'check.tag.category_lowercase',
    EXISTS (
        SELECT 1
        FROM pg_constraint c
        JOIN pg_class t ON t.oid = c.conrelid
        JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'public'
          AND t.relname = 'tag'
          AND c.contype = 'c'
          AND pg_get_constraintdef(c.oid) ILIKE '%lower(%category%)%'
    ),
    'category = lower(category)';

INSERT INTO schema_check (name, ok, detail)
SELECT
    'check.asset.role',
    EXISTS (
        SELECT 1
        FROM pg_constraint c
        JOIN pg_class t ON t.oid = c.conrelid
        JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'public'
          AND t.relname = 'asset'
          AND c.contype = 'c'
          AND pg_get_constraintdef(c.oid) ILIKE '%main_video%'
          AND pg_get_constraintdef(c.oid) ILIKE '%hero_image%'
    ),
    'role closed set from dossier';

INSERT INTO schema_check (name, ok, detail)
SELECT
    'check.asset.kind',
    EXISTS (
        SELECT 1
        FROM pg_constraint c
        JOIN pg_class t ON t.oid = c.conrelid
        JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'public'
          AND t.relname = 'asset'
          AND c.contype = 'c'
          AND pg_get_constraintdef(c.oid) ILIKE '%video%'
          AND pg_get_constraintdef(c.oid) ILIKE '%document%'
          AND pg_get_constraintdef(c.oid) ILIKE '%link%'
    ),
    'kind in (video, document, image, audio, link)';

INSERT INTO schema_check (name, ok, detail)
SELECT
    'check.asset.provider',
    EXISTS (
        SELECT 1
        FROM pg_constraint c
        JOIN pg_class t ON t.oid = c.conrelid
        JOIN pg_namespace n ON n.oid = t.relnamespace
        WHERE n.nspname = 'public'
          AND t.relname = 'asset'
          AND c.contype = 'c'
          AND pg_get_constraintdef(c.oid) ILIKE '%brightcove%'
          AND pg_get_constraintdef(c.oid) ILIKE '%vimeo%'
          AND pg_get_constraintdef(c.oid) ILIKE '%url%'
    ),
    'provider in (brightcove, vimeo, url)';

-- Behaviour: valid graph, then forbidden rows. Smoke rows are deleted afterwards.
DO $$
DECLARE
    family_id uuid := '11111111-1111-1111-1111-111111111111';
    product_a uuid := '22222222-2222-2222-2222-222222222222';
    product_b uuid := '33333333-3333-3333-3333-333333333333';
    item_a uuid := '44444444-4444-4444-4444-444444444444';
    item_b uuid := '55555555-5555-5555-5555-555555555555';
    tag_adults uuid := '66666666-6666-6666-6666-666666666666';
    tag_youth uuid := '77777777-7777-7777-7777-777777777777';
    asset_id uuid := '88888888-8888-8888-8888-888888888888';
    failed text;
BEGIN
    BEGIN
        INSERT INTO language (code) VALUES ('en'), ('fr');
        INSERT INTO market (code, kind, name) VALUES
            ('za', 'country', 'South Africa'),
            ('ssa', 'region', 'Sub-Saharan Africa');
        INSERT INTO product_family (id, code, name, sequence)
            VALUES (family_id, 'alpha-film-series', 'Alpha Film Series', 1);
        INSERT INTO product (id, family_id, code, title, content_language) VALUES
            (product_a, family_id, 'alpha-film-series', 'Alpha Film Series', 'en'),
            (product_b, family_id, 'alpha-film-series-africa', 'Alpha Film Series Africa', 'en');
        INSERT INTO tag (id, category, code, name, sequence) VALUES
            (tag_adults, 'audience', 'adults', 'Adults', 1),
            (tag_youth, 'audience', 'youth', 'Youth', 2);
        INSERT INTO product_tag (product_id, tag_id) VALUES
            (product_a, tag_adults),
            (product_a, tag_youth);
        INSERT INTO product_market (product_id, market_code, launched_on) VALUES
            (product_a, 'ssa', NULL);
        INSERT INTO product_item (id, product_id, kind, code, sequence, title) VALUES
            (item_a, product_a, 'episode', 'ep-01', 1, 'Episode 1'),
            (item_b, product_b, 'episode', 'ep-01', 1, 'Episode 1');
        INSERT INTO asset (
            id, product_id, item_id, role, kind, language_code, provider, provider_asset_id, download_url
        ) VALUES (
            asset_id, product_a, item_a, 'main_video', 'video', 'en', 'brightcove', 'bc-1', 'https://example.invalid/en.mp4'
        );
        INSERT INTO asset (
            product_id, item_id, role, kind, provider
        ) VALUES (
            product_a, NULL, 'hero_image', 'image', 'url'
        );
        INSERT INTO asset_market (asset_id, market_code) VALUES (asset_id, 'za');
        INSERT INTO schema_check VALUES (
            'behaviour.valid_graph_and_two_audiences',
            true,
            'insert succeeded'
        );
    EXCEPTION
        WHEN OTHERS THEN
            INSERT INTO schema_check VALUES (
                'behaviour.valid_graph_and_two_audiences',
                false,
                SQLERRM
            );
    END;

    failed := NULL;
    BEGIN
        INSERT INTO market (code, kind, name) VALUES ('xx', 'planet', 'X');
        failed := 'invalid market.kind was accepted';
    EXCEPTION
        WHEN check_violation THEN
            failed := NULL;
        WHEN OTHERS THEN
            failed := SQLERRM;
    END;
    INSERT INTO schema_check VALUES (
        'behaviour.reject_market_kind',
        failed IS NULL,
        coalesce(failed, 'check_violation')
    );

    failed := NULL;
    BEGIN
        INSERT INTO product_item (product_id, kind, code, sequence, title)
            VALUES (product_a, 'session', 'ep-02', 2, 'Nope');
        failed := 'invalid product_item.kind was accepted';
    EXCEPTION
        WHEN check_violation THEN
            failed := NULL;
        WHEN OTHERS THEN
            failed := SQLERRM;
    END;
    INSERT INTO schema_check VALUES (
        'behaviour.reject_item_kind',
        failed IS NULL,
        coalesce(failed, 'check_violation')
    );

    failed := NULL;
    BEGIN
        INSERT INTO tag (category, code, name, sequence)
            VALUES ('Audience', 'seniors', 'Seniors', 3);
        failed := 'mixed-case tag.category was accepted';
    EXCEPTION
        WHEN check_violation THEN
            failed := NULL;
        WHEN OTHERS THEN
            failed := SQLERRM;
    END;
    INSERT INTO schema_check VALUES (
        'behaviour.reject_tag_category_case',
        failed IS NULL,
        coalesce(failed, 'check_violation')
    );

    failed := NULL;
    BEGIN
        INSERT INTO asset (product_id, item_id, role, kind, provider)
            VALUES (product_a, item_b, 'main_video', 'video', 'url');
        failed := 'asset.item_id from another product was accepted';
    EXCEPTION
        WHEN foreign_key_violation THEN
            failed := NULL;
        WHEN OTHERS THEN
            failed := SQLERRM;
    END;
    INSERT INTO schema_check VALUES (
        'behaviour.reject_asset_item_from_other_product',
        failed IS NULL,
        coalesce(failed, 'foreign_key_violation')
    );

    DELETE FROM product WHERE id IN (product_a, product_b);
    DELETE FROM product_family WHERE id = family_id;
    DELETE FROM tag WHERE id IN (tag_adults, tag_youth);
    DELETE FROM market WHERE code IN ('za', 'ssa');
    DELETE FROM language WHERE code IN ('en', 'fr');
END $$;

SELECT name, ok, detail
FROM schema_check
ORDER BY ok, name;

DO $$
DECLARE
    failures integer;
BEGIN
    SELECT count(*) INTO failures FROM schema_check WHERE NOT ok;
    IF failures > 0 THEN
        RAISE EXCEPTION 'product_schema_smoketest failed (% checks)', failures;
    END IF;
    RAISE NOTICE 'product_schema_smoketest: PASS';
END $$;
