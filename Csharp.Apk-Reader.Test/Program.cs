using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Csharp.Apk_Reader;

namespace Csharp.Apk_Reader.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Apk Reader ...");

            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FileName = string.Format("{0}Resources\\cpu-z-1-26.apk", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));

            Console.WriteLine("Test Apk File Path: "+ FileName);

            var read = new Apk_Reader();
            Console.WriteLine("File Size: " + read.showFileSize(FileName));
            ApkInfo file_info = read.Get_File_Info(FileName);
            Console.WriteLine("apkName: " + file_info.apkName);
            Console.WriteLine("versionName: " + file_info.versionName);
            Console.WriteLine("packageName: " + file_info.packageName);
            Console.WriteLine("sdkVersion: " + file_info.sdkVersion);
            Console.WriteLine("targetSdkVersion: " + file_info.targetSdkVersion);
            Console.WriteLine("versionCode: " + file_info.versionCode);
            Console.WriteLine("nativeCode: " + file_info.nativeCode);

            Console.ReadKey();
        }
    }
}
