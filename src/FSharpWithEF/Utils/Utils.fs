[<AutoOpen>]
module Utils

open MassTransit

type ProcBuilder() =
    member _.Value = ()
    member inline this.Delay([<InlineIfLambda>] f) = f this.Value
    member this.Yield _ = this.Value
    member this.Combine(_, _) = this.Value
    member this.Zero() = this.Value

let proc = ProcBuilder()

type IsClass<'entity when 'entity: not struct> = 'entity
let (|?) a b = a |> Option.defaultValue b

module EntityId =
    let next () = NewId.NextGuid()


module Json =
    open System.Text.Json.Serialization

    let jsonFsharpOptions =
        JsonFSharpOptions
            .Default()
            .WithUnionUnwrapRecordCases()
            .WithUnionUnwrapFieldlessTags()
            .WithUnionUnwrapSingleFieldCases()
            .WithUnionFieldsName("value")
            .WithUnionTagName("case")

    let configure options =
        jsonFsharpOptions.AddToJsonSerializerOptions options
        options.ReferenceHandler <- ReferenceHandler.IgnoreCycles
