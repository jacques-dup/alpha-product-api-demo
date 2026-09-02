/* oxlint-disable react/only-export-components */
import {
  ArrayField,
  BooleanField,
  BooleanInput,
  ChipField,
  Create,
  Datagrid,
  DateInput,
  Edit,
  List,
  NumberField,
  NumberInput,
  ReferenceField,
  SimpleForm,
  SingleFieldList,
  TextField,
  TextInput,
  required,
} from 'react-admin'
import { ReferencePickerInput } from './ReferencePickerInput.tsx'

function productRow(data: Record<string, unknown>) {
  const row: Record<string, unknown> = {
    familyId: data.familyId,
    code: data.code,
    title: data.title,
    summary: data.summary,
    description: data.description,
    contentLanguage: data.contentLanguage,
  }
  if (data.id) {
    row.id = data.id
  }
  return row
}

function emptyToNull(value: unknown) {
  return value === '' ? null : value
}

function assetRow(data: Record<string, unknown>) {
  return {
    ...data,
    itemId: emptyToNull(data.itemId),
    languageCode: emptyToNull(data.languageCode),
  }
}

function Related({
  source,
  reference,
  label,
  emptyText,
}: {
  source: string
  reference: string
  label?: string
  emptyText?: string
}) {
  return (
    <ReferenceField
      source={source}
      reference={reference}
      label={label}
      link="edit"
      emptyText={emptyText}
    />
  )
}

export function LanguageList() {
  return (
    <List>
      <Datagrid rowClick="edit">
        <TextField source="code" />
        <BooleanField source="isActive" />
      </Datagrid>
    </List>
  )
}

export function LanguageEdit() {
  return (
    <Edit>
      <SimpleForm>
        <TextInput source="code" disabled />
        <BooleanInput source="isActive" />
      </SimpleForm>
    </Edit>
  )
}

export function LanguageCreate() {
  return (
    <Create>
      <SimpleForm>
        <TextInput source="code" validate={required()} />
        <BooleanInput source="isActive" />
      </SimpleForm>
    </Create>
  )
}

export function MarketList() {
  return (
    <List>
      <Datagrid rowClick="edit">
        <TextField source="code" />
        <TextField source="name" />
        <TextField source="kind" />
      </Datagrid>
    </List>
  )
}

export function MarketEdit() {
  return (
    <Edit>
      <SimpleForm>
        <TextInput source="code" disabled />
        <TextInput source="name" validate={required()} />
        <TextInput source="kind" validate={required()} />
      </SimpleForm>
    </Edit>
  )
}

export function MarketCreate() {
  return (
    <Create>
      <SimpleForm>
        <TextInput source="code" validate={required()} />
        <TextInput source="name" validate={required()} />
        <TextInput source="kind" validate={required()} />
      </SimpleForm>
    </Create>
  )
}

export function FamilyList() {
  return (
    <List>
      <Datagrid rowClick="edit">
        <TextField source="code" />
        <TextField source="name" />
        <TextField source="summary" />
        <NumberField source="sequence" />
      </Datagrid>
    </List>
  )
}

export function FamilyEdit() {
  return (
    <Edit>
      <SimpleForm>
        <TextInput source="code" validate={required()} />
        <TextInput source="name" validate={required()} />
        <TextInput source="summary" />
        <NumberInput source="sequence" />
      </SimpleForm>
    </Edit>
  )
}

export function FamilyCreate() {
  return (
    <Create>
      <SimpleForm>
        <TextInput source="code" validate={required()} />
        <TextInput source="name" validate={required()} />
        <TextInput source="summary" />
        <NumberInput source="sequence" />
      </SimpleForm>
    </Create>
  )
}

export function TagList() {
  return (
    <List>
      <Datagrid rowClick="edit">
        <TextField source="name" />
        <TextField source="code" />
        <TextField source="category" />
        <BooleanField source="isPublic" />
        <NumberField source="sequence" />
      </Datagrid>
    </List>
  )
}

export function TagEdit() {
  return (
    <Edit>
      <SimpleForm>
        <TextInput source="name" validate={required()} />
        <TextInput source="code" validate={required()} />
        <TextInput source="category" validate={required()} />
        <BooleanInput source="isPublic" />
        <NumberInput source="sequence" />
      </SimpleForm>
    </Edit>
  )
}

