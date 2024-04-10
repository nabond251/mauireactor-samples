using ReactorData;
using Rearch;
using System;
using System.Collections.Generic;
using System.Linq;
using static TodoApp.Capsules.ContextCapsules;
using TodoApp.Models;

namespace TodoApp.Capsules;

public static class TodoCapsules
{
    /// <summary>
    /// Provides a way to create/update and delete todos.
    /// </summary>
    /// <param name="use">Capsule handle.</param>
    /// <returns>Actions to add, update, or delete todos.</returns>
    internal static (
        Action<Todo> AddTodo,
        Action<Todo, string?, bool?> UpdateTodo,
        Action<IEnumerable<Todo>> DeleteTodos)
        TodoItemsManagerCapsule(ICapsuleHandle use)
    {
        var context = use.Invoke(ContextCapsule);

        return (
            AddTodo: todo =>
            {
                context.Add(todo);
                context.Save();
            },
            UpdateTodo: (Todo todo, string? task, bool? done) =>
            {
                context.Replace(todo, new Todo
                {
                    Id = todo.Id,
                    Task = task ?? todo.Task,
                    Done = done ?? todo.Done
                });
                context.Save();
            },
            DeleteTodos: todos =>
            {
                context.Delete(todos.ToArray());
                context.Save();
            }
        );
    }

    /// <summary>
    /// Represents the todos list.
    /// </summary>
    /// <param name="use">Capsule handle.</param>
    /// <returns>Query of to-do list items.</returns>
    internal static IQuery<Todo> TodoItemsCapsule(ICapsuleHandle use)
    {
        var context = use.Invoke(ContextCapsule);

        var todosQuery = use.Memo(
            () => context.Query<Todo>(query => query.OrderBy(_ => _.Task)),
            [context]);

        return todosQuery;
    }
}
