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
                db.Posts
                    .Where(fun p -> p.Meta.State = RecordState.Active)
                    .GroupJoin(
                        db.Comments,
                        _.Id,
                        _.PostId,
                        (fun post comments ->
                            { Id = post.Id
                              BlogId = post.BlogId
                              Title = post.Title
                              Content = post.Content
                              AuthorId = post.AuthorId
                              CreatedAt = post.Meta.CreatedAt
                              UpdatedAt = post.Meta.UpdatedAt
                              Comments = comments }
                            : DTO.Post)
                    )
                    .ToArrayAsync(ct))
        )

    }
