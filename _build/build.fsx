#load "./helpers.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Helpers
open Fake.Core.TargetOperators

initEnvironment ()

help "Clean in every project directory"

target "clean" (fun _ -> !! "**/bin" ++ "**/obj" ++ "artifacts/" |> Shell.cleanDirs)

help "Run dotnet build in every project"

target "build" (fun _ ->
    solutionDir
    |> DotNet.build (fun c ->
        { c with
            Configuration = DotNet.BuildConfiguration.Release }))

help "Run dotnet restore in every project"

target "restore" (fun _ -> DotNet.restore id |> ignore)

help "Update all local tools"
target "update-tools" (fun _ -> updateLocalTools ())

help "Start Postgres in Docker if exists, create new otherwise"

target "db" (fun _ -> Docker.startPostgres "16-alpine" "postgres-sql")

help "Delete local database"

target "delete-db" (fun _ -> ef [ "database"; "drop"; "--force" ])

help "Apply migrations"
target "migrate" (fun _ -> ef [ "database"; "update" ])

help "Apply migrations for connection string"

target "migrate-connection" (fun args ->
    match args.Context.Arguments |> Seq.tryHead with
    | Some conn -> ef [ "database"; "update"; "--connection"; conn ]
    | None -> failwith "Connection string not defined")

help "recreate local database"
target "recreate-db" (fun _ -> Trace.logfn "Recriando banco de dados...")

help "remove last migration"
target "remove-migration" (fun _ -> ef [ "migrations"; "remove"; "--force" ])

help "add new migration"

target "add-migration" (fun args ->
    match args.Context.Arguments |> Seq.tryHead with
    | Some name -> ef [ "migrations"; "add"; name ]
    | None -> failwith "Migration name not defined")

help "install git hook for formating"
target "pre-commit" (fun _ -> installGitHooks ())

target "setup" (fun _ -> Trace.logfn "Env Setup...")

target "lint" (fun _ -> fantomas "--check .")

target "format" (fun _ -> fantomas ".")

"clean" ==> "restore" ==> "build"
"restore" ==> "lint"
"restore" ==> "format"
"build" ==> "add-migration"
"delete-db" ?=> "migrate"
"db" ?=> "migrate"
[ "delete-db"; "migrate" ] *=> "recreate-db"
[ "db"; "pre-commit"; "migrate" ] *=> "setup"

runFaker ()
