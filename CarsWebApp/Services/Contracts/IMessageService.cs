namespace CarsWebApp.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IMessageService
    {
        Task<string> GetChatList(string username);

        Task<string> GetMessages(string from, string to);

        Task SendMessage(string from, string to, string message);

        Task<string> NotificationMessages(string username);
    }
}
