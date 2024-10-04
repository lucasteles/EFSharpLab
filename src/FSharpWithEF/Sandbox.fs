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

        db.Users.add user
        do! db.saveChangesAsync ()

        let! saved = db.Users.TryFirstAsync()

        printfn $"%A{saved}"
    }
