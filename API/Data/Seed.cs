﻿using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task ClearConnections(DataContext context)
    {
        context.Connections.RemoveRange(context.Connections);
        await context.SaveChangesAsync();
    }
    // public static async Task SeedUsers(DataContext context)
    // {
    //     if (await context.Users.AnyAsync()) return;

    //     var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

    //     var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

    //     var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);
        
    //     foreach (var user in users)
    //     {
    //         using var hmac = new HMACSHA512();

    //         user.UserName = user.UserName.ToLower();
    //         // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
    //         // user.PasswordSalt = hmac.Key;

    //         context.Users.Add(user);
    //     }

    //     await context.SaveChangesAsync();
    // }
    public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

        var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

        var roles = new List<AppRole>{
            new() {Name = "Member"},
            new() {Name = "Admin"},
            new() {Name = "Moderator"}
        };

        foreach(var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var user in users)
        {
            user.UserName = user.UserName.ToLower();
            user.Created = DateTime.SpecifyKind(user.Created, DateTimeKind.Utc);
            user.LastActive = DateTime.SpecifyKind(user.LastActive, DateTimeKind.Utc);
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Member");
        }

        var admin = new AppUser
        {
            UserName = "admin"
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRolesAsync(admin, new [] {"Admin", "Moderator"});
    }
}
