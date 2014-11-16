using Engine;
using Engine.Helpers;
using Engine.Model.Client;
using Engine.Model.Server;
using Engine.Plugins.Client;
using System;
using System.IO;
using System.Windows;

namespace ScreenshotPlugin
{
  public class ClientScreenDoneCommand : ClientPluginCommand
  {
    public static ushort CommandId { get { return 50001; } }
    public override ushort Id { get { return ClientScreenDoneCommand.CommandId; } }

    public override void Run(ClientCommandArgs args)
    {
      if (args.PeerConnectionId == null)
        return;

      var receivedContent = Serializer.Deserialize<MessageContent>(args.Message);
      ScreenClientPlugin.Model.API.SendMessage(string.Format("Выполнен снимок у пользователя {0}.", args.PeerConnectionId), ServerModel.MainRoomName);
    }

    [Serializable]
    public class MessageContent
    {
      private string fileName;

      public string FileName { get { return fileName; } set { fileName = value; } }
    }
  }
}
