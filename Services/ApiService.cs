using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<ProviderLocation>> GetItemsAsync(ProviderLocationHandle[] handlers) {
            throw new NotImplementedException();
        }
    }
}
