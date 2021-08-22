using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_project_01.Models;
using test_project_01.Repositories;

namespace test_project_01.Services
{
    public class ApiService
    {
        private readonly ProfileRepository profileRepository;
        private readonly ProviderRepository providerRepository;

        public ApiService(ProfileRepository profileRepository, ProviderRepository providerRepository) {
            this.profileRepository = profileRepository;
            this.providerRepository = providerRepository;
        }

        public async Task<IEnumerable<ProviderLocation>> GetItemsAsync(ProviderLocationHandle[] handlers)
        {
            // Creating Provider-to-Location dictionary, saving Index via 'index' field of tuple
            var locationCodesByProviderId = handlers
                .Select((handler, index) => (handler, index))
                .ToDictionary(
                    item => item.handler.ProviderId,
                    item => (item.handler.LocationCode, item.index)
                );

            // Creating Provider-to-Profile dictionary for active providers.
            var profileIdsByProviderId = (await providerRepository
                .FindAsync(locationCodesByProviderId.Keys.ToArray()))
                .Where(prov => prov.Status == ProvideStatus.Active)
                .ToDictionary(
                    prov => prov.Id,
                    prov => prov.ActiveProfileId
                );

            // Creating Profile-to-InitialOrder dictionary.
            var profileOrders = profileIdsByProviderId.ToDictionary(
                pair => pair.Value,
                pair => locationCodesByProviderId[pair.Key].index
            );

            // Retrieving profiles.
            var profiles = await profileRepository
                .FindAsync(profileIdsByProviderId.Values.ToArray());

            // Creating list.
            var resultList = new List<(ProviderLocation Item, int Order)>();
            foreach (var profile in profiles)
            {
                var order = profileOrders[profile.Id];
                var result = new ProviderLocation
                {
                    ProviderId = profile.ProviderId,
                };
                
                // Case when no provider was found.
                if (!profileIdsByProviderId.TryGetValue(profile.ProviderId, out var activeProfileId))
                {
                    result.Status = ProviderLocationStatus.ProvidrNotFound;
                    resultList.Add((result, order));
                    continue;
                }

                // Case when provider found, but ActiveProfileId not equals to Profile.Id
                if (profile.Id != activeProfileId)
                {
                    result.Status = ProviderLocationStatus.InvalidProfile;
                    resultList.Add((result, order));
                    continue;
                }

                var locationCode = locationCodesByProviderId[profile.ProviderId].LocationCode;
                var location = profile.Locations.FirstOrDefault(loc => loc.code == locationCode);
                // Case when no Location found by code inside the Profile
                if (location == null)
                {
                    result.Status = ProviderLocationStatus.LocationNotFound;
                    result.Profile = profile;
                    result.Location = profile.Locations.First();
                    resultList.Add((result, order));
                    continue;
                }

                // Happy case when Profile and Location found.
                result.Found = true;
                result.Status = ProviderLocationStatus.Found;
                result.Profile = profile;
                result.Location = location;
                resultList.Add((result, order));
            }

            // Return items ordering them by Initial Order of Provider-Location handlers asked.
            return resultList
                .OrderBy(item => item.Order)
                .Select(item => item.Item);
        }
    }
}
