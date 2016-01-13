using System;
using System.Linq;
using System.Threading.Tasks;
using Naylor.Data;

namespace Naylor.Services.Users
{
    public class UserService:IUserService
    {
        private IRepository<User> _userRepository;
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUser(CreateUser createUser)
        {
            try
            {
                var user = new User()
                    {
                        Salary = createUser.Salary,
                        HourlyRate = createUser.HourlyRate
                    };

                user = _userRepository.Save(user);
                return await Task.FromResult(user);

            }
            catch (Exception exception)
            {
                Console.Out.WriteLine(exception.Message);
            }
            return await Task.FromResult<User>(null);
        }

        public async Task<User> GetUserById(GetUserById getUserById)
        {
            try
            {
                var user = _userRepository.AsQueryable().First(p => p.Id == getUserById.Id);
                return await Task.FromResult(user);

            }
            catch (Exception exception)
            {
                Console.Out.WriteLine(exception.Message);
            }
            return await Task.FromResult<User>(null);
        }
    }
}
