namespace Model.Interfaces
{
    public interface ISimulationMediator : IMediator
    {
        /// <summary>
        /// Loads simulation from config file.
        /// </summary>
        /// <param name="fileName">Abolute path of the config file.</param>
        void LoadConfig(string fileName);

        /// <summary>
        /// Saves the log into a file.
        /// </summary>
        /// <param name="filepath">Absolute path where to save the log.</param>
        void SaveSimulation(string filepath);

        /// <summary>
        /// Sets options for the controller.
        /// </summary>
        /// <param name="interval">Milliseconds the controller has for calculation.</param>
        /// <param name="lastStep">The duration of the simulation in steps.</param>
        void SetOptions(int interval, int lastStep);
    }
}
