import type { ProductRecord } from './identifiers.ts'

export function recordLabel(
  resource: string,
  record: ProductRecord | undefined | null,
): string {
  if (!record) {
    return ''
  }

  switch (resource) {
    case 'languages':
      return String(record.code ?? record.id ?? '')
    case 'markets':
      return String(record.name ?? record.code ?? record.id ?? '')
    case 'families':
      return String(record.name ?? record.code ?? record.id ?? '')
    case 'tags':
      return String(record.name ?? record.code ?? record.id ?? '')
    case 'products':
      return String(record.title ?? record.code ?? record.id ?? '')
    case 'items':
      return String(record.title ?? record.code ?? record.id ?? '')
    case 'assets':
      return String(record.title ?? record.role ?? record.id ?? '')
    default:
      return String(record.name ?? record.title ?? record.code ?? record.id ?? '')
  }
}
