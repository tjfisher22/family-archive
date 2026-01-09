using System;
using FamilyArchive.Domain.Services;

namespace FamilyArchive.Application.Services;

public interface IRelationshipService
{
    string CalculateRelationship(Guid memberAId, Guid memberBId);
}

public class RelationshipService : IRelationshipService
{
    private readonly IMemberRepository _repository;
    private readonly RelationshipCalculator _calculator;
    
    public RelationshipService(IMemberRepository repository, RelationshipCalculator calculator)
    {
        _repository = repository;
        _calculator = calculator;
    }
    
    public string CalculateRelationship(Guid memberAId, Guid memberBId)
    {
        var memberA = _repository.GetMemberById(memberAId);
        if (memberA == null)
            throw new InvalidOperationException($"Member with ID {memberAId} not found.");
        
        var memberB = _repository.GetMemberById(memberBId);
        if (memberB == null)
            throw new InvalidOperationException($"Member with ID {memberBId} not found.");
        
        return _calculator.CalculateRelationship(memberA, memberB);
    }
}