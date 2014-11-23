using Engine.Helpers;
using Engine.Model.Server;
using System;
using System.IO;
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
      var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".bmp";
      ScreenClientPlugin.AddFile(fileName);

      var messageContent = Serializer.Serialize(new ClientMakeScreenCommand.MessageContent { FileName = fileName });
      ScreenClientPlugin.Model.Peer.SendMessage(UserNameTextBox.Text, ClientMakeScreenCommand.CommandId, messageContent);
    }
  }
}
