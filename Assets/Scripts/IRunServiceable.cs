using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRunServiceable
{
    // Runs this runserviceable object.
    // Returns true if run is complete.
    public abstract bool Run();
}
