namespace TodoList.Core.Domain.Model.Todos

open System
open TodoList.Core.Domain.Model

type public TodoId(value: int) =
    inherit Identity<int>(value)