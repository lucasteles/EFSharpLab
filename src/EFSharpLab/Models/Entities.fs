namespace EFSharpLab.Models

open System
open System.Collections.Generic

[<CLIMutable>]
type User =
    { Id: UserId
      FirstName: string
      LastName: string option
      Gender: Gender
      Meta: Meta }

[<CLIMutable>]
type Blog =
    { Id: BlogId
      Title: string
      OwnerId: UserId
      Url: Uri
      Rating: decimal
      Meta: Meta }

[<CLIMutable>]
type Comment =
    { Id: CommentId
      Text: string
      AuthorId: UserId
      PostId: PostId
      Meta: Meta }

[<CLIMutable>]
type Post =
    { Id: PostId
      Title: string
      Content: string
      AuthorId: UserId
      BlogId: BlogId
      Comments: IReadOnlyCollection<Comment>
      Meta: Meta }
