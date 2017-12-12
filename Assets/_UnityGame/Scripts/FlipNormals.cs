using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class FlipNormals : MonoBehaviour
{

    public bool ShouldFlip;
    private bool isFlipped;
    private MeshFilter meshFilter;

    public FlipNormals()
    {
        ShouldFlip = true;
        isFlipped = false;
    }

    private void Start()
    {
        meshFilter = transform.GetComponent<MeshFilter>();

        if(meshFilter.sharedMesh == null)
            throw new Exception("Cannot add FlipNormals on a non mesh");
    }

    void OnRenderObject()
    {
        if (ShouldFlip == isFlipped)
            return;

        flipNormals(meshFilter.sharedMesh);
        isFlipped = ShouldFlip;
    }

    private void flipNormals(Mesh mesh)
    {
        mesh.uv = mesh.uv.Select(o => new Vector2(1 - o.x, o.y)).ToArray();
        mesh.triangles = mesh.triangles.Reverse().ToArray();
        mesh.normals = mesh.normals.Select(o => -o).ToArray();
    }
}
