import { Admin, Resource } from 'react-admin'
import { authProvider } from './authProvider.ts'
import { BffLogin } from './BffLogin.tsx'
import { dataProvider } from './dataProvider.ts'
import { PortalLayout } from './PortalLayout.tsx'
import { PortalForbidden, PortalLoading } from './portalStatus.tsx'
import { recordLabel } from './recordLabel.ts'
import {
  AssetCreate,
  AssetEdit,
  AssetList,
  AssetMarketCreate,
  AssetMarketList,
  FamilyCreate,
  FamilyEdit,
  FamilyList,
  ItemCreate,
  ItemEdit,
  ItemList,
  LanguageCreate,
  LanguageEdit,
  LanguageList,
  MarketCreate,
  MarketEdit,
  MarketList,
  ProductCreate,
  ProductEdit,
  ProductList,
  ProductMarketCreate,
  ProductMarketEdit,
  ProductMarketList,
  ProductTagCreate,
  ProductTagList,
  TagCreate,
  TagEdit,
  TagList,
} from './resources.tsx'

function representation(resource: string) {
  return (record: Record<string, unknown>) => recordLabel(resource, record)
}

export default function App() {
  return (
    <Admin
      title="Product"
      requireAuth
      loginPage={BffLogin}
      layout={PortalLayout}
      loading={PortalLoading}
      accessDenied={PortalForbidden}
      authProvider={authProvider}
      dataProvider={dataProvider}
      disableTelemetry
    >
      <Resource
        name="products"
        list={ProductList}
        edit={ProductEdit}
        create={ProductCreate}
        recordRepresentation={representation('products')}
      />
      <Resource
        name="languages"
        list={LanguageList}
        edit={LanguageEdit}
        create={LanguageCreate}
        recordRepresentation={representation('languages')}
      />
      <Resource
        name="markets"
        list={MarketList}
        edit={MarketEdit}
        create={MarketCreate}
        recordRepresentation={representation('markets')}
      />
      <Resource
        name="families"
        list={FamilyList}
        edit={FamilyEdit}
        create={FamilyCreate}
        recordRepresentation={representation('families')}
      />
      <Resource
        name="tags"
        list={TagList}
        edit={TagEdit}
        create={TagCreate}
        recordRepresentation={representation('tags')}
      />
      <Resource
        name="items"
        list={ItemList}
        edit={ItemEdit}
        create={ItemCreate}
        recordRepresentation={representation('items')}
      />
      <Resource
        name="assets"
        list={AssetList}
        edit={AssetEdit}
        create={AssetCreate}
        recordRepresentation={representation('assets')}
      />
      <Resource
        name="product-tags"
        list={ProductTagList}
        create={ProductTagCreate}
      />
      <Resource
        name="product-markets"
        list={ProductMarketList}
        edit={ProductMarketEdit}
        create={ProductMarketCreate}
      />
      <Resource
        name="asset-markets"
        list={AssetMarketList}
        create={AssetMarketCreate}
      />
    </Admin>
  )
}
