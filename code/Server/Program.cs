﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

using System.Diagnostics;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Specialized;
using CounterStrikeLive.Server.Properties;
using doru;

using CounterStrikeLive.Service;

namespace CounterStrikeLive.Server
{

    internal class Program
    {
        public Settings Settings { get { return Properties.Settings.Default; } }
        private static void Main(String[] args)
        {            
            Logging.Setup(args.Length > 0 ? args[0] : "../../../");                        
            new Program();
        }
        
        public Program()
        {
            "started".Trace();
            if (Settings._ResetConfig)
                Config._XmlSerializer.Serialize(Settings._ClientBin + "Config.xml",new Config());
            else
                Config._XmlSerializer.DeserealizeOrCreate<Config>(Settings._ClientBin + "Config.xml", new Config());

            GameServer _Server = new GameServer();

            _Server.StartAsync();            

            PolicyServer ps = new PolicyServer { policyFile = "Server/PolicyFile.xml" };
            ps.StartAsync();

            WebServer _WebServer = new WebServer();
            _WebServer.StartAsync();

            PhpSender _PhpSender = new PhpSender();
            _PhpSender.StartAsync();
            Thread.Sleep(-1);
        }
    }

}