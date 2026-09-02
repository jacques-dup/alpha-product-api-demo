export type PortalAccess = 'allowed' | 'denied'

export class PortalAccessError extends Error {
  readonly status: number

  constructor(status: number) {
    super('Portal access check failed')
    this.status = status
  }
}

const csrfHeaders = { 'X-CSRF': '1' }

let cached: PortalAccess | null = null
let inflight: Promise<PortalAccess> | null = null

export function interpretPortalAccessStatus(status: number): PortalAccess {
  if (status === 403) {
    return 'denied'
  }
  if (status >= 200 && status < 300) {
    return 'allowed'
  }
  throw new PortalAccessError(status)
}

export function getCachedPortalAccess(): PortalAccess | null {
  return cached
}

export function markPortalAccessDenied(): void {
  cached = 'denied'
}

export function resetPortalAccess(): void {
  cached = null
  inflight = null
}

export async function ensurePortalAccess(
  fetcher: typeof fetch = fetch,
): Promise<PortalAccess> {
  if (cached) {
    return cached
  }
  if (!inflight) {
    inflight = probePortalAccess(fetcher)
      .then((access) => {
        cached = access
        return access
      })
      .finally(() => {
        inflight = null
      })
  }
  return inflight
}

async function probePortalAccess(fetcher: typeof fetch): Promise<PortalAccess> {
  const response = await fetcher('/api/languages', {
    credentials: 'include',
    headers: {
      Accept: 'application/json',
      ...csrfHeaders,
    },
  })
  return interpretPortalAccessStatus(response.status)
}

/** 401 ends the SPA session. 403 is authenticated but not allow-listed. */
export function dataErrorKind(
  status?: number,
): 'ok' | 'unauthenticated' | 'forbidden' {
  if (status === 401) {
    return 'unauthenticated'
  }
  if (status === 403) {
    return 'forbidden'
  }
  return 'ok'
}
