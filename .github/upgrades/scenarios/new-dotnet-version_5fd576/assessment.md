# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [Api\Api.csproj](#apiapicsproj)
  - [Application\Application.csproj](#applicationapplicationcsproj)
  - [Domain\Domain.csproj](#domaindomaincsproj)
  - [EfCore\Database.EfCore.csproj](#efcoredatabaseefcorecsproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 4 | All require upgrade |
| Total NuGet Packages | 5 | 3 need upgrade |
| Total Code Files | 29 |  |
| Total Code Files with Incidents | 4 |  |
| Total Lines of Code | 1516 |  |
| Total Number of Issues | 9 |  |
| Estimated LOC to modify | 0+ | at least 0,0% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :--- |
| [Api\Api.csproj](#apiapicsproj) | net9.0 | 🟢 Low | 2 | 0 |  | AspNetCore, Sdk Style = True |
| [Application\Application.csproj](#applicationapplicationcsproj) | net9.0 | 🟢 Low | 1 | 0 |  | ClassLibrary, Sdk Style = True |
| [Domain\Domain.csproj](#domaindomaincsproj) | net9.0 | 🟢 Low | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [EfCore\Database.EfCore.csproj](#efcoredatabaseefcorecsproj) | net9.0 | 🟢 Low | 2 | 0 |  | ClassLibrary, Sdk Style = True |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 2 | 40,0% |
| ⚠️ Incompatible | 0 | 0,0% |
| 🔄 Upgrade Recommended | 3 | 60,0% |
| ***Total NuGet Packages*** | ***5*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 1570 |  |
| ***Total APIs Analyzed*** | ***1570*** |  |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |
| Microsoft.EntityFrameworkCore | 9.0.7 | 10.0.3 | [Api.csproj](#apiapicsproj)<br/>[Application.csproj](#applicationapplicationcsproj)<br/>[Database.EfCore.csproj](#efcoredatabaseefcorecsproj) | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Design | 9.0.7 | 10.0.3 | [Database.EfCore.csproj](#efcoredatabaseefcorecsproj) | NuGet package upgrade is recommended |
| Microsoft.EntityFrameworkCore.Tools | 9.0.7 | 10.0.3 | [Api.csproj](#apiapicsproj) | NuGet package upgrade is recommended |
| Npgsql.EntityFrameworkCore.PostgreSQL | 9.0.4 |  | [Database.EfCore.csproj](#efcoredatabaseefcorecsproj) | ✅Compatible |
| Swashbuckle.AspNetCore | 9.0.3 |  | [Api.csproj](#apiapicsproj) | ✅Compatible |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>📦&nbsp;Api.csproj</b><br/><small>net9.0</small>"]
    P2["<b>📦&nbsp;Application.csproj</b><br/><small>net9.0</small>"]
    P3["<b>📦&nbsp;Database.EfCore.csproj</b><br/><small>net9.0</small>"]
    P4["<b>📦&nbsp;Domain.csproj</b><br/><small>net9.0</small>"]
    P1 --> P3
    P1 --> P2
    P2 --> P4
    P3 --> P4
    P3 --> P2
    click P1 "#apiapicsproj"
    click P2 "#applicationapplicationcsproj"
    click P3 "#efcoredatabaseefcorecsproj"
    click P4 "#domaindomaincsproj"

```

## Project Details

<a id="apiapicsproj"></a>
### Api\Api.csproj

#### Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** AspNetCore
- **Dependencies**: 2
- **Dependants**: 0
- **Number of Files**: 5
- **Number of Files with Incidents**: 1
- **Lines of Code**: 140
- **Estimated LOC to modify**: 0+ (at least 0,0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["Api.csproj"]
        MAIN["<b>📦&nbsp;Api.csproj</b><br/><small>net9.0</small>"]
        click MAIN "#apiapicsproj"
    end
    subgraph downstream["Dependencies (2"]
        P3["<b>📦&nbsp;Database.EfCore.csproj</b><br/><small>net9.0</small>"]
        P2["<b>📦&nbsp;Application.csproj</b><br/><small>net9.0</small>"]
        click P3 "#efcoredatabaseefcorecsproj"
        click P2 "#applicationapplicationcsproj"
    end
    MAIN --> P3
    MAIN --> P2

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 144 |  |
| ***Total APIs Analyzed*** | ***144*** |  |

<a id="applicationapplicationcsproj"></a>
### Application\Application.csproj

#### Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 1
- **Dependants**: 2
- **Number of Files**: 8
- **Number of Files with Incidents**: 1
- **Lines of Code**: 143
- **Estimated LOC to modify**: 0+ (at least 0,0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (2)"]
        P1["<b>📦&nbsp;Api.csproj</b><br/><small>net9.0</small>"]
        P3["<b>📦&nbsp;Database.EfCore.csproj</b><br/><small>net9.0</small>"]
        click P1 "#apiapicsproj"
        click P3 "#efcoredatabaseefcorecsproj"
    end
    subgraph current["Application.csproj"]
        MAIN["<b>📦&nbsp;Application.csproj</b><br/><small>net9.0</small>"]
        click MAIN "#applicationapplicationcsproj"
    end
    subgraph downstream["Dependencies (1"]
        P4["<b>📦&nbsp;Domain.csproj</b><br/><small>net9.0</small>"]
        click P4 "#domaindomaincsproj"
    end
    P1 --> MAIN
    P3 --> MAIN
    MAIN --> P4

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 154 |  |
| ***Total APIs Analyzed*** | ***154*** |  |

<a id="domaindomaincsproj"></a>
### Domain\Domain.csproj

#### Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 0
- **Dependants**: 2
- **Number of Files**: 6
- **Number of Files with Incidents**: 1
- **Lines of Code**: 108
- **Estimated LOC to modify**: 0+ (at least 0,0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (2)"]
        P2["<b>📦&nbsp;Application.csproj</b><br/><small>net9.0</small>"]
        P3["<b>📦&nbsp;Database.EfCore.csproj</b><br/><small>net9.0</small>"]
        click P2 "#applicationapplicationcsproj"
        click P3 "#efcoredatabaseefcorecsproj"
    end
    subgraph current["Domain.csproj"]
        MAIN["<b>📦&nbsp;Domain.csproj</b><br/><small>net9.0</small>"]
        click MAIN "#domaindomaincsproj"
    end
    P2 --> MAIN
    P3 --> MAIN

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 116 |  |
| ***Total APIs Analyzed*** | ***116*** |  |

<a id="efcoredatabaseefcorecsproj"></a>
### EfCore\Database.EfCore.csproj

#### Project Info

- **Current Target Framework:** net9.0
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 2
- **Dependants**: 1
- **Number of Files**: 12
- **Number of Files with Incidents**: 1
- **Lines of Code**: 1125
- **Estimated LOC to modify**: 0+ (at least 0,0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P1["<b>📦&nbsp;Api.csproj</b><br/><small>net9.0</small>"]
        click P1 "#apiapicsproj"
    end
    subgraph current["Database.EfCore.csproj"]
        MAIN["<b>📦&nbsp;Database.EfCore.csproj</b><br/><small>net9.0</small>"]
        click MAIN "#efcoredatabaseefcorecsproj"
    end
    subgraph downstream["Dependencies (2"]
        P4["<b>📦&nbsp;Domain.csproj</b><br/><small>net9.0</small>"]
        P2["<b>📦&nbsp;Application.csproj</b><br/><small>net9.0</small>"]
        click P4 "#domaindomaincsproj"
        click P2 "#applicationapplicationcsproj"
    end
    P1 --> MAIN
    MAIN --> P4
    MAIN --> P2

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 1156 |  |
| ***Total APIs Analyzed*** | ***1156*** |  |

