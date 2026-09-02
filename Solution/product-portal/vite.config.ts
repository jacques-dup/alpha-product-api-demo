import react from '@vitejs/plugin-react'
import { existsSync, readFileSync } from 'node:fs'
import type { IncomingMessage } from 'node:http'
import type { ClientRequest } from 'node:http'
import type { Plugin, ServerOptions } from 'vite'
import { defineConfig } from 'vitest/config'

const applicationRoot =
  process.env.VITE_APPLICATION_ROOT ?? 'https://localhost:7127'

function localHttps(): ServerOptions['https'] {
  const keyPath = './certs/localhost.key'
  const certPath = './certs/localhost.pem'
  if (!existsSync(keyPath) || !existsSync(certPath)) {
    return undefined
  }

  return {
    key: readFileSync(keyPath),
    cert: readFileSync(certPath),
  }
}

function applicationRootProxy() {
  return {
    target: applicationRoot,
    changeOrigin: true,
    secure: false,
    xfwd: true,
    configure(proxy: {
      on: (
        event: 'proxyReq',
        listener: (proxyReq: ClientRequest, req: IncomingMessage) => void,
      ) => void
    }) {
      proxy.on('proxyReq', (proxyReq, req) => {
        const host = req.headers.host
        if (typeof host === 'string') {
          proxyReq.setHeader('X-Forwarded-Host', host)
        }
        proxyReq.setHeader('X-Forwarded-Proto', 'https')
      })
    },
  }
}

/** Leftover /admin bookmarks from the first scaffold. SPA is at `/`. */
function redirectLegacyAdminPaths(): Plugin {
  return {
    name: 'redirect-legacy-admin-paths',
    configureServer(server) {
      server.middlewares.use((req, res, next) => {
        const url = req.url ?? ''
        const path = url.split('?')[0] ?? ''
        if (path === '/admin' || path === '/admin/') {
          res.statusCode = 302
          res.setHeader('Location', '/')
          res.end()
          return
        }
        if (
          path.startsWith('/admin/bff') ||
          path.startsWith('/admin/signin-oidc') ||
          path.startsWith('/admin/signout-callback-oidc')
        ) {
          res.statusCode = 302
          res.setHeader('Location', url.slice('/admin'.length))
          res.end()
          return
        }
        next()
      })
    },
  }
}

export default defineConfig({
  plugins: [redirectLegacyAdminPaths(), react()],
  server: {
    // TLS locally so the OIDC redirect_uri the BFF builds is
    // https://localhost:5173/signin-oidc, matching what the IDP has registered.
    // Cert: dotnet dev-certs https --export-path ./certs/localhost.pem --format Pem --no-password
    https: localHttps(),
    proxy: {
      '/api': applicationRootProxy(),
      '/bff': applicationRootProxy(),
      '/signin-oidc': applicationRootProxy(),
      '/signout-callback-oidc': applicationRootProxy(),
    },
  },
  test: {
    environment: 'node',
  },
})
