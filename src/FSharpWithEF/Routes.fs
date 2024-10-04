module FSharpWithEF.Routes

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Routing

let map (app: IEndpointRouteBuilder) =
    proc {
        let api = app.MapGroup "api"

        api.MapGet("/", (fun () -> "Hello World!"))

    }
