using System.Collections;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Texture2D _blockatlas;
    public Material _blockMaterial;

    public static Texture2D blockatlas;
    public static Material blockMaterial;
    public static BlockAtlasHandler blockAtlasHandler;

	public GameObject chunk;
	public Block[,,] chunkData;

	IEnumerator BuildChunk(int sizeX, int sizeY, int sizeZ)
	{
		chunkData = new Block[sizeX, sizeY, sizeZ];
		for (int z = 0; z < sizeZ; ++z)
			for (int y = 0; y < sizeY; ++y)
				for (int x = 0; x < sizeX; ++x)
				{
					Vector3 pos = new Vector3(x, y, z);
					chunkData[x, y, z] = new Dirt(pos, chunk, this);
				}

		for (int z = 0; z < sizeZ; ++z)
			for (int y = 0; y < sizeY; ++y)
				for (int x = 0; x < sizeX; ++x)
				{
					chunkData[x, y, z].CreateBlock();
				}

		CombineQuads();
		yield return null;
	}

	void Start()
    {
        blockatlas = _blockatlas;
        blockMaterial = _blockMaterial;
        blockAtlasHandler = new BlockAtlasHandler(blockatlas, 16, 16);

		// chunk = new GameObject("Chunk");
		// chunk.transform.position = new Vector3(0, 0, 0);
		chunk = gameObject;

		StartCoroutine(BuildChunk(16, 16, 16));
	}

	private void CombineQuads()
	{

		// Combine all children meshes
		MeshFilter[] meshFilters = chunk.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		for (int i = 0; i < meshFilters.Length; ++i)
		{
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
		}

		// Create a new mesh on the parent object
		MeshFilter newMeshFilter = (MeshFilter)chunk.gameObject.AddComponent(typeof(MeshFilter));
		newMeshFilter.mesh = new Mesh();

		// Add combined meshes on children as the parent's mesh
		newMeshFilter.mesh.CombineMeshes(combine);

		// Create a renderer for the parent
		MeshRenderer renderer = chunk.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		renderer.material = blockMaterial;

		// Delete all uncombined children
		foreach (Transform quad in chunk.transform)
		{
			GameObject.Destroy(quad.gameObject);
		}

	}
}
