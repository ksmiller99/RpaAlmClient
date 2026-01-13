# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is an RPA (Robotic Process Automation) Application Lifecycle Management client application. It provides a Windows Forms desktop interface for managing automation workflows, SLA tracking, virtual identities, and enhancement/user story tracking for an enterprise RPA platform.

**Architecture**: Multi-project monorepo with 4 .NET 9.0 projects:
- **RpaAlm.Shared.Models** - Data models (67 classes: DTOs, requests, responses)
- **RpaAlm.Shared.ApiClient** - HTTP API clients (23 classes)
- **RpaAlm.Shared.Configuration** - Configuration management
- **RpaAlm.Client.WinForms** - Windows Forms application (23 forms)

## Project Structure

```
src/
├── RpaAlm.Shared/
│   ├── RpaAlm.Shared.Models/           # Models in DTOs/, Requests/, Responses/
│   ├── RpaAlm.Shared.ApiClient/        # API clients in Core/, Clients/
│   └── RpaAlm.Shared.Configuration/    # AppConfig.cs
└── RpaAlm.Client.WinForms/             # Program.cs, Forms/, appsettings.json
```

**Key File Locations:**
- Models: `src/RpaAlm.Shared/RpaAlm.Shared.Models/DTOs/*.cs`
- API Clients: `src/RpaAlm.Shared/RpaAlm.Shared.ApiClient/Clients/*ApiClient.cs`
- Forms: `src/RpaAlm.Client.WinForms/Forms/*ManagementForm.cs`
- Config: `src/RpaAlm.Client.WinForms/appsettings.json`

## Database

- **Database Name**: RpaDataDev
- **Platform**: SQL Server Express
- **Connection**: `localhost\SQLEXPRESS01` with Windows Authentication (see `SQLEXP_Connection.txt`)

### Schema Structure

The database follows a normalized design with three table categories:

**Lookup/Reference Tables**: SlaItemType, Enhancement, Complexity, Medal, Function, Region, Segment, Status, AutomationEnvironmentType, ADDomain, StoryPointCost

**Helper Tables** (external data caches):
- `JjedsHelper` - Employee directory cache (keyed by WWID)
- `CmdbHelper` - CMDB application cache (keyed by AppID)

**Main Entity Tables**:
- `Automation` - Core automation records with ownership (BTO, BO, FC, SSE, LSE roles via WWID references)
- `SlaMaster` - SLA agreements linked to automations with complexity/medal levels
- `SlaItem` - Line items within SLA agreements
- `EnhancementUserStory` - Jira integration for tracking story points and costs
- `AutomationEnvironment` - Links automations to CMDB applications
- `VirtualIdentity` - Service accounts and bot identities
- `ViAssignments` - Junction table assigning virtual identities to environments
- `AutomationLogEntry` / `SlaLogEntry` - Audit logging

### Key Relationships

- Automations have multiple owners (BtoWWID, BoWWID, FcWWID, SseWWID, LseWWID) all referencing JjedsHelper
- SLA hierarchy: Automation → SlaMaster → SlaItem
- Virtual identities are assigned to automation environments with percentage allocations

## Commands

```bash
# Build entire solution
dotnet build RpaAlmClient.sln

# Run the Windows Forms application
dotnet run --project src/RpaAlm.Client.WinForms/RpaAlm.Client.WinForms.csproj

# Build a specific project
dotnet build src/RpaAlm.Shared/RpaAlm.Shared.Models/RpaAlm.Shared.Models.csproj
```

## Rules

1. First think through the problem, read the codebase for relevant files, and write a plan to tasks/todo.md
2. The plan should have a list of todo items that you can check off as you complete them
3. Before you begin working, check in with me and I will verify the plan
4. Then, begin working on the todo items, marking them as complete as you go
5. Every step of the way just give a high level explanation of what changes were made
6. Make every task and code change as simple as possible - avoid massive or complex changes, impact as little code as possible, everything is about simplicity
7. Finally, add a review section to the tasks/todo.md file with a summary of the changes made and any other relevant information
