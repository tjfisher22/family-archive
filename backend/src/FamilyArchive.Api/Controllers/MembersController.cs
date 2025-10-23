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

    // Add a name to a member
    [HttpPost("{memberId}/names")]
    public IActionResult AddName(Guid memberId, [FromBody] MemberNameDto dto)
    {
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
            _memberService.SaveMemberChanges();
            // Save changes to the database here (not shown)
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Add a new member
    [HttpPost]
    public IActionResult AddMember([FromBody] MemberDto dto)
    {
        var member = new Member
        {
            Id = Guid.NewGuid(),
            BirthDate = dto.BirthDate,
            DeathDate = dto.DeathDate,
            Gender = dto.Gender,
            FamilyId = dto.FamilyId
        };

        // Optionally add names if provided
        if (dto.Names != null)
        {
            foreach (var nameDto in dto.Names)
            {
                var name = new MemberName
                {
                    Id = Guid.NewGuid(),
                    MemberId = member.Id,
                    Value = nameDto.Value,
                    Type = nameDto.Type,
                    OtherNameType = nameDto.OtherNameType,
                    Order = nameDto.Order,
                    Hidden = nameDto.Hidden
                };
                member.AddName(name);
            }
        }

        // Persist the new member
        _memberService.AddMember(member);
        _memberService.SaveMemberChanges();

        return CreatedAtAction(nameof(GetMember), new { memberId = member.Id }, member);
    }

    // Get a member by ID
    [HttpGet("{memberId}")]
    public IActionResult GetMember(Guid memberId)
    {
        var member = _memberService.GetMemberById(memberId);
        if (member == null)
        {
            return NotFound();
        }
        return Ok(member);
    }

    // Update a member
    [HttpPut("{memberId}")]
    public IActionResult UpdateMember(Guid memberId, [FromBody] MemberDto dto)
    {
        var member = _memberService.GetMemberById(memberId);
        if (member == null)
        {
            return NotFound($"Member with ID {memberId} not found.");
        }

        try
        {
            _memberService.UpdateMember(member, dto);
            _memberService.SaveMemberChanges();
            return Ok(member);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update a name of a member
    [HttpPut("{memberId}/names/{nameId}")]
    public IActionResult UpdateName(Guid memberId, Guid nameId, [FromBody] MemberNameDto dto)
    {
        var member = _memberService.GetMemberById(memberId);
        if (member == null)
            return NotFound($"Member with ID {memberId} not found.");

        dto.Id = nameId; // Ensure the correct name is updated

        try
        {
            _memberService.UpdateName(member, dto);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}