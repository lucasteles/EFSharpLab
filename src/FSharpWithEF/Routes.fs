module FSharpWithEF.Routes

open System.Threading
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Routing
open Microsoft.EntityFrameworkCore

let map (app: IEndpointRouteBuilder) =
    proc {
        let api = app.MapGroup "api"

        api.MapGet("/users", (fun (db: AppDbContext) (ct: CancellationToken) -> db.Users.ToArrayAsync(ct)))
    }
