using System;
using System.Text;
using EnumsNET;
using Owleye.Domain;
using Owleye.Application.Notifications.Messages;

namespace Owleye.Application.Services
{
    public static class NotifyMessagePreparationService
    {
        public static string Prepare(NotifyViaEmailMessage message)
        {
            var stringBuilder = new StringBuilder();

            switch (message.IsServiceAlive)
            {
                case false:
                    {
                        stringBuilder.Append("<b> Owleye Service ALERT</b>" + "<br/>");

                        switch (message.SensorType)
                        {
                            case SensorType.Ping:
                                {
                                    stringBuilder.Append(
                                        $"{SensorType.Ping.AsString(EnumFormat.Description)} for IP {message.IpAddress} failed." + "<br/>");
                                    break;
                                }

                            case SensorType.PageLoad:
                                {
                                    stringBuilder.Append(
                                        $"{SensorType.PageLoad.AsString(EnumFormat.Description)} for url {message.ServiceUrl} failed." + "<br/>");
                                    break;
                                }
                        }

                        break;
                    }

                case true:
                    {
                        stringBuilder.Append("<b> Owleye Service Notification </b>" + "<br/>");

                        switch (message.SensorType)
                        {
                            case SensorType.Ping:
                                {
                                    stringBuilder.Append(
                                        $"{SensorType.Ping.AsString(EnumFormat.Description)} for IP {message.IpAddress} available." + "<br/>");
                                    break;
                                }

                            case SensorType.PageLoad:
                                {
                                    stringBuilder.Append(
                                        $"{SensorType.PageLoad.AsString(EnumFormat.Description)} for url {message.ServiceUrl} available." + "<br/>");
                                    stringBuilder.Append(
                                       $"{SensorType.PageLoad.AsString(EnumFormat.Description)} was unavailable for {Convert.ToInt32((DateTime.Now - message.LastAvailable).TotalMinutes.ToString())} minutes." + "<br/>");
                                    break;
                                }
                        }

                        break;
                    }
            }

            stringBuilder.Append("<hr/> <b> Owleye monitoring system. </b>");
            return stringBuilder.ToString();
        }
        public static string PrepareMailTitle(string endPointUrl, bool status)
        {
            var availStatus = status == true ? "Avilable" : "Down";
            return $"Owleye {endPointUrl} is {availStatus}";
        }
    }
}
