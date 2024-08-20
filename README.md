# Batch File Executor ASP.NET Application

## Overview

This ASP.NET application is designed to execute a batch file on the server and stream its output to the client in real-time. The application is particularly useful for scenarios where you need to run server-side scripts and provide immediate feedback to the user via a web interface.

## Features

- Executes a specified batch file upon page load.
- Streams real-time output from the batch file to the client.
- Displays the current user running the application.
- Ensures secure handling of SSH keys for remote commands.

## Prerequisites

- **.NET Framework 4.7** or higher.
- **IIS** with an Application Pool configured to run the application.

## Installation

1. **Clone the repository**:
    ```bash
    git clone https://github.com/BatchFileCaller/BatchFileCaller.git
    ```

2. **Open the solution**:
    - Open the project in Visual Studio.

3. **Configure the batch file**:
    - Edit the `RunBatchFile` method in `ExecuteBatchFile.aspx.cs` to specify the path to the batch file you wish to execute:
    ```csharp
    FileName = @"C:\scripts\YourBatchFile.bat",
    ```

4. **Set up SSH Keys**:
    - Ensure that the `.ssh` directory on your server is configured correctly with the appropriate permissions. The `id_rsa` private key should be secured and only accessible by the `IIS APPPOOL\WebTrigger` user.

5. **Deploy the application**:
    - Deploy the application to your IIS server. Ensure the Application Pool is running under the `IIS APPPOOL\WebTrigger` identity or an appropriate user with access to the batch file and SSH keys.

6. **Test the application**:
    - Navigate to the deployed URL to test the batch file execution and output streaming.

## Usage

- **Access the Application**: Navigate to the deployed URL in your browser. The application will automatically execute the specified batch file and display the output in real-time.
  
- **Output Display**: The output from the batch file will be displayed in a reverse chronological order, with the most recent output appearing at the top of the page.

## Security Considerations

- Ensure that your SSH keys are securely stored and that permissions are correctly set to avoid unauthorized access.
- The application is designed to run in a secure environment, and it's crucial to configure permissions carefully, especially when dealing with SSH keys and server-side scripts.

## Troubleshooting

- **Permission Issues**: If you encounter "bad permissions" errors related to SSH keys, ensure that only the `IIS APPPOOL\WebTrigger` user has access to the private key files. Use the `icacls` command to adjust permissions as needed. It is likely you will need to create a .ssh file in the C:\Windows directory
- **Batch File Errors**: Ensure the batch file path is correct and that the server has the necessary permissions to execute it.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue to discuss improvements or bugs.

## Contact

For questions or support, please contact [Daniel L. Van Den Bosch](mailto:dvandenbosch@kamplastics.com).
