using Prism.Mvvm;
using System.Reflection;
using UV.EasyXaml.Client.Infrastructure.Interfaces.ViewModels;
using UV.EasyXaml.Client.Infrastructure.Interfaces.Views;

namespace UV.EasyXaml.Client.ViewModels;

internal class SplashScreenViewModel : BindableBase, ISplashScreenViewModel
{
    #region Field
    private string _infoMessage;
    #endregion

    #region ctor
    public SplashScreenViewModel(ISplashScreenView view)
    {
        View = view;
        View.DataContext = this;
    }
    #endregion

    #region Properties
    public static string Title => GetProductName();

    public string InfoMessage
    {
        get => _infoMessage;
        set => SetProperty(ref _infoMessage, value);
    }

    public ISplashScreenView View { get; }
    #endregion

    #region Methods
    private static string GetProductName()
    {
        return Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>()?.Product;
    }
    #endregion
}