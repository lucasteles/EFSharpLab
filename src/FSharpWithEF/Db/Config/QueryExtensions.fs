[<AutoOpen>]
module QueryExtensions

open System
open System.Linq
open System.Linq.Expressions
open System.Runtime.CompilerServices
open System.Threading
open System.Threading.Tasks
open Microsoft.EntityFrameworkCore

type private Pred<'t> = Func<'t, bool>
type private PredExp<'t> = Expression<Pred<'t>>

module Option =
    let ofRef v : 'a option =
        if Object.ReferenceEquals(v, null) then None else Some v

module Query =
    let toArrayAsync ct (query: #IQueryable<_>) = query.ToArrayAsync(ct)

    let toListAsync ct (query: #IQueryable<_>) =
        task {
            let! ret = query.ToListAsync(ct)
            return ret |> List.ofSeq
        }

    let tryFirstAsync ct (query: #IQueryable<'t>) =
        task {
            let! ret = query.FirstOrDefaultAsync(cancellationToken = ct)
            return Option.ofRef ret
        }

    let tryFilterFirstAsync ct (predicate: PredExp<'t> when 't IsClass) (query: #IQueryable<'t>) =
        task {
            let! ret = query.FirstOrDefaultAsync(predicate, cancellationToken = ct)
            return Option.ofRef ret
        }

    let tryFirstValueAsync ct (query: #IQueryable<Nullable<'t>>) =
        task {
            let! ret = query.FirstOrDefaultAsync(cancellationToken = ct)
            return Option.ofNullable ret
        }

    let tryFilterFirstValueAsync ct (predicate: PredExp<Nullable<'t>>) =
        fun (query: #IQueryable<Nullable<'t>>) ->
            task {
                let! ret = query.FirstOrDefaultAsync(predicate, cancellationToken = ct)
                return Option.ofNullable ret
            }

    let trySingleAsync ct (query: #IQueryable<'t> when 't IsClass) =
        task {
            let! ret = query.SingleOrDefaultAsync(ct)
            return Option.ofRef ret
        }

    let tryFilterSingleAsync ct (predicate: PredExp<'t> when 't IsClass) =
        fun (query: #IQueryable<'t>) ->
            task {
                let! ret = query.SingleOrDefaultAsync(predicate, cancellationToken = ct)
                return Option.ofRef ret
            }

    let trySingleValueAsync ct (query: #IQueryable<Nullable<'t>>) =
        task {
            let! ret = query.SingleOrDefaultAsync(cancellationToken = ct)
            return Option.ofNullable ret
        }

    let tryFilterSingleValueAsync ct (predicate: PredExp<Nullable<'t>>) =
        fun (query: #IQueryable<Nullable<'t>>) ->
            task {
                let! ret = query.SingleOrDefaultAsync(predicate, cancellationToken = ct)
                return Option.ofNullable ret
            }

type IQueryableExtensions =
    [<Extension>]
    static member TryFirst(this: IQueryable<Nullable<'t>>, ?predicate: PredExp<Nullable<'t>>) =
        match predicate with
        | Some f -> this.FirstOrDefault(predicate = f)
        | None -> this.FirstOrDefault()
        |> Option.ofNullable

    [<Extension>]
    static member TryFirst<'t when 't IsClass>(this: IQueryable<'t>, ?predicate: PredExp<'t>) =
        match predicate with
        | Some f -> this.FirstOrDefault(predicate = f)
        | None -> this.FirstOrDefault()
        |> Option.ofRef

    [<Extension>]
    static member TrySingle(this: IQueryable<Nullable<'t>>, ?predicate: PredExp<Nullable<'t>>) =
        match predicate with
        | Some f -> this.SingleOrDefault(predicate = f)
        | None -> this.SingleOrDefault()
        |> Option.ofNullable

    [<Extension>]
    static member TrySingle(this: IQueryable<'t>, ?predicate: PredExp<'t> when 't IsClass) =
        match predicate with
        | Some f -> this.SingleOrDefault(predicate = f)
        | None -> this.SingleOrDefault()
        |> Option.ofRef

    [<Extension>]
    static member ToListAsync(this: IQueryable<'t>) =
        Query.toListAsync CancellationToken.None this

    [<Extension>]
    static member ToListAsync(this: IQueryable<'t>, ct) = Query.toListAsync ct this

    [<Extension>]
    static member TryFirstAsync(this: IQueryable<'t> when 't IsClass, ?ct) =
        Query.tryFirstAsync (ct |? CancellationToken.None) this

    [<Extension>]
    static member TryFirstAsync(this: IQueryable<Nullable<'t>>, ?ct) =
        Query.tryFirstValueAsync (ct |? CancellationToken.None) this

    [<Extension>]
    static member TryFirstAsync(this: IQueryable<'t>, predicate: PredExp<'t> when 't IsClass, ?ct) =
        this |> Query.tryFilterFirstAsync (ct |? CancellationToken.None) predicate

    [<Extension>]
    static member TryFirstAsync(this: IQueryable<Nullable<'t>>, predicate: PredExp<Nullable<'t>>, ?ct) =
        this |> Query.tryFilterFirstValueAsync (ct |? CancellationToken.None) predicate

    [<Extension>]
    static member TrySingleAsync(this: IQueryable<'t> when 't IsClass, ?ct) =
        Query.trySingleAsync (ct |? CancellationToken.None) this

    [<Extension>]
    static member TrySingleAsync(this: IQueryable<Nullable<'t>>, ?ct) =
        Query.trySingleValueAsync (ct |? CancellationToken.None) this

    [<Extension>]
    static member TrySingleAsync(this: IQueryable<'t>, predicate: PredExp<'t> when 't IsClass, ?ct) =
        this |> Query.tryFilterSingleAsync (ct |? CancellationToken.None) predicate

    [<Extension>]
    static member TrySingleAsync(this: IQueryable<Nullable<'t>>, predicate: PredExp<Nullable<'t>>, ?ct) =
        this |> Query.tryFilterSingleValueAsync (ct |? CancellationToken.None) predicate


type DbSet<'t when 't IsClass> with

    member this.add entity = this.Add(entity) |> ignore
    member this.addRange(entities: _ seq) = this.AddRange(entities)

    member this.remove entity = this.Remove(entity) |> ignore
    member this.removeRange(entities: _ seq) = this.RemoveRange(entities)

    member this.tryFind(k: obj) = this.Find k |> Option.ofRef

    member this.tryFindAsync(k: obj) =
        task {
            let! v = this.FindAsync(k)
            return v |> Option.ofRef
        }

type DbContext with

    member this.saveChangesAsync() = this.SaveChangesAsync() :> Task
