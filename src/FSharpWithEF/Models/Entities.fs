namespace FSharpWithEF.Models

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
      Owner: User
      Meta: Meta }

[<CLIMutable>]
type Comment =
    { Id: CommentId
      Text: string
      Author: User
      PostId: PostId
      Meta: Meta }

[<CLIMutable>]
type Post =
    { Id: PostId
      Title: string
      Content: string
      Author: User
      BlogId: BlogId
      Comments: Comment[]
      Meta: Meta }
