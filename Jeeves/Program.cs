using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;
using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace Jeeves
{
    public partial class Program
    {
        const string BASE_WEBADDRESS = "http://192.168.1.2:7756/";

        /// <summary>
        /// Initial method call on device boot up.
        /// </summary>
        void ProgramStarted()
        {
            Init();
        }
 
        /// <summary>
        /// Sets up a local web server on the device.
        /// </summary>
        void SetupEthernet()
        {
            ethernet.UseThisNetworkInterface();

            const int ONE_SECOND_IN_MS = 1000;
            var checkIPAddress = new GT.Timer(ONE_SECOND_IN_MS);

            checkIPAddress.Tick += checkIPAddress_Tick;
            checkIPAddress.Start();
        }

        /// <summary>
        /// Checks to see if an IP Address has been established.
        /// </summary>
        /// <param name="tick"></param>
        void checkIPAddress_Tick(GT.Timer tick)
        {
            const int WEB_SERVER_DEFAULT_PORT = 4738;
            if (ethernet.NetworkInterface.IPAddress != "0.0.0.0")
            {
                WebServer.StartLocalServer(ethernet.NetworkInterface.IPAddress, WEB_SERVER_DEFAULT_PORT);
                tick.Stop();

                CheckInternetConnectivity();
            }
        }

        /// <summary>
        /// Sets up all of the events the device will be listening for.
        /// </summary>
        void Init()
        {
            var webEventUpdateLightStatus = WebServer.SetupWebEvent("light");
            webEventUpdateLightStatus.WebEventReceived += webEventUpdateLightStatus_WebEventReceived;

            SetupEthernet();
            StartTemperaturePolling();
        }

        /// <summary>
        /// Verifies that the device can reach the outside world every one minute.
        /// </summary>
        void CheckInternetConnectivity()
        {
            const int ONE_MINUTE_IN_MS = 60000;
            var pingInternetConnectivity = new GT.Timer(ONE_MINUTE_IN_MS);

            pingInternetConnectivity.Tick += pingInternetConnectivity_Tick;
            pingInternetConnectivity.Start();
        }

        /// <summary>
        /// If no network is detected, setup the ethernet on the device.
        /// </summary>
        /// <param name="tick">The timer.</param>
        void pingInternetConnectivity_Tick(GT.Timer tick)
        {
            if(ethernet.IsNetworkUp == false)
            {
                SetupEthernet();
            }
        }

        /// <summary>
        /// Periodically called to get the current temperature of the environment.
        /// </summary>
        void StartTemperaturePolling()
        {
            const int FIVE_MINUTES_IN_MS = 300000;
            var getCurrentTemperature = new GT.Timer(FIVE_MINUTES_IN_MS);

            getCurrentTemperature.Tick += getCurrentTemperature_Tick;
            getCurrentTemperature.Start();
        }

        /// <summary>
        /// Takes a snapshop of the current temperature, and relays the information to the MVC web application.
        /// </summary>
        /// <param name="tick">The timer.</param>
        void getCurrentTemperature_Tick(GT.Timer tick)
        {
            // don't poll for a temperature if the device isn't connected to the internet
            if(ethernet.IsNetworkUp == false)
            {
                return;
            }

            var currentTemp = tempHumidSI70.TakeMeasurement().TemperatureFahrenheit;
            var currentDateTime = DateTime.Now.ToString("MM/dd/yyyy") + "%20" + DateTime.Now.ToString("HH:MM:ss");
            var requestUrl = BASE_WEBADDRESS + "Sensor/LogTemperature?ReadDate=" + currentDateTime + "&Reading=" + (int)currentTemp;

            var postContext = new POSTContent();
            var request = HttpHelper.CreateHttpPostRequest(requestUrl, postContext, null);

            request.SendRequest();
        }

        /// <summary>
        /// Event for turning the light on and off
        /// </summary>
        /// <param name="path">The path of the requested resource.</param>
        /// <param name="method">The incoming http method.</param>
        /// <param name="responder">Contains request data sent by the client and functionality to respond to the request.</param>
        void webEventUpdateLightStatus_WebEventReceived(string path, WebServer.HttpMethod method, Responder responder)
        {
            var status = responder.UrlParameters["status"].ToString();
            if(status == "on")
            {
                relayX1.TurnOn();
            }
            else
            {
                relayX1.TurnOff();
            }
        }
    }
}
