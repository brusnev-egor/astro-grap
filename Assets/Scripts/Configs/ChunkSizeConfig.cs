using UnityEngine;

[CreateAssetMenu(menuName = "Config/ChunkSizeConfig")]
public class ChunkSizeConfig : ScriptableObject
{
    [Header("Chunk length")]
    public float easyChunkLength = 15f;
    public float mediumChunkLength = 10f;
    public float hardChunkLength = 7f;
}

public static class ChunkSizeConfigGetter
{
    private static ChunkSizeConfig _config;

    private static ChunkSizeConfig Config
    {
        get
        {
            if (_config == null)
            {
                _config = Resources.Load<ChunkSizeConfig>("Configs/ChunkSizeConfig");
            }

            return _config;
        }
    }

    public static float EasyChunkLength => Config.easyChunkLength;
    public static float MediumChunkLength => Config.mediumChunkLength;
    public static float HardChunkLength => Config.hardChunkLength;
}