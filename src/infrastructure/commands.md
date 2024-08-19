# Deploy (dev)

`az deployment sub create --location polandcentral --template-file main.bicep --parameters sqlAdminLogin=SQLADMINLOGIN sqlAdminPassword=SQLADMINPASSWORD`

# Deploy (prod)

`az deployment sub create --location polandcentral --template-file main.bicep --parameters sqlAdminLogin=SQLADMINLOGIN sqlAdminPassword=SQLADMINPASSWORD envName=prod`
