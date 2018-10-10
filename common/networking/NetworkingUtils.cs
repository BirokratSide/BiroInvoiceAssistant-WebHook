using si.birokrat.next.common.logging;
using si.birokrat.next.common.shell;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace si.birokrat.next.common.networking {
    public static class NetworkingUtils {
        public static string GetLocalIPAddress() {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
                return null;
            }
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }

        public static bool OpenFirewallPort(string name, int port, string dir = "in", string action = "allow", string protocol = "TCP", string profile = "private") {
            string command;
            string result;

            command = $"netsh advfirewall firewall show rule name={name}";
            result = PowerShell.Execute(command);
            if (!result.Contains("No rules match the specified criteria.")) {
                if (Regex.IsMatch(result, $@"LocalPort:.*{port}{Environment.NewLine}")) {
                    Logger.Log("Firewall", $"Port {port} already opened.");
                    return true;
                } else {
                    command = $"netsh advfirewall firewall delete rule name={name}";
                    try {
                        result = PowerShell.Execute(command, administrator: true);
                    } catch (UnauthorizedAccessException ex) {
                        Logger.Log("Exception", ex.Message);
                        Logger.Log("Napaka", "Nimate administratorskih pravic.", toLog: false);
                        return false;
                    }
                }
            }

            command = $"netsh advfirewall firewall add rule name={name} dir=in action={action} protocol={protocol} profile={profile} localport={port}";
            try {
                result = PowerShell.Execute(command, administrator: true);
            } catch (UnauthorizedAccessException ex) {
                Logger.Log("Exception", ex.Message);
                Logger.Log("Napaka", "Nimate administratorskih pravic.", toLog: false);
                return false;
            }

            bool success = result.Contains("Ok.");
            if (success) {
                Logger.Log("Firewall", $"Opened port {port}.");
            }

            return success;
        }

        public static bool ReserveUrlForNonAdministratorUsersAndAccounts(int port, string scheme = "http") {
            string command;
            string result;

            command = $"netsh http show urlacl url={scheme}://+:{port}/";
            result = PowerShell.Execute(command);
            if (result.Contains("Reserved URL")) {
                Logger.Log("Url Reservation", $"Url {scheme}://+:{port}/ already has a reservation.");
                return true;
            }

            command = $"netsh http add urlacl {scheme}://+:{port}/ user=$env:UserName";
            try {
                result = PowerShell.Execute(command, administrator: true);
            } catch (UnauthorizedAccessException ex) {
                Logger.Log("Exception", ex.Message);
                Logger.Log("Napaka", "Nimate administratorskih pravic.", toLog: false);
                return false;
            }

            bool success = result.Contains("URL reservation successfully added");
            if (success) {
                Logger.Log("Url Reservation", $"Url {scheme}://+:{port}/ reserved.");
            }

            return success;
        }
    }
}
