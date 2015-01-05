using Engine;
using Engine.Helpers;
using Engine.Model.Server;
using Engine.Plugins.Client;
using System;
using System.IO;

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

      ScreenClientPlugin.Model.API.SendMessage(null, string.Format("Выполнен снимок у пользователя {0}.", args.PeerConnectionId), ServerModel.MainRoomName);
    }
  }
}
