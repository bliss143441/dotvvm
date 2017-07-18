﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Tracing;
using Microsoft.ApplicationInsights;

namespace DotVVM.Tracing.ApplicationInsights
{
    public class ApplicationInsightsBulkTracer : IRequestTracer
    {
        private readonly TelemetryClient telemetryClient;
        private readonly IStopwatch stopwatch;

        private List<Event> events = new List<Event>();

        public ApplicationInsightsBulkTracer(TelemetryClient telemetryClient, IStopwatch stopwatch)
        {
            this.telemetryClient = telemetryClient;
            this.stopwatch = stopwatch;
        }

        public Task EndRequest()
        {
            foreach (var @event in events)
            {
                telemetryClient.TrackMetric(@event.Name, @event.TimeStamp);
            }

            return Task.CompletedTask;
        }

        public Task TraceEvent(string eventName, IDotvvmRequestContext context)
        {
            var newEvent = new Event
            {
                Name = eventName,
                TimeStamp = stopwatch.GetElapsedMiliseconds()
            };

            events.Add(newEvent);

            return Task.CompletedTask;
        }

        internal struct Event
        {
            public string Name { get; set; }
            public long TimeStamp { get; set; }
        }
    }
}
