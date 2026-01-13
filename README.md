# RPA ALM Client

A Windows Forms desktop client application for managing RPA (Robotic Process Automation) Application Lifecycle Management. This client provides a user-friendly interface for managing automation workflows, SLA tracking, virtual identities, and enhancement/user story tracking for an enterprise RPA platform.

## Features

- **Complete Entity Management** - Full CRUD operations for all 22 RPA ALM entities
- **Main Menu Navigation** - Organized access to all management forms by category
- **API Integration** - RESTful API client for backend communication with all endpoints
- **Configuration Management** - Robust configuration with validation and error handling
- **Windows Forms UI** - Modern desktop interface for comprehensive data management

### Available Management Forms

**Reference Data (11 forms):**
- Status, Complexity, Medal, Region, Segment, Function
- SLA Item Type, Automation Environment Type, AD Domain
- Enhancement, Story Point Cost

**Core Entities (9 forms):**
- Automation Management (workflows and ownership)
- SLA Master & SLA Item (service level agreements)
- Virtual Identity & VI Assignments (bot accounts and assignments)
- Enhancement User Story (Jira integration)
- Automation Environment (CMDB linkage)
- Automation Log Entry & SLA Log Entry (audit trails)

**Helper Tables (2 forms):**
- Employee Directory (JJEDS Helper)
- CMDB Helper (application cache)

## Prerequisites

- [.NET 9.0](https://dotnet.microsoft.com/download/dotnet/9.0) or later
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
│   └── AppConfig.cs              # Configuration management with validation
├── Forms/
│   ├── MainMenuForm.cs           # Main navigation menu
│   ├── *ManagementForm.cs        # 22 entity management forms (Status, Automation, etc.)
│   └── ...
├── Models/
│   ├── *Dto.cs                   # 22 entity DTOs (data transfer objects)
│   ├── *CreateRequest.cs         # 22 create request models
│   ├── *UpdateRequest.cs         # 22 update request models
│   └── ApiResponse.cs            # API response wrapper
├── Services/
│   ├── ApiClient.cs              # Generic HTTP client base class
│   ├── *ApiClient.cs             # 22 entity-specific API clients
│   └── ...
├── Program.cs                    # Application entry point
├── appsettings.json              # Configuration file
└── README.md
```

**Statistics:**
- 23 Forms (22 entity forms + 1 main menu)
- 66 Model classes (22 DTOs + 22 CreateRequests + 22 UpdateRequests)
- 22 API Client services
- Full CRUD operations for all entities

## Technologies Used

- **.NET 9.0** - Target framework
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

The client integrates with all 22 RPA ALM API endpoints with full CRUD operations:

**Reference Data Endpoints (11):**
- `/api/status`, `/api/complexity`, `/api/medal`, `/api/region`, `/api/segment`
- `/api/function`, `/api/slaitemtype`, `/api/automationenvironmenttype`
- `/api/addomain`, `/api/enhancement`, `/api/storypointcost`

**Core Entity Endpoints (9):**
- `/api/automation` - RPA automation workflows with ownership
- `/api/slamaster` - SLA agreements
- `/api/slaitem` - SLA line items
- `/api/virtualidentity` - Service accounts and bot identities
- `/api/viassignments` - Virtual identity assignments
- `/api/enhancementuserstory` - Jira integration
- `/api/automationenvironment` - CMDB application linkage
- `/api/automationlogentry` - Automation audit logs
- `/api/slalogentry` - SLA audit logs

**Helper Table Endpoints (2):**
- `/api/jjedshelper` - Employee directory cache
- `/api/cmdbhelper` - CMDB application cache

Each endpoint supports: `GET /`, `GET /{id}`, `POST /`, `PUT /{id}`, `DELETE /{id}`

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
