using MauiReactor;
using Rearch;
using Rearch.Reactor.Components;
using static TodoApp.Capsules.TodoCapsules;
using TodoApp.Models;

namespace TodoApp.Components;

partial class Body : CapsuleConsumer
{
    public override VisualNode Render(ICapsuleHandle use)
    {
        var todoItems = use.Invoke(TodoItemsCapsule);
        var (_, _, DeleteTodos) = use.Invoke(TodoItemsManagerCapsule);

        return
            ContentPage(
                Grid("Auto, *, Auto", "*",
                    new TodoEditor(),

                    CollectionView()
                        .ItemsSource(todoItems, RenderItem)
                        .GridRow(1),

                    Button("Clear List")
                        .OnClicked(OnClearList)
                        .GridRow(2)

                ));

        VisualNode RenderItem(Todo item) =>
            new Item(item);

        void OnClearList() =>
            DeleteTodos([.. todoItems]);
    }
}
