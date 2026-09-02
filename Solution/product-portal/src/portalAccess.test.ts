import { afterEach, describe, expect, it } from 'vitest'
import {
  dataErrorKind,
  ensurePortalAccess,
  interpretPortalAccessStatus,
  PortalAccessError,
  resetPortalAccess,
} from './portalAccess.ts'

afterEach(() => {
  resetPortalAccess()
})

describe('interpretPortalAccessStatus', () => {
  it('treats 403 as deny (allow-list miss)', () => {
    expect(interpretPortalAccessStatus(403)).toBe('denied')
  })

  it('treats 2xx as allow', () => {
    expect(interpretPortalAccessStatus(200)).toBe('allowed')
  })

  it('treats 401 as a session error', () => {
    expect(() => interpretPortalAccessStatus(401)).toThrow(PortalAccessError)
  })
})

describe('dataErrorKind', () => {
  it('keeps 403 in-app (no auto logout)', () => {
    expect(dataErrorKind(403)).toBe('forbidden')
  })

  it('sends 401 to login', () => {
    expect(dataErrorKind(401)).toBe('unauthenticated')
  })
})

describe('ensurePortalAccess', () => {
  it('treats a 403 probe as denied and caches it', async () => {
    let calls = 0
    const fetcher = (async () => {
      calls += 1
      return { status: 403 }
    }) as unknown as typeof fetch
    await expect(ensurePortalAccess(fetcher)).resolves.toBe('denied')
    await expect(ensurePortalAccess(fetcher)).resolves.toBe('denied')
    expect(calls).toBe(1)
  })
})
