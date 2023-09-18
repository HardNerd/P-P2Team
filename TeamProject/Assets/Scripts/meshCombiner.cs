using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class meshCombiner : MonoBehaviour
{
    [ContextMenu("Combine Meshes")]
    private void CombineMeshes()
    {
        SkinnedMeshRenderer[] meshFilters = GetComponentsInChildren<SkinnedMeshRenderer>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        transform.GetComponent<SkinnedMeshRenderer>().sharedMesh = mesh;
        transform.gameObject.SetActive(true);
    }
}
