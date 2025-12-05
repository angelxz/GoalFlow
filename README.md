# GoalFlow

GoalFlow is a cross-platform mobile application built with .NET MAUI designed to help users track and achieve their personal goals. It allows users to manage goals across various categories, track their progress, and stay motivated with relevant daily content.

## Features

- **Goal Management**: Create, view, and delete goals.
- **Categorization**: Organize goals into four main categories:
    - 💰 **Finance**
    - 🍎 **Health**
    - 💡 **Personal**
    - 🎓 **Education**
- **Progress Tracking**: Mark goals as completed and earn points.
- **Daily Inspiration & Info**: Fetches dynamic content relevant to your goals:
    - **Daily Quote**: Motivational quotes to start your day.
    - **Finance**: Current USD to EUR exchange rates.
    - **Health**: Current weather updates (defaulted to Sofia).
    - **Personal**: Random motivational quotes.
    - **Education**: Interesting random facts.
- **Local Storage**: Your data is saved locally on your device using `Preferences`.

## Technology Stack

- **Framework**: [.NET MAUI](https://dotnet.microsoft.com/en-us/apps/maui) (Multi-platform App UI)
- **Language**: C#
- **External APIs**:
    - [Frankfurter API](https://www.frankfurter.app/) (Currency exchange)
    - [Open-Meteo](https://open-meteo.com/) (Weather)
    - [DummyJSON](https://dummyjson.com/) (Quotes)
    - [UselessFacts](https://uselessfacts.jsph.pl/) (Random facts)

## Getting Started

### Prerequisites

- Visual Studio 2022 (with .NET MAUI workload installed) or Visual Studio Code (with .NET MAUI extension).
- .NET SDK matching the project configuration.
  > **Note:** The project file currently specifies `net10.0` frameworks. Ensure your environment is configured to support the target frameworks defined in `GoalFlow/GoalFlow.csproj`.

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/GoalFlow.git
   ```
2. Navigate to the project directory:
   ```bash
   cd GoalFlow
   ```
3. Restore dependencies:
   ```bash
   dotnet restore
   ```

### Running the App

You can run the application on an emulator or a physical device.

**Using Visual Studio:**
1. Open `GoalFlow.sln`.
2. Select your target device/emulator.
3. Press `F5` or click "Start Debugging".

**Using Command Line:**
```bash
# For Android
dotnet build -t:Run -f net10.0-android

# For iOS (macOS only)
dotnet build -t:Run -f net10.0-ios

# For Windows
dotnet build -t:Run -f net10.0-windows10.0.19041.0
```

## Project Structure

- **GoalFlow/**: Main project directory.
    - **Models/**: Data models (`Goal`, `GoalCategory`, `GoalCompletionRecord`).
    - **Services/**: Business logic and data access (`GoalService`, `ApiService`).
    - **ViewModels/**: MVVM ViewModels.
    - **Views/**: UI Pages.
    - **Resources/**: Images, fonts, and other assets.
    - **Platforms/**: Platform-specific code (Android, iOS, Windows, MacCatalyst).

## License

This project is licensed under the MIT License.
