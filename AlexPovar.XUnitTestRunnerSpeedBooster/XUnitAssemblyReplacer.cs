using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using AlexPovar.XUnitTestRunnerSpeedBooster.Properties;
using JetBrains.Annotations;
using JetBrains.Application.Environment;

namespace AlexPovar.XUnitTestRunnerSpeedBooster
{
  [EnvironmentComponent]
  public class XUnitAssemblyReplacer
  {
    private const string XUnitAssemblyName = "xunit.runner.utility.desktop.dll";
    private const string XUnitAssemblyBackupName = "xunit.runner.utility.desktop.dll.original";
    private const string XUnitAssemblyTempNewFileName = "xunit.runner.utility.desktop.dll.patched";

    public XUnitAssemblyReplacer()
    {
      Task.Run(() => DoAssemblyReplaceIfNeeded());
    }

    private void DoAssemblyReplaceIfNeeded()
    {
      try
      {
        var currentAssemblyLocation = Assembly.GetExecutingAssembly().Location;

        var pluginDir = Path.GetDirectoryName(currentAssemblyLocation);
        if (pluginDir == null) return;

        var xUnitAssemblyBackupPath = Path.Combine(pluginDir, XUnitAssemblyBackupName);

        //Likely, we already replaced assembly.
        if (File.Exists(xUnitAssemblyBackupPath)) return;

        var xUnitAssemblyPath = Path.Combine(pluginDir, XUnitAssemblyName);
        if (!File.Exists(xUnitAssemblyPath)) return;

        var currentVersion = FileVersionInfo.GetVersionInfo(xUnitAssemblyPath);
        if (!string.Equals(currentVersion.FileVersion, "2.2.0.3239", StringComparison.Ordinal)) return;

        //This way we identify that assembly was not already patched. Our own library has different description.
        if (!currentVersion.FileDescription.Equals("xUnit.net Runner Utility (desktop)", StringComparison.Ordinal))
          return;

        BackupOriginalXUnitAssembly(xUnitAssemblyPath, xUnitAssemblyBackupPath);

        var tempNewAssemblyPath = Path.Combine(pluginDir, XUnitAssemblyTempNewFileName);
        ExtractPatchedXUnitAssembly(tempNewAssemblyPath);

        ReplaceOriginalAssemblyWithPathed(xUnitAssemblyPath, tempNewAssemblyPath, xUnitAssemblyBackupPath);
      }
      catch
      {
        //Mute any exception.
      }
    }

    private void BackupOriginalXUnitAssembly([NotNull] string assemblyPath, [NotNull] string backupPath)
    {
      File.Copy(assemblyPath, backupPath, true);
    }

    private void ExtractPatchedXUnitAssembly([NotNull] string assemblyPath)
    {
      if (File.Exists(assemblyPath)) File.Delete(assemblyPath);
      File.WriteAllBytes(assemblyPath, Resources.PatchedXUnitLibrary);
    }

    private void ReplaceOriginalAssemblyWithPathed([NotNull] string assemblyPath, [NotNull] string newAssemblyPath, [NotNull] string backupAssemblyPath)
    {
      try
      {
        if (File.Exists(assemblyPath)) File.Delete(assemblyPath);
        File.Move(newAssemblyPath, assemblyPath);
      }
      catch
      {
        //If error occurred, it could happen that we already deleted the original assembly.
        //Try recover it from backup if no original file.
        try
        {
          if (!File.Exists(assemblyPath)) File.Copy(backupAssemblyPath, assemblyPath);
        }
        catch
        {
          //Well, something weird happened. No other way - just ignore.
        }
      }
    }
  }
}