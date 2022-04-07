namespace SavingSystem
{
    public interface ISavable
    {
        /// <summary>
        /// Captures object of any type by boxing to object.
        /// </summary>
        /// <returns>Object to serialize.</returns>
        object CaptureState();

        /// <summary>
        /// Restoring data. To avoid unboxing errors provide exactly the same type as it was in CaptureState.
        /// </summary>
        /// <param name="state">Serialized object from saving system (captured state).</param>
        void RestoreState(object state);
    }
}