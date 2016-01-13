using Nancy;
using Nancy.ModelBinding;
using Naylor.Gateway.Models;
using Naylor.Services;

namespace Naylor.Gateway.Modules
{
    public class UserModule:NancyModule
    {
           public UserModule(IUserService userService):base("/users")
           {
               Get["/{id}", true] = async (ctx, cancel) =>
                   {
                       var request = this.Bind<GetUserByIdRequest>();

                       var user = await userService.GetUserById(new GetUserById()
                           {
                               Id = request.Id
                           });
                       return Response.AsJson(user);
                   };

               Post["/", true] = async (ctx, cancel) =>
                   {
                       var request = this.Bind<CreateUserRequest>();

                       var user = await userService.CreateUser(new CreateUser()
                           {
                               HourlyRate = request.HourlyRate,
                               Salary = request.Salary
                           });
                       
                       return Response.AsJson(user);
                   };
           }
    }
}
