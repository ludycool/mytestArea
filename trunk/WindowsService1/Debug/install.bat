%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe  E:\project\testArea\trunk\WindowsService1\Debug\WindowsService1.exe 
net start Service1
sc config Service1 start= auto
pause