using MauiReactor;
using ReactorData;
using Rearch;
using Rearch.Reactor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TodoApp.Capsules.TodoCapsules;
using TodoApp.Models;

namespace TodoApp.Components;

partial class Body : CapsuleConsumer
{
    [Inject]
    IModelContext _modelContext;

    public override VisualNode Render(ICapsuleHandle use)
    {
        var todoItems = use.Invoke(TodoItemsCapsule);

        return
            ContentPage(
                Grid("Auto, *, Auto", "*",
                    TodoEditor(OnCreatedNewTask),

                    CollectionView()
                        .ItemsSource(todoItems, RenderItem)
                        .GridRow(1),

                    Button("Clear List")
                        .OnClicked(OnClearList)
                        .GridRow(2)

                ));

        void OnCreatedNewTask(Todo todo)
        {
            _modelContext.Add(todo);
            _modelContext.Save();
        }

        void OnClearList()
        {
            _modelContext.Delete([.. todoItems]);
            _modelContext.Save();
        }
    }

    VisualNode RenderItem(Todo item)
        => Grid("54", "Auto, *",
            CheckBox()
                .IsChecked(item.Done)
                .OnCheckedChanged((s, args) => OnItemDoneChanged(item, args.Value)),
            Label(item.Task)
                .TextDecorations(item.Done ? TextDecorations.Strikethrough : TextDecorations.None)
                .VCenter()
                .GridColumn(1));

    static VisualNode TodoEditor(Action<Todo> created)
        => Render<string>(state =>
            Grid("*", "*,Auto",
                Entry()
                    .Text(state.Value ?? string.Empty)
                    .OnTextChanged(text => state.Set(s => text, false)),
                Button("Create")
                    .GridColumn(1)
                    .OnClicked(() =>
                    {
                        created(new Todo { Task = state.Value ?? "New Task" });
                        state.Set(s => string.Empty);
                    })
                )
            );

    void OnItemDoneChanged(Todo item, bool done)
    {
        _modelContext.Replace(item, new Todo { Id = item.Id, Task = item.Task, Done = done });
        _modelContext.Save();
    }
}
