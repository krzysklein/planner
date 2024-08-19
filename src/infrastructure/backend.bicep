@description('SQL Server administrator username')
param sqlAdminLogin string

@description('SQL Server administrator password')
@secure()
param sqlAdminPassword string

var resourceGroupLocation = resourceGroup().location
var resourceGroupUniqueId = uniqueString(resourceGroup().id)

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: 'appserviceplan'
  location: resourceGroupLocation
  sku: {
    name: 'F1'
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource tasksApiAppService 'Microsoft.Web/sites@2023-12-01' = {
  name: 'tasks-api-${resourceGroupUniqueId}'
  location: resourceGroupLocation
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
    }
    httpsOnly: true
  }
}

resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: 'sqlserver-${resourceGroupUniqueId}'
  location: resourceGroupLocation
  properties: {
    administratorLogin: sqlAdminLogin
    administratorLoginPassword: sqlAdminPassword
  }
}

resource tasksSqlDatabase 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  parent: sqlServer
  name: 'tasks'
  location: resourceGroupLocation
  sku: {
    name: 'GP_S_Gen5_2'
    tier: 'GeneralPurpose'
  }
}
