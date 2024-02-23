using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

namespace BEData
{
    public class Logger
    {

        static string dateFormat = "ddMMM";
        public enum LoggerType { Error, Info, Warning }

        /// <summary>
        /// creates a folder for today in the format "01jan"
        /// </summary>
        /// <param name="folderLocation">folder location till the last directort eg: c://users//test</param>
        public string CreateAndOrGetFolderName()
        {
            string folderLocation = "";// get from config
            folderLocation = System.Configuration.ConfigurationManager.AppSettings["LoggerLocation"];
            string folderName = DateTime.Now.ToString(dateFormat);
            string folder = folderLocation + "\\" + folderName;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }

        string CreateAndeOrGetLogFile(string userName, string folderLocation)
        {

            if (!File.Exists(folderLocation + "\\" + userName + ".html"))
            {
                string content = "  <html><head><title></title><style type=\"text/css\">" +
         " .Error { background-color: #e3a8a8;   color: Black;  font-size: smaller; }" +
          ".Info { background-color: #c7dec9;  color: Black;  font-size: smaller;   }" +
          " .Warning { background-color: #f7f76c;  color: #000000;  font-size: smaller;  }" +
     " </style></head><body>   <form id=\"form1\" runat=\"server\" >" +
     " <table width=\"100%\" style=\"font-family: Verdana; font-size: small;\">" +
          "<tr style=\"background-color: black; font-size: medium; color: White; font-weight: bold;\">" +
              "<td style=\"width: 7%\">" +
              "Time  </td> <td style=\"width: 20%\">   Source   </td>   <td>" +
                  "Message, Stack Trace , Information   </td>  </tr>";
                // File.Create(folderLocation + "\\" + userName + ".html");
                File.AppendAllText(folderLocation + "\\" + userName + ".html", content);
            }
            return folderLocation + "\\" + userName + ".html";

        }



        public   void LogErrorToServer(LoggerType errorType, string filename, string methodName, string message, string innerException)
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString().Split('\\')[1];
            string folder = CreateAndOrGetFolderName(); // 01Mar
            string file = CreateAndeOrGetLogFile(userName, folder);
            string contents = CreateRow(errorType, filename, methodName, message, innerException);
            File.AppendAllText(file, contents);
        }






        string CreateRow(LoggerType typeOfError, string sourceFile, string FunctionName, string message, string information)
        {
            string temp = string.Empty;
            StringBuilder builder = new StringBuilder();
            builder.Append(@" <tr class=""" + typeOfError.ToString() + @""">");
            builder.AppendFormat("<td>{0}</td>", DateTime.Now.ToString("t"));
            builder.AppendFormat(" <td><b> {0}</b><br />", sourceFile);
            builder.AppendFormat(" <i style=\"font-size: smaller\"> {0}  </i></td>", FunctionName);
            builder.AppendFormat("<td> <b>{0}</b> <br /> ", message);
            builder.AppendFormat(" <i> {0} </i> ", information);
            builder.Append(" /</td></tr>");
            return builder.ToString();
        }

    }
}