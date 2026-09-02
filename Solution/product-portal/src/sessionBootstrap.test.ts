import { describe, expect, it } from 'vitest'
import {
  interpretSessionStatus,
  probeSessionStatus,
  shouldRedirectToLogin,
} from './sessionBootstrap.ts'

describe('interpretSessionStatus', () => {
  it('treats 401 as anonymous', () => {
    expect(interpretSessionStatus(401)).toBe('anonymous')
  })

  it('treats 2xx as authenticated', () => {
    expect(interpretSessionStatus(200)).toBe('authenticated')
  })

  it('treats anything else as unknown so the app still boots', () => {
    expect(interpretSessionStatus(500)).toBe('unknown')
    expect(interpretSessionStatus(302)).toBe('unknown')
  })
})

describe('shouldRedirectToLogin', () => {
  it('redirects an anonymous visitor', () => {
    expect(shouldRedirectToLogin('anonymous', false)).toBe(true)
  })

  it('leaves an authenticated visitor alone', () => {
    expect(shouldRedirectToLogin('authenticated', false)).toBe(false)
  })

  it('does not redirect on an unknown status', () => {
    expect(shouldRedirectToLogin('unknown', false)).toBe(false)
  })

  it('breaks a redirect loop after a recent attempt', () => {
    expect(shouldRedirectToLogin('anonymous', true)).toBe(false)
  })
})

describe('probeSessionStatus', () => {
  it('asks Product.Bff with the CSRF header and the session cookie', async () => {
    let seen: RequestInit | undefined
    const status = await probeSessionStatus((async (
      _input: string,
      init?: RequestInit,
    ) => {
      seen = init
      return { status: 200 } as Response
    }) as unknown as typeof fetch)

    const headers = seen?.headers as Record<string, string> | undefined
    expect(status).toBe('authenticated')
    expect(seen?.credentials).toBe('include')
    expect(headers?.['X-CSRF']).toBe('1')
  })

  it('reports unknown when the request fails outright', async () => {
    const status = await probeSessionStatus((() => {
      throw new Error('offline')
    }) as unknown as typeof fetch)

    expect(status).toBe('unknown')
  })
})
