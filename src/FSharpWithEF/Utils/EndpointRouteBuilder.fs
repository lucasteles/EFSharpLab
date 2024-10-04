[<AutoOpen>]
module EndpointRouteBuilderExtensions

open System.Diagnostics.CodeAnalysis
open Microsoft.AspNetCore.Routing
open System
open FSharp.Core
open Microsoft.AspNetCore.Builder

module Delegate =
    let inline fromFuncWithMaybeUnit (func: Func<'a, 'b>) : Delegate =
        match box func with
        | :? Func<unit, 'b> as f -> Func<'b>((f |> unbox<Func<unit, 'b>>).Invoke) :> Delegate
        | _ -> func :> Delegate

[<Literal>]
let private route = "Route"

type IEndpointRouteBuilder with

    member builder.MapGet([<StringSyntax(route)>] pattern: string, handler: Func<_, _>) =
        builder.MapGet(pattern, Delegate.fromFuncWithMaybeUnit handler)

    member builder.MapGet([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _>) =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _>) =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _>) =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _>) =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _>) =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _>) =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _>) =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _>) =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _>) =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _>) =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapGet
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapGet(pattern, handler :> Delegate)

    member builder.MapPost([<StringSyntax(route)>] pattern: string, handler: Func<_, _>) =
        builder.MapPost(pattern, Delegate.fromFuncWithMaybeUnit handler)

    member builder.MapPost([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _>) =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _>) =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _>) =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _>) =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _>) =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _>) =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _>) =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _>) =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _>) =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _>) =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPost
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapPost(pattern, handler :> Delegate)

    member builder.MapPut([<StringSyntax(route)>] pattern: string, handler: Func<_, _>) =
        builder.MapPut(pattern, Delegate.fromFuncWithMaybeUnit handler)

    member builder.MapPut([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _>) =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _>) =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _>) =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _>) =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _>) =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _>) =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _>) =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _>) =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _>) =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _>) =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapPut
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapPut(pattern, handler :> Delegate)

    member builder.MapDelete([<StringSyntax(route)>] pattern: string, handler: Func<_, _>) =
        builder.MapDelete(pattern, Delegate.fromFuncWithMaybeUnit handler)

    member builder.MapDelete([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _>) =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _>) =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _>) =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _>) =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _>) =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _>) =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _>) =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _>) =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _>) =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapDelete(pattern, handler :> Delegate)

    member builder.MapDelete
        ([<StringSyntax(route)>] pattern: string, handler: Func<_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _>)
        =
        builder.MapDelete(pattern, handler :> Delegate)
