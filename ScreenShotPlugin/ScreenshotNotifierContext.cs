using Engine;
using Engine.Model.Client;
using Engine.Model.Entities;
using Engine.Model.Server;
using System;
using System.IO;

namespace ScreenshotPlugin
{
  public class ScreenshotNotifierContext : ClientNotifierContext
  {
    protected override void OnReceiveMessage(ReceiveMessageEventArgs args)
    {
      if (args.Type != MessageType.File)
        return;

      var file = args.State as FileDescription;
      if (file == null)
        return;

      var downaladDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "downloads");
      if (!Directory.Exists(downaladDirectory))
        Directory.CreateDirectory(downaladDirectory);

      var path = Path.Combine(downaladDirectory, file.Name);

      if (ScreenClientPlugin.NeedDownload(file.Name))
      {
        ScreenClientPlugin.RemoveFile(file.Name);
        ScreenClientPlugin.Model.API.DownloadFile(path, ServerModel.MainRoomName, file);
      }
    }
  }
}
