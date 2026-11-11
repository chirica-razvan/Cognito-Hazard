using UnityEngine;

[ExecuteInEditMode]
public class InvertCubeNormals : MonoBehaviour
{
    [ContextMenu("Inverseaza Normale")]
    void Invert()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        if (filter == null || filter.sharedMesh == null) return;

        Mesh mesh = Instantiate(filter.sharedMesh);

        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
            normals[i] = -normals[i];
        mesh.normals = normals;

        for (int m = 0; m < mesh.subMeshCount; m++)
        {
            int[] triangles = mesh.GetTriangles(m);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int temp = triangles[i];
                triangles[i] = triangles[i + 1];
                triangles[i + 1] = temp;
            }
            mesh.SetTriangles(triangles, m);
        }

        filter.sharedMesh = mesh;
        Debug.Log("Normalele cubului au fost inversate cu succes spre interior!");
    }
}