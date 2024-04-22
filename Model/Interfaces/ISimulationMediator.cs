namespace Model.Interfaces
{
    public interface ISimulationMediator : IMediator
    {
        void LoadConfig(string fileName);
        void SaveSimulation(string filepath);

    }
}
