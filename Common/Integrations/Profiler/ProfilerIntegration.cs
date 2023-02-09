using System;
using System.Collections.Generic;
using System.Text;
using Pathoschild.Stardew.Common.Integrations;
using StardewModdingAPI;

namespace Common.Integrations.Profiler
{
    internal class ProfilerIntegration : BaseIntegration<IProfilerAPI>
    {
        public ProfilerIntegration(IModRegistry modRegistry, IMonitor monitor)
            : base("Profiler", "SinZ.Profiler", "2.0.0-beta2", modRegistry, monitor) { }

        public IDisposable? RecordSection(string EventType, string Details)
        {
            if (this.IsLoaded)
            {
                return this.ModApi.RecordSection(this.ModRegistry.ModID, EventType, Details);
            } else
            {
                return null;
            }
        }
    }
}
