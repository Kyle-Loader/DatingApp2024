using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class VisitsRepository : IVisitsRepository
{
    private readonly DataContext _context;
    public VisitsRepository (DataContext context)
    {
        _context = context;
    }

    public async Task<UserVisit> GetUserVisit(int sourceUserId, int targetUserId)
    {
        return await _context.Visits.FindAsync(sourceUserId, targetUserId);
    }

    public async Task AddUserVisit(UserVisit userVisit)
    {
        await _context.Visits.AddAsync(userVisit);
    }

    public void Update(UserVisit userVisit)
    {
        _context.Entry(userVisit).State = EntityState.Modified;
    }

    public async Task<PagedList<VisitDto>> GetUserVisits(VisitsParams visitsParams)
    {
        var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
        var visitQuery = _context.Visits.AsQueryable();

        visitQuery = visitsParams.OrderBy switch
        {
            "pastMonth" => visitQuery.Where(x => x.DateVisited >= DateTime.UtcNow.AddDays(-30)),
            "All" => visitQuery.OrderBy(x => x.DateVisited),
            _ => visitQuery
        };

        if (visitsParams.Predicate == "visited")
        {
            visitQuery = visitQuery.Where(visit => visit.SourceUserId == visitsParams.UserId);
            users = visitQuery.Select(visit => visit.TargetUser);
        }

        if (visitsParams.Predicate == "visitedby")
        {
            visitQuery = visitQuery.Where(visit => visit.TargetUserId == visitsParams.UserId);
            users = visitQuery.Select(visit => visit.SourceUser);
        }

        var visitedUsers =  users.Select(user => new VisitDto
        {
            UserName = user.UserName,
            KnownAs = user.KnownAs,
            Age = user.DateOfBirth.CalculateAge(),
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
            City = user.City,
            Id = user.Id
        });

        return await PagedList<VisitDto>.CreateAsync(visitedUsers, visitsParams.PageNumber, visitsParams.PageSize);
    }

    public async Task<AppUser> GetUserWithVisits(int userId)
    {
        return await _context.Users.Include(x => x.VisitedUsers).FirstOrDefaultAsync(x => x.Id == userId);
    }
}
