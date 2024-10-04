open System
open FSharpWithEF
open Microsoft.AspNetCore.Builder
open FSharpWithEF.Db
open Microsoft.Extensions.DependencyInjection

let configure (builder: WebApplicationBuilder) =
    proc {
        let services = builder.Services

        services
            .AddSingleton(TimeProvider.System)
            .AddSingleton(Random.Shared)
            .AddSingleton(Json.jsonFsharpOptions)

        services
            .AddDbContext<AppDbContext>(DbSetup.configureService builder "DefaultConnection")
            .Configure(fun (options: Microsoft.AspNetCore.Mvc.JsonOptions) ->
                Json.configure options.JsonSerializerOptions)
            .ConfigureHttpJsonOptions(fun options -> Json.configure options.SerializerOptions)
    }

let createWebServer (args: string[]) =
    let builder = WebApplication.CreateBuilder(args)
    configure builder

    let app = builder.Build()
    Routes.map app

    app

[<EntryPoint>]
let main args =
    let app = createWebServer args
    app.RunAsync().GetAwaiter().GetResult()
    0
