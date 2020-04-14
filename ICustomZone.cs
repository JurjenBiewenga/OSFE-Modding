namespace OSFEModding
{
    /// <summary>
    /// Custom zone, must have parameterless constructor
    /// </summary>
    public interface ICustomZone
    {
        void GenerateZone(SpawnCtrl spawnCtrl, ZoneType zoneType);
        void SetupZone(WorldBar worldBar);
    }
}