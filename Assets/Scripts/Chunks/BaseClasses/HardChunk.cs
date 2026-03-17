public class HardChunk : WorldChunkBase
{
    public override float ChunkWidth
    {
        get
        {
            return ChunkSizeConfigGetter.HardChunkLength;
        }
    }
}