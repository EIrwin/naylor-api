using System.Threading.Tasks;
using Naylor.Data;

namespace Naylor.Services
{
    public interface IUserService
    {
        Task<User> CreateUser(CreateUser createUser);
        Task<User> GetUserById(GetUserById getUserById);
    }
}
