# TransferWebRequests Function

This Azure Function pulls W3C IIS logs from a Log Analytics workspace and writes them into a SQL Managed Instance database. It leverages Azure Managed Identity for secure, passwordless authentication and runs on the Flex Consumption Plan with VNet integration.

---

## 📋 Table of Contents

1. [Features](#features)
2. [Prerequisites](#prerequisites)
3. [Configuration](#configuration)
4. [Identity Setup](#identity-setup)
5. [Network Configuration](#network-configuration)
6. [Deployment](#deployment)
7. [Code Highlights](#code-highlights)
8. [License](#license)

---

## ✨ Features

* **Event-driven**: Time-triggered 
* **Log Ingestion**: Queries W3C IIS logs via Azure Monitor Logs SDK
* **Data Sink**: Writes log entries into Azure SQL Managed Instance
* **Secure**: Uses Azure Managed Identity (no secrets stored)
* **Scalable**: Executes on Flex Consumption Plan with VNet support
* **DI-ready**: Built on .NET Isolated Worker with dependency injection

---

## 🛠 Prerequisites

* Azure subscription with:

  * Log Analytics workspace
  * SQL Managed Instance (with database)
  * Function App (Flex Consumption Plan)
* .NET 9.0 SDK
* Azure CLI or Azure PowerShell
* (Optional) Azure Key Vault for connection string

---

## ⚙️ Configuration

| Setting                     | Description                                            |
| --------------------------- | ------------------------------------------------------ |
| `LogAnalytics__WorkspaceId` | Your Log Analytics Workspace ID                        |
| `SQLConnectionString`       | SQL MI connection string using Managed Identity format |

### SQL Connection String Formats

* **System-assigned Managed Identity**:

  ```ini
  Server=<your-sql-mi>.private.database.windows.net;
  Initial Catalog=<your-db>;
  Authentication=Active Directory Default;
  ```

* **Key Vault reference**:

  ```ini
  @Microsoft.KeyVault(SecretUri=https://myvault.vault.azure.net/secrets/mysecret)
  ```

  or

  ```ini
  @Microsoft.KeyVault(VaultName=myvault;SecretName=mysecret)
  ```

---

## 🔐 Identity Setup

1. **Enable Managed Identity**

   * In the Azure Portal, navigate to your Function App > **Identity**.
   * Switch on **System Assigned**.

2. **Grant Log Analytics Access**

   * Resource: Your Log Analytics Workspace
   * Role: **Log Analytics Reader** (or **Contributor**)
   * Assign to: Function App’s Managed Identity

3. **Grant SQL Permissions** (run on your target database):

   ```sql
   CREATE USER [<function-app-name>] FROM EXTERNAL PROVIDER;
   ALTER ROLE db_datareader ADD MEMBER [<function-app-name>];
   ALTER ROLE db_datawriter ADD MEMBER [<function-app-name>];
   -- Or: ALTER ROLE db_owner ADD MEMBER [<function-app-name>];
   ```

---

## 🌐 Network Configuration

* **VNet Integration**: Function App must be deployed into a VNet
* **Subnet**: Delegated to `Microsoft.App/environments`
* **SQL MI & Function App**: Same VNet or peered
* **NSG Rules**:

  * Allow outbound TCP 1433 from the Function subnet
  * Allow AzureCloud service tag on SQL MI subnet
* **Private Link** (if used):

  * Link `privatelink.database.windows.net` Private DNS Zone to your VNet

---

## 🚀 Deployment

Deploy the Function App into your VNet at creation:

```bash
# Create Resource Group
az group create --name my-rg --location westeurope

# Create Function Plan
az functionapp plan create \
  --name my-plan \
  --resource-group my-rg \
  --location westeurope \
  --number-of-workers 1 \
  --sku EP1 \
  --is-linux

# Create Function App with VNet integration
az functionapp create \
  --name my-func-app \
  --storage-account mystorageacct \
  --resource-group my-rg \
  --plan my-plan \
  --functions-version 4 \
  --assign-identity \
  --vnet my-vnet \
  --subnet my-subnet
```

Set App Settings:

```bash
az functionapp config appsettings set \
  --name my-func-app \
  --resource-group my-rg \
  --settings \
    LogAnalytics__WorkspaceId=<workspace-id> \
    SQLConnectionString="Server=...;Initial Catalog=...;Authentication=Active Directory Default;"
```

---

## 🧩 Code Highlights

### Dependency Injection & Identity

```csharp
var credential = new DefaultAzureCredential();
services.AddSingleton<TokenCredential>(credential);
services.AddSingleton(new LogsQueryClient(credential));
```

### SQL MI Connection

```csharp
var connectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();
```

---

