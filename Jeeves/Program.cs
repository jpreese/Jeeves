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

            Debug.Print("Found IP: " + ethernet.NetworkInterface.IPAddress);
            Debug.Print("Network Up: " + ethernet.IsNetworkUp);

            WebServer.StartLocalServer(ethernet.NetworkInterface.IPAddress, 4738);
        }

        /// <summary>
        /// Sets up all of the events the device will be listening for.
        /// </summary>
        void SetupEvents()
        {
            // default event
            WebServer.DefaultEvent.WebEventReceived += DefaultEvent_WebEventReceived;

            // event for turning the light on and off
            var webEventUpdateLightStatus = WebServer.SetupWebEvent("light");
            webEventUpdateLightStatus.WebEventReceived += webEventUpdateLightStatus_WebEventReceived;
        }

        void PollTemperature()
        {
            const int FIVE_MINUTES_IN_MS = 300000;
            var getCurrentTemperature = new GT.Timer(FIVE_MINUTES_IN_MS);
            getCurrentTemperature.Tick += getCurrentTemperature_Tick;

            getCurrentTemperature.Start();
        }

        void getCurrentTemperature_Tick(GT.Timer tick)
        {
            var currentTemp = tempHumidSI70.TakeMeasurement().TemperatureFahrenheit;
            var postContext = new POSTContent();

            var currentDateTime = DateTime.Now.ToString("MM/dd/yyyy") + "%20" + DateTime.Now.ToString("HH:MM:ss");

            var requestUrl = BASE_WEBADDRESS + "Sensor/LogTemperature?ReadDate=" + currentDateTime + "&Reading=" + (int)currentTemp;
            var request = HttpHelper.CreateHttpPostRequest(requestUrl, postContext, null);

            request.ResponseReceived += new HttpRequest.ResponseHandler(req_ResponseReceived);
            request.SendRequest();
        }

        void req_ResponseReceived(HttpRequest sender, HttpResponse response)
        {
            if(response.StatusCode != "200")
            {
                //networkdown.. do stuff?
            }
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

        void DefaultEvent_WebEventReceived(string path, WebServer.HttpMethod method, Responder responder)
        {
            string _data = "<html><head></head><body><h1>.NET Gadgeteer</h1>"
                            + " <p>IT'S UP AND RUNNING YO.  -  <3 John</p>"
                            + "<a href=\"http://68.37.152.77:4738/led?color=blue\">BLUE</a>&nbsp;&nbsp;&nbsp;<a href=\"http://68.37.152.77:4738/led?color=red\">RED</a>&nbsp;&nbsp;&nbsp;<a href=\"http://68.37.152.77:4738/led?color=purple\">PURPLE</a>"
                            + "</body></html>";

            responder.Respond(System.Text.Encoding.UTF8.GetBytes(_data), "text/html;charset=utf-8");
        }
    }
}
