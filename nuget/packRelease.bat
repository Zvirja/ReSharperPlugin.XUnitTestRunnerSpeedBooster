"c:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" ..\AlexPovar.XUnitTestRunnerSpeedBooster.sln  /t:Build /p:Configuration=Release
nuget pack ..\AlexPovar.XUnitTestRunnerSpeedBooster.nuspec -BasePath ..\AlexPovar.XUnitTestRunnerSpeedBooster\bin\Release