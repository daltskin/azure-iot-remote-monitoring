using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.MSBand.Devices;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.SimulatorCore.CommandProcessors;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.SimulatorCore.Transport;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.MSBand.CommandProcessors
{
    /// <summary>
    /// Command processor to start telemetry data
    public class StartCommandProcessor : CommandProcessor
    {
        private const string START_TELEMETRY = "StartTelemetry";

        public StartCommandProcessor(MSBandDevice device)
            : base(device)
        {

        }

        public async override Task<CommandProcessingResult> HandleCommandAsync(DeserializableCommand deserializableCommand)
        {
            if (deserializableCommand.CommandName == START_TELEMETRY)
            {
                var command = deserializableCommand.Command;

                try
                {
                    var device = Device as MSBandDevice;
                    device.StartTelemetryData();
                    return CommandProcessingResult.Success;
                }
                catch (Exception)
                {
                    return CommandProcessingResult.RetryLater;
                }

            }
            else if (NextCommandProcessor != null)
            {
                return await NextCommandProcessor.HandleCommandAsync(deserializableCommand);
            }

            return CommandProcessingResult.CannotComplete;
        }
    }
}
