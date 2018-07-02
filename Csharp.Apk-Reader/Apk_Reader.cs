using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Resources;

namespace Csharp.Apk_Reader
{
    public class Apk_Reader
    {
        private String[] aapt_Argument = { "dump", "badging", "" };

        public ApkInfo Get_File_Info(String apk_path)
        {
            String Mainifest = ReadApk("\"" + @apk_path + "\"");
            ApkInfo apkInfo = new ApkInfo(Mainifest);
            return apkInfo;
        }


        public String showFileSize(String filePath)
        {
            long filesize = new FileInfo(filePath).Length;
            if (filesize <= 1024) { return filesize + "B"; }
            double filesize_KB = filesize / 1024.0;
            if (filesize_KB <= 1024) { return filesize_KB.ToString("#.##") + "KB"; }
            return (filesize_KB / 1024).ToString("#.##") + "MB(" + filesize_KB.ToString("#") + "KB)";
        }

        private String executeCommand(String fileName, String[] args)
        {
            string exePath = Path.Combine(Path.GetTempPath(), fileName);
            if (File.Exists(exePath))
            {
                if((new FileInfo(exePath).Length) != Csharp.Apk_Reader.Properties.Resources.aapt.Length)
                {
                    exePath = Path.Combine(Path.GetTempPath(), "apk_reader_"+fileName);
                }
            }
            if (!File.Exists(exePath))
            {
                File.WriteAllBytes(exePath, Csharp.Apk_Reader.Properties.Resources.aapt);
            }


            Process myProcess = new Process();
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(exePath);
            String argument = null;
            for (int i = 0; i < args.Length; i++)
            {
                argument += " " + args[i];
            }
            myProcessStartInfo.Arguments = argument;
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.CreateNoWindow = true;
            myProcessStartInfo.RedirectStandardOutput = true;
            myProcessStartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8; // this is the most important part, to get correct myString, see below
            myProcess.StartInfo = myProcessStartInfo;
            myProcess.Start();
            StreamReader myStreamReader = myProcess.StandardOutput;
            string myString = myStreamReader.ReadToEnd();
            myProcess.WaitForExit();
            myProcess.Close();
            return myString;
        }

        private String ReadApk(String apkPath)
        {
            aapt_Argument[2] = apkPath;
            return executeCommand("aapt.exe", aapt_Argument);
        }

        private void ExtractResource(string resource, string path)
        {
            Stream stream = GetType().Assembly.GetManifestResourceStream(resource);
            byte[] bytes = new byte[(int)stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            File.WriteAllBytes(path, bytes);
        }

    }

    public class ApkInfo
    {
        public String allManifest { get; }
        public String apkName { get; }
        public String versionName { get; }
        public String packageName { get; }
        public String sdkVersion { get; }
        public String targetSdkVersion { get; }
        public String versionCode { get; }

        public ApkInfo(String Mainifest)
        {
            allManifest = Mainifest;
            apkName = getAttribution(@"application-label:'(.*?)'");
            versionName = getAttribution(@"versionName='(.*?)'");
            packageName = getAttribution(@"package: name='(.*?)'");
            sdkVersion = getAttribution(@"sdkVersion:'(.*?)'");
            targetSdkVersion = getAttribution(@"targetSdkVersion:'(.*?)'");
            versionCode = getAttribution(@"versionCode='(.*?)'");
        }

        public String ToString()
        {
            string result = "apkName: "+ apkName;
            result += " | versionName: "+ versionName;
            result += " | packageName: "+ packageName;
            result += " | sdkVersion: "+ sdkVersion;
            result += " | targetSdkVersion: "+ targetSdkVersion;
            result += " | versionCode: "+ versionCode;
            return result;
        }

        private String getAttribution(String regex)
        {
            Regex reg = new Regex(regex);
            Match m = reg.Match(allManifest);
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            return "";
        }
    }
}
