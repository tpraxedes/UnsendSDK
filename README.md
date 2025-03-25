# UnsendSDK 🚀

A C# SDK for interacting with the [Unsend API](https://unsend.dev/). This library allows developers to easily send emails, manage contacts, and schedule messages.

## Supported versions
- Unsend 1.4.x

## 📦 Installation

You can install the package from NuGet:

```sh
dotnet add package UnsendSDK --version 1.0.1
```

Or, using the NuGet Package Manager:

```sh
PM> NuGet\Install-Package UnsendSDK -Version 1.0.1
```

## 🚀 Usage Example

### **Initialize the Client**
```csharp
using UnsendSDK;

var client = new UnsendClient("your-api-key", "YOUR URL FOR UNSEND OR LEAVE IT BLANK FOR UNSEND CLOUD");
```

### **Send an Email**
```csharp
var sendedMail = await client.emailService.SendEmailAsync(
    emailTo, 
    "C# SDK TEST", 
    "from@mail.com", 
    html: html);
Console.WriteLine($"Email ID: {sendedMail.emailId}");
```

### **Retrieve Email Details**
```csharp
var emailDetails = await client.emailService.GetEmailAsync("your-email-id");
Console.WriteLine($"Email ID: {emailDetails.subject}");
```

### **Update Scheduled Email**
```csharp
var updatedEmail = await client.emailService.UpdateScheduleAsync("your-email-id", DateTime.UtcNow.AddMinutes(30));
Console.WriteLine($"Updated Email ID: {updatedEmail.emailId}");
```

### **Cancel a Scheduled Email**
```csharp
var canceledEmail = await client.emailService.CancelScheduleAsync("your-email-id");
Console.WriteLine($"Canceled Email ID: {canceledEmail.emailId}");
```

## 🛠️ Features
- ✅ Send Emails
- ✅ Schedule Emails
- ✅ Retrieve Email Status
- ✅ Cancel Scheduled Emails
- ✅ Get Domains Information


## 💬 Support
If you encounter any issues, feel free to create an issue on [GitHub](https://github.com/tpraxedes/UnsendSDK/issues).

