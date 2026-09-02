import { useEffect } from 'react'
import { beginBffLogin } from './bffAuth.ts'
import { PortalLoading } from './portalStatus.tsx'

export function BffLogin() {
  useEffect(() => {
    beginBffLogin()
  }, [])

  return (
    <PortalLoading
      loadingPrimary="Signing in"
      loadingSecondary="Redirecting to the identity provider"
    />
  )
}
