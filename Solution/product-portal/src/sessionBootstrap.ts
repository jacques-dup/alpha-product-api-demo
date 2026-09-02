/**
 * First-paint session probe.
 *
 * React Admin can only turn an anonymous visitor away once the Admin shell has
 * mounted and `checkAuth` has run, so `/` briefly boots the whole app for someone
 * who was never going to see it. Asking Product.Bff who we are before the first
 * render lets an anonymous request for the root go straight to `/bff/login`.
 *
 * The probe starts at module evaluation so it overlaps bundle execution instead
 * of adding a round trip in front of it.
 */

import { beginBffLogin } from './bffAuth.ts'

export type SessionStatus = 'authenticated' | 'anonymous' | 'unknown'

const csrfHeaders = { 'X-CSRF': '1' }

/**
 * Guards against a redirect loop. If `/bff/login` ever lands us back here still
 * anonymous — a misrouted callback, a dropped cookie — we boot the app once and
 * let React Admin show the login page rather than bouncing forever.
 */
const loginAttemptKey = 'portal:bff-login-attempt'
const loginAttemptWindowMs = 10_000

/** Only 401 is a definite "no session". Anything else boots the app. */
export function interpretSessionStatus(status: number): SessionStatus {
  if (status === 401) {
    return 'anonymous'
  }
  if (status >= 200 && status < 300) {
    return 'authenticated'
  }
  return 'unknown'
}

export function shouldRedirectToLogin(
  status: SessionStatus,
  hasRecentLoginAttempt: boolean,
): boolean {
  return status === 'anonymous' && !hasRecentLoginAttempt
}

export async function probeSessionStatus(
  fetcher: typeof fetch = fetch,
): Promise<SessionStatus> {
  try {
    const response = await fetcher('/bff/user', {
      credentials: 'include',
      headers: {
        Accept: 'application/json',
        ...csrfHeaders,
      },
    })
    return interpretSessionStatus(response.status)
  } catch {
    // Offline or the BFF is down. Render the app and let React Admin report it;
    // a transient failure must not send anyone to the identity provider.
    return 'unknown'
  }
}

function hasRecentLoginAttempt(): boolean {
  try {
    const marked = window.sessionStorage.getItem(loginAttemptKey)
    return marked !== null && Date.now() - Number(marked) < loginAttemptWindowMs
  } catch {
    // Private mode or blocked storage. Without the guard one extra hop is the
    // worst case, and React Admin still handles login after it.
    return false
  }
}

function markLoginAttempt(): void {
  try {
    window.sessionStorage.setItem(loginAttemptKey, String(Date.now()))
  } catch {
    /* storage unavailable */
  }
}

function clearLoginAttempt(): void {
  try {
    window.sessionStorage.removeItem(loginAttemptKey)
  } catch {
    /* storage unavailable */
  }
}

// Eager in the browser, inert under the node test environment.
const sessionProbe: Promise<SessionStatus> | null =
  typeof window === 'undefined' ? null : probeSessionStatus()

/** Resolves true when the browser is on its way to `/bff/login` and must not render. */
export async function redirectAnonymousToLogin(): Promise<boolean> {
  const status = await (sessionProbe ?? Promise.resolve<SessionStatus>('unknown'))

  if (status === 'authenticated') {
    clearLoginAttempt()
  }

  if (!shouldRedirectToLogin(status, hasRecentLoginAttempt())) {
    return false
  }

  markLoginAttempt()
  beginBffLogin()
  return true
}
