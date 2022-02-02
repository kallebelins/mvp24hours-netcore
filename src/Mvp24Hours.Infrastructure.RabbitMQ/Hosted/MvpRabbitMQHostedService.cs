//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvp24Hours.Infrastructure.RabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    public class MvpRabbitMQHostedService : IHostedService
    {
        private readonly TimerCallback callback;
        private readonly object state;
        private readonly TimeSpan dueTime;
        private readonly TimeSpan period;

        /// <summary>
        /// 
        /// </summary>
        public MvpRabbitMQHostedService(TimerCallback callback, object state = null, TimeSpan? dueTime = null, TimeSpan? period = null)
        {
            this.callback = callback;
            this.state = state;
            this.dueTime = dueTime ?? TimeSpan.Zero;
            this.period = period ?? TimeSpan.FromSeconds(3);
        }

        /// <summary>
        /// 
        /// </summary>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            new Timer(callback, state, dueTime, period);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
