# Csharp.Apk-Reader
.NET library written in C# for Reading .apk Info (Manifest)


.Net Framework 3.5 And Newer is Supported





# Exmaple Get .apk details:

`string FileName = @"C:\file.apk";`

`var read = new Apk_Reader();`

`ApkInfo file_info = read.Get_File_Info(FileName);`




`Console.WriteLine("apkName: " + file_info.apkName);`

`Console.WriteLine("versionName: " + file_info.versionName);`

`Console.WriteLine("packageName: " + file_info.packageName);`

`Console.WriteLine("sdkVersion: " + file_info.sdkVersion);`

`Console.WriteLine("targetSdkVersion: " + file_info.targetSdkVersion);`

`Console.WriteLine("versionCode: " + file_info.versionCode);`
