﻿using Selenium.Core;
using Selenium.Internal;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace Selenium {

    /// <summary>
    /// Web driver for Opera
    /// </summary>
    /// 
    /// <example>
    /// 
    /// VBScript:
    /// <code lang="vbs">	
    /// Class Script
    ///     Dim driver
    ///     
    ///     Sub Class_Initialize
    ///         Set driver = CreateObject("Selenium.OperaDriver")
    ///         driver.Get "http://www.google.com"
    ///     End Sub
    /// 
    ///     Sub Class_Terminate
    ///         driver.Quit
    ///     End Sub
    /// End Class
    /// 
    /// Set s = New Script
    /// </code>
    /// 
    /// VBA:
    /// <code lang="vbs">	
    /// Public Sub Script()
    ///   Dim driver As New OperaDriver
    ///   driver.Get "http://www.google.com"
    ///   ...
    ///   driver.Quit
    /// End Sub
    /// </code>
    /// 
    /// </example>
    [ProgId("Selenium.OperaDriver")]
    [Guid("0277FC34-FD1B-4616-BB19-9E7F9EF1D002")]
    [ComVisible(true), ClassInterface(ClassInterfaceType.None)]
    public class OperaDriver : WebDriver, ComInterfaces._WebDriver {

        internal override IDriverService StartService() {
            return OperaDriver.StartService(this);
        }

        internal override Capabilities ExtendCapabilities() {
            return OperaDriver.ExtendCapabilities(this);
        }

        internal static IDriverService StartService(WebDriver wd) {
            var svc = new DriverService(IPAddress.Loopback);
            svc.AddArgument("--port=" + svc.EndPoint.Port.ToString());
            svc.AddArgument("--silent");
            svc.Start("operadriver.exe");
            return svc;
        }

        static readonly string[] OPTIONS = { "binary", "args", "extensions", "prefs" };

        internal static Capabilities ExtendCapabilities(WebDriver wd, bool remote = false) {
            var capa = wd.Capabilities;
            capa.Browser = "opera";

            var opts = new Dictionary();

            foreach (string key in OPTIONS) {
                string value;
                if (capa.TryGetValue(key, out value)) {
                    capa.Remove(key);
                    opts.Add(key, value);
                }
            }

            if (wd.Profile != null)
                wd.Arguments.Add("user-data-dir=" + ExpandProfile(wd.Profile, remote));

            if (wd.Arguments.Count != 0)
                opts.Add("args", wd.Arguments);

            if (wd.Extensions.Count != 0)
                opts.Add("extensions", wd.Extensions);

            if (wd.Preferences.Count != 0)
                opts.Add("prefs", wd.Preferences);

            capa["operaOptions"] = opts;

            return capa;
        }

        private static string ExpandProfile(string profile, bool remote) {
            if (!remote) {
                if (IOExt.IsPath(profile)) {
                    profile = IOExt.ExpandPath(profile);
                } else {
                    profile = IOExt.AppDataFolder + @"\Opera Software\Opera\Profiles\" + profile;
                }
                Directory.CreateDirectory(profile);
            }
            return profile;
        }

    }

}