using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyArchive.Domain.Enums;

public enum RelationshipType
{
    BiologicalMother,
    BiologicalFather,
    StepMother,
    StepFather,
    AdoptiveMother,
    AdoptiveFather,
    //typically used in polyamorous or non-traditional family structures
    OtherMother,
    OtherFather
}
