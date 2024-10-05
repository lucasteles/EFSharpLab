open System
open System.ComponentModel
open System.Threading.Tasks
open FSharp.SystemTextJson.Swagger
open EFSharpLab
open EFSharpLab.Models.Settings
open Microsoft.AspNetCore.Builder
open EFSharpLab.Db
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open FSharp.MinimalApi
open Microsoft.Extensions.Options

let configure (builder: WebApplicationBuilder) =
    block {
        let services = builder.Services

        builder.Services
            .AddOptions<MyCustomSettings>()
            .BindConfiguration("MyCustomSettings")
            .ValidateOnStart()

        services
            .AddSingleton(TimeProvider.System)
            .AddSingleton(Random.Shared)
            .AddTuples()
            .AddEndpointsApiExplorer()
            .AddSwaggerForSystemTextJson(Json.jsonFsharpOptions)
            .AddDbContext<AppDbContext>(DbSetup.configureService builder "DefaultConnection")
            .Configure(fun (options: Microsoft.AspNetCore.Mvc.JsonOptions) ->
                Json.configure options.JsonSerializerOptions)
            .ConfigureHttpJsonOptions(fun options -> Json.configure options.SerializerOptions)

        services.AddHealthChecks().AddDbContextCheck<AppDbContext>()
    }

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
    block {
        app.UseSwagger().UseSwaggerUI()

        app.MapHealthChecks("/health")

        app
            .MapGet("/version", (fun (options: IOptions<MyCustomSettings>) -> options.Value))
            .AllowAnonymous()
            .ExcludeFromDescription()

        Routes.map app
    }

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
