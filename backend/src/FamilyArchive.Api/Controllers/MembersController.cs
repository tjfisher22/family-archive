using FamilyArchive.Application.DTOs;
using FamilyArchive.Application.Services;
using FamilyArchive.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
    // Add a new member
    [HttpPost]
    public async Task<IActionResult> AddMember([FromBody] MemberDto dto)
    {
        var memberId = await _memberService.AddMemberFromDtoAsync(dto);
        await _memberService.SaveMemberChangesAsync();
        return CreatedAtAction(nameof(GetMember), new { memberId }, memberId);
    }

    // Get a member by ID
    [HttpGet("{memberId}")]
    public async Task<IActionResult> GetMember(Guid memberId)
    {
        try
        {
            var memberDto = await _memberService.GetMemberByIdAsync(memberId);
            return Ok(memberDto);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    // Update a member
    [HttpPut("{memberId}")]
    public async Task<IActionResult> UpdateMember(Guid memberId, [FromBody] MemberDto dto)
    {
        try
        {
            await _memberService.UpdateMemberByIdAsync(memberId, dto);
            await _memberService.SaveMemberChangesAsync();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    // Add a name to a member
    [HttpPost("{memberId}/names")]
    public async Task<IActionResult> AddName(Guid memberId, [FromBody] MemberNameDto dto)
    {
        try
        {
            await _memberService.AddNameToMemberAsync(memberId, dto);
            await _memberService.SaveMemberChangesAsync();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    // Update a name of a member
    [HttpPut("{memberId}/names/{nameId}/value")]
    public async Task<IActionResult> UpdateName(Guid memberId, Guid nameId, [FromBody] string newName)
    {
        try
        {
            await _memberService.UpdateNameOfMemberAsync(memberId, nameId, newName);
            await _memberService.SaveMemberChangesAsync();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update the order of a name of a member
    [HttpPut("{memberId}/names/{nameId}/order")]
    public async Task<IActionResult> UpdateNameOrder(Guid memberId, Guid nameId, [FromBody] int newOrder)
    {
        try
        {
            await _memberService.UpdateNameOrderOfMemberAsync(memberId, nameId, newOrder);
            await _memberService.SaveMemberChangesAsync();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update the type of a name of a member
    [HttpPut("{memberId}/names/{nameId}/type")]
    public async Task<IActionResult> UpdateNameType(Guid memberId, Guid nameId, [FromBody] UpdateNameTypeRequest request)
    {
        try
        {
            await _memberService.UpdateNameTypeOfMemberAsync(memberId, nameId, request.NewType, request.OtherNameType);
            await _memberService.SaveMemberChangesAsync();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update the hidden status of a name of a member
    [HttpPut("{memberId}/names/{nameId}/hidden")]
    public async Task<IActionResult> UpdateNameHidden(Guid memberId, Guid nameId, [FromBody] bool hidden)
    {
        try
        {
            await _memberService.UpdateNameHiddenOfMemberAsync(memberId, nameId, hidden);
            await _memberService.SaveMemberChangesAsync();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}