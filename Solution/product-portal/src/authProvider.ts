import { HttpError } from 'react-admin'
import type { AuthProvider } from 'react-admin'
import { beginBffLogin, bffLogoutRedirect, logoutUrlFromUser } from './bffAuth.ts'
import {
  dataErrorKind,
  ensurePortalAccess,
  markPortalAccessDenied,
  PortalAccessError,
  resetPortalAccess,
} from './portalAccess.ts'

const csrfHeaders = { 'X-CSRF': '1' }

const forbiddenRedirect = {
  status: 403,
  logoutUser: false,
  redirectTo: '/access-denied',
  message: false,
}

async function loadUser(): Promise<unknown> {
  const response = await fetch('/bff/user', {
    credentials: 'include',
    headers: {
      Accept: 'application/json',
      ...csrfHeaders,
    },
  })
  if (!response.ok) {
    throw new HttpError('Not authenticated', response.status)
  }
  return response.json()
}

function claimMap(payload: unknown): Record<string, string> {
  if (!Array.isArray(payload)) {
    return {}
  }

  const claims: Record<string, string> = {}
  for (const item of payload) {
    if (
      item &&
      typeof item === 'object' &&
      'type' in item &&
      'value' in item
    ) {
      claims[String(item.type)] = String(item.value)
    }
  }
  return claims
}

function silentAuthError(status: number): never {
  throw { status, message: false }
}

export const authProvider: AuthProvider = {
  login: async () => {
    beginBffLogin()
  },

  logout: async () => {
    resetPortalAccess()
    try {
      const payload = await loadUser()
      // Absolute URL so React Admin does a full navigation to Product.Bff
      // `/bff/logout?sid=...` (Duende requires sid). Relative paths stay inside the SPA.
      return bffLogoutRedirect(window.location.origin, logoutUrlFromUser(payload))
    } catch {
      return false
    }
  },

  checkAuth: async () => {
    try {
      await loadUser()
      await ensurePortalAccess()
    } catch (error) {
      if (error instanceof HttpError && error.status === 401) {
        resetPortalAccess()
        silentAuthError(401)
      }
      if (error instanceof PortalAccessError && error.status === 401) {
        resetPortalAccess()
        silentAuthError(401)
      }
      throw error
    }
  },

  checkError: async (error: { status?: number }) => {
    const kind = dataErrorKind(error.status)
    if (kind === 'unauthenticated') {
      resetPortalAccess()
      silentAuthError(401)
    }
    if (kind === 'forbidden') {
      markPortalAccessDenied()
      throw forbiddenRedirect
    }
  },

  canAccess: async () => (await ensurePortalAccess()) === 'allowed',

  getIdentity: async () => {
    const claims = claimMap(await loadUser())
    const id = claims.sub ?? claims.email ?? 'staff'
    const fullName = claims.name ?? claims.email ?? 'Staff'
    return { id, fullName }
  },
}
