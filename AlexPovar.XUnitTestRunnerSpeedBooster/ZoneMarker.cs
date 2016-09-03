using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Feature.Services;
using JetBrains.ReSharper.Psi.CSharp;

namespace AlexPovar.XUnitTestRunnerSpeedBooster
{
  [ZoneMarker]
  public class ZoneMarker : IRequire<IEnvironmentZone>
  {
  }
}
