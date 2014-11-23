using Engine;
using Engine.Helpers;
using Engine.Model.Server;
using Engine.Plugins.Client;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ScreenshotPlugin
{
  public class ClientMakeScreenCommand : ClientPluginCommand
  {
    public static ushort CommandId { get { return 50000; } }
    public override ushort Id { get { return ClientMakeScreenCommand.CommandId; } }

    public override void Run(ClientCommandArgs args)
    {
      if (args.PeerConnectionId == null)
        return;

      var receivedContent = Serializer.Deserialize<MessageContent>(args.Message);

      var screenDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "screens");
      if (!Directory.Exists(screenDirectory))
        Directory.CreateDirectory(screenDirectory);

      var fullPath = Path.Combine(screenDirectory, receivedContent.FileName);
     
      using (var bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
      using (var graphic = Graphics.FromImage(bmpScreenCapture))
      {
        graphic.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, bmpScreenCapture.Size, CopyPixelOperation.SourceCopy);
        bmpScreenCapture.Save(fullPath);
      }

      ScreenClientPlugin.Model.API.AddFileToRoom(ServerModel.MainRoomName, fullPath);
      ScreenClientPlugin.Model.Peer.SendMessage(args.PeerConnectionId, ClientScreenDoneCommand.CommandId, null);
    }

    [Serializable]
    public class MessageContent
    {
      private string fileName;

      public string FileName { get { return fileName; } set { fileName = value; } }
    }
  }
}
