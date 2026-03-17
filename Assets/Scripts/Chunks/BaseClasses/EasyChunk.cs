using System.Collections.Generic;

public class EasyChunk : WorldChunkBase
{
    public override List<float> Lanes
    {
        get
        {
            return new(new float[] { 4f, 0f, -4f });
        }
    }

    public override float ChunkWidth
    {
        get
        {
            return ChunkSizeConfigGetter.EasyChunkLength;
        }
    }
}