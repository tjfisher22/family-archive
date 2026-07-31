# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

FamilyArchive: a .NET 9 backend (ASP.NET Core Web API + EF Core/PostgreSQL) for modeling family trees — members, their names, parent/child relationships, partnerships, and computing the closest natural-language relationship between any two members (e.g. "Second Cousin Once Removed"). There is no frontend yet (see ToDo list in the project's Trello board). Currently no README exists in the repo.

## Commands

All commands are run from `backend/` (the `.sln` lives at repo root, so `dotnet` commands also work from repo root if you pass the solution/project explicitly).

```bash
# Build
dotnet build FamilyArchive.sln          # from repo root
cd backend && dotnet build               # or from backend/

# Run the API (Swagger UI at /swagger in Development)
cd backend/src/FamilyArchive.Api && dotnet run

# Run all tests
cd backend/test/FamilyArchive.UnitTests && dotnet test

# Run a single test (xUnit filter by fully qualified name or method name)
dotnet test --filter "FullyQualifiedName~RelationshipCalculatorTests.CalculateClosestRelationship_DirectChild_ReturnsChild"
dotnet test --filter "DisplayName~AreSiblings"

# EF Core migrations (run from FamilyArchive.Infrastructure so the design-time factory resolves appsettings from ../FamilyArchive.Api)
cd backend/src/FamilyArchive.Infrastructure
dotnet ef migrations add <Name> --startup-project ../FamilyArchive.Api
dotnet ef database update --startup-project ../FamilyArchive.Api
```

The connection string (`appsettings.json`) expects a local Postgres at `localhost:5432`, database `FamilyArchiveDb`, with the password read from a `PGPASSWORD` value (user secrets in Development, env var otherwise) — see `FamilyArchiveDbContextFactory`.

## Architecture

Standard four-layer/Clean Architecture split, each a separate project referencing only the layers below it:

- **FamilyArchive.Domain** — no dependencies on other layers. Entities (`Member`, `MemberName`, `MemberRelationship`, `MemberPartnership`, `Family`, `Clan`) and domain services (`RelationshipCalculator`, `RelationshipPath`, `RelationshipConnection`). Business rules and invariants live on the entities themselves (e.g. `Member.AddChild`/`AddPartner`/name-ordering methods throw `InvalidOperationException` on invariant violations like circular parent-child links or duplicate name order) — not in the application layer.
- **FamilyArchive.Application** — DTOs and application services (`MemberService`, `MemberNameService`, `MemberRelationshipService`, `RelationshipService`) that orchestrate domain entities via repository interfaces (`IMemberRepository`, defined here, implemented in Infrastructure). Services map to/from DTOs and call `SaveChanges()` explicitly — the API controllers, not the services, decide when to commit (each controller action calls the service method, then `SaveChanges()`).
- **FamilyArchive.Infrastructure** — EF Core (`FamilyArchiveDbContext`, Npgsql), `MemberRepository`, migrations, and `FamilyArchiveDbContextFactory` (design-time factory for `dotnet ef`, reads config from the API project's appsettings + user secrets).
- **FamilyArchive.Api** — ASP.NET Core controllers (`MembersController`, `ImportController` — the latter currently stubbed out/commented), request DTOs, DI wiring and Swagger setup in `Program.cs`.

### Relationship modeling

Only two structural relationship types are stored directly: `MemberRelationship` (vertical, parent↔child, typed via `RelationshipType` enum e.g. BiologicalMother/AdoptiveFather) and `MemberPartnership` (member↔partner, typed via `PartnershipType`). Everything else — siblings, grandparents, aunts/uncles, cousins of arbitrary degree/removal — is *derived* at query time by `RelationshipCalculator`:

1. Check "close" relationships directly (self, partner, direct parent/child, siblings via shared parent).
2. Otherwise, BFS up the ancestor tree from both members independently, find common ancestors, and pick the shortest combined path (`FindAllRelationshipPaths`/`GetAncestorsBfs`/`ReconstructPath`).
3. Classify the path shape into grandparent/grandchild, aunt/uncle/niece/nephew, or Nth-cousin-M-times-removed, with gender-aware terms via `GetGenderSpecificTerm` (falls back to neutral terms like "Pibling"/"Nibling"/"Sibling" when gender is unknown/other).

When touching this logic, `CalculateAllRelationships` and `CalculateClosestRelationship` must stay consistent (same "close relationship" short-circuit, same path-sorting order).

Grouping concepts: `Family` = children of a specific set of parents; `Clan` = broader multi-generational grouping. Both are simple `Member` groupings via `FamilyId`/`ClanId`, unrelated to the relationship-calculation logic above.

### EF Core notes

- `Member.Names` is a private-backed `IReadOnlyCollection` exposed via a backing field (`UsePropertyAccessMode(PropertyAccessMode.Field)`) — mutate names only through `Member`'s own methods (`AddName`, `RemoveName`, `UpdateNameOrder`, etc.), never by manipulating the collection directly, or EF's change tracking and the class's invariants (unique order, `OtherNameType` enforcement) will get out of sync.
- `MemberRelationship`/`MemberPartnership` use `Cascade` delete on the "owning" side (child/member) and `Restrict` on the other side (parent/partner) to avoid multiple cascade paths to the same `Member` row.

## Project tracking

Work is tracked on a private Trello board ("Family App"), not in this repo — check with the user for current priorities rather than assuming from git history alone.
