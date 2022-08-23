using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DotLogix.WebServices.EntityFramework.Conventions; 

public class DateTimeKindSpecifyConverter : ValueConverter<DateTime, DateTime> {
    private DateTimeKindSpecifyConverter(DateTimeKind kind) : base(d => d, d => DateTime.SpecifyKind(d, kind)) {
    }

    public static DateTimeKindSpecifyConverter Utc { get; } = new DateTimeKindSpecifyConverter(DateTimeKind.Utc);
    public static DateTimeKindSpecifyConverter Local { get; } = new DateTimeKindSpecifyConverter(DateTimeKind.Local);
    public static DateTimeKindSpecifyConverter Unspecified { get; } = new DateTimeKindSpecifyConverter(DateTimeKind.Unspecified);
}