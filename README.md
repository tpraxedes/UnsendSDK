# Unsend C#

C# SDK for [unsend.dev](https://unsend.dev)

## Supported versions
- Unsend 1.4.x

## About this project
This was built to be an SDK that can be used with both the cloud hosted and self hosted versions of Unsend

## How to use

- Run `dotnet add package UnsendSDK --version 1.0.0` to install the SDK to your project
- Initialize such as this:
- UnsendSDK.UnsendClient client = new UnsendSDK.UnsendClient("YOUR-API-KEY", "YOUR-UNSEND-URL OR LEAVE IT EMPTY FOR CLOUD");

### Cloud Hosted

- Generate an API key for Unsend by following [this guide](https://app.unsend.dev/dev-settings/api-keys)

### Self-hosted
- Generate an API key for Unsend by following [this guide](https://app.unsend.dev/dev-settings/api-keys)

| Variable Name     | Required | Default                      |
|-------------------|----------|------------------------------|
| `UNSEND_API_KEY`  | `YES`    | N/A                          |
| `UNSEND_BASE_URL` | `NO`     | `https://app.unsend.dev/` |
