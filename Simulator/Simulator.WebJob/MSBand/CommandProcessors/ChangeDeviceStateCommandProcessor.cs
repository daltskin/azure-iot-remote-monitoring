using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.DeviceSchema;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Helpers;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.MSBand.Devices;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.SimulatorCore.CommandProcessors;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.SimulatorCore.Transport;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.MSBand.CommandProcessors
{
    /// <summary>
    /// Command processor to handle the change in device state.
    /// Currently this just changes the DeviceState string on the device.
    /// </summary>
    public class ChangeDeviceStateCommandProcessor : CommandProcessor
    {
        private const string CHANGE_DEVICE_STATE = "ChangeDeviceState";

        public ChangeDeviceStateCommandProcessor(MSBandDevice device)
            : base(device)
        {

        }

        public async override Task<CommandProcessingResult> HandleCommandAsync(DeserializableCommand deserializableCommand)
        {
            if (deserializableCommand.CommandName == CHANGE_DEVICE_STATE)
            {
                var command = deserializableCommand.Command;

                try
                {
                    var device = Device as MSBandDevice;
                    if (device != null)
                    {
                        dynamic parameters = WireCommandSchemaHelper.GetParameters(command);
                        if (parameters != null)
                        {
                            dynamic deviceState = ReflectionHelper.GetNamedPropertyValue(
                                parameters,
                                "DeviceState",
                                usesCaseSensitivePropertyNameMatch: true,
                                exceptionThrownIfNoMatch: true);

                            if (deviceState != null)
                            {
                                device.ChangeDeviceState(deviceState.ToString());

                                return CommandProcessingResult.Success;
                            }
                            else
                            {
                                // DeviceState is a null reference.
                                return CommandProcessingResult.CannotComplete;
                            }
                        }
                        else
                        {
                            // parameters is a null reference.
                            return CommandProcessingResult.CannotComplete;
                        }
                    }
                    else
                    {
                        // Unsupported Device type.
                        return CommandProcessingResult.CannotComplete;
                }
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
