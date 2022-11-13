using Data.EntityModel;

namespace Data
{
    public interface IRepository:IDisposable
    {
        IQueryable<PhoneBookEntry> GetPhoneBookEntries(PhoneBookEntryPagination phoneBookEntryParameters);
        
        bool AddPhoneBookEntries(IEnumerable<PhoneBookEntry> phoneBookEntries);
        
        User? AddUser(User user);

        User? GetUser(string username);

        byte[]? GetUserImage(string username);
    }
}
