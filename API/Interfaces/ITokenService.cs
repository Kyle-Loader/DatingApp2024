using API.Entities;

namespace API.Interfaces
{
    //This service is used to create a token which each user will use to access code that requires authentication.
    //The token allows for authentication without any database calls.
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}