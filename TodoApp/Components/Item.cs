using MauiReactor;
using Rearch;
using Rearch.Reactor.Components;
using static TodoApp.Capsules.TodoCapsules;
using TodoApp.Models;

namespace TodoApp.Components;

class Item(Todo item) : CapsuleConsumer
{
    public override VisualNode Render(ICapsuleHandle use)
    {
        var (_, UpdateTodo, _) = use.Invoke(TodoItemsManagerCapsule);

        return Grid("54", "Auto, *",
            CheckBox()
                .IsChecked(item.Done)
                .OnCheckedChanged((s, args) => OnItemDoneChanged(item, args.Value)),
            Label(item.Task)
                .TextDecorations(item.Done ? TextDecorations.Strikethrough : TextDecorations.None)
                .VCenter()
                .GridColumn(1));

        void OnItemDoneChanged(Todo item, bool done)
        {
            UpdateTodo(item, null, done);
        }
    }
}
