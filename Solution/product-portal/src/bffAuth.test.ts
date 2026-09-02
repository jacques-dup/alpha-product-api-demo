import { describe, expect, it } from 'vitest'
import {
  bffLoginPath,
  bffLogoutPath,
  bffLogoutRedirect,
  logoutUrlFromUser,
  portalReturnPath,
} from './bffAuth.ts'

describe('bffLoginPath', () => {
  it('uses the BFF management endpoint, not a React Admin route', () => {
    expect(bffLoginPath()).toBe('/bff/login?returnUrl=%2F')
    expect(bffLoginPath().startsWith('/bff/')).toBe(true)
  })

  it('keeps returnUrl on the same host (relative)', () => {
    expect(portalReturnPath).toBe('/')
    expect(decodeURIComponent(bffLoginPath().split('returnUrl=')[1] ?? '')).toBe(
      '/',
    )
  })
})

describe('bffLogoutPath', () => {
  it('uses the BFF logout endpoint', () => {
    expect(bffLogoutPath()).toBe('/bff/logout')
  })

  it('keeps the sid query from bff:logout_url', () => {
    expect(bffLogoutPath('/bff/logout?sid=abc')).toBe('/bff/logout?sid=abc')
  })

  it('ignores open redirects', () => {
    expect(bffLogoutPath('https://evil.example/bff/logout')).toBe('/bff/logout')
    expect(bffLogoutPath('/login')).toBe('/bff/logout')
  })
})

describe('logoutUrlFromUser', () => {
  it('reads bff:logout_url from the user claim list', () => {
    expect(
      logoutUrlFromUser([
        { type: 'sub', value: '1' },
        { type: 'bff:logout_url', value: '/bff/logout?sid=abc' },
      ]),
    ).toBe('/bff/logout?sid=abc')
  })
})

describe('bffLogoutRedirect', () => {
  it('returns an absolute same-origin URL for React Admin', () => {
    expect(
      bffLogoutRedirect('https://localhost:5173', '/bff/logout?sid=abc'),
    ).toBe('https://localhost:5173/bff/logout?sid=abc')
  })
})
