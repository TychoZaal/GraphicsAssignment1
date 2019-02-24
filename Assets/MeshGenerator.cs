﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    public int speed = 5;

    public int xSize = 20;
    public int zSize = 20;

    public float perlinNoiseModifier = 2;
    public float waveModifier = .3f;

    // Start is called before the first frame update
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
    }

    private void Update()
    {
        UpdateMesh();
        MoveSea();
    }

    private void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float newY = GetNewHeightValue(x, z);
                newY += Mathf.Sin(z + Time.deltaTime);
                vertices[i] = new Vector3(x, newY, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        uvs = new Vector2[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
                i++;
            }
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
    }

    private float GetNewHeightValue(int x, int z)
    {
        float yPerlin = (float)Mathf.PerlinNoise(x * .3f, z * .3f) * perlinNoiseModifier;
        return yPerlin;
    }

    private void MoveSea()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            float newY = GetNewHeightValue((int)vertices[i].x, (int)vertices[i].z);
            newY += Mathf.Sin(vertices[i].z + Time.deltaTime);
        }
    }
}
