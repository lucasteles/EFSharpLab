module FSharpWithEF.Routes

open FSharp.MinimalApi
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Routing
open Microsoft.AspNetCore.Http
open Microsoft.EntityFrameworkCore

let map (app: IEndpointRouteBuilder) =
    block {
        let api = app.MapGroup("api").WithTags("Endpoints")

        api.MapGet("/users", (fun (db: AppDbContext) (ct: CancellationToken) -> db.Users.ToArrayAsync(ct)))

        api.MapGet("/posts", (fun (db: AppDbContext) (ct: CancellationToken) -> db.Posts.ToArrayAsync(ct)))
    }
