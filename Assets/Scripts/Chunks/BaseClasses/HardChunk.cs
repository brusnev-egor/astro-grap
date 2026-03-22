using System.Collections.Generic;

public class HardChunk : WorldChunkBase
{
    public override List<float> Lanes
    {
        get
        {
            return new(new float[] { -8f, -4f, 0f, 4f, 8f });
        }
    }
    public override float ChunkWidth
    {
        get
        {
            return ChunkSizeConfigGetter.HardChunkLength;
        }
    }
}