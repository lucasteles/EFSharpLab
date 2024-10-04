module FSharpWithEF.Db.DbSetup

open System.IO
open System.Reflection
open System.Runtime.Loader
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting

[<Literal>]
let migrationsAssembly = "FSharpWithEF.Migrations"

let applyMigrations (db: DbContext) =
    task {
        AssemblyLoadContext.Default.add_Resolving (fun ctx asmName ->
            if asmName.Name = migrationsAssembly then
                let path = Assembly.GetExecutingAssembly().Location |> Path.GetDirectoryName
                let file = Path.Combine(path, migrationsAssembly + ".dll")
                ctx.LoadFromAssemblyPath file
            else
                null)

        do! db.Database.MigrateAsync()
    }

let configureService (builder: IHostApplicationBuilder) (name: string) (options: DbContextOptionsBuilder) =
    proc {
        let connString = builder.Configuration.GetConnectionString name

        options
            .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
            .UseNpgsql(
                connString,
                (fun opt ->
                    opt
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                        .MigrationsAssembly(migrationsAssembly)
                    |> ignore)
            )
    }
