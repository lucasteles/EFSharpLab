module EFSharpLab.Routes

open FSharp.MinimalApi
open EFSharpLab.Models.Settings
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Routing
open Microsoft.AspNetCore.Http
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.Options

let map (app: IEndpointRouteBuilder) =
    block {
        let api = app.MapGroup("api").WithTags("Endpoints")

        api.MapGet("/users", (fun (db: AppDbContext) (ct: CancellationToken) -> db.Users.ToArrayAsync(ct)))

        api.MapGet("/blogs", (fun (db: AppDbContext) (ct: CancellationToken) -> db.Blogs.ToArrayAsync(ct)))

        api.MapGet("/posts", (fun (db: AppDbContext) (ct: CancellationToken) -> db.Posts.ToArrayAsync(ct)))

    }
