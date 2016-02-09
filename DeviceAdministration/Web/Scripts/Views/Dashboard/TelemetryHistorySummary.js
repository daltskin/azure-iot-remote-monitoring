IoTApp.createModule(
    'IoTApp.Dashboard.TelemetryHistorySummary',
    function initTelemetryHistorySummary() {
        'use strict';

        var averageDeviceHeartRateContainer;
        var averageDeviceHeartRateControl;
        var averageDeviceHeartRateLabel;
        var averageHeartRateVisual;
        var lastAvgHeartRate;
        var lastMaxHeartRate;
        var lastMinHeartRate;
        var maxDeviceHeartRateContainer;
        var maxDeviceHeartRateControl;
        var maxDeviceHeartRateLabel;
        var maxHeartRateVisual;
        var maxValue;
        var minDeviceHeartRateContainer;
        var minDeviceHeartRateControl;
        var minDeviceHeartRateLabel;
        var minHeartRateVisual;
        var minValue;

        var createDataView = function createDataView(indicatedValue) {

            var categoryMetadata;
            var dataView;
            var dataViewTransform;
            var graphMetadata;

            dataViewTransform = powerbi.data.DataViewTransform;

            graphMetadata = {
                columns: [
                    {
                        isMeasure: true,
                        roles: { 'Y': true },
                        objects: { general: { formatString: resources.telemetryGaugeNumericFormat } },
                    },
                    {
                        isMeasure: true,
                        roles: { 'MinValue': true },
                    },
                    {
                        isMeasure: true,
                        roles: { 'MaxValue': true },
                    }
                ],
                groups: [],
                measures: [0]
            };

            categoryMetadata = {
                values: dataViewTransform.createValueColumns([
                    {
                        source: graphMetadata.columns[0],
                        values: [indicatedValue],
                    }, {
                        source: graphMetadata.columns[1],
                        values: [minValue],
                    }, {
                        source: graphMetadata.columns[2],
                        values: [maxValue],
                    }])
            };

            dataView = {
                metadata: graphMetadata,
                single: { value: indicatedValue },
                categorical: categoryMetadata
            };

            return dataView;
        };

        var createDefaultStyles = function createDefaultStyles() {

            var dataColors = new powerbi.visuals.DataColorPalette();

            return {
                titleText: {
                    color: { value: 'rgba(51,51,51,1)' }
                },
                subTitleText: {
                    color: { value: 'rgba(145,145,145,1)' }
                },
                colorPalette: {
                    dataColors: dataColors,
                },
                labelText: {
                    color: {
                        value: 'rgba(51,51,51,1)',
                    },
                    fontSize: '11px'
                },
                isHighContrast: false,
            };
        };

        var createVisual = function createVisual(targetControl) {

            var height;
            var pluginService;
            var singleVisualHostServices;
            var visual;
            var width;

            height = $(targetControl).height();
            width = $(targetControl).width();

            pluginService = powerbi.visuals.visualPluginFactory.create();
            singleVisualHostServices = powerbi.visuals.singleVisualHostServices;

            // Get a plugin
            visual = pluginService.getPlugin('gauge').create();

            visual.init({
                element: targetControl,
                host: singleVisualHostServices,
                style: createDefaultStyles(),
                viewport: {
                    height: height,
                    width: width
                },
                settings: { slicingEnabled: true },
                interactivity: { isInteractiveLegend: false, selection: false },
                animation: { transitionImmediate: true }
            });

            return visual;
        };

        var init = function init(telemetryHistorySummarySettings) {

            maxValue = telemetryHistorySummarySettings.gaugeMaxValue;
            minValue = telemetryHistorySummarySettings.gaugeMinValue;

            averageDeviceHeartRateContainer = telemetryHistorySummarySettings.averageDeviceHeartRateContainer;
            averageDeviceHeartRateControl = telemetryHistorySummarySettings.averageDeviceHeartRateControl;
            averageDeviceHeartRateLabel = telemetryHistorySummarySettings.averageDeviceHeartRateLabel;
            maxDeviceHeartRateContainer = telemetryHistorySummarySettings.maxDeviceHeartRateContainer;
            maxDeviceHeartRateControl = telemetryHistorySummarySettings.maxDeviceHeartRateControl;
            maxDeviceHeartRateLabel = telemetryHistorySummarySettings.maxDeviceHeartRateLabel;
            minDeviceHeartRateContainer = telemetryHistorySummarySettings.minDeviceHeartRateContainer;
            minDeviceHeartRateControl = telemetryHistorySummarySettings.minDeviceHeartRateControl;
            minDeviceHeartRateLabel = telemetryHistorySummarySettings.minDeviceHeartRateLabel;

            averageHeartRateVisual = createVisual(averageDeviceHeartRateControl);
            maxHeartRateVisual = createVisual(maxDeviceHeartRateControl);
            minHeartRateVisual = createVisual(minDeviceHeartRateControl);
        };

        var redraw = function redraw() {
            var height;
            var width;

            if (minDeviceHeartRateControl &&
                minHeartRateVisual &&
                (lastMinHeartRate || (lastMinHeartRate === 0))) {
                height = minDeviceHeartRateControl.height();
                width = minDeviceHeartRateControl.width();

                minHeartRateVisual.update({
                    dataViews: [createDataView(lastMinHeartRate)],
                    viewport: {
                        height: height,
                        width: width
                    },
                    duration: 0
                });
            }

            if (maxDeviceHeartRateControl &&
                maxHeartRateVisual &&
                (lastMaxHeartRate || (lastMaxHeartRate === 0))) {
                height = maxDeviceHeartRateControl.height();
                width = maxDeviceHeartRateControl.width();

                maxHeartRateVisual.update({
                    dataViews: [createDataView(lastMaxHeartRate)],
                    viewport: {
                        height: height,
                        width: width
                    },
                    duration: 0
                });
            }

            if (averageDeviceHeartRateControl &&
                averageHeartRateVisual &&
                (lastAvgHeartRate || (lastAvgHeartRate === 0))) {
                height = averageDeviceHeartRateControl.height();
                width = averageDeviceHeartRateControl.width();

                averageHeartRateVisual.update({
                    dataViews: [createDataView(lastAvgHeartRate)],
                    viewport: {
                        height: height,
                        width: width
                    },
                    duration: 0
                });
            }
        };

        var resizeTelemetryHistorySummaryGuages =
            function resizeTelemetryHistorySummaryGuages() {

                var height;
                var padding;
                var width;

                padding = 0;

                if (averageDeviceHeartRateContainer &&
                    averageDeviceHeartRateLabel &&
                    averageDeviceHeartRateControl) {

                    height =
                        averageDeviceHeartRateContainer.height() -
                        averageDeviceHeartRateLabel.height() -
                        padding;

                    width = averageDeviceHeartRateContainer.width() - padding;

                    averageDeviceHeartRateControl.height(height);
                    averageDeviceHeartRateControl.width(width);
                }

                if (maxDeviceHeartRateContainer &&
                    maxDeviceHeartRateLabel &&
                    maxDeviceHeartRateControl) {

                    height =
                        maxDeviceHeartRateContainer.height() -
                        maxDeviceHeartRateLabel.height() -
                        padding;

                    width = maxDeviceHeartRateContainer.width() - padding;

                    maxDeviceHeartRateControl.height(height);
                    maxDeviceHeartRateControl.width(width);
                }

                if (minDeviceHeartRateContainer &&
                    minDeviceHeartRateLabel &&
                    minDeviceHeartRateControl) {

                    height =
                        minDeviceHeartRateContainer.height() -
                        minDeviceHeartRateLabel.height() -
                        padding;

                    width = minDeviceHeartRateContainer.width() - padding;

                    minDeviceHeartRateControl.height(height);
                    minDeviceHeartRateControl.width(width);
                }

                redraw();
            };

        var updateTelemetryHistorySummaryData =
            function updateTelemetryHistorySummaryData(
                minHeartRate,
                maxHeartRate,
                avgHeartRate) {

                lastAvgHeartRate = avgHeartRate;
                lastMaxHeartRate = maxHeartRate;
                lastMinHeartRate = minHeartRate;

                redraw();
        };

        return {
            init: init,
            resizeTelemetryHistorySummaryGuages: resizeTelemetryHistorySummaryGuages,
            updateTelemetryHistorySummaryData: updateTelemetryHistorySummaryData
        };
    },
    [jQuery, resources, powerbi]);