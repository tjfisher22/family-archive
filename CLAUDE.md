# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build FamilyArchive.sln

# Run tests
dotnet test FamilyArchive.sln

# Run a single test project
dotnet test backend/test/FamilyArchive.UnitTests/FamilyArchive.UnitTests.csproj

# Run a single test by name
dotnet test backend/test/FamilyArchive.UnitTests/FamilyArchive.UnitTests.csproj --filter "FullyQualifiedName~TestMethodName"

# Run the API
dotnet run --project backend/src/FamilyArchive.Api/

# Add EF Core migration
dotnet ef migrations add MigrationName --project backend/src/FamilyArchive.Infrastructure --startup-project backend/src/FamilyArchive.Api

# Apply migrations
dotnet ef database update --project backend/src/FamilyArchive.Infrastructure --startup-project backend/src/FamilyArchive.Api
```

## Architecture

Clean Architecture with four layers:

- **Domain** (`FamilyArchive.Domain`) — Entities, enums, and `RelationshipCalculator`. No external dependencies.
- **Application** (`FamilyArchive.Application`) — Services, DTOs, and `IMemberRepository` interface. Orchestrates domain logic.
- **Infrastructure** (`FamilyArchive.Infrastructure`) — EF Core `FamilyArchiveDbContext`, `MemberRepository`, and migrations.
- **Api** (`FamilyArchive.Api`) — ASP.NET Core controllers. Base route: `/api/members`.

## Domain Model

Core entities and their relationships:

- **Member** — Central entity. Has collections of `MemberName`, parent/child `MemberRelationship`s, `MemberPartnership`s, and optional `Family`/`Clan` references.
- **MemberName** — A name record with a `NameType` enum (FirstName, MiddleName, LastName, MaidenName, ChosenName, NickName, DeadName, LegalName, Other) and display order.
- **MemberRelationship** — Parent-child link with `RelationshipType` (BiologicalMother, BiologicalFather, StepMother, StepFather, AdoptiveMother, AdoptiveFather, etc.).
- **MemberPartnership** — Partnership between two members with `PartnershipType` (Marriage, CivilUnion, DomesticPartnership, etc.).
- **Family** — Groups children of specific parents.
- **Clan** — Extended family grouping spanning generations.
- **RelationshipCalculator** — BFS-based algorithm in the Domain layer that calculates descriptive relationship strings (e.g., "first cousin once removed") between any two members.

## Database

PostgreSQL via EF Core 9. Connection string is in `appsettings.json`; the password is injected via `${PGPASSWORD}` environment variable. User secrets are also supported (`UserSecretsId: a5c4e389-585d-4953-a957-25f76609ca79`).

Key EF Core configuration notes (in `FamilyArchiveDbContext.OnModelCreating`):
- `Names` collection on `Member` uses a backing field.
- Cascade delete on parent-child relationships; restrict delete on partnerships; set-null on Family/Clan assignments.
