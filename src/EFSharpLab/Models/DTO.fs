module EFSharpLab.Models.DTO

open System

type Post =
    { Id: PostId
      BlogId: BlogId
      Title: string
      Content: string
      AuthorId: UserId
      AuthorName: UserId
      CreatedAt: DateTime
      UpdatedAt: DateTime
      Comments: Comment list }
