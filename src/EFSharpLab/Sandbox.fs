module EFSharpLab.Sandbox

open System
open System.Collections.Generic
open System.Collections.ObjectModel
open EFSharpLab.Models

let run (db: AppDbContext) =
    task {

        let user =
            { Id = Id.next UserId
              FirstName = "John"
              LastName = None
              Gender = Gender.Male
              Meta = Meta.now () }

        let user2 =
            { Id = Id.next UserId
              FirstName = "Maria"
              LastName = Some "Silva"
              Gender = Gender.Female
              Meta = Meta.now () }

        let blog =
            { Id = Id.next BlogId
              Title = "Super Curious"
              OwnerId = user.Id
              Rating = 0m
              Url = Uri("https://super-curious.fakeblog")
              Meta = Meta.now () }

        let post =
            { Id = Id.next PostId
              Title = "How to create a good post"
              BlogId = blog.Id
              Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry."
              AuthorId = user.Id
              Meta = Meta.now () }

        let comments =
            [ { Id = Id.next CommentId
                Text = "Nice post!"
                AuthorId = user2.Id
                PostId = post.Id
                Meta = Meta.now () }

              { Id = Id.next CommentId
                Text = "Thank you!"
                AuthorId = user.Id
                PostId = post.Id
                Meta = Meta.now () } ]

        db.Users.addRange [ user; user2 ]
        db.Blogs.add blog
        db.Posts.add post
        db.Comments.addRange comments

        do! db.saveChangesAsync ()

        let! saved = db.Users.TryFirstAsync()
        printfn $"%A{saved}"
    }
