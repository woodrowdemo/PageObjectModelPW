 ---

# PageObjectModelPW

This is a Page Object Model (POM) project using [Playwright](https://playwright.dev/dotnet/) with C#. It demonstrates how to structure automated browser tests for web applications using the POM design pattern, which enhances test maintenance and code reusability.

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Features

- Built with C# and Playwright for reliable end-to-end browser automation.
- Implements the Page Object Model design pattern for scalable and maintainable tests.
- Simple and clear codebase for learning and extending.

## Getting Started

### Prerequisites

- [.NET 6.0 SDK or later](https://dotnet.microsoft.com/download)
- [Node.js (required by Playwright)](https://nodejs.org/)
- [Playwright for .NET](https://playwright.dev/dotnet/docs/intro)

### Installation

1. **Clone the repository:**
    ```bash
    git clone https://github.com/woodrowdemo/PageObjectModelPW.git
    cd PageObjectModelPW
    ```

2. **Restore dependencies:**
    ```bash
    dotnet restore
    ```

3. **Install Playwright browsers:**
    ```bash
    pwsh bin/Debug/net6.0/playwright.ps1 install
    ```
    Or, if using bash:
    ```bash
    bash bin/Debug/net6.0/playwright.sh install
    ```

## Project Structure

```
PageObjectModelPW/
├── Pages/              # Page objects representing UI components
├── Tests/              # Test classes using the page objects
├── Utilities/          # Helper classes and utilities
├── PageObjectModelPW.csproj
└── README.md
```

## Usage

To run the sample tests:

```bash
dotnet test
```

You can add or modify tests in the `Tests/` folder, and add new page object classes in the `Pages/` folder following the POM pattern.

## Contributing

Contributions are welcome! Please open issues or submit pull requests for improvements or bug fixes.

## License

This project is licensed under the MIT License.

---

