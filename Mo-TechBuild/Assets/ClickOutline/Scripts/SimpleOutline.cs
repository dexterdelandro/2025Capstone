using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[DisallowMultipleComponent]
public class SimpleOutline : MonoBehaviour
{
    private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

    public Color OutlineColor
    {
        get { return outlineColor; }
        set
        {
            outlineColor = value;
            updateOnce = true;
        }
    }

    public float OutlineWidth
    {
        get { return outlineWidth; }
        set
        {
            outlineWidth = value;
            updateOnce = true;
        }
    }


    [SerializeField] private Color outlineColor = Color.black;
    [SerializeField, Range( 0f, 20f )] private float outlineWidth = 10f;

    private Renderer[] renderers;
    private Material outlineMaskMaterial;
    private Material outlineFillMaterial;
    private bool updateOnce;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        outlineMaskMaterial = Instantiate( Resources.Load<Material>( @"Materials/OutlineMask" ) );
        outlineFillMaterial = Instantiate( Resources.Load<Material>( @"Materials/OutlineFill" ) );
        LoadSmoothNormals();
        updateOnce = true;
    }

    private void OnEnable()
    {
        foreach ( var renderer in renderers )
        {
            var materials = renderer.sharedMaterials.ToList();
            materials.Add( outlineMaskMaterial );
            materials.Add( outlineFillMaterial );
            renderer.materials = materials.ToArray();
        }
    }

    private void OnValidate()
    {
        updateOnce = true;
    }

    private void Update()
    {
        if ( updateOnce )
        {
            updateOnce = false;
            UpdateMaterialProperties();
        }
    }

    private void OnDisable()
    {
        foreach ( var renderer in renderers )
        {
            var materials = renderer.sharedMaterials.ToList();
            materials.Remove( outlineMaskMaterial );
            materials.Remove( outlineFillMaterial );
            renderer.materials = materials.ToArray();
        }
    }

    private void OnDestroy()
    {
        Destroy( outlineMaskMaterial );
        Destroy( outlineFillMaterial );
    }

    private void LoadSmoothNormals()
    {
        foreach ( var meshFilter in GetComponentsInChildren<MeshFilter>() )
        {
            if ( !registeredMeshes.Add( meshFilter.sharedMesh ) )
            {
                continue;
            }

            var smoothNormals = GetSmoothNormals( meshFilter.sharedMesh );
            meshFilter.sharedMesh.SetUVs( 3, smoothNormals );
            var renderer = meshFilter.GetComponent<Renderer>();

            if ( renderer != null )
            {
                CombineSubmeshes( meshFilter.sharedMesh, renderer.sharedMaterials );
            }
        }

        foreach ( var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>() )
        {
            if ( !registeredMeshes.Add( skinnedMeshRenderer.sharedMesh ) )
            {
                continue;
            }
            skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[ skinnedMeshRenderer.sharedMesh.vertexCount ];
            CombineSubmeshes( skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials );
        }
    }

    private List<Vector3> GetSmoothNormals( Mesh mesh )
    {
        var groups = mesh.vertices.Select( ( vertex, index ) => new KeyValuePair<Vector3, int>( vertex, index ) ).GroupBy( pair => pair.Key );
        var smoothNormals = new List<Vector3>( mesh.normals );

        foreach ( var group in groups )
        {
            if ( group.Count() == 1 )
            {
                continue;
            }
            var smoothNormal = Vector3.zero;

            foreach ( var pair in group )
            {
                smoothNormal += smoothNormals[ pair.Value ];
            }
            smoothNormal.Normalize();

            foreach ( var pair in group )
            {
                smoothNormals[ pair.Value ] = smoothNormal;
            }
        }

        return smoothNormals;
    }

    private void CombineSubmeshes( Mesh mesh, Material[] materials )
    {
        if ( mesh.subMeshCount == 1 )
        {
            return;
        }

        if ( mesh.subMeshCount > materials.Length )
        {
            return;
        }

        mesh.subMeshCount++;
        mesh.SetTriangles( mesh.triangles, mesh.subMeshCount - 1 );
    }

    private void UpdateMaterialProperties()
    {
        outlineFillMaterial.SetColor( "_OutlineColor", outlineColor );
        outlineFillMaterial.SetFloat( "_OutlineWidth", outlineWidth );
    }
}
