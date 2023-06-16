using System.IO;
using System.Windows;
using Kropka.EasyXaml.Client.Infrastructure.Events;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Views;
using Prism.Events;

namespace Kropka.EasyXaml.Client.Views.Views;

public partial class FolderConverterView : IFolderConverterView
{
    #region Fields
    private readonly IEventAggregator _eventAggregator;
    #endregion

    #region Constructors
    public FolderConverterView(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;

        InitializeComponent();
    }
    #endregion

    #region Methods
    private void OnFolderDrop(object sender, DragEventArgs e)
    {
        var folders = (string[])e.Data.GetData(DataFormats.FileDrop);

        if (folders == null) return;

        var folderPath = Path.GetFullPath(folders[0]);

        _eventAggregator.GetEvent<FolderDroppedFolderModeEvent>().Publish(folderPath);
    }
    #endregion
}