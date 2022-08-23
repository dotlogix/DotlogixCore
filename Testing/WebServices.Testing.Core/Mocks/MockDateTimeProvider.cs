using System;
using DotLogix.WebServices.Core.Time;

namespace DotLogix.WebServices.Testing.Mocks; 

public class MockDateTimeProvider : IDateTimeProvider
{
    private Func<DateTime> _getCurrentFunc;

    public MockDateTimeProvider()
    {
        Reset();
    }

    public DateTime UtcNow => _getCurrentFunc.Invoke();

    public void UseTime(DateTime dateTime)
    {
        _getCurrentFunc = () => dateTime;
    }

    public void UseTime(Func<DateTime> getTimeFunc)
    {
        _getCurrentFunc = getTimeFunc;
    }

    public void Reset()
    {
        _getCurrentFunc = () => DateTime.UtcNow;
    }
}