namespace EFSharpLab.Db.Config.Converters

open Microsoft.EntityFrameworkCore.Storage.ValueConversion

open EFSharpLab.Models

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
        ValueConverter<RecordState, char>(
            (fun x ->
                match x with
                | RecordState.Active -> '1'
                | RecordState.Inactive -> '0'),
            (fun x ->
                match x with
                | '1' -> RecordState.Active
                | '0' -> RecordState.Inactive
                | _ -> Unchecked.defaultof<_>)
        )
