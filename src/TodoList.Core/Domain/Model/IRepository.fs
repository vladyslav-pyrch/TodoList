namespace TodoList.Core.Domain.Model

open System.Threading
open System.Threading.Tasks

type IRepository<'TEntity, 'TId when 'TEntity :> IEntity<'TId> and 'TId :> IIdentity> =
    abstract member NewId: unit -> 'TId
    abstract member SaveChanges: CancellationToken -> Task
    