import { HttpError } from 'react-admin'
import type { DataProvider } from 'react-admin'
import { itemPath, withId } from './identifiers.ts'
import type { ProductRecord } from './identifiers.ts'

const csrfHeaders = { 'X-CSRF': '1' }

async function fetchJson(url: string, init: RequestInit = {}): Promise<unknown> {
  const response = await fetch(url, {
    credentials: 'include',
    ...init,
    headers: {
      Accept: 'application/json',
      ...csrfHeaders,
      ...(init.body ? { 'Content-Type': 'application/json' } : {}),
      ...init.headers,
    },
  })

  const text = await response.text()
  let body: unknown = text
  if (text) {
    try {
      body = JSON.parse(text)
    } catch {
      body = text
    }
  } else {
    body = null
  }

  if (!response.ok) {
    const message =
      body &&
      typeof body === 'object' &&
      'title' in body &&
      typeof body.title === 'string'
        ? body.title
        : response.statusText
    throw new HttpError(message, response.status, body)
  }

  return body
}

async function fetchAll(
  resource: string,
  filter: Record<string, unknown> = {},
): Promise<Array<ProductRecord & { id: string }>> {
  const query = new URLSearchParams()
  for (const [key, value] of Object.entries(filter)) {
    if (value === undefined || value === null || value === '') {
      continue
    }
    query.set(key, String(value))
  }
  const suffix = query.size > 0 ? `?${query.toString()}` : ''
  const json = await fetchJson(`/api/${resource}${suffix}`)
  const rows = Array.isArray(json) ? json : []
  return rows.map((row) => withId(resource, row as ProductRecord))
}

function compare(left: unknown, right: unknown, order: string): number {
  const a = left == null ? '' : String(left)
  const b = right == null ? '' : String(right)
  const result = a.localeCompare(b, undefined, { numeric: true })
  return order === 'DESC' ? -result : result
}

function pageRows(
  rows: Array<ProductRecord & { id: string }>,
  params: {
    sort?: { field: string; order: string }
    pagination?: { page: number; perPage: number }
  },
) {
  const sort = params.sort ?? { field: 'id', order: 'ASC' }
  const pagination = params.pagination ?? { page: 1, perPage: 25 }
  const sorted = [...rows].sort((left, right) =>
    compare(left[sort.field], right[sort.field], sort.order),
  )
  const start = (pagination.page - 1) * pagination.perPage
  return {
    data: sorted.slice(start, start + pagination.perPage),
    total: sorted.length,
  }
}

export const dataProvider = {
  getList: async (resource, params) => {
    const rows = await fetchAll(
      resource,
      (params.filter ?? {}) as Record<string, unknown>,
    )
    return pageRows(rows, params)
  },

  getOne: async (resource, params) => {
    const json = await fetchJson(itemPath(resource, params.id))
    return { data: withId(resource, json as ProductRecord) }
  },

  getMany: async (resource, params) => {
    const rows = await fetchAll(resource)
    const wanted = new Set(params.ids.map(String))
    return { data: rows.filter((row) => wanted.has(row.id)) }
  },

  getManyReference: async (resource, params) => {
    const rows = await fetchAll(resource, {
      ...((params.filter ?? {}) as Record<string, unknown>),
      [params.target]: params.id,
    })
    return pageRows(rows, params)
  },

  create: async (resource, params) => {
    const json = await fetchJson(`/api/${resource}`, {
      method: 'POST',
      body: JSON.stringify(params.data),
    })
    const record =
      json && typeof json === 'object'
        ? withId(resource, json as ProductRecord)
        : withId(resource, params.data as ProductRecord)
    return { data: record }
  },

  update: async (resource, params) => {
    const json = await fetchJson(itemPath(resource, params.id), {
      method: 'PUT',
      body: JSON.stringify(params.data),
    })
    const record =
      json && typeof json === 'object'
        ? withId(resource, json as ProductRecord)
        : withId(resource, params.data as ProductRecord)
    return { data: record }
  },

  updateMany: async (resource, params) => {
    for (const id of params.ids) {
      await fetchJson(itemPath(resource, id), {
        method: 'PUT',
        body: JSON.stringify(params.data),
      })
    }
    return { data: params.ids }
  },

  delete: async (resource, params) => {
    await fetchJson(itemPath(resource, params.id), { method: 'DELETE' })
    const previous =
      params.previousData && typeof params.previousData === 'object'
        ? (params.previousData as ProductRecord)
        : { id: String(params.id) }
    return { data: withId(resource, previous) }
  },

  deleteMany: async (resource, params) => {
    for (const id of params.ids) {
      await fetchJson(itemPath(resource, id), { method: 'DELETE' })
    }
    return { data: params.ids }
  },
} as DataProvider
