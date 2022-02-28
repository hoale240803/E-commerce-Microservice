using WebAppRazor.Entities;

namespace WebAppRazor.Repositories
{
    public interface IContactRepository
    {
        Task<Contact> SendMessage(Contact contact);

        Task<Contact> Subscribe(string address);
    }
}