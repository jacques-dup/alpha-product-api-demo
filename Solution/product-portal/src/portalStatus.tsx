import { Loading, useLogout } from 'react-admin'

export function PortalLoading({
  loadingPrimary = 'Checking access',
  loadingSecondary = 'Please wait',
}: {
  loadingPrimary?: string
  loadingSecondary?: string
}) {
  return (
    <div className="portal-status">
      <Loading
        timeout={0}
        loadingPrimary={loadingPrimary}
        loadingSecondary={loadingSecondary}
      />
    </div>
  )
}

export function PortalForbidden() {
  const logout = useLogout()

  return (
    <div className="portal-status">
      <h1>403 Unauthorized</h1>
      <p>This account is not allowed to use the product portal.</p>
      <button
        type="button"
        className="portal-status__button"
        onClick={() => {
          void logout()
        }}
      >
        Log out
      </button>
    </div>
  )
}
