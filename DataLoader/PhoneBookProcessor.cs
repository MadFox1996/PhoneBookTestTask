using Data;
using DataLoader.Model;
using DataLoader.Mapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace DataLoader
{
    public class PhoneBookProcessor : IPhoneBookProcessor
    {
        private readonly IRepository _db;
        private readonly ILogger<PhoneBookProcessor> _log;
        private readonly IConfiguration _config;

        public PhoneBookProcessor(ILogger<PhoneBookProcessor> log, IRepository db, IConfiguration config)
        {
            _log = log;
            _db = db;
            _config = config;
        }

        public async Task LoadPhoneBook()
        {
            int loadAmount = _config.GetValue<int>("LoadAmount");
            string urlPattern = $"?results={loadAmount}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(urlPattern))
            {
                if (response.IsSuccessStatusCode)
                {
                    var phoneBookEntryModels = await response.Content.ReadAsAsync<PhoneBookResultDTO>();

                    await LoadImages(phoneBookEntryModels.Results);

                    var result = _db.AddPhoneBookEntries(phoneBookEntryModels.Results.ToModel());

                    if (result)
                    {
                        _log.LogInformation("Data loaded successfully");
                    }
                    else
                    {
                        _log.LogError("Error saving data");
                    }
                }
                else
                {
                    _log.LogError("Error loading data");
                }
            }
           
        }

        private async Task LoadImages(ICollection<PhoneBookEntryDTO> phoneBookEntryModel)
        {
            foreach (var phoneBookEntry in phoneBookEntryModel)
            {
                string uri = phoneBookEntry.picture.medium;
                var response = await ApiHelper.ApiClient.GetByteArrayAsync(uri);
                phoneBookEntry.picture.mediumImageData = response;
            }
        }
    }
}
