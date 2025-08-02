📱 Reminder App
A simple and elegant Android application built using .NET 8 MAUI to help you keep track of your monthly payment reminders. Users can set a PIN for security, add payment reminders, and mark them as complete. When marked complete, reminders automatically roll over to the next month.

🔒 Login Page
When the app is launched for the first time, the user will be asked to Set a 4-digit PIN.

Once the PIN is set, the set PIN section is hidden.

From next launch onwards, the user must enter the 4-digit PIN to access the app.

Note: If the user forgets the PIN, the app must be uninstalled and reinstalled to reset it.

📊 Dashboard
The dashboard displays:

Total pending amount 💰

Total reminder count 📌

A list of pending reminders

A Complete button for each reminder, which:

Updates the reminder to next month with same details

➕ Add Reminder
You can add a new monthly reminder by entering:

Reminder Name

Description

Date

Amount

Each saved reminder is automatically included in your dashboard.

💡 Features
🔐 Secure login with 4-digit PIN

📅 Add monthly payment reminders

🔁 Roll over reminders for the next month upon completion

📱 Android support via .NET 9 MAUI

🌈 Clean and intuitive UI

🛠 Tech Stack
.NET 9

MAUI (Multi-platform App UI)

Android platform

📦 Installation
Clone the repository

bash
Copy
Edit
git clone https://github.com/your-username/reminder-app.git
Open the project in Visual Studio 2022 with MAUI workload

Build and deploy the app to an Android device or emulator

📲 Installing on Android Device
To install and test the app on your Android device:

In Visual Studio, select Release mode.

Build the project and run it on an emulator or connected device.

After successful build, navigate to the bin\Release\net8.0-android\... folder.

Find the signed APK generated and copy it to your Android device.

Open the file on your Android device and install it.

Important:

If the install fails the first time, try again.

Make sure to enable “Install from unknown sources” in your device settings.
