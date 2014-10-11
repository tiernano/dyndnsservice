using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using NLog;

namespace DNSUpdater.Controllers
{
    public class nicController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        // GET: nic
        public string Update(string hostname, string myip)
        {
            if (string.IsNullOrWhiteSpace(hostname) || string.IsNullOrWhiteSpace(myip))
            {
                logger.Error("Hostname or IP is blank: Host: {0} IP {1}", hostname, myip);
                return "Whoops!";
            }
            string fileName = string.Format(HttpContext.Server.MapPath("~/App_Data/{0}.txt"), hostname);
            string existingIP = string.Empty;
            if (System.IO.File.Exists(fileName))
            {
                existingIP = System.IO.File.ReadAllText(fileName);
                if (existingIP == myip)
                {
                    logger.Debug("Existing IP same as New IP... Skipping: Host {0} -  IP {1}",hostname, myip);
                }
                else
                {
                    logger.Info("New IP Detected for {0} - {1} was {2}", hostname, myip, existingIP);
                    System.IO.File.Delete(fileName);
                    System.IO.File.WriteAllText(fileName, myip);
                }
            }
            else
            {
                logger.Info("New Host {0} IP {1}", hostname, myip);
                System.IO.File.WriteAllText(fileName, myip);
            }
            
            return "done";
        }

        public string Get(string hostname)
        {
            if (string.IsNullOrWhiteSpace(hostname))
            {
                logger.Error("Host is empty");
                return "whoops!";
            }
            else
            {
                string fileName = string.Format(HttpContext.Server.MapPath("~/App_Data/{0}.txt"), hostname);

                if (System.IO.File.Exists(fileName))
                {
                    string result = System.IO.File.ReadAllText(fileName);
                    return result;
                }
                else
                {
                    logger.Error("Host {0} does not exist", hostname);
                    return "whoops!";
                }
                
            }
        }
    }
}