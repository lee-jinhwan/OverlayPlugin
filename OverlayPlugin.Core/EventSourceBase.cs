﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json.Linq;

namespace RainbowMage.OverlayPlugin
{
    public abstract class EventSourceBase : IEventSource
    {
        public string Name { get; protected set; }

        public event EventHandler<LogEventArgs> OnLog;
        protected Timer timer;

        public EventSourceBase()
        {
            timer = new Timer(1000);
            timer.Elapsed += (o, e) =>
            {
                try
                {
                    Update();
                }
                catch (Exception ex)
                {
                    Log(LogLevel.Error, "Update: {0}", ex);
                }
            };
        }

        protected void Log(LogLevel level, string message, params object[] args)
        {
            OnLog?.Invoke(this, new LogEventArgs(level, string.Format(message, args)));
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        abstract protected void Update();

        protected void RegisterEventTypes(List<string> types)
        {
            EventDispatcher.RegisterEventTypes(types);
        }

        protected void RegisterEventHandler(string name, Action<JObject, Action<JObject>> handler)
        {
            EventDispatcher.RegisterHandler(name, handler);
        }

        protected void DispatchEvent(JObject e)
        {
            EventDispatcher.DispatchEvent(e);
        }
    }
}
