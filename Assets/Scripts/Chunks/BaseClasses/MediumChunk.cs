public class MediumChunk : WorldChunkBase
{
    protected override float ChunkWidth
    {
        get
        {
            return ChunkSizeConfigGetter.MediumChunkLength;
        }
    }
}