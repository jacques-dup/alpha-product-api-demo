import { useMemo, useRef, useState } from 'react'
import { useGetList, useGetOne, useInput } from 'react-admin'
import type { Identifier, InputProps } from 'react-admin'
import { recordLabel } from './recordLabel.ts'

type ReferencePickerInputProps = {
  source: string
  reference: string
  label?: string
  allowEmpty?: boolean
  validate?: InputProps['validate']
}

export function ReferencePickerInput({
  source,
  reference,
  label,
  allowEmpty = false,
  validate,
}: ReferencePickerInputProps) {
  const { field, fieldState } = useInput({ source, validate })
  const dialogRef = useRef<HTMLDialogElement>(null)
  const [query, setQuery] = useState('')
  const selectedId = field.value as Identifier | null | undefined
  const hasValue = selectedId !== undefined && selectedId !== null && selectedId !== ''

  const { data: selected } = useGetOne(
    reference,
    { id: selectedId ?? '' },
    { enabled: hasValue },
  )
  const { data: choices = [], isPending } = useGetList(reference, {
    pagination: { page: 1, perPage: 200 },
    sort: { field: 'id', order: 'ASC' },
  })

  const filtered = useMemo(() => {
    const needle = query.trim().toLowerCase()
    if (!needle) {
      return choices
    }
    return choices.filter((record) =>
      recordLabel(reference, record).toLowerCase().includes(needle),
    )
  }, [choices, query, reference])

  const caption = hasValue
    ? recordLabel(reference, selected) || String(selectedId)
    : 'None selected'

  function openPicker() {
    setQuery('')
    dialogRef.current?.showModal()
  }

  function closePicker() {
    dialogRef.current?.close()
  }

  function pick(id: Identifier) {
    field.onChange(id)
    closePicker()
  }

  function clear() {
    field.onChange(allowEmpty ? null : '')
  }

  return (
    <div style={{ marginBottom: '1rem' }}>
      <div style={{ fontSize: 12, marginBottom: 4 }}>{label ?? source}</div>
      <div style={{ display: 'flex', gap: 8, alignItems: 'center', flexWrap: 'wrap' }}>
        <strong>{caption}</strong>
        <button type="button" onClick={openPicker}>
          Choose
        </button>
        {allowEmpty && hasValue ? (
          <button type="button" onClick={clear}>
            Clear
          </button>
        ) : null}
      </div>
      {fieldState.error?.message ? (
        <div style={{ color: '#c00', fontSize: 12 }}>{fieldState.error.message}</div>
      ) : null}

      <dialog ref={dialogRef} className="reference-picker" onClose={closePicker}>
        <form method="dialog" style={{ margin: 0 }} onSubmit={(event) => event.preventDefault()}>
          <header
            style={{
              display: 'flex',
              justifyContent: 'space-between',
              gap: 16,
              padding: '12px 16px',
              borderBottom: '1px solid #ddd',
            }}
          >
            <strong>Select {label ?? reference}</strong>
            <button type="button" onClick={closePicker}>
              Close
            </button>
          </header>
          <div style={{ padding: '12px 16px' }}>
            <input
              type="search"
              placeholder="Filter"
              value={query}
              onChange={(event) => setQuery(event.target.value)}
              style={{ width: '100%', marginBottom: 12, padding: 8, boxSizing: 'border-box' }}
            />
            {isPending ? <p>Loading...</p> : null}
            <div style={{ maxHeight: 360, overflow: 'auto' }}>
              <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                <tbody>
                  {filtered.map((record) => (
                    <tr key={String(record.id)}>
                      <td style={{ padding: '6px 0', borderBottom: '1px solid #eee' }}>
                        <button
                          type="button"
                          onClick={() => pick(record.id)}
                          style={{
                            background: 'none',
                            border: 'none',
                            padding: 0,
                            cursor: 'pointer',
                            textAlign: 'left',
                            font: 'inherit',
                          }}
                        >
                          {recordLabel(reference, record) || String(record.id)}
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
              {!isPending && filtered.length === 0 ? <p>No matching records.</p> : null}
            </div>
          </div>
        </form>
      </dialog>
    </div>
  )
}
