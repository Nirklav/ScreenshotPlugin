using Engine.Model.Client;
using Engine.Plugins.Client;
using System.Collections.Generic;

namespace ScreenshotPlugin
{
  public class ScreenClientPlugin : ClientPlugin
  {
    private List<ClientPluginCommand> commands;
    private ScreenshotNotifierContext notifier;

    private static List<string> downloadingFiles;

    #region ClientPlugin

    protected override void Initialize()
    {
      downloadingFiles = new List<string>();
      notifier = new ScreenshotNotifierContext();
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

    public override ClientNotifierContext NotifierContext
    {
      get { return notifier; }
    }

    public override List<ClientPluginCommand> Commands
    { 
      get { return commands; }
    }

    #endregion

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
