namespace FSharpWithEF.Models

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
