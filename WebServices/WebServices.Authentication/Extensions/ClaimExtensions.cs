// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  ClaimExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 17.04.2022 21:30
// LastEdited:  17.04.2022 21:30
// ==================================================

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DotLogix.Core.Extensions;

namespace DotLogix.WebServices.Authentication.Extensions; 

public static class ClaimExtensions {
    public static Claim GetClaim(this IEnumerable<Claim> claims, string type) {
        return claims.FirstOrDefault(c => c.Type == type);
    }
    public static bool TryGetClaim(this IEnumerable<Claim> claims, string type, out Claim claim) {
        claim = claims.FirstOrDefault(c => c.Type == type);
        return claim is not null;
    }
    
    public static string GetClaimValue(this IEnumerable<Claim> claims, string type, string defaultValue = default) {
        return TryGetClaim(claims, type, out var claim) ? claim.Value : defaultValue;
    }
    public static bool TryGetClaimValue(this IEnumerable<Claim> claims, string type, out string value) {
        if(TryGetClaim(claims, type, out var claim)) {
            value = claim.Value;
            return true;
        }
        value = default;
        return false;
    }

    public static T GetClaimValue<T>(this IEnumerable<Claim> claims, string type, T defaultValue = default) {
        if(TryGetClaim(claims, type, out var claim) && claim.Value.TryConvertTo<T>(out var value)) {
            return value;
        }
        return defaultValue;
    }
    public static bool TryGetClaimValue<T>(this IEnumerable<Claim> claims, string type, out T value) {
        if(TryGetClaim(claims, type, out var claim) && claim.Value.TryConvertTo<T>(out value)) {
            return true;
        }
        value = default;
        return false;
    }
}