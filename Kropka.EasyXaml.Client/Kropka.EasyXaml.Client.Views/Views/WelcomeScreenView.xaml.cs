using System.IO;
using System.Windows;
using Kropka.EasyXaml.Client.Infrastructure.Events;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Views;
using Prism.Events;

namespace Kropka.EasyXaml.Client.Views.Views;

public partial class WelcomeScreenView : IWelcomeScreenView
{
    private readonly IEventAggregator _eventAggregator;

    public WelcomeScreenView(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;

        InitializeComponent();
    }

    private void OnFileDrop(object sender, DragEventArgs e)
    {
        var files = (string[])e.Data.GetData(DataFormats.FileDrop);

        if (files == null) return;

        var filePath = Path.GetFullPath(files[0]);

        _eventAggregator.GetEvent<FileDroppedEvent>().Publish(filePath);
    }

    private void OnFolderDrop(object sender, DragEventArgs e)
    {
        var folders = (string[])e.Data.GetData(DataFormats.FileDrop);

        if (folders == null) return;

        var folderPath = Path.GetFullPath(folders[0]);

        _eventAggregator.GetEvent<FolderDroppedEvent>().Publish(folderPath);
    }
}