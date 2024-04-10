﻿using ReactorData;
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
        Action<Todo> UpdateTodo,
        Action<IEnumerable<Todo>> DeleteTodo)
        TodoItemsManagerCapsule(ICapsuleHandle use)
    {
        var context = use.Invoke(ContextCapsule);

        return (
            AddTodo: todo =>
            {
                context.Add(todo);
                context.Save();
            },
            UpdateTodo: todo =>
            {
                context.Replace(todo, new Todo
                {
                    Id = todo.Id,
                    Task = todo.Task,
                    Done = todo.Done
                });
                context.Save();
            },
            DeleteTodo: todos =>
            {
                context.Delete(todos.ToArray());
                context.Save();
            }
        );
    }

    /// <summary>
    /// Represents the todos list using the filter from the
    /// <see cref="FilterCapsule"/>.
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
