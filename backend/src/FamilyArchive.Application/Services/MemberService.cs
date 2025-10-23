using FamilyArchive.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyArchive.Application.Services;

public interface IMemberService
{
    void AddName(Member member, MemberName name);
}
public class MemberService : IMemberService
{
    public void AddName(Member member, MemberName name)
    {
        member.AddName(name);
    }
}
