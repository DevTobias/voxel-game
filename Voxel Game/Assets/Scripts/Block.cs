using UnityEngine;

public abstract class Block
{
    protected enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };

    protected bool isSolid;
    protected Vector3 position;
    protected Vector2[] blockUVs;

    private readonly Chunk owner;  
    protected readonly GameObject parent;

    public Block(Vector3 position, GameObject parent, Chunk owner)
    {
        this.position = position;
        this.parent = parent;
        this.owner = owner;
    }

    private void CreateQuad(Cubeside side)
    {
        // Create a new mesh and give it a name
        Mesh mesh = new Mesh
        {
            name = "CubeMesh" + side.ToString()
        };

        // Declare the needed vectors for the mesh
        Vector3[] vertices = null, normals = null;

        // Intitialize the triangles vector
        int[] triangles = new int[] { 3, 1, 0, 3, 2, 1 };

        // Initialize all the possible verticies
        Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

        // Set the value of the vectors based on the cubeside
        switch (side)
        {
            case Cubeside.BOTTOM:
                vertices = new Vector3[] { p0, p1, p2, p3 };
                normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                break;
            case Cubeside.TOP:
                vertices = new Vector3[] { p7, p6, p5, p4 };
                normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                break;
            case Cubeside.LEFT:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                break;
            case Cubeside.RIGHT:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                break;
            case Cubeside.FRONT:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                break;
            case Cubeside.BACK:
                vertices = new Vector3[] { p6, p7, p3, p2 };
                normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                break;
        }

        // Save the created vectors in the mesh and recalculate bounding volume
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;

        mesh.uv = new Vector2[] { blockUVs[3], blockUVs[2], blockUVs[0], blockUVs[1] }; ;
        mesh.RecalculateBounds();

        // Create the new GameObject for the Quad
        GameObject quad = new GameObject("Quad");

        // Set the position, parent object
        quad.transform.position = position;
        quad.transform.parent = parent.transform;

        // Add MeshFilter Component to the GameObject
        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
    }

    private bool HasSolidNeighbour(int x, int y, int z)
    {
        Block[,,] chunks = parent.GetComponent<Chunk>().chunkData;
        try
        {
            return chunks[x, y, z].isSolid;
        }
        catch (System.IndexOutOfRangeException) { }
        return false;
    }

    public void CreateBlock()
    {
        if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z + 1))
            CreateQuad(Cubeside.FRONT);
        if (!HasSolidNeighbour((int)position.x, (int)position.y, (int)position.z - 1))
            CreateQuad(Cubeside.BACK);
        if (!HasSolidNeighbour((int)position.x, (int)position.y + 1, (int)position.z))
            CreateQuad(Cubeside.TOP);
        if (!HasSolidNeighbour((int)position.x, (int)position.y - 1, (int)position.z))
            CreateQuad(Cubeside.BOTTOM);
        if (!HasSolidNeighbour((int)position.x - 1, (int)position.y, (int)position.z))
            CreateQuad(Cubeside.LEFT);
        if (!HasSolidNeighbour((int)position.x + 1, (int)position.y, (int)position.z))
            CreateQuad(Cubeside.RIGHT);
    }
}
