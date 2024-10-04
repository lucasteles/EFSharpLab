open System
open System.Threading.Tasks
open FSharpWithEF
open Microsoft.AspNetCore.Builder
open FSharpWithEF.Db
open Microsoft.Extensions.DependencyInjection

let configure (builder: WebApplicationBuilder) =
    let services = builder.Services

    services
        .AddSingleton(TimeProvider.System)
        .AddSingleton(Random.Shared)
        .AddSingleton(Json.jsonFsharpOptions)
        .AddDbContext<AppDbContext>(DbSetup.configureService builder "DefaultConnection")
        .Configure(fun (options: Microsoft.AspNetCore.Mvc.JsonOptions) -> Json.configure options.JsonSerializerOptions)
        .ConfigureHttpJsonOptions(fun options -> Json.configure options.SerializerOptions)
    |> ignore

let createWebServer (args: string[]) =
    let builder = WebApplication.CreateBuilder(args)
    configure builder

    let app = builder.Build()
    Routes.map app

    app

let start (app: WebApplication) =
    task {
        use scope = app.Services.CreateScope()
        let services = scope.ServiceProvider
        let db = services.GetRequiredService<AppDbContext>()

        do! db.Database.EnsureDeletedAsync() :> Task
        do! DbSetup.applyMigrations db

        do! Sandbox.run db
        return! app.RunAsync()
    }

[<EntryPoint>]
let main args =
    start(createWebServer args).GetAwaiter().GetResult()
    0
