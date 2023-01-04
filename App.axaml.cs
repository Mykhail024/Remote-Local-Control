using Avalonia;
using Avalonia.ReactiveUI;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using RLCClient.ViewModels;
using System.Diagnostics;
using System.Threading;

namespace RLCClient;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        new Views.MainView().Show();
        base.OnFrameworkInitializationCompleted();
    }
}