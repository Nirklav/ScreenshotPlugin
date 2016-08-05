using Engine;
using Engine.Exceptions;
using Engine.Model.Client;
using Engine.Model.Common;
using Engine.Model.Entities;
using Engine.Model.Server;
using Engine.Plugins.Client;
using System;
using System.Collections.Generic;
using System.IO;

namespace ScreenshotPlugin
{
  public class ScreenClientPlugin : ClientPlugin
  {
    private static List<string> downloadingFiles;
    private List<ClientPluginCommand> commands;
    private IClientNotifierContext notifierContext;

    #region ClientPlugin

    protected override void Initialize()
    {
      downloadingFiles = new List<string>();
      notifierContext = NotifierGenerator.MakeContext<IClientNotifierContext>();
      notifierContext.ReceiveMessage += OnReceiveMessage;

      commands = new List<ClientPluginCommand>
      {
        new ClientMakeScreenCommand(),
        new ClientScreenDoneCommand()
      };
    }

    public override void InvokeMenuHandler()
    {
      var dialog = new PluginDialog();
      dialog.ShowDialog();
    }

    public override string Name
    {
      get { return "ScreenClientPlugin"; }
    }

    public override string MenuCaption
    {
      get { return "Сделать скриншот"; }
    }

    public override IEnumerable<ClientPluginCommand> Commands
    { 
      get { return commands; }
    }

    public override object NotifierContext
    {
      get { return notifierContext; }
    }

    #endregion

    private void OnReceiveMessage(object sender, ReceiveMessageEventArgs args)
    {
      if (args.Type != MessageType.File)
        return;

      using (var client = Model.Get())
      {
        Room room;
        if (!client.Rooms.TryGetValue(args.RoomName, out room))
          throw new ModelException(ErrorCode.RoomNotFound);

        var file = room.Files.Find(f => f.Id == args.FileId);
        var downaladDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "downloads");
        if (!Directory.Exists(downaladDirectory))
          Directory.CreateDirectory(downaladDirectory);

        var path = Path.Combine(downaladDirectory, file.Name);
        if (NeedDownload(file.Name))
        {
          RemoveFile(file.Name);
          Model.Api.DownloadFile(path, ServerModel.MainRoomName, args.FileId);
        }
      }
    }

    public static void AddFile(string fileName)
    {
      lock (downloadingFiles)
        downloadingFiles.Add(fileName);
    }

    public static void RemoveFile(string fileName)
    {
      lock (downloadingFiles)
        downloadingFiles.Remove(fileName);
    }

    public static bool NeedDownload(string fileName)
    {
      lock (downloadingFiles)
        return downloadingFiles.Contains(fileName);
    }
  }
}
