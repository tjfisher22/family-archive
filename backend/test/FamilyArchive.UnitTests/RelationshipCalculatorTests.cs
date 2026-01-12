using FamilyArchive.Domain.Entities;
using FamilyArchive.Domain.Enums;
using FamilyArchive.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyArchive.UnitTests;

public class RelationshipCalculatorTests
{
    private readonly RelationshipCalculator _calculator;
    public RelationshipCalculatorTests()
    {
        _calculator = new RelationshipCalculator();
    }

    #region Self Relationship Test
    [Fact]
    public void CalculateClosestRelationship_SameMember_ReturnsSelf()
    {
        // Arrange
        var member = CreateMember(Gender.Male);
        // Act
        var result = _calculator.CalculateClosestRelationship(member, member);
        // Assert
        Assert.Equal("Self", result);
    }

    [Fact]
    public void CalculateAllRelationships_SameMember_ReturnsSelf()
    {
        // Arrange
        var member = CreateMember(Gender.Male);
        // Act
        var result = _calculator.CalculateAllRelationships(member, member);
        // Assert
        Assert.Single(result);
        Assert.Equal("Self", result[0]);
    }
    #endregion

    #region Parent-Child Tests

    [Fact]
    public void CalculateClosestRelationship_DirectChild_ReturnsChild()
    {
        // Arrange
        var parent = CreateMember(Gender.Female);
        var child = CreateMember(Gender.Male);
        parent.AddChild(child, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(parent, child);

        // Assert
        Assert.Equal("Son", result);
    }

    [Fact]
    public void CalculateAllRelationships_DirectChild_ReturnsChild()
    {
        // Arrange
        var parent = CreateMember(Gender.Female);
        var child = CreateMember(Gender.NonBinary);
        parent.AddChild(child, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateAllRelationships(parent, child);

        // Assert
        Assert.Single(result);
        Assert.Equal("Child", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_BiologicalMother_ReturnsBiologicalMother()
    {
        // Arrange
        var mother = CreateMember(Gender.Female);
        var child = CreateMember(Gender.Male);
        mother.AddChild(child, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(child, mother);

        // Assert
        Assert.Equal("Biological Mother", result);
    }

    [Fact]
    public void CalculateAllRelationships_BiologicalMother_ReturnsBiologicalMother()
    {
        // Arrange
        var mother = CreateMember(Gender.Female);
        var child = CreateMember(Gender.Male);
        mother.AddChild(child, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateAllRelationships(child, mother);

        // Assert
        Assert.Single(result);
        Assert.Equal("Biological Mother", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_BiologicalFather_ReturnsBiologicalFather()
    {
        // Arrange
        var father = CreateMember(Gender.Male);
        var child = CreateMember(Gender.Female);
        father.AddChild(child, RelationshipType.BiologicalFather, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(child, father);

        // Assert
        Assert.Equal("Biological Father", result);
    }

    [Fact]
    public void CalculateAllRelationships_BiologicalFather_ReturnsBiologicalFather()
    {
        // Arrange
        var father = CreateMember(Gender.Male);
        var child = CreateMember(Gender.Female);
        father.AddChild(child, RelationshipType.BiologicalFather, null);

        // Act
        var result = _calculator.CalculateAllRelationships(child, father);

        // Assert
        Assert.Single(result);
        Assert.Equal("Biological Father", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_AdoptiveParent_ReturnsAdoptiveMother()
    {
        // Arrange
        var adoptiveMother = CreateMember(Gender.Female);
        var child = CreateMember(Gender.Male);
        adoptiveMother.AddChild(child, RelationshipType.AdoptiveMother, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(child, adoptiveMother);

        // Assert
        Assert.Equal("Adoptive Mother", result);
    }

    [Fact]
    public void CalculateAllRelationships_AdoptiveParent_ReturnsAdoptiveMother()
    {
        // Arrange
        var adoptiveMother = CreateMember(Gender.Female);
        var child = CreateMember(Gender.Male);
        adoptiveMother.AddChild(child, RelationshipType.AdoptiveMother, null);

        // Act
        var result = _calculator.CalculateAllRelationships(child, adoptiveMother);

        // Assert
        Assert.Single(result);
        Assert.Equal("Adoptive Mother", result[0]);
    }

    #endregion

    #region Sibling Tests

    [Fact]
    public void CalculateClosestRelationship_FullSiblings_ReturnsFullSister()
    {
        // Arrange
        var mother = CreateMember(Gender.Female);
        var father = CreateMember(Gender.Male);
        var childA = CreateMember(Gender.Male);
        var childB = CreateMember(Gender.Female);

        mother.AddChild(childA, RelationshipType.BiologicalMother, null);
        father.AddChild(childA, RelationshipType.BiologicalFather, null);
        mother.AddChild(childB, RelationshipType.BiologicalMother, null);
        father.AddChild(childB, RelationshipType.BiologicalFather, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(childA, childB);

        // Assert
        Assert.Equal("Full Sister", result);
    }

    [Fact]
    public void CalculateAllRelationships_FullSiblings_ReturnsFullSister()
    {
        // Arrange
        var mother = CreateMember(Gender.Female);
        var father = CreateMember(Gender.Male);
        var childA = CreateMember(Gender.Male);
        var childB = CreateMember(Gender.Female);

        mother.AddChild(childA, RelationshipType.BiologicalMother, null);
        father.AddChild(childA, RelationshipType.BiologicalFather, null);
        mother.AddChild(childB, RelationshipType.BiologicalMother, null);
        father.AddChild(childB, RelationshipType.BiologicalFather, null);

        // Act
        var result = _calculator.CalculateAllRelationships(childA, childB);

        // Assert
        Assert.Single(result);
        Assert.Equal("Full Sister", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_FullSiblings_ReturnsFullBrother()
    {
        // Arrange
        var mother = CreateMember(Gender.Female);
        var father = CreateMember(Gender.Male);
        var childA = CreateMember(Gender.Female);
        var childB = CreateMember(Gender.Male);

        mother.AddChild(childA, RelationshipType.BiologicalMother, null);
        father.AddChild(childA, RelationshipType.BiologicalFather, null);
        mother.AddChild(childB, RelationshipType.BiologicalMother, null);
        father.AddChild(childB, RelationshipType.BiologicalFather, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(childA, childB);

        // Assert
        Assert.Equal("Full Brother", result);
    }

    [Fact]
    public void CalculateAllRelationships_FullSiblings_ReturnsFullBrother()
    {
        // Arrange
        var mother = CreateMember(Gender.Female);
        var father = CreateMember(Gender.Male);
        var childA = CreateMember(Gender.Female);
        var childB = CreateMember(Gender.Male);

        mother.AddChild(childA, RelationshipType.BiologicalMother, null);
        father.AddChild(childA, RelationshipType.BiologicalFather, null);
        mother.AddChild(childB, RelationshipType.BiologicalMother, null);
        father.AddChild(childB, RelationshipType.BiologicalFather, null);

        // Act
        var result = _calculator.CalculateAllRelationships(childA, childB);

        // Assert
        Assert.Single(result);
        Assert.Equal("Full Brother", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_HalfSiblings_ReturnsHalfSister()
    {
        // Arrange
        var mother = CreateMember(Gender.Female);
        var childA = CreateMember(Gender.Male);
        var childB = CreateMember(Gender.Female);

        mother.AddChild(childA, RelationshipType.BiologicalMother, null);
        mother.AddChild(childB, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(childA, childB);

        // Assert
        Assert.Equal("Half Sister", result);
    }

    [Fact]
    public void CalculateAllRelationships_HalfSiblings_ReturnsHalfSister()
    {
        // Arrange
        var mother = CreateMember(Gender.Female);
        var childA = CreateMember(Gender.Male);
        var childB = CreateMember(Gender.Female);

        mother.AddChild(childA, RelationshipType.BiologicalMother, null);
        mother.AddChild(childB, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateAllRelationships(childA, childB);

        // Assert
        Assert.Single(result);
        Assert.Equal("Half Sister", result[0]);
    }

    #endregion

    #region Partnership Tests

    [Fact]
    public void CalculateClosestRelationship_Married_ReturnsWife()
    {
        // Arrange
        var husband = CreateMember(Gender.Male);
        var wife = CreateMember(Gender.Female);
        husband.AddPartner(wife, PartnershipType.Marriage, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(husband, wife);

        // Assert
        Assert.Equal("Wife", result);
    }

    [Fact]
    public void CalculateAllRelationships_Married_ReturnsWife()
    {
        // Arrange
        var husband = CreateMember(Gender.Male);
        var wife = CreateMember(Gender.Female);
        husband.AddPartner(wife, PartnershipType.Marriage, null);

        // Act
        var result = _calculator.CalculateAllRelationships(husband, wife);

        // Assert
        Assert.Single(result);
        Assert.Equal("Wife", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_Married_ReturnsHusband()
    {
        // Arrange
        var husband = CreateMember(Gender.Male);
        var wife = CreateMember(Gender.Female);
        husband.AddPartner(wife, PartnershipType.Marriage, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(wife, husband);

        // Assert
        Assert.Equal("Husband", result);
    }

    [Fact]
    public void CalculateAllRelationships_Married_ReturnsHusband()
    {
        // Arrange
        var husband = CreateMember(Gender.Male);
        var wife = CreateMember(Gender.Female);
        husband.AddPartner(wife, PartnershipType.Marriage, null);

        // Act
        var result = _calculator.CalculateAllRelationships(wife, husband);

        // Assert
        Assert.Single(result);
        Assert.Equal("Husband", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_MarriedNonBinary_ReturnsSpouse()
    {
        // Arrange
        var spouse = CreateMember(Gender.NonBinary);
        var wife = CreateMember(Gender.Female);
        wife.AddPartner(spouse, PartnershipType.Marriage, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(wife, spouse);

        // Assert
        Assert.Equal("Spouse", result);
    }

    [Fact]
    public void CalculateAllRelationships_MarriedNonBinary_ReturnsSpouse()
    {
        // Arrange
        var spouse = CreateMember(Gender.NonBinary);
        var wife = CreateMember(Gender.Female);
        wife.AddPartner(spouse, PartnershipType.Marriage, null);

        // Act
        var result = _calculator.CalculateAllRelationships(wife, spouse);

        // Assert
        Assert.Single(result);
        Assert.Equal("Spouse", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_CivilUnion_ReturnsCivilUnion()
    {
        // Arrange
        var personA = CreateMember(Gender.Male);
        var personB = CreateMember(Gender.Male);
        personA.AddPartner(personB, PartnershipType.CivilUnion, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(personA, personB);

        // Assert
        Assert.Equal("Civil Union", result);
    }

    [Fact]
    public void CalculateAllRelationships_CivilUnion_ReturnsCivilUnion()
    {
        // Arrange
        var personA = CreateMember(Gender.Male);
        var personB = CreateMember(Gender.Male);
        personA.AddPartner(personB, PartnershipType.CivilUnion, null);

        // Act
        var result = _calculator.CalculateAllRelationships(personA, personB);

        // Assert
        Assert.Single(result);
        Assert.Equal("Civil Union", result[0]);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void CalculateClosestRelationship_NonBinarySibling_ReturnsSibling()
    {
        // Arrange
        var mother = CreateMember(Gender.Female);
        var childA = CreateMember(Gender.Male);
        var childB = CreateMember(Gender.NonBinary);

        mother.AddChild(childA, RelationshipType.BiologicalMother, null);
        mother.AddChild(childB, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(childA, childB);

        // Assert
        Assert.Equal("Half Sibling", result);
    }

    [Fact]
    public void CalculateAllRelationships_NonBinarySibling_ReturnsSibling()
    {
        // Arrange
        var mother = CreateMember(Gender.Female);
        var childA = CreateMember(Gender.Male);
        var childB = CreateMember(Gender.NonBinary);

        mother.AddChild(childA, RelationshipType.BiologicalMother, null);
        mother.AddChild(childB, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateAllRelationships(childA, childB);

        // Assert
        Assert.Single(result);
        Assert.Equal("Half Sibling", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_MultipleParents_StillRecognizesSiblings()
    {
        // Arrange - Polyamorous family with 3 parents
        var parent1 = CreateMember(Gender.Female);
        var parent2 = CreateMember(Gender.Male);
        var parent3 = CreateMember(Gender.Female);
        var childA = CreateMember(Gender.Male);
        var childB = CreateMember(Gender.Female);

        // Both children have all three parents
        parent1.AddChild(childA, RelationshipType.BiologicalMother, null);
        parent2.AddChild(childA, RelationshipType.BiologicalFather, null);
        parent3.AddChild(childA, RelationshipType.OtherMother, null);

        parent1.AddChild(childB, RelationshipType.BiologicalMother, null);
        parent2.AddChild(childB, RelationshipType.BiologicalFather, null);
        parent3.AddChild(childB, RelationshipType.OtherMother, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(childA, childB);

        // Assert
        Assert.Equal("Full Sister", result);
    }

    [Fact]
    public void CalculateAllRelationships_MultipleParents_StillRecognizesSiblings()
    {
        // Arrange - Polyamorous family with 3 parents
        var parent1 = CreateMember(Gender.Female);
        var parent2 = CreateMember(Gender.Male);
        var parent3 = CreateMember(Gender.Female);
        var childA = CreateMember(Gender.Male);
        var childB = CreateMember(Gender.Female);

        // Both children have all three parents
        parent1.AddChild(childA, RelationshipType.BiologicalMother, null);
        parent2.AddChild(childA, RelationshipType.BiologicalFather, null);
        parent3.AddChild(childA, RelationshipType.OtherMother, null);

        parent1.AddChild(childB, RelationshipType.BiologicalMother, null);
        parent2.AddChild(childB, RelationshipType.BiologicalFather, null);
        parent3.AddChild(childB, RelationshipType.OtherMother, null);

        // Act
        var result = _calculator.CalculateAllRelationships(childA, childB);

        // Assert
        Assert.Single(result);
        Assert.Equal("Full Sister", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_NotRelated_ReturnsNotRelated()
    {
        // Arrange
        var memberA = CreateMember(Gender.Male);
        var memberB = CreateMember(Gender.Female);
        // Act
        var result = _calculator.CalculateClosestRelationship(memberA, memberB);
        // Assert
        Assert.Equal("Not related", result);
    }

    [Fact]
    public void CalculateAllRelationships_NotRelated_ReturnsNotRelated()
    {
        // Arrange
        var memberA = CreateMember(Gender.Male);
        var memberB = CreateMember(Gender.Female);
        // Act
        var result = _calculator.CalculateAllRelationships(memberA, memberB);
        // Assert
        Assert.Single(result);
        Assert.Equal("Not related", result[0]);
    }

    #endregion

    #region Complex Relationships

    [Fact]
    public void CalculateClosestRelationship_Uncle_ReturnsUncle()
    {
        // Arrange
        var grandmother = CreateMember(Gender.Female);
        var mother = CreateMember(Gender.Female);
        var uncle = CreateMember(Gender.Male);
        var child = CreateMember(Gender.Male);

        grandmother.AddChild(mother, RelationshipType.BiologicalMother, null);
        grandmother.AddChild(uncle, RelationshipType.BiologicalMother, null);
        mother.AddChild(child, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(child, uncle);

        // Assert
        Assert.Equal("Uncle", result);
    }

    [Fact]
    public void CalculateAllRelationships_Uncle_ReturnsUncle()
    {
        // Arrange
        var grandmother = CreateMember(Gender.Female);
        var mother = CreateMember(Gender.Female);
        var uncle = CreateMember(Gender.Male);
        var child = CreateMember(Gender.Male);

        grandmother.AddChild(mother, RelationshipType.BiologicalMother, null);
        grandmother.AddChild(uncle, RelationshipType.BiologicalMother, null);
        mother.AddChild(child, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateAllRelationships(child, uncle);

        // Assert
        Assert.Single(result);
        Assert.Equal("Uncle", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_Grandparent_ReturnsGrandmother()
    {
        // Arrange
        var grandmother = CreateMember(Gender.Female);
        var mother = CreateMember(Gender.Female);
        var grandchild = CreateMember(Gender.Male);

        grandmother.AddChild(mother, RelationshipType.BiologicalMother, null);
        mother.AddChild(grandchild, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(grandchild, grandmother);

        // Assert
        Assert.Equal("Grandmother", result);
    }

    [Fact]
    public void CalculateAllRelationships_Grandparent_ReturnsGrandmother()
    {
        // Arrange
        var grandmother = CreateMember(Gender.Female);
        var mother = CreateMember(Gender.Female);
        var grandchild = CreateMember(Gender.Male);

        grandmother.AddChild(mother, RelationshipType.BiologicalMother, null);
        mother.AddChild(grandchild, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateAllRelationships(grandchild, grandmother);

        // Assert
        Assert.Single(result);
        Assert.Equal("Grandmother", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_FirstCousin_ReturnsFirstCousin()
    {
        // Arrange
        var grandparent = CreateMember(Gender.Female);
        var parent1 = CreateMember(Gender.Female);
        var parent2 = CreateMember(Gender.Male);
        var cousinA = CreateMember(Gender.Male);
        var cousinB = CreateMember(Gender.Female);

        grandparent.AddChild(parent1, RelationshipType.BiologicalMother, null);
        grandparent.AddChild(parent2, RelationshipType.BiologicalMother, null);
        parent1.AddChild(cousinA, RelationshipType.BiologicalMother, null);
        parent2.AddChild(cousinB, RelationshipType.BiologicalFather, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(cousinA, cousinB);

        // Assert
        Assert.Equal("First Cousin", result);
    }

    [Fact]
    public void CalculateAllRelationships_FirstCousin_ReturnsFirstCousin()
    {
        // Arrange
        var grandparent = CreateMember(Gender.Female);
        var parent1 = CreateMember(Gender.Female);
        var parent2 = CreateMember(Gender.Male);
        var cousinA = CreateMember(Gender.Male);
        var cousinB = CreateMember(Gender.Female);

        grandparent.AddChild(parent1, RelationshipType.BiologicalMother, null);
        grandparent.AddChild(parent2, RelationshipType.BiologicalMother, null);
        parent1.AddChild(cousinA, RelationshipType.BiologicalMother, null);
        parent2.AddChild(cousinB, RelationshipType.BiologicalFather, null);

        // Act
        var result = _calculator.CalculateAllRelationships(cousinA, cousinB);

        // Assert
        Assert.Single(result);
        Assert.Equal("First Cousin", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_SecondCousin_ReturnsSecondCousin()
    {
        // Arrange
        var greatGrandparent = CreateMember(Gender.Male);
        var grandparent1 = CreateMember(Gender.Female);
        var grandparent2 = CreateMember(Gender.Female);
        var parent1 = CreateMember(Gender.Male);
        var parent2 = CreateMember(Gender.Female);
        var cousinA = CreateMember(Gender.Male);
        var cousinB = CreateMember(Gender.Male);

        greatGrandparent.AddChild(grandparent1, RelationshipType.BiologicalFather, null);
        greatGrandparent.AddChild(grandparent2, RelationshipType.BiologicalFather, null);
        grandparent1.AddChild(parent1, RelationshipType.BiologicalMother, null);
        grandparent2.AddChild(parent2, RelationshipType.BiologicalMother, null);
        parent1.AddChild(cousinA, RelationshipType.BiologicalFather, null);
        parent2.AddChild(cousinB, RelationshipType.BiologicalMother, null);
        // Act
        var result = _calculator.CalculateClosestRelationship(cousinA, cousinB);
        // Assert
        Assert.Equal("Second Cousin", result);
    }

    [Fact]
    public void CalculateAllRelationships_SecondCousin_ReturnsSecondCousin()
    {
        // Arrange
        var greatGrandparent = CreateMember(Gender.Male);
        var grandparent1 = CreateMember(Gender.Female);
        var grandparent2 = CreateMember(Gender.Female);
        var parent1 = CreateMember(Gender.Male);
        var parent2 = CreateMember(Gender.Female);
        var cousinA = CreateMember(Gender.Male);
        var cousinB = CreateMember(Gender.Male);

        greatGrandparent.AddChild(grandparent1, RelationshipType.BiologicalFather, null);
        greatGrandparent.AddChild(grandparent2, RelationshipType.BiologicalFather, null);
        grandparent1.AddChild(parent1, RelationshipType.BiologicalMother, null);
        grandparent2.AddChild(parent2, RelationshipType.BiologicalMother, null);
        parent1.AddChild(cousinA, RelationshipType.BiologicalFather, null);
        parent2.AddChild(cousinB, RelationshipType.BiologicalMother, null);
        // Act
        var result = _calculator.CalculateAllRelationships(cousinA, cousinB);
        // Assert
        Assert.Single(result);
        Assert.Equal("Second Cousin", result[0]);
    }

    [Fact]
    public void CalculateClosestRelationship_FirstCousinOnceRemoved_ReturnsFirstCousinOnceRemoved()
    {
        // Arrange
        var greatGrandparent = CreateMember(Gender.Male);
        var grandparent1 = CreateMember(Gender.Female);
        var parent1 = CreateMember(Gender.Male);
        var parent2 = CreateMember(Gender.Female);
        var cousinA = CreateMember(Gender.Male);
        var cousinB = CreateMember(Gender.Male);

        greatGrandparent.AddChild(grandparent1, RelationshipType.BiologicalFather, null);
        greatGrandparent.AddChild(parent2, RelationshipType.BiologicalFather, null);
        grandparent1.AddChild(parent1, RelationshipType.BiologicalMother, null);
        parent1.AddChild(cousinA, RelationshipType.BiologicalFather, null);
        parent2.AddChild(cousinB, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateClosestRelationship(cousinA, cousinB);

        // Assert
        Assert.Equal("First Cousin Once Removed", result);
    }

    [Fact]
    public void CalculateAllRelationships_FirstCousinOnceRemoved_ReturnsFirstCousinOnceRemoved()
    {
        // Arrange
        var greatGrandparent = CreateMember(Gender.Male);
        var grandparent1 = CreateMember(Gender.Female);
        var parent1 = CreateMember(Gender.Male);
        var parent2 = CreateMember(Gender.Female);
        var cousinA = CreateMember(Gender.Male);
        var cousinB = CreateMember(Gender.Male);

        greatGrandparent.AddChild(grandparent1, RelationshipType.BiologicalFather, null);
        greatGrandparent.AddChild(parent2, RelationshipType.BiologicalFather, null);
        grandparent1.AddChild(parent1, RelationshipType.BiologicalMother, null);
        parent1.AddChild(cousinA, RelationshipType.BiologicalFather, null);
        parent2.AddChild(cousinB, RelationshipType.BiologicalMother, null);

        // Act
        var result = _calculator.CalculateAllRelationships(cousinA, cousinB);

        // Assert
        Assert.Single(result);
        Assert.Equal("First Cousin Once Removed", result[0]);
    }

    #endregion

    #region Helper Methods

    private Member CreateMember(Gender gender)
    {
        var member = new Member
        {
            Id = Guid.NewGuid(),
            Gender = gender,
            BirthDate = DateTime.Now.AddYears(-30)
        };
        return member;
    }

    #endregion
}