export function TagCreate() {
  return (
    <Create>
      <SimpleForm>
        <TextInput source="name" validate={required()} />
        <TextInput source="code" validate={required()} />
        <TextInput source="category" validate={required()} />
        <BooleanInput source="isPublic" />
        <NumberInput source="sequence" />
      </SimpleForm>
    </Create>
  )
}

export function ProductList() {
  return (
    <List>
      <Datagrid rowClick="edit">
        <TextField source="title" />
        <TextField source="code" />
        <Related source="familyId" reference="families" label="Family" />
        <Related source="contentLanguage" reference="languages" label="Language" />
        <ArrayField source="tags" label="Tags">
          <SingleFieldList linkType={false}>
            <ChipField source="name" />
          </SingleFieldList>
        </ArrayField>
        <TextField source="summary" />
      </Datagrid>
    </List>
  )
}

export function ProductEdit() {
  return (
    <Edit transform={productRow}>
      <SimpleForm>
        <ReferencePickerInput
          source="familyId"
          reference="families"
          label="Family"
          validate={[required()]}
        />
        <TextInput source="code" validate={required()} />
        <TextInput source="title" validate={required()} />
        <TextInput source="summary" />
        <TextInput source="description" multiline />
        <ReferencePickerInput
          source="contentLanguage"
          reference="languages"
          label="Content language"
          validate={[required()]}
        />
      </SimpleForm>
    </Edit>
  )
}

export function ProductCreate() {
  return (
    <Create transform={productRow}>
      <SimpleForm>
        <ReferencePickerInput
          source="familyId"
          reference="families"
          label="Family"
          validate={[required()]}
        />
        <TextInput source="code" validate={required()} />
        <TextInput source="title" validate={required()} />
        <TextInput source="summary" />
        <TextInput source="description" multiline />
        <ReferencePickerInput
          source="contentLanguage"
          reference="languages"
          label="Content language"
          validate={[required()]}
        />
      </SimpleForm>
    </Create>
  )
}

export function ItemList() {
  return (
    <List>
      <Datagrid rowClick="edit">
        <TextField source="title" />
        <TextField source="code" />
        <TextField source="kind" />
        <Related source="productId" reference="products" label="Product" />
        <NumberField source="sequence" />
        <BooleanField source="isOptional" />
      </Datagrid>
    </List>
  )
}

export function ItemEdit() {
  return (
    <Edit>
      <SimpleForm>
        <ReferencePickerInput
          source="productId"
          reference="products"
          label="Product"
          validate={[required()]}
        />
        <TextInput source="kind" validate={required()} />
        <TextInput source="code" validate={required()} />
        <NumberInput source="sequence" />
        <TextInput source="title" validate={required()} />
        <TextInput source="summary" />
        <TextInput source="grouping" />
        <BooleanInput source="isOptional" />
      </SimpleForm>
    </Edit>
  )
}

export function ItemCreate() {
  return (
    <Create>
      <SimpleForm>
        <ReferencePickerInput
          source="productId"
          reference="products"
          label="Product"
          validate={[required()]}
        />
        <TextInput source="kind" validate={required()} />
        <TextInput source="code" validate={required()} />
        <NumberInput source="sequence" />
        <TextInput source="title" validate={required()} />
        <TextInput source="summary" />
        <TextInput source="grouping" />
        <BooleanInput source="isOptional" />
      </SimpleForm>
    </Create>
  )
}

export function AssetList() {
  return (
    <List>
      <Datagrid rowClick="edit">
        <TextField source="title" />
        <TextField source="role" />
        <TextField source="kind" />
        <Related source="productId" reference="products" label="Product" />
        <Related source="itemId" reference="items" label="Item" emptyText="-" />
        <Related
          source="languageCode"
          reference="languages"
          label="Language"
          emptyText="-"
        />
        <TextField source="provider" />
      </Datagrid>
    </List>
  )
}

