namespace Tools.GamePatterns
{
    /// <summary>
    /// Interface for Pooled prefab
    /// </summary>
    public interface IPooledObject
    {
        /// <summary>
        /// called when the object is spawned
        /// </summary>
        void OnObjectSpawn();
    }
}