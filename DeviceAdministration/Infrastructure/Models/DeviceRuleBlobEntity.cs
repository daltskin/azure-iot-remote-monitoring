namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Infrastructure.Models
{
    public class DeviceRuleBlobEntity
    {
        public DeviceRuleBlobEntity(string deviceId)
        {
            DeviceId = deviceId;
        }

        public string DeviceId { get; private set; }
        public double? Temperature { get; set; }
        public double? HeartRate { get; set; }
        public string TemperatureRuleOutput { get; set; }
        public string HeartRateRuleOutput { get; set; }
    }
}
