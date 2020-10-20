// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

using OpcPublisher.Configurations;
using System.Threading;

namespace OpcPublisher
{
    /// <summary>
    /// Class with unit test helper methods.
    /// </summary>
    public static class UnitTestHelper
    {
        public static string GetMethodName([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            return memberName;
        }

        public static int WaitTilItemsAreMonitored()
        {
            // wait till monitoring starts
            int iter = 0;
            int startNum = Program.NodeConfiguration.NumberOfOpcMonitoredItemsMonitored;
            while (Program.NodeConfiguration.NumberOfOpcMonitoredItemsMonitored  == 0 && iter < _maxIterations)
            {
                Thread.Sleep(_sleepMilliseconds);
                iter++;
            }
            return iter < _maxIterations ? iter * _sleepMilliseconds / 1000 : -1;
        }
        public static int WaitTilItemsAreMonitoredAndFirstEventReceived()
        {
            // wait till monitoring starts
            int iter = 0;
            long numberOfEventsStart = Program._clientWrapper.NumberOfEvents;
            while ((Program.NodeConfiguration.NumberOfOpcMonitoredItemsMonitored == 0 || (Program._clientWrapper.NumberOfEvents - numberOfEventsStart) == 0) && iter < _maxIterations)
            {
                Thread.Sleep(_sleepMilliseconds);
                iter++;
            }
            return iter < _maxIterations ? iter * _sleepMilliseconds / 1000 : -1;
        }

        public static void SetPublisherDefaults()
        {
            OpcApplicationConfiguration.OpcSamplingInterval = 1000;
            OpcApplicationConfiguration.OpcPublishingInterval = 0;
            OpcSecurityConfiguration.AutoAcceptCerts = true;
            Program._clientWrapper.DefaultSendIntervalSeconds = 0;
            Program._clientWrapper.HubMessageSize = 0;
            OpcUaMonitoredItemManager.SkipFirstDefault = false;
            OpcUaMonitoredItemManager.HeartbeatIntervalDefault = 0;
        }

        private const int _maxTimeSeconds = 30;
        private const int _sleepMilliseconds = 100;
        private const int _maxIterations = _maxTimeSeconds * 1000 / _sleepMilliseconds;
    }
}
