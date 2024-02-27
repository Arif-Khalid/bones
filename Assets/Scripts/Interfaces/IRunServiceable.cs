/**
 * Implemented by classes that use the run service
 */
public interface IRunServiceable
{
    // Runs this runserviceable object.
    // Returns true if run is complete, false otherwise
    public abstract bool Run();
}
