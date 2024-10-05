[<AutoOpen>]
module Utils

open MassTransit

type BlockBuilder() =
    member _.Value = ()
    member inline this.Delay([<InlineIfLambda>] f) = f this.Value
    member this.Yield _ = this.Value
    member this.Combine(_, _) = this.Value
    member this.Zero() = this.Value

let block = BlockBuilder()

type IsClass<'entity when 'entity: not struct> = 'entity

type Task = System.Threading.Tasks.Task
type Task<'t> = System.Threading.Tasks.Task<'t>
type CancellationToken = System.Threading.CancellationToken

let (|?) a b = a |> Option.defaultValue b

module Id =
    let nextGuid () = NewId.NextGuid()
    let next<'t> (ctor: _ -> 't) = nextGuid () |> ctor

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
