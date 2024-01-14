# CD backup splitter

The Folder Splitter Utility is a .NET tool designed to split a large folder and its subfolders into CD or DVD-sized chunks. This utility is handy for creating backups that fit on standard optical storage media.

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Prerequisites](#prerequisites)
4. [Installation](#installation)
5. [Usage](#usage)
6. [Configuration](#configuration)
7. [Contributing](#contributing)
8. [License](#license)

## Introduction

Backing up large folders with diverse content can be challenging. This utility simplifies the process by breaking down the folder into manageable CD or DVD-sized chunks. This approach ensures that your backup fits on standard optical storage media.

## Features

- **Folder Splitting:** Divides a large folder and its subfolders into CD or DVD-sized chunks.
- **Optical Media Compatibility:** Ensures that each chunk fits on standard CD or DVD storage media.
- **Intuitive Command-Line Interface:** Simple and straightforward usage with command-line arguments.

## Prerequisites

- **.NET SDK:** Ensure that you have the .NET SDK installed on your machine. If not, you can download it [here](https://dotnet.microsoft.com/download).

## Installation

1. **Clone the repository:**

    ```bash
    git clone https://github.com/yourusername/folder-splitter-utility-dotnet.git
    ```

2. **Navigate to the project directory:**

    ```bash
    cd folder-splitter-utility-dotnet
    ```

3. **Build the Solution:**

    ```bash
    dotnet build
    ```

## Usage

1. **Run the Utility:**

    ```bash
    dotnet run --source /path/to/large/folder --destination /path/to/output/folder --chunkSize 700
    ```

    - Replace `/path/to/large/folder` with the path to the folder you want to split.
    - Replace `/path/to/output/folder` with the desired output folder for the chunks.
    - Adjust the `--chunkSize` argument based on the size of your CD or DVD storage media (in megabytes).

2. **Review Results:**

    The utility will create CD or DVD-sized chunks in the specified output folder.

## Configuration

The utility uses command-line arguments for configuration. The key arguments are:
   - `--source`: The path to the large folder to be split.
   - `--destination`: The path to the output folder for the chunks.
   - `--chunkSize`: The size of each chunk in megabytes.

## Contributing

Contributions are welcome! If you have additional features, improvements, or bug fixes, feel free to open an issue or submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE). See the LICENSE file for details.

Happy splitting and backing up!
