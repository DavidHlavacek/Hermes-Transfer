using System;
using Rebex.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebex.IO;

namespace experimentSFTP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * Download using Nuget, Rebex.Net and Rebex.Sftp; 
             * using Rebex.Net; using Rebex.IO;
             
              Traversal mode explanation
               
                Rebex docs: https://blog.rebex.net/download-and-upload-with-ftp-and-sftp-components-using-traversalmode
                On short: it is how it searches for the files

                Transfer Method - still researching
                
            */



            //30 day license key, used since 23/06
            Rebex.Licensing.Key = "==AJxMmIrQeJ87QJaW9ySkUdVVE3N+N9me5ell+rGMjs4Y==";


            //Download directory from sftp
            /*
            try
            {
                Sftp client = new Sftp();
                client.Connect("hermes.serverict.nl");
                client.Login("hermes", "RCF&9xdr");
                Console.WriteLine("Connected to server!");
                client.Download("/httpdocs/", @"D:\downloadSftp", TraversalMode.Recursive); //sftp folder, local PC folder to be downloaded in
                Console.WriteLine("Files Downloaded");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            */

            //Upload entire folder files to server
            /*
            Sftp client = new Sftp();
            try
            {
                client.Connect("hermes.serverict.nl");
                client.Login("hermes", "RCF&9xdr");
                Console.WriteLine("Connected to server!");
                client.Upload(@"D:\thisAllPC\*", "/httpdocs", TraversalMode.MatchFilesShallow,
                TransferMethod.Copy, ActionOnExistingFiles.OverwriteAll); //change path leave the \* so everything before \*
                Console.WriteLine("Directory uploaded");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
            */

            //Connect to server
            /*
            Sftp client = new Sftp();
            try
            {
                client.Connect("hermes.serverict.nl");
                client.Login("hermes", "RCF&9xdr");
                Console.WriteLine("Success!");
                client.Disconnect();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
            */

            Console.ReadLine();
        }
    }
}

