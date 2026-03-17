using System.Collections.Generic;

public class MediumChunk : WorldChunkBase
{
    public override List<float> Lanes
    {
        get
        {
            return new(new float[] { -6f, -2f, 2f, 6f });
        }
    }

    public override float ChunkWidth
    {
        get
        {
            return ChunkSizeConfigGetter.MediumChunkLength;
        }
    }
}