export function AssetEdit() {
  return (
    <Edit transform={assetRow}>
      <SimpleForm>
        <ReferencePickerInput
          source="productId"
          reference="products"
          label="Product"
          validate={[required()]}
        />
        <ReferencePickerInput source="itemId" reference="items" label="Item" allowEmpty />
        <TextInput source="role" validate={required()} />
        <TextInput source="kind" validate={required()} />
        <ReferencePickerInput
          source="languageCode"
          reference="languages"
          label="Language"
          allowEmpty
        />
        <TextInput source="title" />
        <TextInput source="groupCode" />
        <TextInput source="provider" validate={required()} />
        <TextInput source="providerAssetId" />
        <TextInput source="streamUrl" />
        <TextInput source="downloadUrl" />
        <BooleanInput source="allowStream" />
        <BooleanInput source="allowDownload" />
        <NumberInput source="durationSeconds" />
        <NumberInput source="fileSizeBytes" />
      </SimpleForm>
    </Edit>
  )
}

export function AssetCreate() {
  return (
    <Create transform={assetRow}>
      <SimpleForm>
        <ReferencePickerInput
          source="productId"
          reference="products"
          label="Product"
          validate={[required()]}
        />
        <ReferencePickerInput source="itemId" reference="items" label="Item" allowEmpty />
        <TextInput source="role" validate={required()} />
        <TextInput source="kind" validate={required()} />
        <ReferencePickerInput
          source="languageCode"
          reference="languages"
          label="Language"
          allowEmpty
        />
        <TextInput source="title" />
        <TextInput source="groupCode" />
        <TextInput source="provider" validate={required()} />
        <TextInput source="providerAssetId" />
        <TextInput source="streamUrl" />
        <TextInput source="downloadUrl" />
        <BooleanInput source="allowStream" />
        <BooleanInput source="allowDownload" />
        <NumberInput source="durationSeconds" />
        <NumberInput source="fileSizeBytes" />
      </SimpleForm>
    </Create>
  )
}

export function ProductTagList() {
  return (
    <List>
      <Datagrid>
        <Related source="productId" reference="products" label="Product" />
        <Related source="tagId" reference="tags" label="Tag" />
      </Datagrid>
    </List>
  )
}

export function ProductTagCreate() {
  return (
    <Create>
      <SimpleForm>
        <ReferencePickerInput
          source="productId"
          reference="products"
          label="Product"
          validate={[required()]}
        />
        <ReferencePickerInput source="tagId" reference="tags" label="Tag" validate={[required()]} />
      </SimpleForm>
    </Create>
  )
}

export function ProductMarketList() {
  return (
    <List>
      <Datagrid rowClick="edit">
        <Related source="productId" reference="products" label="Product" />
        <Related source="marketCode" reference="markets" label="Market" />
        <TextField source="launchedOn" />
      </Datagrid>
    </List>
  )
}

export function ProductMarketEdit() {
  return (
    <Edit>
      <SimpleForm>
        <ReferencePickerInput
          source="productId"
          reference="products"
          label="Product"
          validate={[required()]}
        />
        <ReferencePickerInput
          source="marketCode"
          reference="markets"
          label="Market"
          validate={[required()]}
        />
        <DateInput source="launchedOn" />
      </SimpleForm>
    </Edit>
  )
}

export function ProductMarketCreate() {
  return (
    <Create>
      <SimpleForm>
        <ReferencePickerInput
          source="productId"
          reference="products"
          label="Product"
          validate={[required()]}
        />
        <ReferencePickerInput
          source="marketCode"
          reference="markets"
          label="Market"
          validate={[required()]}
        />
        <DateInput source="launchedOn" />
      </SimpleForm>
    </Create>
  )
}

export function AssetMarketList() {
  return (
    <List>
      <Datagrid>
        <Related source="assetId" reference="assets" label="Asset" />
        <Related source="marketCode" reference="markets" label="Market" />
      </Datagrid>
    </List>
  )
}

export function AssetMarketCreate() {
  return (
    <Create>
      <SimpleForm>
        <ReferencePickerInput
          source="assetId"
          reference="assets"
          label="Asset"
          validate={[required()]}
        />
        <ReferencePickerInput
          source="marketCode"
          reference="markets"
          label="Market"
          validate={[required()]}
        />
      </SimpleForm>
    </Create>
  )
}
