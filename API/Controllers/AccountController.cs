using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManger;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(UserManager<AppUser> userManger, ITokenService tokenService, IMapper mapper)
    {
        _userManger = userManger;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("register")] // POST: api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExists(registerDto.Username))
        {
            return BadRequest("Username is already taken.");
        }

        var user = _mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.Username.ToLower();
        //This is a hashing algorithm that uses SHA512
        //using is added as we want the hmac variable to to be disposed of in memory by the garbage collector after using the 
        // using var hmac = new HMACSHA512();

        // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        // user.PasswordSalt = hmac.Key;

        var result = await _userManger.CreateAsync(user, registerDto.Password);
        
        if(!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        var roleResult = await _userManger.AddToRoleAsync(user, "Member");

        if(!roleResult.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return new UserDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login (LoginDto loginDto)
    {
        var user = await _userManger.Users.Include(p => p.Photos).FirstOrDefaultAsync(user => user.UserName == loginDto.Username);

        if(user == null) return Unauthorized("Invalid username.");

        //This gets the hased version of the password in the database
        //using var hmac =  new HMACSHA512(user.PasswordSalt);

        //The following gets the hash of the password the user entered. 
        //var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        //The hashes of the password in the database and the password entered by the user are then compared.
        //for (int i = 0; i < computedHash.Length; i++)
        //{
        //    if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        //}

        var result = await _userManger.CheckPasswordAsync(user, loginDto.Password);

        if(!result) return Unauthorized("Invalid Password");

        return new UserDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }

    private async Task<bool> UserExists (string username)
    {
        return await _userManger.Users.AnyAsync(user => user.UserName == username.ToLower());
    }

}
