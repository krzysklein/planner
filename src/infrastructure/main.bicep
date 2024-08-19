targetScope='subscription'

@description('Deployment environment name')
param envName string = 'dev'

@description('Location for all resources')
param location string = 'polandcentral'

@description('SQL Server administrator username')
param sqlAdminLogin string

@description('SQL Server administrator password')
@secure()
param sqlAdminPassword string

resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: 'plannerapp-${envName}'
  location: location
}

module backend 'backend.bicep' = {
  name:'backendModule'
  scope: resourceGroup
  params: {
    sqlAdminLogin: sqlAdminLogin
    sqlAdminPassword: sqlAdminPassword
  }
}
