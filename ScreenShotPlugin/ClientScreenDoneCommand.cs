using Engine.API;
using Engine.Model.Server;
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

    protected override void OnRun(ClientCommandArgs args)
    {
      var message = string.Format("Выполнен снимок у пользователя {0}.", args.PeerConnectionId);
      ScreenClientPlugin.Model.Api.SendMessage(null, message, ServerModel.MainRoomName);
    }
  }
}
