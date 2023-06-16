using System.IO;
using System.Windows;
using Kropka.EasyXaml.Client.Infrastructure.Events;
using Kropka.EasyXaml.Client.Infrastructure.Interfaces.Views;
using Prism.Events;

namespace Kropka.EasyXaml.Client.Views.Views;

public partial class SingleFileConverterView : ISingleFileConverterView
{
    #region Fields
    private readonly IEventAggregator _eventAggregator;
    #endregion

    #region Constructors
    public SingleFileConverterView(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;

        InitializeComponent();
    }
    #endregion

    #region Methods
    private void OnFileDrop(object sender, DragEventArgs e)
    {
        var files = (string[])e.Data.GetData(DataFormats.FileDrop);

        if (files == null) return;

        var filePath = Path.GetFullPath(files[0]);

        _eventAggregator.GetEvent<FileDroppedSingleModeEvent>().Publish(filePath);
    }
    #endregion
}