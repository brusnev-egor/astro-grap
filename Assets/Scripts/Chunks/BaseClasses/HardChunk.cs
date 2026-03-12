public class HardChunk : WorldChunkBase
{
    protected override float ChunkWidth
    {
        get
        {
            return ChunkSizeConfigGetter.HardChunkLength;
        }
    }
}