# ℹ️ README Template for New Integrations

This template outlines the recommended structure for your integrations README.
It's designed to showcase all the key features of your integration, provide clear usage instructions and help maintain consistent documentation across all integrations.

Example sentences / hints have been provided for each section to get your started but these should be changed to fit the context of your integration.

Hints are marked with an ℹ️ ( ) ℹ️ and should be removed from your README.

For convenience, you can search and replace the following keywords for quicker boiler plate setup:

| Keyword | Description | Example |
|-----------|--------------|----------|
| [integration-name] | Name of the integration | YourIntegration |
| [CompatibleVersion] | Version the integration works with | V3 |

## 📦 Integration Structure
Each integration should be organized into its own group folder. Copy this README template file into the root of your integration's directory and rename it to `README.md`. For example:
```
[group-name]/
  ├── README.md
  ├── Elsa.Integrations.[integration-name]/
  │   ├── Services/
  │   ├── Activities/
  │   ├── AI/
  └── Elsa.Integrations.[integration-name].Implementation/
      ├── Services/
      ├── Activities/
      ├── AI/
```


⛔ **Remove this header section from your README.** ⛔

---

# [integration-name] Integration

<details>
  <summary>📖 Table of Contents</summary>
  <ol>
    <li><a href="#overview">Overview</a></li>
    <li><a href="#features">Features</a></li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
     <li>
      <a href="#configuration">Configuration</a>
      <ul>
        <li><a href="#program.cs">Program.cs</a></li>
        <li><a href="#appsettings.json">Appsettings.json</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#activities">Activities</a></li>
    <li><a href="#examples">Examples</a></li>
    <li><a href="#planned-features">Planned Features</a></li>
    <li><a href="#limitations">Limitations</a></li>
    <li><a href="#troubleshooting">Troubleshooting</a></li>
    <li><a href="#notes">Notes & Comments</a></li>
  </ol>
</details>

## 🧠 Overview

This package extends [Elsa Workflows](https://github.com/elsa-workflows/elsa-core) with support for **[integration-name]**. It introduces custom activities that make it easy to integrate [integration-name] features directly into your workflow logic.

## ✨ Key Features

- Activities: `DoSomething`, `DoSomethingElse` and `DoSomethingAlso`
- Highlight the key features from your integration.

---

## ⚡ Getting Started

### 📋 Prerequisites

- Elsa Workflows **[CompatibleVersion]** installed in your project.
- Access to the [integration-name] system or API (credentials, URL, etc.).

## 🛠 Installation

The following NuGet packages are available for this integration:

```bash
Elsa.Integration.[integration-name]
Elsa.Integration.[integration-name].Implementation1
Elsa.Integration.[integration-name].Implementation2
```

You can install the clients via NuGet:

```bash
dotnet add package Elsa.Integration.[integration-name].Implementation1
```

## ⚙️ Configuration

### Program.cs

Register the integration in your application startup:

```csharp
using Elsa.Extensions;
using Elsa.[integration-name];

services.AddElsa(elsa =>
    {
        elsa.Use[integration-name](options =>
            {
                options.Property1 = "value1"
                options.Config = options =>
                {
                    options.Property2 = "value2";
                };
            })
    }
```

### Appsettings.json (If applicable)
Or via `appsettings.json`:

```json
"[integration-name]": {
    "Property1": "value1",
    "Config": {
        "Property2": "value2"
    }
}
```

---

## 📌 Usage

Once the implementation is registered with your required implementations, the activities will be ready to use, either via code or [Elsa Studio](https://github.com/elsa-workflows/elsa-studio).

## 🚀 Activities 

This integration comes with the following activities:

### DoSomething

| Properties | Type | Description | Input/Output | Notes |
| ---------- | ---- | ----------- | --- | ----- |
| SomeInput | int? | A brief summery of what this input is for. | Input | Perhaps the min / max values allowed. |
| OtherInput | string? / Secret | A brief summery of what this input is for. | Input | - |
| TheOutput | object? | A brief summery of what this output is for. | Output | - |

### DoSomethingElse

| Properties | Type | Description | Input/Output | Notes |
| ---------- | ---- | ----------- | --- | ----- |
| SomeInput | int? | A brief summery of what this input is for. | Input | Perhaps the min / max values allowed. |
| OtherInput | string? / Secret | A brief summery of what this input is for. | Input | - |
| TheOutput | object? | A brief summery of what this output is for. | Output | - |


## 🧪 Examples

### Example of your feature

ℹ️ (List some code snippets / picture examples of how to use aspects of your integration to help others learn how to use them.) ℹ️

---

## 🚧 Limitations

ℹ️ (Bullet point known limitations, if any. For example:) ℹ️

- Does not support async
- Requires an active and reachable [integration-name] service endpoint

---

## 🆘 Troubleshooting

### Common Errors

ℹ️ (Bullet points known errors, if any. For example:) ℹ️

- **`NullReferenceException`**  
  Ensure all required configuration values (API key, endpoint, etc.) are correctly set.

- **`401 Unauthorized`**  
  Verify that your API key is valid and the endpoint is correct.

- **`Activity not found`**  
  Ensure the activity was registered in `Program.cs` or `Startup.cs` using `.AddActivity<>()`.

---

## 🗺️ Planned Features

ℹ️ (Checkbox points for planned features, if any. For example:) ℹ️

- [ ] Add async retry/backoff support
- [ ] Add integration tests

---

## 🗒️ Notes & Comments

This integration was developed to add [integration-name] functionality to Elsa Workflows.  
If you have ideas for improvement, encounter issues, or want to share how you're using it, feel free to open an issue or start a discussion!