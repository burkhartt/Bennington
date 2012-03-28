using System.Linq;
using Bennington.Cms.PrincipalProvider.Repositories;

namespace Bennington.Cms.PrincipalProvider.Helpers
{
    public interface IDetermineWhoHasTheAbilityToPublish
    {
        bool DetermineIfThisUserCanPublish(string username);
    }

    public class DetermineWhoHasTheAbilityToPublish : IDetermineWhoHasTheAbilityToPublish
    {
        private readonly IUserRepository userRepository;

        public DetermineWhoHasTheAbilityToPublish(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public bool DetermineIfThisUserCanPublish(string username)
        {
            var user = userRepository.GetAll().FirstOrDefault(x => x.Username == username);
            return user.UserType == "Publish";
        }
    }
}