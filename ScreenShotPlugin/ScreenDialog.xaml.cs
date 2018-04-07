using Engine.Helpers;
using System.IO;
using System.Windows;

namespace ScreenshotPlugin
{
  public partial class PluginDialog : Window
  {
    public PluginDialog()
    {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      var nick = UserNameTextBox.Text;
      var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".bmp";
      ScreenClientPlugin.AddFile(fileName);

      var messageContent = Serializer.Serialize(new ClientMakeScreenCommand.MessageContent { FileName = fileName });
      using (var client = ScreenClientPlugin.Model.Get())
      {
        var user = client.Chat.FindUser(nick);
        if (user != null)
          ScreenClientPlugin.Model.Peer.SendMessage(user.Id, ClientMakeScreenCommand.CommandId, messageContent);
      }
    }
  }
}
