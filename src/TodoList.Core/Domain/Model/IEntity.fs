namespace TodoList.Core.Domain.Model

type public IEntity<'TId when 'TId :> IIdentity> =
    interface
        abstract member Id: 'TId with get
    end