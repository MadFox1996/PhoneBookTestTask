using Data.EntityModel;
using DataLoader.Model;

namespace DataLoader.Mapper
{
    public static class PhoneBookEntryExtensions
    {
        public static IEnumerable<PhoneBookEntry> ToModel(this IEnumerable<PhoneBookEntryDTO> source)
        {
            return source.Select(model => new PhoneBookEntry
            {
                Id = Guid.Parse(model.login.uuid),
                Name = $"{model.name.title} {model.name.first} {model.name.last} ",
                Email = model.email,
                Birthday = model.dob.date,
                Address = $"{model.location.street.number} {model.location.street.name}",
                PhoneNumber = model.phone,
                Password = model.login.password,
                MediumImageUrl = model.picture.medium,
                MediumImageData = model.picture.mediumImageData
            });
        }
    }
}
