#!/usr/bin/env bash
#
# Build Product.ApplicationRoot + the product portal SPA into a single publish
# payload and deploy it to Azure App Service.
#
# The portal is a same-origin client of Product.Bff: it holds a cookie session
# and posts X-CSRF, and the OIDC callbacks (/signin-oidc, /signout-callback-oidc)
# land on the portal's own origin. Hosting the SPA inside the API's wwwroot is
# what makes that origin one host, so this script always ships them together.
#
# Usage:
#   ./deploy-to-azure.sh                 build, confirm, deploy
#   ./deploy-to-azure.sh --yes           skip the confirmation prompt
#   ./deploy-to-azure.sh --no-deploy     build the payload only, leave it on disk
#   ./deploy-to-azure.sh --tests         run dotnet test before building
#
# Config (override by exporting before you run):
#   RG, APP, SUBSCRIPTION, HOST

set -euo pipefail

RG="${RG:-alpha-jacq-playground-rg}"
APP="${APP:-jac-profile-api-demo}"
SUBSCRIPTION="${SUBSCRIPTION:-Development}"
HOST="${HOST:-https://jac-profile-api-demo-bufwb9ekhzcfawh0.uksouth-01.azurewebsites.net}"

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
API_PROJECT="$REPO_ROOT/Solution/product/src/Product.ApplicationRoot/Product.ApplicationRoot.csproj"
SOLUTION="$REPO_ROOT/Solution/product/Product.sln"
PORTAL_DIR="$REPO_ROOT/Solution/product-portal"

BUILD_DIR="${BUILD_DIR:-$(mktemp -d -t product-publish)}"
ZIP_PATH="$BUILD_DIR.zip"

# The original ApplicationRoot demo page. The SPA takes over "/", so these are
# removed from the publish output rather than left to fight over index.html.
DEMO_PAGE_FILES=(index.html app.js styles.css)

RUN_TESTS=false
SKIP_CONFIRM=false
DEPLOY=true

for arg in "$@"; do
  case "$arg" in
    --tests)     RUN_TESTS=true ;;
    --yes|-y)    SKIP_CONFIRM=true ;;
    --no-deploy) DEPLOY=false ;;
    -h|--help)   sed -n '2,20p' "${BASH_SOURCE[0]}"; exit 0 ;;
    *)           echo "unknown argument: $arg" >&2; exit 2 ;;
  esac
done

step() { printf '\n\033[1m==> %s\033[0m\n' "$1"; }
fail() { printf '\033[31merror: %s\033[0m\n' "$1" >&2; exit 1; }

# ---------------------------------------------------------------- preflight

step "Preflight"

for tool in dotnet pnpm zip; do
  command -v "$tool" >/dev/null 2>&1 || fail "$tool is not on PATH"
done

if [ "$DEPLOY" = true ]; then
  command -v az >/dev/null 2>&1 || fail "az is not on PATH"
  az account show >/dev/null 2>&1 || fail "not logged in — run: az login"
  az account set --subscription "$SUBSCRIPTION"
  az webapp show -g "$RG" -n "$APP" -o none 2>/dev/null \
    || fail "web app $APP not found in resource group $RG"
fi

echo "resource group : $RG"
echo "web app        : $APP"
echo "payload        : $BUILD_DIR"

# ---------------------------------------------------------------- build

if [ "$RUN_TESTS" = true ]; then
  step "Tests"
  dotnet test "$SOLUTION" --nologo
fi

step "Publish Product.ApplicationRoot"
rm -rf "$BUILD_DIR"
dotnet publish "$API_PROJECT" -c Release -o "$BUILD_DIR" --nologo

[ -d "$BUILD_DIR/wwwroot" ] || fail "publish produced no wwwroot at $BUILD_DIR"

step "Build the portal SPA"
# vite.config.ts only reads VITE_APPLICATION_ROOT for the dev proxy, so the
# production bundle needs no origin baked in — it calls /api on its own host.
pnpm --dir "$PORTAL_DIR" install --frozen-lockfile
pnpm --dir "$PORTAL_DIR" build

[ -f "$PORTAL_DIR/dist/index.html" ] || fail "portal build produced no dist/index.html"

step "Merge the SPA into wwwroot"
for file in "${DEMO_PAGE_FILES[@]}"; do
  if [ -f "$BUILD_DIR/wwwroot/$file" ]; then
    echo "removing demo page file: wwwroot/$file"
    rm -f "$BUILD_DIR/wwwroot/$file"
  fi
done
cp -R "$PORTAL_DIR/dist/." "$BUILD_DIR/wwwroot/"
echo "wwwroot now contains:"
ls -1 "$BUILD_DIR/wwwroot"

step "Package"
rm -f "$ZIP_PATH"
# App Service expects the publish output at the ZIP root, not nested.
(cd "$BUILD_DIR" && zip -qr "$ZIP_PATH" .)
echo "$ZIP_PATH ($(du -h "$ZIP_PATH" | cut -f1))"

if [ "$DEPLOY" = false ]; then
  step "Done (--no-deploy)"
  echo "payload : $BUILD_DIR"
  echo "zip     : $ZIP_PATH"
  exit 0
fi

# ---------------------------------------------------------------- deploy

if [ "$SKIP_CONFIRM" = false ]; then
  printf '\nDeploy this payload to %s (%s)? [y/N] ' "$APP" "$HOST"
  read -r reply
  case "$reply" in
    [yY]|[yY][eE][sS]) ;;
    *) echo "aborted"; exit 0 ;;
  esac
fi

step "Deploy to $APP"
# Uses your Entra token, so the app's disabled SCM basic auth does not matter.
az webapp deploy -g "$RG" -n "$APP" --src-path "$ZIP_PATH" --type zip

step "Verify"
# The app needs a moment to recycle before it answers.
for _ in $(seq 1 30); do
  code=$(curl -s -o /dev/null -w '%{http_code}' --max-time 10 "$HOST/" || true)
  [ "$code" = "200" ] && break
  sleep 5
done

printf 'root                    %s  (expect 200)\n' \
  "$(curl -s -o /dev/null -w '%{http_code}' --max-time 20 "$HOST/")"
printf 'product/languages       %s  (expect 401)\n' \
  "$(curl -s -o /dev/null -w '%{http_code}' --max-time 20 "$HOST/product/languages")"
printf 'bff/user                %s  (expect 401)\n' \
  "$(curl -s -o /dev/null -w '%{http_code}' --max-time 20 "$HOST/bff/user")"

step "Done"
echo "Portal: $HOST/"
echo
echo "Portal routes are hash-based (#/products), so no SPA fallback route is needed."
