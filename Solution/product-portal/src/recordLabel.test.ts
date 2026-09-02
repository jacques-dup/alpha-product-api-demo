import { describe, expect, it } from 'vitest'
import { recordLabel } from './recordLabel.ts'

describe('recordLabel', () => {
  it('prefers names and titles over ids', () => {
    expect(recordLabel('families', { id: '1', name: 'Life Essentials', code: 'le' })).toBe(
      'Life Essentials',
    )
    expect(recordLabel('products', { id: '1', title: 'Course A', code: 'a' })).toBe(
      'Course A',
    )
    expect(recordLabel('tags', { id: '1', name: 'Youth', code: 'youth' })).toBe(
      'Youth',
    )
    expect(recordLabel('markets', { id: 'ZA', code: 'ZA', name: 'South Africa' })).toBe(
      'South Africa',
    )
  })

  it('falls back to code or id', () => {
    expect(recordLabel('languages', { id: 'en', code: 'en' })).toBe('en')
    expect(recordLabel('assets', { id: 'a1', role: 'hero' })).toBe('hero')
    expect(recordLabel('families', { id: '1' })).toBe('1')
  })
})
