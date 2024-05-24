namespace API.Helpers;

public class VisitsParams : PaginationParams
{
    public int UserId { get; set; }
    public string Predicate { get; set; } = "visited";
    public string OrderBy { get; set; } = "All";
}
