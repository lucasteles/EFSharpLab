namespace FSharpWithEF.Db.Config.Converters

open Microsoft.EntityFrameworkCore.Storage.ValueConversion

open FSharpWithEF.Models

type GenderConverter() =
    inherit
        ValueConverter<Gender, char>(
            (fun x -> x.ToString()[0]),
            (fun x ->
                match x with
                | 'M' -> Gender.Male
                | 'F' -> Gender.Female
                | _ -> Unchecked.defaultof<_>)
        )

type EntityStateConverter() =
    inherit
        ValueConverter<EntityState, char>(
            (fun x ->
                match x with
                | EntityState.Active -> '0'
                | EntityState.Inactive -> '1'),
            (fun x ->
                match x with
                | '0' -> EntityState.Active
                | '1' -> EntityState.Inactive
                | _ -> Unchecked.defaultof<_>)
        )
