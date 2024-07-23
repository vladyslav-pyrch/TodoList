namespace TodoList.Core.Domain.Model

[<AbstractClass>]
type Identity<'T>(value: 'T) =
    let value = value
    member public this.Value = value
    
    interface IIdentity with
        