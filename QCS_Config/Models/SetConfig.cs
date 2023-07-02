using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;

using System.Runtime.InteropServices;
using System;


namespace QCS_Config.Models
{
    public static class SetConfig
    {
        //public static List<publicAppSettingModel> subSettings;
        public static List<Variables> Variables { get; set; }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetActiveWindow();
        public static bool init(string appSettingFileAddress)
        {
            try
            {
                //subSettings = new List<publicAppSettingModel>();
                Variables = new List<Variables>();

                var myJsonString = File.ReadAllText(appSettingFileAddress);


                JObject o = JObject.Parse(myJsonString);


                var infoTag = o.SelectToken("Info");
                if (infoTag != null)
                {
                    foreach (JProperty subTag in infoTag.Children())
                    {
                        Variables.Add(new Models.Variables { key = subTag.Name, value = subTag.Value.ToString() });
                    }
                }
                //var result = WNetAddConnection2(netResource,credentials.Password,userName,0);
                if (o.SelectToken("Info.@fromFile") != null)
                {
                    //var declareLines = File.ReadLines(Path.Combine(o.SelectToken("Info.@fromFile").Value<string>(), "Declare.txt"));

                    var file = o.SelectToken("Info.@fromFile").Value<string>();
                    string VariableAllText;
                    if (file.IndexOf("@") > -1)
                    {
                        var remoteInfo = file.Split("@");
                        var ip = remoteInfo[0].Replace(@"\\", "");
                        ip = ip.Substring(0, ip.IndexOf(@"\"));
                        //NetworkCredential SMBCredential = new NetworkCredential
                        //{
                        //    UserName = remoteInfo[1],
                        //    Password = remoteInfo[2]
                        //};
                        int rc = 0;
                       
                     
                        

                        //using (new WNet.Connection.NetworkConnection(@"\\"+ ip, SMBCredential))
                        //{
                        if (File.Exists(remoteInfo[0]) == false) throw new System.Exception("Varibles file does not exist");
                        VariableAllText = File.ReadAllText(remoteInfo[0]);
                       
                        //}
                    }
                    else
                    {

                        if (File.Exists(file) == false) throw new System.Exception("Varibles file does not exist");
                        VariableAllText = File.ReadAllText(file);
                    }
                    string[] VariableLines = VariableAllText.Split(";");


                    foreach (var Variable in VariableLines)
                    {
                        if (string.IsNullOrEmpty(Variable)) continue;
                        var v = Variable.Split("=");
                        var key = v[0].TrimStart().TrimEnd();
                        var value = v[1].TrimStart().TrimEnd();
                        if (string.IsNullOrEmpty(key)) continue;
                        Variables.Add(new Models.Variables { key = key, value = value });
                    }



                    //foreach (var declareLine in declareLines)
                    //{
                    //    var splitLine = declareLine.Split(":");
                    //    subSettings.Add(new publicAppSettingModel
                    //    {
                    //        tag = splitLine[0],
                    //        file = splitLine[1]
                    //    });
                    //}
                    //foreach (var subSett in subSettings)
                    //{
                    //    if (o.SelectToken(subSett.tag) != null)
                    //    {
                    //        var fileSubConfig = File.ReadAllText(Path.Combine(o.SelectToken("Info.@fromFile").Value<string>(), subSett.file));
                    //        foreach (var SubConfig in fileSubConfig)
                    //        {
                    //            var tag = SubConfig.ToString().Split(":")[0].Replace("\"", "").TrimEnd().TrimStart();

                    //            if (o.SelectToken(tag) == null)
                    //            {
                    //                var value = SubConfig.ToString().Split(":")[0].Replace("\"", "").TrimEnd().TrimStart();
                    //                o.Add(value, subSett.tag + "." + tag);
                    //            }
                    //        }
                    //    }
                    //}
                }
                foreach (var Variable in Variables)
                {
                    //Variable.key = Variable.key.Replace(@"\n","");
                    if (Variable.key.Contains("\n") )
                    {
                    Variable.key = Variable.key.Split("\n")[1];

                    }
                    if (Variable.key.Contains("ARR"))
                    {
                        myJsonString = myJsonString.Replace("\"%(" + Variable.key + ")\"", Variable.value);
                    }
                    else
                    {

                    myJsonString = myJsonString.Replace("%(" + Variable.key + ")", Variable.value);
                    }
                }
                if (myJsonString.IndexOf("%(") > -1) 
                    throw new System.Exception("No value found for some variables");
                var myJObject = JObject.Parse(myJsonString);
                var obj = myJObject["Config"] as JObject;
                Config.All = obj.ToObject<ConfigModels>();



                return true;
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
    }

    public class publicAppSettingModel
    {
        public string tag { get; set; }
        public string file { get; set; }
    }
    public class Variables
    {
        public string key { get; set; }
        public string value { get; set; }

    }
}
