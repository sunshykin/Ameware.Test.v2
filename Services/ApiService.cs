using System;
using System.Collections.Generic;
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

        public IEnumerable<ProviderLocation> GetItems(ProviderLocationHandle[] handlers) {
            throw new NotImplementedException();
        }
    }
}
