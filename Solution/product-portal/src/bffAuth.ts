/** Product.Bff management paths. Same origin as the SPA at `/`. */

export const portalReturnPath = '/'

export const bffLogoutUrlClaim = 'bff:logout_url'

export function bffLoginPath(returnUrl = portalReturnPath): string {
  return `/bff/login?returnUrl=${encodeURIComponent(returnUrl)}`
}

/** Duende BFF logout. Prefer `bff:logout_url` from `/bff/user` (includes `sid`). */
export function bffLogoutPath(logoutUrl?: string | null): string {
  if (
    typeof logoutUrl === 'string' &&
    (logoutUrl === '/bff/logout' || logoutUrl.startsWith('/bff/logout?'))
  ) {
    return logoutUrl
  }
  return '/bff/logout'
}

export function logoutUrlFromUser(payload: unknown): string | undefined {
  if (!Array.isArray(payload)) {
    return undefined
  }

  for (const item of payload) {
    if (
      item &&
      typeof item === 'object' &&
      'type' in item &&
      'value' in item &&
      String(item.type) === bffLogoutUrlClaim
    ) {
      return String(item.value)
    }
  }

  return undefined
}

export function bffLogoutRedirect(
  origin: string,
  logoutUrl?: string | null,
): string {
  return new URL(bffLogoutPath(logoutUrl), origin).href
}

export function beginBffLogin(): void {
  window.location.replace(bffLoginPath())
}

export function beginBffLogout(logoutUrl?: string | null): void {
  window.location.replace(bffLogoutPath(logoutUrl))
}
