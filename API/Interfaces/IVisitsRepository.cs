using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IVisitsRepository
{
    Task<UserVisit> GetUserVisit (int sourceUserId, int targetUserId);
    Task<AppUser> GetUserWithVisits(int userId);
    Task<PagedList<VisitDto>> GetUserVisits(VisitsParams visitsParams);
    Task AddUserVisit(UserVisit userVisit);
    void Update(UserVisit userVisit);
}
