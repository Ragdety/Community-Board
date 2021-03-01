using CommunityBoard.Core.DomainObjects;
using System.Threading.Tasks;

namespace CommunityBoard.Core.Interfaces
{
    public interface IIdentityRepository
    {
        Task<AuthenticationResult> RegisterAsync(
            string firstName, string lastName, string userName, string email, string password);

    }
}
