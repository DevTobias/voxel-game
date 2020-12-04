using System.Collections.Generic;
using UnityEngine;

public class UVInformation
{
    public Vector2[] BlockUV { get; private set; }

    public UVInformation(List<float> coordinates)
    {
        BlockUV = new Vector2[4];
        int j = 0;
        for (int i = 0; i < 4; ++i)
        {
            BlockUV[i] = new Vector2(coordinates[j], coordinates[j + 1]);
            j += 2;
        }
    }

    public override string ToString()
    {
        string result = "{ ";
        foreach (Vector2 coordinate in BlockUV)
        {
            result += "[" + coordinate.x + " | " + coordinate.y + "], ";
        }
        return result.Remove(result.Length - 2, 2) + " }";
    }
}

public class BlockAtlasHandler
{
    private readonly Texture2D texture;
    private readonly int colCount;
    private readonly int rowCount;
    private readonly UVInformation[,] UVMap;

    public BlockAtlasHandler(Texture2D texture, int colCount, int rowCount)
    {
        this.texture = texture;
        this.colCount = colCount;
        this.rowCount = rowCount;
        this.UVMap = new UVInformation[rowCount, colCount];
        CalculateUVS();
    }

    private void CalculateUVS()
    {
        float textureWidth = (float)texture.width;
        float textureHeight = (float)texture.height;
        int blockWidth = texture.width / colCount;
        int blockHeight = texture.height / rowCount;

        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < colCount; c++)
            {
                float uv_x1 = (c * blockWidth) / textureWidth;
                float uv_y1 = (r * blockHeight) / textureHeight;
                float uv_x2 = (c * blockWidth + blockWidth) / textureWidth;
                float uv_y2 = (r * blockHeight) / textureHeight;
                float uv_x3 = (c * blockWidth) / textureWidth;
                float uv_y3 = (r * blockHeight + blockHeight) / textureHeight;
                float uv_x4 = (c * blockWidth + blockWidth) / textureWidth;
                float uv_y4 = (r * blockHeight + blockHeight) / textureHeight;
                UVMap[r, c] = new UVInformation(new List<float> { uv_x1, uv_y1, uv_x2, uv_y2, uv_x3, uv_y3, uv_x4, uv_y4 });
            }
        }
    }

    public UVInformation GetUVInformation(int x, int y)
    {
        return UVMap[colCount - y - 1, x];
    }
}