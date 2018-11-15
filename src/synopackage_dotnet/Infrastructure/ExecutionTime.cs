using System;

namespace synopackage_dotnet
{
  public class ExecutionTime
  {
    private DateTime? start;
    private DateTime? stop;
    public void Start()
    {
      start = DateTime.Now;
    }
    public void Stop()
    {
      if (start == null)
        throw new Exception("Invalid Stop usage");
      stop = DateTime.Now;
    }

    public void Reset()
    {
      this.start = null;
      this.start = null;
    }

    public int GetDiff()
    {
      if (!start.HasValue && !stop.HasValue)
        throw new Exception("Invalid Start/Stop usage");
      TimeSpan ts = stop.Value - start.Value;
      return Convert.ToInt32(ts.TotalMilliseconds);
    }
  }
}