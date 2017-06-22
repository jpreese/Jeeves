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
            SetupEthernet();
            SetupEvents();
            PollTemperature();
        }
 
        /// <summary>
        /// Sets up a local web server on the device.
        /// </summary>
        void SetupEthernet()
        {
            ethernet.UseThisNetworkInterface();

            while(ethernet.NetworkInterface.IPAddress == "0.0.0.0")
            {
                Debug.Print("Establishing IP...");
                Thread.Sleep(1000);
            }

            WebServer.StartLocalServer(ethernet.NetworkInterface.IPAddress, 4738);
        }

        /// <summary>
        /// Sets up all of the events the device will be listening for.
        /// </summary>
        void SetupEvents()
        {
            var webEventUpdateLightStatus = WebServer.SetupWebEvent("light");
            webEventUpdateLightStatus.WebEventReceived += webEventUpdateLightStatus_WebEventReceived;
        }

        /// <summary>
        /// Verifies that the device can reach the outside world every one minute.
        /// </summary>
        void CheckInternetConnectivity()
        {
            const int ONE_MINUTE_IN_MS = 60000;
            var pingInternetConnectivity = new GT.Timer(ONE_MINUTE_IN_MS);

            pingInternetConnectivity.Tick += pingInternetConnectivity_Tick;
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
        void PollTemperature()
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
            var currentTemp = tempHumidSI70.TakeMeasurement().TemperatureFahrenheit;
            var postContext = new POSTContent();

            var currentDateTime = DateTime.Now.ToString("MM/dd/yyyy") + "%20" + DateTime.Now.ToString("HH:MM:ss");

            var requestUrl = BASE_WEBADDRESS + "Sensor/LogTemperature?ReadDate=" + currentDateTime + "&Reading=" + (int)currentTemp;
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
            if(responder.UrlParameters["status"].ToString() == "on")
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
