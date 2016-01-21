namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.MSBand.Telemetry.Data
{
    public class RemoteMonitorTelemetryData
    {
        public string DeviceId { get; set; }
        public double HeartRate { get; set; }
        public double Speed { get; set; }
        public double? ExternalTemperature { get; set; }
    }
}
