import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { redirectAnonymousToLogin } from './sessionBootstrap.ts'

function renderPortal() {
  createRoot(document.getElementById('root')!).render(
    <StrictMode>
      <App />
    </StrictMode>,
  )
}

// An anonymous request for `/` leaves for Product.Bff instead of booting the
// Admin shell only for `checkAuth` to bounce it to the login page.
redirectAnonymousToLogin().then((redirecting) => {
  if (!redirecting) {
    renderPortal()
  }
})
