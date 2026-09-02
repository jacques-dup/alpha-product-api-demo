import { describe, expect, it } from 'vitest'
import { itemPath, recordId, withId } from './identifiers.ts'

describe('recordId', () => {
  it('maps language and market code to id', () => {
    expect(recordId('languages', { code: 'en', name: 'English' })).toBe('en')
    expect(recordId('markets', { code: 'ZA', name: 'South Africa' })).toBe('ZA')
  })

  it('keeps guid ids for ordinary rows', () => {
    const id = 'a0000000-0000-4000-a000-000000000001'
    expect(recordId('products', { id, title: 'Example' })).toBe(id)
  })

  it('joins composite keys', () => {
    expect(
      recordId('product-tags', {
        productId: 'p1',
        tagId: 't1',
      }),
    ).toBe('p1_t1')
    expect(
      recordId('product-markets', {
        productId: 'p1',
        marketCode: 'ZA',
      }),
    ).toBe('p1_ZA')
    expect(
      recordId('asset-markets', {
        assetId: 'a1',
        marketCode: 'ZA',
      }),
    ).toBe('a1_ZA')
  })
})

describe('withId', () => {
  it('adds id without dropping source fields', () => {
    expect(withId('languages', { code: 'en', name: 'English' })).toEqual({
      code: 'en',
      name: 'English',
      id: 'en',
    })
  })
})

describe('itemPath', () => {
  it('builds BFF item URLs', () => {
    expect(itemPath('languages', 'en')).toBe('/api/languages/en')
    expect(itemPath('product-tags', 'p1_t1')).toBe('/api/product-tags/p1/t1')
    expect(itemPath('product-markets', 'p1_ZA')).toBe(
      '/api/product-markets/p1/ZA',
    )
    expect(itemPath('asset-markets', 'a1_ZA')).toBe('/api/asset-markets/a1/ZA')
  })
})
