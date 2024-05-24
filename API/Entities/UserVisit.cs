namespace API.Entities;

public class UserVisit
{
    public AppUser SourceUser { get; set; }
    public int SourceUserId { get; set; }
    public AppUser TargetUser { get; set; }
    public int TargetUserId { get; set; }
    public DateTime DateVisited { get; set; } = DateTime.UtcNow;
}
