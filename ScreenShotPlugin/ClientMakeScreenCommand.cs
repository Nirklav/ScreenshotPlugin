using Engine.Api;
using Engine.Api.Client.Files;
using Engine.Model.Server.Entities;
using Engine.Plugins.Client;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ThirtyNineEighty.BinarySerializer;

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

    protected override void OnRun(MessageContent content, CommandArgs args)
    {
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

      ScreenClientPlugin.Model.Api.Perform(new ClientAddFileAction(ServerChat.MainRoomName, fullPath));
      ScreenClientPlugin.Model.Peer.SendMessage(args.ConnectionId, ClientScreenDoneCommand.CommandId);
    }

    [Serializable]
    [BinType("ClientMakeScreen")]
    public class MessageContent
    {
      [BinField("f")]
      public string FileName;
    }
  }
}
