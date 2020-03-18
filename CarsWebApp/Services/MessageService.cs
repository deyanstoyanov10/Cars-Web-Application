namespace CarsWebApp.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Microsoft.AspNetCore.Identity;

    using CarsWebApp.Data;
    using CarsWebApp.Services.Contracts;

    public class MessageService : IMessageService
    {
        private readonly CarsDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public MessageService(CarsDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<string> GetChatList(string username)
        {
            var listContacts = (new[] {
                new
                {
                    From = "Unknown",
                    Message = "Unknown",
                    Date = DateTime.UtcNow,
                    Email = "test",
                    Phone = "test",
                    UnseenMessages = 0
                }
            }).ToList();

            var contacts = this.context.Messages.Select(x => new { From = x.From, x.To, x.Date }).Where(x => x.To == username).OrderByDescending(x => x.Date).Select(x => x.From).Distinct().ToList();

            for (int i = 0; i < contacts.Count; i++)
            {
                var messages = this.context.Messages.Where(x => x.From == contacts[i]).OrderByDescending(x => x.Date).Take(1).ToList();
                var unseenMessages = this.context.Messages.Where(x => x.From == contacts[i]).Where(x => x.To == username).Where(x => x.IsNew == true).ToArray();
                int countUnseenMessages = unseenMessages.Length;
                var user = await userManager.FindByNameAsync(contacts[i]);
                listContacts.Add(new
                {
                    From = contacts[i],
                    Message = messages[0].Message,
                    Date = messages[0].Date,
                    Email = await userManager.GetEmailAsync(user),
                    Phone = await userManager.GetPhoneNumberAsync(user),
                    UnseenMessages = countUnseenMessages
                });
            }

            var listResult = listContacts.OrderByDescending(x => x.Date).Skip(1).Select(x => new { From = x.From, Message = x.Message, Email = x.Email, Phone = x.Phone, UnseenMessages = x.UnseenMessages }).ToList();
            var result = JsonConvert.SerializeObject(listResult);
            return await Task.Run(() => result);
        }

        public async Task<string> GetMessages(string from, string to)
        {
            var messages = this.context.Messages.Where(x => (x.From == from && x.To == to) || (x.From == to && x.To == from)).OrderBy(x => x.Date).Select(m => new { From = m.From, To = m.To, Message = m.Message, Date = m.Date }).ToList();
            await SeenMessages(from, to);
            return await Task.Run(() => JsonConvert.SerializeObject(messages));
        }

        private async Task SeenMessages(string from, string to)
        {
            var unseen = this.context.Messages.Where(x => x.To == to).Where(x => x.IsNew == true && x.From == from).ToList();

            for (int i = 0; i < unseen.Count; i++)
            {
                unseen[i].IsNew = false;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<string> NotificationMessages(string username)
        {
            var messages = this.context.Messages.Where(x => x.To == username && x.IsNew == true).ToArray();

            return await Task.Run(() => JsonConvert.SerializeObject(messages.Length));
        }

        public async Task SendMessage(string from, string to, string message)
        {
            this.context.Messages.Add(new Models.Messages()
            {
                From = from,
                To = to,
                Message = message,
                Date = DateTime.UtcNow
            });
            await this.context.SaveChangesAsync();
        }
    }
}
