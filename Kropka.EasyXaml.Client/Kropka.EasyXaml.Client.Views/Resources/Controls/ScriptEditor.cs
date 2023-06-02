using System;
using System.Windows;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Search;

namespace Kropka.EasyXaml.Client.Views.Resources.Controls;

public class ScriptEditor : TextEditor
{
    #region Constructors
    public ScriptEditor()
    {
        Options.ConvertTabsToSpaces = true;
        SearchPanel.Install(TextArea);
    }
    #endregion

    #region Properties
    public string Script
    {
        get => GetValue(ScriptProperty) as string;
        set => SetValue(ScriptProperty, value);
    }

    public static readonly DependencyProperty ScriptProperty = DependencyProperty.Register(
        nameof(Script), typeof(string), typeof(ScriptEditor), new FrameworkPropertyMetadata(string.Empty, OnScriptChanged));
    #endregion

    #region Methods
    protected override void OnTextChanged(EventArgs e)
    {
        Script = Text;
    }

    public static void OnScriptChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        var editor = sender as ScriptEditor;
        var newValue = e.NewValue as string;

        if (editor?.Text == newValue)
        {
            return;
        }

        if (editor != null)
        {
            editor.Text = newValue;
        }
    }
    #endregion
}