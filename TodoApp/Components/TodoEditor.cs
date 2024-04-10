using MauiReactor;
using Rearch;
using Rearch.Reactor.Components;
using static TodoApp.Capsules.TodoCapsules;
using TodoApp.Models;

namespace TodoApp.Components;

class TodoEditor : CapsuleConsumer
{
    public override VisualNode Render(ICapsuleHandle use)
    {
        var (AddTodo, _, _) = use.Invoke(TodoItemsManagerCapsule);

        return Render<string>(state =>
            Grid("*", "*,Auto",
                Entry()
                    .Text(state.Value ?? string.Empty)
                    .OnTextChanged(text => state.Set(s => text, false)),
                Button("Create")
                    .GridColumn(1)
                    .OnClicked(() =>
                    {
                        AddTodo(new Todo { Task = state.Value ?? "New Task" });
                        state.Set(s => string.Empty);
                    })
                )
            );
    }
}
