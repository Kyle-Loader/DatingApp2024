using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
   private readonly DataContext _context;
   private readonly ITokenService _tokenService;
    public AccountController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")] // POST: api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExists(registerDto.Username))
        {
            return BadRequest("Username is already taken.");
        }

        //This is a hashing algorithm that uses SHA512
        //using is added as we want the hmac variable to to be disposed of in memory by the garbage collector after using the 
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login (LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == loginDto.Username);

        if(user == null) return Unauthorized("Invalid username.");

        //This gets the hased version of the password in the database
        using var hmac =  new HMACSHA512(user.PasswordSalt);

        //The following gets the hash of the password the user entered. 
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        //The hashes of the password in the database and the password entered by the user are then compared.
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }

        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExists (string username)
    {
        return await _context.Users.AnyAsync(user => user.UserName == username.ToLower());
    }

}
