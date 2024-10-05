#r "nuget: MSBuild.StructuredLogger, 2.2.291"
#r "nuget: Fake.Core.Target, 6.1.0"
#r "nuget: Fake.DotNet.Cli, 6.1.0"
#r "nuget: Fake.IO.FileSystem, 6.1.0"
#r "nuget: Fake.Core.CommandLineParsing, 6.1.0"
#r "nuget: Fake.DotNet.Testing.Coverlet, 6.1.0"

open Fake.Core
open Fake.DotNet
open Fake.IO
open System
open System.Threading.Tasks
open Fake.Core.TargetOperators

let (/) path1 path2 = Path.combine path1 path2
let solutionDir = __SOURCE_DIRECTORY__ / ".." |> Path.getFullName

let commandArgs = Environment.GetCommandLineArgs()[2..]

let target name action = Target.create name action

[<Literal>]
let projectName = "EFSharpLab"

module Task =
    let wait (t: #Task) = t.GetAwaiter().GetResult()

let targetAsync name (action: _ -> #Task) =
    target name (fun arg -> action arg |> Task.wait)

let help = Target.description

let install = lazy DotNet.install DotNet.Versions.FromGlobalJson


let ( *=> ) deps target =
    for d in deps do
        d ==> target |> ignore

    target

let initEnvironment () =
    [ "CI", "true"
      "DOTNET_CLI_UI_LANGUAGE", "en-US"
      "DOTNET_SKIP_FIRST_TIME_EXPERIENCE", "true"
      "DOTNET_CLI_TELEMETRY_OPTOUT", "true" ]
    |> List.iter (fun e -> e ||> Environment.setEnvironVar)

    commandArgs
    |> Array.toList
    |> Context.FakeExecutionContext.Create false __SOURCE_FILE__
    |> Context.RuntimeContext.Fake
    |> Context.setExecutionContext

    help "list FAKE actions"
    target "help" <| fun _ -> Target.listAvailable ()

    DotNet.exec (fun o -> { o with RedirectOutput = true }) "tool" "restore"
    |> ignore

    Trace.traceLine ()
    Trace.log " FAKE Build - 🚀🦖 AstroRex Api"
    Trace.traceLine ()

module Arguments =
    let toString (args: Arguments) =
        if Environment.isWindows then
            args.ToWindowsCommandLine
        else
            args.ToLinuxShellCommandLine

let runFaker () =
    Target.WithContext.runOrDefaultWithArguments "help" |> Target.updateBuildStatus

let ef cmd =
    Arguments.OfArgs
        [ yield! cmd
          "--project"
          $"src/{projectName}.Migrations"
          "-s"
          $"src/{projectName}" ]
    |> Arguments.toString
    |> DotNet.exec id "ef"
    |> ignore

let fantomas args =
    let result = DotNet.exec id "fantomas" args

    if not result.OK then
        printfn $"Errors while formatting all files: %A{result.Messages}"

let updateLocalTools () =
    let result = DotNet.exec (fun o -> { o with RedirectOutput = true }) "tool" "list"

    if not result.OK then
        printfn $"Update error: %A{result.Errors}"

    result.Results
    |> List.map _.Message
    |> List.skip 2
    |> List.map ((String.splitStr " ") >> List.head)
    |> List.iter (fun tool -> DotNet.exec id "tool" $"update {tool}" |> ignore)

let preCommitFile = solutionDir / ".git" / "hooks" / "pre-commit"

let installGitHooks () =
    File.writeString
        false
        preCommitFile
        """#!/bin/sh
ECHO 'Running pre-commit for F# files'
git diff --cached --name-only --diff-filter=ACM -z -- *.fs *.fsx *.fsi | xargs -0 dotnet fantomas
git diff --cached --name-only --diff-filter=ACM -z -- *.fs *.fsx *.fsi | xargs -0 git add
"""

module Docker =

    [<Literal>]
    let hostName = "host.docker.internal"

    let cli' args =
        let docker = ProcessUtils.tryFindFileOnPath "docker"

        match docker with
        | Some d -> Shell.Exec(d, args)
        | None -> failwith "docker not found"

    let cli = cli' >> ignore

    let runPostgres imageVersion name =
        let image = $"postgres:{imageVersion}"

        cli $"rm -f {name}"
        cli $"run --name {name} -e \"POSTGRES_PASSWORD=postgres\" -p 5432:5432 -d {image}"

    let startPostgres imageVersion containerName =
        if cli' $"start {containerName}" <> 0 then
            runPostgres imageVersion containerName
