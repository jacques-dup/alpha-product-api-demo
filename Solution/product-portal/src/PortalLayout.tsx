import { Layout } from 'react-admin'
import type { LayoutProps } from 'react-admin'
import { getCachedPortalAccess } from './portalAccess.ts'
import { PortalForbidden, PortalLoading } from './portalStatus.tsx'

export function PortalLayout(props: LayoutProps) {
  const access = getCachedPortalAccess()
  if (access === 'denied') {
    return <PortalForbidden />
  }
  if (access !== 'allowed') {
    return <PortalLoading />
  }
  return <Layout {...props} />
}
