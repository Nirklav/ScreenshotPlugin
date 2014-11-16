using Engine.Model.Server;
using System;
using System.Linq;
using System.Windows;

namespace ScreenshotPlugin
{
  /// <summary>
  /// Логика взаимодействия для PluginDialog.xaml
  /// </summary>
  public partial class PluginDialog : Window
  {
    public PluginDialog()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      ScreenClientPlugin.Model.Peer.SendMessage(UserNameTextBox.Text, ClientMakeScreenCommand.CommandId, null);
    }
  }
}
