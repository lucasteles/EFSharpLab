open System
open System.ComponentModel
open System.Threading.Tasks
open FSharp.SystemTextJson.Swagger
open FSharpWithEF
open Microsoft.AspNetCore.Builder
open FSharpWithEF.Db
open Microsoft.Extensions.DependencyInjection
open FSharp.MinimalApi

let configure (builder: WebApplicationBuilder) =
    let services = builder.Services

    services
        .AddSingleton(TimeProvider.System)
        .AddSingleton(Random.Shared)
        .AddTuples()
        .AddEndpointsApiExplorer()
        .AddSwaggerForSystemTextJson(Json.jsonFsharpOptions)
        .AddDbContext<AppDbContext>(DbSetup.configureService builder "DefaultConnection")
        .Configure(fun (options: Microsoft.AspNetCore.Mvc.JsonOptions) -> Json.configure options.JsonSerializerOptions)
        .ConfigureHttpJsonOptions(fun options -> Json.configure options.SerializerOptions)
    |> ignore

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

let configureApp (app: WebApplication) =
    Routes.map app

    app.UseSwagger().UseSwaggerUI() |> ignore

[<EntryPoint>]
let main args =
    // basic support for option, single union and fieldless unions (for appsettings)
    TypeDescriptor.addUnionTypesInAssemblyContaining<AppDbContext>
    TypeDescriptor.addDefaultOptionTypes ()

    let builder = WebApplication.CreateBuilder(args)
    configure builder

    let app = builder.Build()
    configureApp app

    start(app).GetAwaiter().GetResult()
    0
