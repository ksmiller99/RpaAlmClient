# RPA ALM Client

A Windows Forms desktop client application for managing RPA (Robotic Process Automation) Application Lifecycle Management. This client provides a user-friendly interface for managing automation workflows, SLA tracking, virtual identities, and enhancement/user story tracking for an enterprise RPA platform.

## Features

- **Status Management** - CRUD operations for automation status tracking
- **API Integration** - RESTful API client for backend communication
- **Configuration Management** - Robust configuration with validation and error handling
- **Windows Forms UI** - Modern desktop interface for data management

## Prerequisites

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Windows OS
- [RPA ALM API](https://github.com/ksmiller99/ALM) server running (backend)
- SQL Server Express (for the API backend)

## Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/ksmiller99/RpaAlmClient.git
   cd RpaAlmClient
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure the API endpoint**

   Edit `appsettings.json` to point to your RPA ALM API server:
   ```json
   {
     "ApiSettings": {
       "BaseUrl": "http://localhost:5021"
     }
   }
   ```

4. **Build the application**
   ```bash
   dotnet build
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

   Or use the startup script:
   ```bash
   StartClaudeAlmClient.bat
   ```

## Configuration

### appsettings.json

The application uses `appsettings.json` for configuration:

- **ApiSettings:BaseUrl** - The base URL of the RPA ALM API server (required)

The configuration system includes:
- File existence validation
- Required setting validation
- Clear error messages if misconfigured
- Thread-safe initialization

## Project Structure

```
RpaAlmClient/
├── Configuration/
│   └── AppConfig.cs          # Configuration management with validation
├── Forms/
│   └── StatusManagementForm.cs  # Status management UI
├── Models/
│   ├── StatusDto.cs          # Status data transfer object
│   ├── StatusCreateRequest.cs
│   ├── StatusUpdateRequest.cs
│   └── ApiResponse.cs        # API response wrapper
├── Services/
│   ├── ApiClient.cs          # Generic HTTP client
│   └── StatusApiClient.cs    # Status-specific API client
├── Program.cs                # Application entry point
├── appsettings.json          # Configuration file
└── README.md
```

## Technologies Used

- **.NET 8.0** - Target framework
- **Windows Forms** - Desktop UI framework
- **System.Text.Json** - JSON serialization
- **HttpClient** - HTTP communication
- **Microsoft.Extensions.Configuration** - Configuration management

## Database Schema

The backend uses SQL Server with the following main entities:

**Lookup Tables**: Status, SlaItemType, Enhancement, Complexity, Medal, Function, Region, Segment, AutomationEnvironmentType, ADDomain, StoryPointCost

**Helper Tables**:
- `JjedsHelper` - Employee directory cache
- `CmdbHelper` - CMDB application cache

**Main Tables**:
- `Automation` - Core automation records with ownership tracking
- `SlaMaster` - SLA agreements with complexity/medal levels
- `SlaItem` - SLA line items
- `EnhancementUserStory` - Jira integration for story tracking
- `AutomationEnvironment` - Automation-to-application mappings
- `VirtualIdentity` - Service accounts and bot identities
- `ViAssignments` - Virtual identity assignments

## API Endpoints

The client currently integrates with the following API endpoints:

- `GET /api/status` - Retrieve all statuses
- `GET /api/status/{id}` - Retrieve a specific status
- `POST /api/status` - Create a new status
- `PUT /api/status/{id}` - Update an existing status
- `DELETE /api/status/{id}` - Delete a status

## Development

### Building

```bash
dotnet build
```

### Running Tests

```bash
dotnet test
```

### Code Structure

- Follow existing patterns for adding new entity management forms
- Use the `ApiClient` base class for HTTP communication
- Extend `StatusApiClient` pattern for new entity-specific clients
- All configuration should go through `AppConfig` class

## Troubleshooting

### "Configuration file not found" error
- Ensure `appsettings.json` exists in the application directory
- Check that the file is set to copy to output directory in the `.csproj` file

### "API Base URL is not configured" error
- Verify `appsettings.json` contains the `ApiSettings:BaseUrl` setting
- Ensure the URL is not empty

### API Connection errors
- Verify the API server is running on the configured port
- Check the `BaseUrl` in `appsettings.json` matches the API server URL
- Ensure no firewall is blocking the connection

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is part of an enterprise RPA platform for internal use.

## Related Projects

- [RPA ALM API](https://github.com/ksmiller99/ALM) - Backend API server
- RPA ALM Database - SQL Server database schema and stored procedures

## Acknowledgments

Built with assistance from [Claude Code](https://claude.ai/code)
