# QuartzWebScheduler

## Overview

QuartzWebScheduler is a web-based application designed to create, manage, and log Quartz jobs. It features a user management system and supports various HTTP methods.

## Features

- **User Management**: Includes an authentication system to manage users.
- **Job Management**: Create, update, and delete jobs seamlessly.
- **Automatic Job Updates**: Jobs are automatically added, removed, or updated without requiring a restart of Quartz.
- **HTTP Request Support**: Supports GET, POST, PATCH, PUT, and DELETE requests with request bodies and Bearer authentication.
- **Logging**: Loggs each Job execution with used parameters and responses

## Installation
- Clone this repository to your local workspace.
- Open the project in Visual Studio.

## Configuration
- Create a configuration file named `appsettings.json` in the root directory of the `QuartzWebScheduler.Web` Project. The Connectionstring should be named `WebDbContextConnection`.
- It should look like this:
- ```{
  "ConnectionStrings": {
    "WebDbContextConnection": "Server=myServerAddress;Database=myDatabase;User Id=myUsername;Password=myPassword;Trusted_Connection=False;Encrypt=False;"
  },
- Open a command prompt or terminal and navigate to the root directory of the `IIS-Manager.Web` project.
- Run the following command to apply the database migrations and create the database:
- `dotnet ef database update`

## Running the Application
- Make sure you have the required dependencies installed.
- Start the application in Visual Studio.

## Contributors
We welcome contributions to improve this project. If you have suggestions or want to report issues, please open an issue or create a pull request.

## License
This project is licensed under the CC-BY-SA 4.0 License.
