module FSharpWithEF.Json

open System.Text.Json.Serialization

let jsonFsharpOptions =
    JsonFSharpOptions
        .Default()
        .WithIncludeRecordProperties()
        .WithUnionUnwrapRecordCases()
        .WithUnionUnwrapFieldlessTags()
        .WithUnionUnwrapSingleFieldCases()
        .WithUnionFieldsName("value")
        .WithUnionTagName("case")

let configure options =
    jsonFsharpOptions.AddToJsonSerializerOptions options
    options.ReferenceHandler <- ReferenceHandler.IgnoreCycles
