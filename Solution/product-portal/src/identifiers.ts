export type ProductRecord = Record<string, unknown>

export function recordId(resource: string, record: ProductRecord): string {
  switch (resource) {
    case 'languages':
    case 'markets':
      return String(record.code)
    case 'product-tags':
      return `${record.productId}_${record.tagId}`
    case 'product-markets':
      return `${record.productId}_${record.marketCode}`
    case 'asset-markets':
      return `${record.assetId}_${record.marketCode}`
    default:
      return String(record.id)
  }
}

export function withId(
  resource: string,
  record: ProductRecord,
): ProductRecord & { id: string } {
  return { ...record, id: recordId(resource, record) }
}

export function splitCompositeId(id: string): [string, string] {
  const separator = id.indexOf('_')
  if (separator < 0) {
    throw new Error(`Expected composite id, got '${id}'`)
  }
  return [id.slice(0, separator), id.slice(separator + 1)]
}

export function itemPath(resource: string, id: string | number): string {
  const key = String(id)
  switch (resource) {
    case 'product-tags': {
      const [productId, tagId] = splitCompositeId(key)
      return `/api/product-tags/${productId}/${tagId}`
    }
    case 'product-markets': {
      const [productId, marketCode] = splitCompositeId(key)
      return `/api/product-markets/${productId}/${marketCode}`
    }
    case 'asset-markets': {
      const [assetId, marketCode] = splitCompositeId(key)
      return `/api/asset-markets/${assetId}/${marketCode}`
    }
    default:
      return `/api/${resource}/${key}`
  }
}
