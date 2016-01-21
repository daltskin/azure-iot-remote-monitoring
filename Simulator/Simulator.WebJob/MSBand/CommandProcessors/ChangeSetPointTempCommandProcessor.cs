using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.DeviceSchema;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Helpers;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.MSBand.Devices;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.SimulatorCore.CommandProcessors;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.SimulatorCore.Transport;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.Simulator.WebJob.MSBand.CommandProcessors
{
    /// <summary>
    /// Command processor to handle the change in the temperature range
    /// </summary>
    public class ChangeSetPointTempCommandProcessor : CommandProcessor
    {
        private const string CHANGE_SET_POINT_HEARTRATE = "ChangeSetPointHeartRate";

        public ChangeSetPointTempCommandProcessor(MSBandDevice device)
            : base(device)
        {

        }

        public async override Task<CommandProcessingResult> HandleCommandAsync(DeserializableCommand deserializableCommand)
        {
            if (deserializableCommand.CommandName == CHANGE_SET_POINT_HEARTRATE)
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
                            dynamic setPointTempDynamic = ReflectionHelper.GetNamedPropertyValue(
                                parameters,
                                "SetPointHeartRate",
                                usesCaseSensitivePropertyNameMatch: true,
                                exceptionThrownIfNoMatch: true);

                            if (setPointTempDynamic != null)
                            {
                                double setPointHeartRate;
                                if (Double.TryParse(setPointTempDynamic.ToString(), out setPointHeartRate))
                                {
                                    device.ChangeSetPointHeartRate(setPointHeartRate);

                                    return CommandProcessingResult.Success;
                                }
                                else
                                {
                                    // SetPointTemp cannot be parsed as a double.
                                    return CommandProcessingResult.CannotComplete;
                                }
                            }
                            else
                            {
                                // setPointTempDynamic is a null reference.
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
