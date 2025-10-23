using FamilyArchive.Application.DTOs;
using FamilyArchive.Application.Services;
using FamilyArchive.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Metadata;

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
    public IActionResult AddMember([FromBody] MemberDto dto)
    {
        var memberId = _memberService.AddMemberFromDto(dto);
        _memberService.SaveMemberChanges();
        return CreatedAtAction(nameof(GetMember), new { memberId }, memberId);
    }

    // Get a member by ID
    [HttpGet("{memberId}")]
    public IActionResult GetMember(Guid memberId)
    {
        try
        {
            var memberDto = _memberService.GetMemberById(memberId);
            return Ok(memberDto);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    // Update a member
    [HttpPut("{memberId}")]
    public IActionResult UpdateMember(Guid memberId, [FromBody] MemberDto dto)
    {
        try
        {
            _memberService.UpdateMemberById(memberId, dto);
            _memberService.SaveMemberChanges();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    // Add a name to a member
    [HttpPost("{memberId}/names")]
    public IActionResult AddName(Guid memberId, [FromBody] MemberNameDto dto)
    {
        try
        {
            _memberService.AddNameToMember(memberId, dto);
            _memberService.SaveMemberChanges();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    // Update a name of a member
    [HttpPut("{memberId}/names/{nameId}/value")]
    public IActionResult UpdateName(Guid memberId, Guid nameId, [FromBody] string newName)
    {
        try
        {
            _memberService.UpdateNameOfMember(memberId, nameId, newName);
            _memberService.SaveMemberChanges();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update the order of a name of a member
    [HttpPut("{memberId}/names/{nameId}/order")]
    public IActionResult UpdateNameOrder(Guid memberId, Guid nameId, [FromBody] int newOrder)
    {
        try
        {
            _memberService.UpdateNameOrderOfMember(memberId, nameId, newOrder);
            _memberService.SaveMemberChanges();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update the type of a name of a member
    [HttpPut("{memberId}/names/{nameId}/type")]
    public IActionResult UpdateNameType(Guid memberId, Guid nameId, [FromBody] UpdateNameTypeRequest request)
    {
        try
        {
            _memberService.UpdateNameTypeOfMember(memberId, nameId, request.NewType, request.OtherNameType);
            _memberService.SaveMemberChanges();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Update the hidden status of a name of a member
    [HttpPut("{memberId}/names/{nameId}/hidden")]
    public IActionResult UpdateNameHidden(Guid memberId, Guid nameId, [FromBody] bool hidden)
    {
        try
        {
            _memberService.UpdateNameHiddenOfMember(memberId, nameId, hidden);
            _memberService.SaveMemberChanges();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}