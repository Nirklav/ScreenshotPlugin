using Engine.API;
using Engine.Model.Server;
using Engine.Plugins.Client;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ScreenshotPlugin
{
  public class ClientMakeScreenCommand : ClientPluginCommand<ClientMakeScreenCommand.MessageContent>
  {
    public const long CommandId = 50000;

    protected override bool IsPeerCommand
    {
      get { return true; }
    }

    public override long Id
    {
      get { return CommandId; }
    }

    protected override void OnRun(MessageContent content, ClientCommandArgs args)
    {
      if (args.PeerConnectionId == null)
        return;

      var screenDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "screens");
      if (!Directory.Exists(screenDirectory))
        Directory.CreateDirectory(screenDirectory);

      var fullPath = Path.Combine(screenDirectory, content.FileName);
     
      using (var bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
      using (var graphic = Graphics.FromImage(bmpScreenCapture))
      {
        graphic.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, bmpScreenCapture.Size, CopyPixelOperation.SourceCopy);
        bmpScreenCapture.Save(fullPath);
      }

      ScreenClientPlugin.Model.Api.AddFileToRoom(ServerModel.MainRoomName, fullPath);
      ScreenClientPlugin.Model.Peer.SendMessage(args.PeerConnectionId, ClientScreenDoneCommand.CommandId);
    }

    [Serializable]
    public class MessageContent
    {
      private string fileName;

      public string FileName
      {
        get { return fileName; }
        set { fileName = value; }
      }
    }
  }
}
