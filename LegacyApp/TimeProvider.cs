using System;

namespace LegacyApp
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime Now  => DateTime.Now;
    }
}