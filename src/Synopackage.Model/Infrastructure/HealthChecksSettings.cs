using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synopackage
{
  public class HealthChecksSettings
  {
    public bool Enabled { get; set; }
    public int EvaluationTimeInSeconds { get; set; }
    public int DegragadedTimeInHours { get; set; }
  }
}
