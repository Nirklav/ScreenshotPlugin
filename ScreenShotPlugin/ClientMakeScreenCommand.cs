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

      string screenDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "screens");
      if (!Directory.Exists(screenDirectory))
        Directory.CreateDirectory(screenDirectory);

      string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".bmp";
      string fullPath = Path.Combine(screenDirectory, fileName);
     
      using (Bitmap bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
      using (Graphics graphic = Graphics.FromImage(bmpScreenCapture))
      {
        graphic.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, bmpScreenCapture.Size, CopyPixelOperation.SourceCopy);
        bmpScreenCapture.Save(fullPath);
      }

      ScreenClientPlugin.Model.API.AddFileToRoom(ServerModel.MainRoomName, fullPath);

      var messageContent = Serializer.Serialize(new ClientScreenDoneCommand.MessageContent { FileName = fullPath });
      ScreenClientPlugin.Model.Peer.SendMessage(args.PeerConnectionId, ClientScreenDoneCommand.CommandId, messageContent);
    }
  }
}
