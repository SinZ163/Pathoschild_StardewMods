using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Integrations.Profiler
{
    public interface IProfilerAPI
    {
        public IDisposable RecordSection(string ModId, string EventType, string Details);
    }
}
