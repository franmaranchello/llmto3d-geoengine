using Plato.DoublePrecision;
using Plato.Geometry.Unity;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public abstract class ProceduralMeshComponent : MonoBehaviour
{
   
    public abstract TriangleMesh GetTriangleMesh();

    public bool FlipWindingOrder;
    public bool DoubleSided;
    public bool ZUp = true;

    // TODO: stretch ...
    // TODO: show balls at points. 

    /*
    public bool DrawPoints;
    public double PointSphereRadius = 0.2;
    public Color PointColor = Color.blue;
    public bool DrawEdges;
    public double EdgeCylinderRadius = 0.1;
    public Color EdgeColor = Color.green;
    public bool DrawFaceNormals;
    */
    public string Stats;

    private Mesh UnityMesh;
    
    public void Update()
    {
        var mesh = GetTriangleMesh();

        if (FlipWindingOrder)
            mesh = mesh.FlipWindingOrder();

        if (DoubleSided)
            mesh = mesh.DoubleSided();

        if (ZUp)
            mesh = mesh.Rotate(AxisAngle.New(Vector3D.UnitX, 90.Degrees()));

        UnityMesh = mesh.ToUnity();

        Stats = $"# vertices {UnityMesh.vertexCount}, # triangles = {UnityMesh.triangles.Length / 3}";

        GetComponent<MeshFilter>().sharedMesh = UnityMesh;

    }
}