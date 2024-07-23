namespace TodoList.Core.Domain.Model.Todos

open System
open Microsoft.FSharp.Core
open TodoList.Core.Domain.Model

type public Todo(id: TodoId, name: string, description: string, isDone: bool) as this =
    inherit Entity<TodoId>(id)
    let mutable name = name
    let mutable description = description
    let mutable isDone = isDone
    
    do this.ValidateName name 
    do this.ValidateDescription description
    
    new(id: TodoId, name: string, description: string) = Todo(id, name, description, false)
    new(id: TodoId, name: string) = Todo(id, name, "")
    
    member public this.Name = name
    member public this.Description = description
    member public this.IsDone = isDone
        
    member public this.ChangeName newName : unit =
        this.ValidateName newName
        name <- newName
        
    member public this.ChangeDescription newDescription : unit =
        this.ValidateDescription newDescription
        description <- newDescription
        
    member public this.MarkDone(): unit = isDone <- true
    
    member public this.MarkUndone(): unit = isDone <- false
    
    member private this.ValidateName name : unit =
        if String.IsNullOrWhiteSpace name then
            raise <| ArgumentException "The name may not be null, empty or whitespace."
            
    member private this.ValidateDescription description : unit =
        if description = null then
            raise <| ArgumentNullException "The description may not be null."