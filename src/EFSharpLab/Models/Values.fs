namespace EFSharpLab.Models

open System

type BlogId = BlogId of Guid
type PostId = PostId of Guid
type UserId = UserId of Guid
type CommentId = CommentId of Guid

type Gender =
    | Male
    | Female

[<RequireQualifiedAccess>]
type RecordState =
    | Active
    | Inactive

[<CLIMutable>]
type Meta =
    { CreatedAt: DateTime
      UpdatedAt: DateTime
      State: RecordState }

module Meta =
    let now () =
        { CreatedAt = DateTime.UtcNow
          UpdatedAt = DateTime.UtcNow
          State = RecordState.Active }
