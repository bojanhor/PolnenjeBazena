using System;
using Microsoft
namespace InstallWebPage

{
    class Program
    {



        static void Main(string[] args)
        {
            ServerManager manager = ServerManager.OpenRemote("serverName");
        }
    }
}
