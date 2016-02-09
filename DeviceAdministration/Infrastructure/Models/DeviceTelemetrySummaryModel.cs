using System;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models
{
    /// <summary>
    /// A model that summarizes a period of Device telemetry.
    /// </summary>
    public class DeviceTelemetrySummaryModel
    {
        /// <summary>
        /// Gets or sets the covered period's average heartrate.
        /// </summary>
        public double? AverageHeartRate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ID of the Device, covered by the summarized 
        /// telemetry.
        /// </summary>
        public string DeviceId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the covered period's maximum heartrate.
        /// </summary>
        public double? MaximumHeartRate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the covered period's minimum heartrate.
        /// </summary>
        public double? MinimumHeartRate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of minutes the represented period covers.
        /// </summary>
        public double? TimeFrameMinutes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time of record for the represented telemetry 
        /// recording.
        /// </summary>
        public DateTime? Timestamp
        {
            get;
            set;
        }
    }
}
