using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : Block
{
    public Dirt(Vector3 position, GameObject parent, Chunk owner) : base(position, parent, owner)
    {
        isSolid = true;
        blockUVs = Chunk.blockAtlasHandler.GetUVInformation(2, 0).BlockUV;
    }

}
