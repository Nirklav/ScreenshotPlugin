using Engine.Api;
using Engine.Api.Client.Messages;
using Engine.Model.Server.Entities;
using Engine.Plugins.Client;

namespace ScreenshotPlugin
{
  public class ClientScreenDoneCommand : ClientPluginCommand
  {
    public const long CommandId = 50001;

    protected override bool IsPeerCommand
    {
      get { return true; }
    }

    public override long Id
    {
      get { return CommandId; }
    }

    protected override void OnRun(CommandArgs args)
    {
      var message = string.Format("Выполнен снимок у пользователя {0}.", args.ConnectionId);
      ScreenClientPlugin.Model.Api.Perform(new ClientSendMessageAction(ServerChat.MainRoomName, message));
    }
  }
}
