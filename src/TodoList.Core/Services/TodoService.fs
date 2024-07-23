namespace TodoList.Core.Services

open System
open System.Threading
open System.Threading.Tasks
open Microsoft.FSharp.Control
open TodoList.Core.Domain.Model.Todos

type TodoService(todoRepository: ITodoRepository) =
    let todoRepository = todoRepository
    
    member public this.Add (name: string) (description: string option): TodoId Task =
        let description1 =
            match description with
            | Some value -> value
            | None -> ""
        let id = todoRepository.NewId()
        task {
            let todo = Todo(id, name, description1)
            let! _ = todoRepository.Add todo
            let! _ = todoRepository.SaveChanges (CancellationToken())
            return id
        }
    
    member public this.UpdateName (todoId: TodoId) (newName: string): Task =
        task {
            let! todo = todoRepository.GetById todoId
            let _ = todo.ChangeName newName
            let! _ = todoRepository.Update todo
            return! todoRepository.SaveChanges (CancellationToken())
        }
        
    member public this.UpdateDescription (todoId: TodoId) (newDescription: string): Task =
        task {
            let! todo = todoRepository.GetById todoId
            let _ = todo.ChangeDescription newDescription
            let! _ = todoRepository.Update todo
            return! todoRepository.SaveChanges (CancellationToken())
        }
    
    member public this.MarkDone (todoId: TodoId) : Task =
        task {
            let! todo = todoRepository.GetById todoId
            let _ = todo.MarkDone ()
            let! _ = todoRepository.Update todo
            return! todoRepository.SaveChanges (CancellationToken())
        }
        
    member public this.MarkUndone (todoId: TodoId): Task =
        task {
            let! todo = todoRepository.GetById todoId
            let _ = todo.MarkUndone ()
            let! _ = todoRepository.Update todo
            return! todoRepository.SaveChanges (CancellationToken())
        }
        
    member public this.GetAll(): Todo list Task =
        task {
            return! todoRepository.GetAll()
        }
    
    member public this.Delete (todoId: TodoId): Task =
        task {
            let! _ = todoRepository.Remove todoId
            return! todoRepository.SaveChanges (CancellationToken())
        }
    
    member public this.DeleteAllDone(): Task =
        task {
            let! todos = todoRepository.GetDone() 
            for todo in todos do
                 let! _ = todoRepository.Remove todo.Id
                 ()
                
            return! todoRepository.SaveChanges (CancellationToken())
        }
    
    member public this.ThereExists(todoId: TodoId): Task<bool> =
        task {
            return! todoRepository.ThereExists todoId
        }