# stgsharp.vss.dev

VSS architecture development repository — upstream development bed for
[`StgSharp.UserInterface`](https://github.com/Nitload-NSI/StgSharp).

## What is this repository?

`stgsharp.vss.dev` is a focused, standalone development repository for the
**VSS (View–State–Service)** architecture pattern.

Its role in the StgSharp ecosystem mirrors the relationship between Fedora and RHEL:
new VSS type primitives, interface contracts and architecture experiments are developed
and stabilised here first, then upstreamed into the main `StgSharp` repository once proven
in production.

```
stgsharp.vss.dev  ──────────────────→  Nitload-NSI/StgSharp
  (upstream dev bed)    back-flow        (main release repo)
```

## Repository layout

```
stgsharp.vss.dev/
├── VssDev.Core/            # VSS type scaffold (Intent, StateSnapshot, adapters …)
├── Directory.Build.props   # shared MSBuild properties (net8.0, LangVersion preview …)
├── Directory.Build.targets # shared post-build copy target
├── .editorconfig           # editor conventions (UTF-8, LF, CS1591 suggestion)
├── .gitattributes          # line-ending policy (LF repo-wide, CRLF for .bat/.cmd/.ps1)
├── .gitignore              # standard .NET ignore list
├── LICENSE                 # MIT
└── stgsharp.vss.dev.sln    # Visual Studio solution
```

## Upstream / downstream boundaries

| What belongs here | What belongs in StgSharp |
|---|---|
| VSS interface contracts (`IInputAdapter`, `IOutputAdapter`, `IVssState`) | Stable, versioned releases of those contracts |
| `Intent` / `StateSnapshot` base types and related utilities | Published NuGet packages (future) |
| Architecture experiments and proof-of-concept implementations | Graphics / mathematics / native interop |
| Production VSS state machines and service adapters (company project layer) | — |

**Rule**: generic improvements flow *up* to StgSharp. Business-specific code stays here.

## VSS Architecture overview

VSS separates an application into three layers with strictly one-way knowledge:

```
V  (View / I/O adapters)
      ↓ Intent           ↑ StateSnapshot
S  (State machine — sole owner of application state)
      ↓ Command          ↑ Event
S  (Service layer — devices, I/O, long-running work)
```

See [`StgSharp.UserInterface/VSS_Arch.md`](https://github.com/Nitload-NSI/StgSharp/blob/main/StgSharp.UserInterface/VSS_Arch.md)
for the full design rationale.

## Requirements

- .NET 8.0 SDK or later
- Windows 10/11 or Linux
- x64 or ARM64 architecture

## Getting started

### Clone and build

```bash
git clone https://github.com/Nitload-NSI/stgsharp.vss.dev.git vssdev
cd vssdev
dotnet build
```

### Connect a local repository named `vssdev` to the remote

If you have already initialised a local repository:

```bash
# inside your local vssdev directory
git remote add origin https://github.com/Nitload-NSI/stgsharp.vss.dev.git
git fetch origin
git checkout -b main --track origin/main
# push your first commit
git push -u origin main
```

Or start fresh:

```bash
mkdir vssdev && cd vssdev
git init -b main
git remote add origin https://github.com/Nitload-NSI/stgsharp.vss.dev.git
# copy bootstrap files, then:
git add .
git commit -m "chore: initial repository bootstrap"
git push -u origin main
```

## License

MIT — see [LICENSE](LICENSE).
