using MauiReactor;
using Rearch;
using Rearch.Reactor.Components;
using TodoApp.Components;

namespace TodoApp.Pages;

partial class MainPage : CapsuleConsumer
{
    public override VisualNode Render(ICapsuleHandle use)
    {
        return new GlobalWarmUps(new Body());
    }
}
