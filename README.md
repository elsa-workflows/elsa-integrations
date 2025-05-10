# Elsa Integrations

Welcome to the **Elsa Integrations** repository! This project provides a collection of integration modules for [Elsa Workflows](https://github.com/elsa-workflows/elsa-core), enabling seamless workflow automation across various third-party services.

---

## 🚀 Integration Status

Below is the current status of each integration. Icons indicate implementation progress:

 - ✅ = Released
 - ⏳ = Soon to be released
 - 🚧 = In development
 - 🔲 = Not started

### 📨 Messaging & Communication
| Status | Integration | Description | Module Name | Issue |
|--------|------------|-------------|-------------|-------|
| 🔲 | **Telegram** | Send & receive messages, file downloads | `Elsa.Integrations.Telegram` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| [✅]( https://github.com/elsa-workflows/elsa-integrations/tree/main/src/slack/Elsa.Integrations.Slack) | **Slack** | Channel messages, user notifications | `Elsa.Integrations.Slack` | [View Issue](https://github.com/elsa-workflows/elsa-integrations/issues/26) |
| 🔲 | **Discord** | Bot commands, message triggers | `Elsa.Integrations.Discord` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Microsoft Teams** | Chat automation, meeting reminders | `Elsa.Integrations.Teams` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| [✅](https://github.com/elsa-workflows/elsa-integrations/tree/main/src/telnyx/Elsa.Integrations.Telnyx) | **Telnyx** | Telephony automation | `Elsa.Integrations.Telnyx` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |

### 📧 Email & Productivity
| Status | Integration | Description | Module Name | Issue |
|--------|------------|-------------|-------------|-------|
| 🔲 | **Gmail** | Send, receive, label, draft emails | `Elsa.Integrations.Gmail` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Outlook (Office 365)** | Email management via Microsoft Graph API | `Elsa.Integrations.Outlook` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Google Calendar** | Event scheduling and updates | `Elsa.Integrations.GoogleCalendar` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Microsoft Calendar** | Office 365 calendar integration | `Elsa.Integrations.Office365Calendar` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |

### 🗄️ Storage Services
| Status | Integration | Description | Module Name | Issue |
|--------|------------|-------------|-------------|-------|
| 🔲 | **Google Drive** | Upload, download, manage files | `Elsa.Integrations.GoogleDrive` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **OneDrive** | File storage and access for Office 365 | `Elsa.Integrations.OneDrive` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🚧 | **Azure Storage** | Blob storage management | `Elsa.Integrations.AzureStorage` | [View Issue](https://github.com/elsa-workflows/elsa-integrations/issues/1) |
| 🔲 | **Dropbox** | Cloud storage and file sync | `Elsa.Integrations.Dropbox` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |

### 🗃️ Databases (SQL)
| Status | Integration | Description | Module Name | Issue |
|--------|------------|-------------|-------------|-------|
| [✅]( https://github.com/elsa-workflows/elsa-integrations/tree/main/src/sql/README.md) | **MySql** | Connect and query MySQL databases | `Elsa.Integrations.Sql.MySql` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| [✅]( https://github.com/elsa-workflows/elsa-integrations/tree/main/src/sql/README.md) | **PostgreSql** | Connect and query PostgreSql databases | `Elsa.Integrations.Sql.PostgreSql` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| [✅]( https://github.com/elsa-workflows/elsa-integrations/tree/main/src/sql/README.md) | **Sqlite** | Connect and query Sqlite databases | `Elsa.Integrations.Sql.Sqlite` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| [✅]( https://github.com/elsa-workflows/elsa-integrations/tree/main/src/sql/README.md) | **Sql Server** | Connect and query Microsoft SQL Server databases | `Elsa.Integrations.Sql.SqlServer` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |

### 🛠 DevOps & Monitoring
| Status | Integration | Description | Module Name | Issue |
|--------|------------|-------------|-------------|-------|
| 🔲 | **Azure DevOps** | Pipelines, repositories, work items | `Elsa.Integrations.AzureDevOps` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **GitHub** | PR automation, repo events | `Elsa.Integrations.GitHub` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **GitLab** | CI/CD triggers and repo management | `Elsa.Integrations.GitLab` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Jenkins** | Pipeline automation and job execution | `Elsa.Integrations.Jenkins` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Datadog** | Monitoring, logging, and alerts | `Elsa.Integrations.Datadog` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |

### ☁️ Cloud Compute & Serverless
| Status | Integration | Description | Module Name | Issue |
|--------|------------|-------------|-------------|-------|
| 🔲 | **Azure Functions** | Serverless function triggers | `Elsa.Integrations.AzureFunctions` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **AWS Lambda** | Invoke and trigger Lambda functions | `Elsa.Integrations.AWSLambda` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Google Cloud Functions** | Event-driven function automation | `Elsa.Integrations.GoogleCloudFunctions` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |

### 📊 CRM & Sales Automation
| Status | Integration | Description | Module Name | Issue |
|--------|------------|-------------|-------------|-------|
| 🔲 | **Salesforce** | Lead management, opportunity tracking | `Elsa.Integrations.Salesforce` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **HubSpot** | Contacts, deals, email automation | `Elsa.Integrations.HubSpot` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Zoho CRM** | Lead scoring, campaign tracking | `Elsa.Integrations.ZohoCRM` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Pipedrive** | Sales pipeline automation | `Elsa.Integrations.Pipedrive` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |

### 💰 Payments & Finance
| Status | Integration | Description | Module Name | Issue |
|--------|------------|-------------|-------------|-------|
| 🔲 | **Stripe** | Payment processing, subscriptions | `Elsa.Integrations.Stripe` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **PayPal** | Transactions, refunds, invoicing | `Elsa.Integrations.PayPal` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Square** | POS and e-commerce transactions | `Elsa.Integrations.Square` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **QuickBooks** | Invoice and expense automation | `Elsa.Integrations.QuickBooks` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |

### 🤖 AI & Automation
| Status | Integration | Description | Module Name | Issue |
|--------|------------|-------------|-------------|-------|
| 🔲 | **OpenAI** | GPT-based text generation, chatbots | `Elsa.Integrations.OpenAI` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Google AI** | AI-enhanced search, translation | `Elsa.Integrations.GoogleAI` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **AWS Comprehend** | NLP services for text analysis | `Elsa.Integrations.AWSComprehend` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Azure AI** | Vision, speech, language processing | `Elsa.Integrations.AzureAI` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |

### 🎥 Video & Streaming Platforms
| Status | Integration | Description | Module Name | Issue |
|--------|------------|-------------|-------------|-------|
| 🔲 | **YouTube** | Upload videos, retrieve video details, manage playlists | `Elsa.Integrations.YouTube` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new)|
| 🔲 | **Rumble** | Upload videos, fetch channel videos, manage content | `Elsa.Integrations.Rumble` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Twitch** | Live stream events, chat automation, clip management | `Elsa.Integrations.Twitch` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Vimeo** | Upload videos, manage privacy settings, get video analytics | `Elsa.Integrations.Vimeo` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |

### 🏭 Industrial Communication Protocols
| Status | Integration | Description | Module Name | Issue |
|--------|------------|-------------|-------------|-------|
| 🔲 | **OPC UA** | Browse nodes, Read/Write values, Subscribe/Unsubscribe nodes | `Elsa.Integrations.OPC.UA` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **Modbus** | Read/Write coils, Read discrete Inputs, Read/Write registers | `Elsa.Integrations.Modbus` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |
| 🔲 | **MQTT Sparkplug** | Discover and Subscribe to topics, Publish messages | `Elsa.Integrations.MQTT.Sparkplug` | [Open Issue](https://github.com/elsa-workflows/elsa-integrations/issues/new) |

---

## 📦 Structure
Each integration is structured as a standalone package under the `Elsa.Integrations` namespace. Example:


```plaintext
...
├─azure/
│   ├── Elsa.Integrations.AzureServiceBus/
│   │   ├── README.md
│   │   ├── Services/
│   │   ├── Activities/
│   │   ├── AI/
│   │   └── ...
│   └── Elsa.Integrations.AzureStorage/
│       ├── README.md
│       ├── Services/
│       ├── Activities/
│       ├── AI/
│       └── ...
├─sql/
│   ├── README.md
│   ├── Elsa.Integrations.Sql/
│   │   ├── Services/
│   │   ├── Activities/
│   │   ├── AI/
│   │   └── ...
│   └── Elsa.Integrations.Sql.Sqlite/
│       ├── Services/
│       ├── Activities/
│       ├── AI/
│       └── ...
...
```

## ⚡ Getting Started
To install a specific integration:
```sh
dotnet add package Elsa.Integrations.Gmail
```
To enable it in Elsa Workflows:
```csharp
services.AddElsa()
        .AddGmailIntegration();
```

## 🔥 Contributing
We welcome contributions! See our [Contributing Guide](CONTRIBUTING.md) for more details.

## 📜 License
This repository is licensed under the [MIT License](https://github.com/elsa-workflows/elsa-integrations/blob/main/LICENSE).
