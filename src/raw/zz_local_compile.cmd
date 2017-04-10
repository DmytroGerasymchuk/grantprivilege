@echo off
"%windir%\Microsoft.NET\Framework\v2.0.50727\csc.exe" /out:grantprivilege.exe AssemblyInfo.cs LsaUtility.cs Program.cs