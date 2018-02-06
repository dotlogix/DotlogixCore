using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotLogix.Core.Reflection.Dynamics
{
    public static class AccessorTypesExtension
    {
        public static MemberTypes GetMemberTypes(this AccessorTypes accessorTypes)
        {
            MemberTypes memberTypes;
            switch (accessorTypes)
            {
                case AccessorTypes.None:
                    memberTypes = 0;
                    break;
                case AccessorTypes.Property:
                    memberTypes = MemberTypes.Property;
                    break;
                case AccessorTypes.Field:
                    memberTypes = MemberTypes.Field;
                    break;
                case AccessorTypes.Any:
                    memberTypes = MemberTypes.Property | MemberTypes.Field;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(accessorTypes), accessorTypes, null);
            }
            return memberTypes;
        }
    }
}
