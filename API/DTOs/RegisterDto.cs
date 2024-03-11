using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

//This is a Data Transfer Object, this object encapsulates data, and sends it from one subsystem to another. In this case it is from the API to the Client.
public class RegisterDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}