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
    private readonly IMemberNameService _memberNameService;

    public MembersController(IMemberService memberService, IMemberNameService memberNameService)
    {
        _memberService = memberService;
        _memberNameService = memberNameService;
    }
    // Add a new member
    [HttpPost]
    public IActionResult AddMember([FromBody] MemberDto dto)
    {
        var memberId = _memberService.AddMemberFromDto(dto);
        _memberService.SaveChanges();
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
            _memberService.SaveChanges();
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
            _memberNameService.AddNameToMember(memberId, dto);
            _memberNameService.SaveChanges();
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
            _memberNameService.UpdateNameOfMember(memberId, nameId, newName);
            _memberNameService.SaveChanges();
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
            _memberNameService.UpdateNameOrderOfMember(memberId, nameId, newOrder);
            _memberNameService.SaveChanges();
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
            _memberNameService.UpdateNameTypeOfMember(memberId, nameId, request.NewType, request.OtherNameType);
            _memberNameService.SaveChanges();
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
            _memberNameService.UpdateNameHiddenOfMember(memberId, nameId, hidden);
            _memberNameService.SaveChanges();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}