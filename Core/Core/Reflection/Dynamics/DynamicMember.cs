using System;
using System.Reflection;

namespace DotLogix.Core.Reflection.Dynamics; 

public abstract class DynamicMember {
    /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
    protected DynamicMember(MemberInfo memberInfo) {
        MemberInfo = memberInfo;
    }

    /// <summary>
    /// The name
    /// </summary>
    public string Name => MemberInfo.Name;

    /// <summary>
    /// The declaring type
    /// </summary>
    public Type DeclaringType => MemberInfo.DeclaringType;

    /// <summary>
    /// The reflected type
    /// </summary>
    public Type ReflectedType => MemberInfo.ReflectedType;

    /// <summary>
    /// The original member info
    /// </summary>
    public MemberInfo MemberInfo { get; }

    protected bool Equals(DynamicMember other) {
        return Equals(MemberInfo, other.MemberInfo);
    }

    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object obj) {
        if(ReferenceEquals(null, obj))
            return false;
        if(ReferenceEquals(this, obj))
            return true;
        if(obj.GetType() != this.GetType())
            return false;
        return Equals((DynamicMember)obj);
    }

    /// <summary>Serves as the default hash function.</summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() {
        return (MemberInfo is not null
            ? MemberInfo.GetHashCode()
            : 0);
    }

    /// <summary>Returns a value that indicates whether the values of two <see cref="T:DotLogix.Core.Reflection.Dynamics.DynamicMember" /> objects are equal.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise, false.</returns>
    public static bool operator ==(DynamicMember left, DynamicMember right) {
        return Equals(left, right);
    }

    /// <summary>Returns a value that indicates whether two <see cref="T:DotLogix.Core.Reflection.Dynamics.DynamicMember" /> objects have different values.</summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
    public static bool operator !=(DynamicMember left, DynamicMember right) {
        return !Equals(left, right);
    }
}