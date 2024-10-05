module EFSharpLab.Routes

open EFSharpLab.Models
open FSharp.MinimalApi
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Routing
open Microsoft.AspNetCore.Http
open Microsoft.EntityFrameworkCore
open System.Linq

let map (app: IEndpointRouteBuilder) =
    block {
        let api = app.MapGroup("api").WithTags("Endpoints")

        api.MapGet("/users", (fun (db: AppDbContext) (ct: CancellationToken) -> db.Users.ToArrayAsync(ct)))

        api.MapGet("/blogs", (fun (db: AppDbContext) (ct: CancellationToken) -> db.Blogs.ToArrayAsync(ct)))

        api.MapGet(
            "/posts",
            (fun (db: AppDbContext) (ct: CancellationToken) ->
                let query =
                    db.Posts
                        .Where(fun p -> p.Meta.State = RecordState.Active)
                        .Join(db.Users, _.AuthorId, _.Id, (fun post author -> {| Post = post; Author = author |}))
                        .GroupJoin(
                            db.Comments,
                            _.Post.Id,
                            _.PostId,
                            (fun r comments -> r
                            // {| Post = r.Post
                            //    Author = r.Author
                            //    Comments = comments |}
                            )
                        )

                query.ToArrayAsync(ct))
        )

    }
