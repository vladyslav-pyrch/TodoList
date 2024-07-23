namespace TodoList.Core.Domain.Model

[<AbstractClass>]
type public Entity<'TId when 'TId :> IIdentity>
    (id: 'TId) =
    let id = id
    
    member public this.Id = (this :> IEntity<'TId>).Id
    
    interface IEntity<'TId> with
        member this.Id = id
    