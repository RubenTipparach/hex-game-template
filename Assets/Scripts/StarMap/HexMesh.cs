using UnityEngine;
using System.Collections.Generic;

// Cat like coding has great tutorials on this!
//https://catlikecoding.com/unity/tutorials/hex-map/
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {

	Mesh hexMesh;
	List<Vector3> vertices;
	List<Color> colors;
	List<int> triangles;
    List<Vector2> uvs;
	MeshCollider meshCollider;

	void Awake () {
		GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
		meshCollider = gameObject.AddComponent<MeshCollider>();
		hexMesh.name = "Hex Mesh";
		vertices = new List<Vector3>();
		colors = new List<Color>();
		triangles = new List<int>();
        uvs = new List<Vector2>();

    }

    public void Triangulate(HexCell[] cells)
    {
        hexMesh.Clear();
        vertices.Clear();
        colors.Clear();
        triangles.Clear();
        uvs.Clear();

        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        hexMesh.vertices = vertices.ToArray();
        hexMesh.colors = colors.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();
        //hexMesh.uv = new Vector2[]{
        //new Vector2(0, HexMetrics.outerRadius),
        //new Vector2(0,HexMetrics.outerRadius) };
        meshCollider.sharedMesh = hexMesh;
        hexMesh.uv = uvs.ToArray();
    }

	void Triangulate (HexCell cell) {
		Vector3 center = cell.transform.localPosition;
		for (int i = 0; i < 6; i++) {
			AddTriangle(
				center,
				center + HexMetrics.corners[i],
				center + HexMetrics.corners[i + 1]
			);
            var raceSelection = cell.raceSelectionData;
            
            if (raceSelection == null)
            {
                cell.raceType = RaceType.Neutral;
                raceSelection = cell.raceSelectionData;
            }

			AddTriangleColor(raceSelection.PrimaryColor);

            uvs.AddRange(new Vector2[] { 
                new Vector2(0.5f, 0.5f), 
                new Vector2(1f, .5f), 
                new Vector2(1f, .5f) });

        }
    }

	void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}

	void AddTriangleColor (Color color) {
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

    public void AddTriangleUV(Vector2 uv1, Vector2 uv2, Vector3 uv3)
    {
        //uvs.Add(uv1);
        //uvs.Add(uv2);
        //uvs.Add(uv3);
    }
}