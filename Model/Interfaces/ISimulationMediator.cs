namespace Model.Interfaces
{
    public interface ISimulationMediator : IMediator
    {
        /// <summary>
        /// Load simulation from config file.
        /// </summary>
        /// <param name="fileName">Abolute path of the config file</param>
        void LoadConfig(string fileName);

        /// <summary>
        /// Save the log into a file.
        /// </summary>
        /// <param name="filepath">Absolute path to save the log</param>
        void SaveSimulation(string filepath);

    }
}
