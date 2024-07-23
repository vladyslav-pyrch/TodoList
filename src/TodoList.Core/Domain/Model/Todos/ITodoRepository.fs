namespace TodoList.Core.Domain.Model.Todos

open System.Threading.Tasks
open TodoList.Core.Domain.Model

type ITodoRepository =
    inherit IRepository<Todo, TodoId>
    abstract member GetAll: unit -> Todo list Task
    abstract member GetById: TodoId -> Todo Task
    abstract member Add: Todo -> Task
    abstract member Update: Todo -> Task
    abstract member Remove: TodoId -> Task
    abstract member GetDone: unit -> Todo list Task
    abstract member ThereExists: TodoId -> bool Task