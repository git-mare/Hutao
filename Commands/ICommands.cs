namespace HutaoWaifuBot.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync(NetCord.Gateway.Message message, NetCord.Rest.RestClient client);
    }
}