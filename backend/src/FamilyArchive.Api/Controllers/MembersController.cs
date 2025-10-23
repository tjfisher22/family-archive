using FamilyArchive.Application.DTOs;
using FamilyArchive.Application.Services;
using FamilyArchive.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FamilyArchive.Api.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    //Add a name to a member
    [HttpPost("{memberId}/names")]
    public IActionResult AddName(Guid memberId, [FromBody] MemberNameDto dto)
    {
        // Replace placeholder with actual DB call
        var member = _memberService.GetMemberById(memberId);
        if (member == null)
        {
            return NotFound($"Member with ID {memberId} not found.");
        }

        var name = new MemberName
        {
            Id = Guid.NewGuid(),
            MemberId = memberId,
            Value = dto.Value,
            Type = dto.Type,
            OtherNameType = dto.OtherNameType,
            Order = dto.Order,
            Hidden = dto.Hidden
        };

        try
        {
            _memberService.AddName(member, name);
            // Save changes to the database here (not shown)
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}