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
    private readonly IMemberRelationshipService _memberRelationshipService;

    public MembersController(IMemberService memberService, IMemberNameService memberNameService, IMemberRelationshipService memberRelationshipService)
    {
        _memberService = memberService;
        _memberNameService = memberNameService;
        _memberRelationshipService = memberRelationshipService;
    }
    #region Member CRUD operations
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

    // Get all members
    [HttpGet]
    public IActionResult GetAllMembers()
    {
        var members = _memberService.GetAllMembers();
        return Ok(members);
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
    // Delete a member
    [HttpDelete("{memberId}")]
    public IActionResult DeleteMember(Guid memberId)
    {
        try
        {
            _memberService.RemoveMemberById(memberId);
            _memberService.SaveChanges();
            return Ok();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
    //Update Gender of a member
    [HttpPut("{memberId}/gender")]
    public IActionResult UpdateGender(Guid memberId, [FromBody] UpdateGenderRequest request)
    {
        try
        {
            _memberService.UpdateGenderOfMember(memberId, request.Gender, request.OtherGender);
            _memberService.SaveChanges();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion
    #region Member Name operations
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
    // Delete a name from a member
    [HttpDelete("{memberId}/names/{nameId}")]
    public IActionResult DeleteName(Guid memberId, Guid nameId)
    {
        try
        {
            _memberNameService.RemoveNameFromMember(memberId, nameId);
            _memberNameService.SaveChanges();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion
    #region Member Relationship operations
    // Add a child to a member
    [HttpPost("{memberId}/children")]
    public IActionResult AddChildToMember(Guid memberId, [FromBody] AddChildRequest request)
    {
        try
        {
            _memberRelationshipService.AddChildToMember(memberId, request.childId, request.RelationshipType, request.OtherRelationshipType, request.EstablishedDate);
            _memberRelationshipService.SaveChanges();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    // Remove a child from a member
    [HttpDelete("{memberId}/children/{childId}")]
    public IActionResult RemoveChildFromMember(Guid memberId, Guid childId)
    {
        try
        {
            _memberRelationshipService.RemoveChildFromMember(memberId, childId);
            _memberRelationshipService.SaveChanges();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Add Partner to a member
    [HttpPost("{memberId}/partners")]
    public IActionResult AddPartnerToMember(Guid memberId, [FromBody] AddPartnerRequest request)
    {
        try
        {
            _memberRelationshipService.AddPartnerToMember(memberId, request.PartnerId, request.PartnershipType, request.OtherPartnershipType, request.StartDate);
            _memberRelationshipService.SaveChanges();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    // End a partnership
    [HttpPost("{memberId}/partnerships/{partnershipId}/end")]
    public IActionResult EndPartnership(Guid memberId, Guid partnershipId, [FromBody] DateTime endDate)
    {
        try
        {
            _memberRelationshipService.EndPartnership(memberId, partnershipId, endDate);
            _memberRelationshipService.SaveChanges();
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion
}