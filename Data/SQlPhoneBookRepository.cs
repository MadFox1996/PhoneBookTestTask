using Data.EntityModel;
using Data.Hash;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class SQlPhoneBookRepository : IRepository
    {
        private readonly IHashingHelper _hashing;
        private readonly PhoneBookContext _db;
        private bool _disposed = false;

        public SQlPhoneBookRepository(IHashingHelper hashing)
        {
            _db = GetDbContext();
            _hashing = hashing;
        }

        public bool AddPhoneBookEntries(IEnumerable<PhoneBookEntry> phoneBookEntries)
        {
            HashPhoneBookEntryPasswords(ref phoneBookEntries);
            _db.PhoneBookEntries.AddRange(phoneBookEntries);
            return _db.SaveChanges() >= 1;
        }

        public IQueryable<PhoneBookEntry> GetPhoneBookEntries(PhoneBookEntryPagination phoneBookEntryParameters)
        {
            var result = _db.PhoneBookEntries.AsQueryable();
                
            ApplySort(ref result, phoneBookEntryParameters.NameOrderBy, phoneBookEntryParameters.DateOfBirthOrderBy);

            return result
                .Skip((phoneBookEntryParameters.PageNumber - 1) * phoneBookEntryParameters.PageSize)
                .Take(phoneBookEntryParameters.PageSize); ;
                
        }

        public User? AddUser(User user)
        {
            if(_db.Users.Any(u=>u.Login == user.Login))
            {
                return null;
            }           
            _db.Users.Add(user);
            _db.SaveChanges();
            return user;
        }

        public User? GetUser(string login)
        {
            return _db.Users.FirstOrDefault(u => u.Login == login);
        }

        public byte[]? GetUserImage(string username)
        {
            return _db.PhoneBookEntries.FirstOrDefault(p => p.Name == username)?.MediumImageData;
        }

        public void Dispose(bool disposing)
        {
            if (!this._disposed && disposing)
            {
                _db.Dispose();
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private PhoneBookContext GetDbContext()
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory());

            builder.AddJsonFile("appsettings.json");

            var config = builder.Build();

            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<PhoneBookContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            return new PhoneBookContext(options);
        }

        private void HashPhoneBookEntryPasswords(ref IEnumerable<PhoneBookEntry> phoneBookEntries)
        {
            IList<PhoneBookEntry> output = new List<PhoneBookEntry>();

            foreach (PhoneBookEntry phoneBookEntry in phoneBookEntries)
            {
                _hashing.CreatePasswordHash(phoneBookEntry.Password, out byte[] hash, out byte[] salt);
                phoneBookEntry.PasswordHash = hash;
                phoneBookEntry.PasswordSalt = salt;
                output.Add(phoneBookEntry);
            }
            phoneBookEntries = output;
        }

        private void ApplySort(ref IQueryable<PhoneBookEntry?> phoneBookEntries, string nameOrderBy, string dateOfBirthOrderBy)
        {
            if (!phoneBookEntries.Any())
                return;

            IOrderedQueryable<PhoneBookEntry?> output;

            switch (nameOrderBy)
            {
                case "desc":
                    output = phoneBookEntries.OrderByDescending(x => x.Name);
                    break;
                default:
                    output = phoneBookEntries.OrderBy(x => x.Name);
                    break;
            }

            switch (dateOfBirthOrderBy)
            {
                case "desc":
                    phoneBookEntries = output.ThenByDescending(x => x.Birthday);
                    break;
                default:
                    phoneBookEntries = output.ThenBy(x => x.Birthday);
                    break;
            }
        }
    }
}
