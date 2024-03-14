using System;

namespace LegacyApp
{
    public interface ITimeProvider
    {
        DateTime Now { get; }
    }
}