using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class VisitsController : BaseApiController
{
     private readonly IUnitOfWork _uow;

    public VisitsController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddVisit(string username)
    {
        var sourceUserId = User.GetUserId();
        var targetUser = await _uow.UserRepository.GetUserByUsernameAsync(username);
        var sourceUser = await _uow.VisitsRepository.GetUserWithVisits(sourceUserId);

        if (targetUser == null) return NotFound();

        if (sourceUser == null) return NotFound();

        if (sourceUser.UserName == username) return Ok();

        var existingVisit = await _uow.VisitsRepository.GetUserVisit(sourceUserId, targetUser.Id);

        if (existingVisit != null)
        {
            existingVisit.DateVisited = DateTime.UtcNow;
            _uow.VisitsRepository.Update(existingVisit);
            return Ok();
        }

        var newVisit = new UserVisit
        {
            SourceUserId = sourceUserId,
            TargetUserId = targetUser.Id,
            DateVisited = DateTime.UtcNow
        };

        await _uow.VisitsRepository.AddUserVisit(newVisit);

        if (await _uow.Complete()) return Ok();

        return BadRequest("Failed to record visit.");
    }

    [Authorize(Policy = "RequireVIPRole")]
    [HttpGet]
    public async Task<ActionResult<PagedList<VisitDto>>> GetUserVisits([FromQuery]VisitsParams visitsParams)
    {
        visitsParams.UserId = User.GetUserId();

        var users =  await _uow.VisitsRepository.GetUserVisits(visitsParams);

        Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

        return Ok(users);
    }
}
