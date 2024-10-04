module FSharpWithEF.Sandbox

open FSharpWithEF.Models

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
              Owner = user
              Meta = Meta.now () }

        let post =
            { Id = Id.next PostId
              Title = "How to create a good post"
              BlogId = blog.Id
              Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry."
              Author = user
              Meta = Meta.now ()
              Comments = [||] }

        db.Users.addRange [ user; user2 ]
        db.Blogs.add blog
        db.Posts.add post

        do! db.saveChangesAsync ()

        let! saved = db.Users.TryFirstAsync()
        printfn $"%A{saved}"
    }
