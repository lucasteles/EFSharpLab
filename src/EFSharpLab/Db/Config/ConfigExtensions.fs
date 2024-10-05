[<AutoOpen>]
module EFSharpLab.Db.Config.EFConfigExtensions

open System
open System.Linq.Expressions
open System.Runtime.CompilerServices
open Microsoft.EntityFrameworkCore
open Microsoft.EntityFrameworkCore.Metadata.Builders
open Microsoft.EntityFrameworkCore.Storage.ValueConversion
open Microsoft.FSharp.Reflection

type DateTimeUtcConverter() =
    inherit
        ValueConverter<DateTime, DateTime>(
            (fun (source: DateTime) ->
                match source.Kind with
                | DateTimeKind.Utc -> source
                | DateTimeKind.Local -> source.ToUniversalTime()
                | _ -> DateTime.SpecifyKind(source, DateTimeKind.Utc)),
            (fun (target: DateTime) -> DateTime.SpecifyKind(target, DateTimeKind.Utc))
        )

type DateTimeUnspecifiedConverter() =
    inherit
        ValueConverter<DateTime, DateTime>(
            (fun (source: DateTime) -> DateTime.SpecifyKind(source, DateTimeKind.Utc)),
            (fun (target: DateTime) -> DateTime.SpecifyKind(target, DateTimeKind.Unspecified))
        )

type OptionConverter<'T>() =
    inherit
        ValueConverter<'T option, 'T>(
            (fun (source: 'T option) ->
                match source with
                | Some y -> y
                | None -> Unchecked.defaultof<'T>),

            (fun (target: 'T) ->
                match box target with
                | null -> None
                | _ -> Some target),

            convertsNulls = true
        )

type SingleCaseUnionConverter<'U, 'T>() =
    inherit
        ValueConverter<'U, 'T>(
            (fun (source: 'U) -> FSharpValue.GetUnionFields(source, source.GetType()) |> snd |> Seq.head :?> 'T),
            (fun (target: 'T) ->
                FSharpValue.MakeUnion(FSharpType.GetUnionCases(typedefof<'U>) |> Array.exactlyOne, [| target :> obj |])
                :?> 'U)
        )

type EntityTypeBuilder<'T when 'T: not struct> with

    member this.hasKey(key: Expression<Func<'T, obj>>) = this.HasKey(keyExpression = key)

    member this.hasIndex(key: Expression<Func<'T, obj>>) = this.HasIndex(indexExpression = key)

    member this.ignore(prop: Expression<Func<'T, obj>>) = this.Ignore(prop)

    member this.ownsOne(prop: Expression<Func<'T, 'P>>, ?config: OwnedNavigationBuilder<'T, 'P> -> _) =
        this.OwnsOne(prop, (fun b -> config |> Option.iter (fun f -> f b |> ignore)))

    member this.complexProperty(prop: Expression<Func<'T, 'P>>, ?config: ComplexPropertyBuilder<'P> -> _) =
        this.ComplexProperty(
            prop,
            (fun b ->
                b.IsRequired() |> ignore
                config |> Option.iter (fun f -> f b |> ignore))
        )

    member this.hasMany(prop: Expression<Func<'T, 'P seq>>) = this.HasMany(prop)

type ReferenceCollectionBuilder<'T, 'TDep when 'TDep: not struct and 'T: not struct> with

    member this.hasForeignKey(prop: Expression<Func<'TDep, obj>>) =
        this.HasForeignKey(foreignKeyExpression = prop)

type ModelConfigurationBuilder with

    member this.UseDatetimeUtc() =
        this.Properties<DateTime>().HaveConversion<DateTimeUtcConverter>()

    member this.UseSingleCase<'union, 'value>() =
        this
            .Properties<'union>()
            .HaveConversion<SingleCaseUnionConverter<'union, 'value>>()

    member this.UseSingleCase<'union>() = this.UseSingleCase<'union, Guid>()

type PropertyBuilderExtensions() =
    [<Extension>]
    static member IsOptional(this: PropertyBuilder<'t option>) =
        this.IsRequired(false).HasConversion<OptionConverter<'t>>()
