using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Plato.DoublePrecision;
using Plato.Geometry.IO;
using Plato.Geometry.Scenes;
using UnityEngine;
 
[ExecuteAlways]
public class TopoHackScript : ProceduralMeshComponent
{
    public int NumRoomsPerFloor = 4;
    public int NumFloors = 3;
    public double FloorHeight = 2;
    public double SiteWidth = 5;
    public double SiteDepth = 5;
    public double CoreWidth = 1;
    public double FloorThickness = 0.2;
    public double ColumnWidth = 0.5;
    public string Prompt;
    public bool GetResponse;
    public bool ShouldLoadJson;
    public bool WriteObj;

    [System.Serializable]
    public class Root
    {
        public Building building;
    }
    [System.Serializable]
    public class Building
    {
        public string name;
        public string address;
        public List<Floor> floors = new List<Floor>();
        public Site site;
    }
    [System.Serializable]
    public class Floor
    {
        public int floorNumber;
        public double height;
        public List<Room> rooms = new List<Room>();
    }
    [System.Serializable]
    public class Room
    {
        public string roomID;
        public string name;
        public string area;
    }
    [System.Serializable]
    public class Site
    {
        public double width;
        public double length;
    }

    public void LoadJson()
    {
        ShouldLoadJson = false;

        // Replace with the path to your JSON file
        string filePath = Path.Combine(Application.dataPath, "response.json");
        var jsonFilePath = filePath;

        // Read the JSON file content
        var jsonContent = File.ReadAllText(jsonFilePath);
        
        // Deserialize the JSON into the Building object using Newtonsoft.Json
        var root = JsonUtility.FromJson<Root>(jsonContent);
        var building = root.building;
        
        // Output some data to verify
        Debug.Log($"Building Name: {building.name}");
        Debug.Log($"Address: {building.address}");
        Debug.Log($"Number of Floors: {building.floors.Count}");
        Debug.Log($"Site Width: {building.site.width}, Length: {building.site.length}");

        NumFloors = building.floors.Count;
        SiteWidth = building.site.width;
        SiteDepth = building.site.length;
    }

    public override TriangleMesh GetTriangleMesh()
    {
        var scene = new Scene();
        var root = scene.Root;
        var floorSize = new Vector3D(SiteWidth, SiteDepth, FloorHeight);
        var mesh = PlatonicSolids.Cube.TriangleMesh;
        for (var i = 0; i < NumFloors; i++)
        {
            var elevation = i * FloorHeight;
            var min = new Vector3D(-SiteWidth / 2, -SiteDepth / 2, elevation);
            var max = min + (SiteWidth, SiteDepth, FloorThickness);
            
            var floorBounds = new Bounds3D(min, max);
            var gridSize = (int)Math.Ceiling(Math.Sqrt(NumRoomsPerFloor));

            //var tform = new TRSTransform(new Transform3D((0, 0, elevation), Rotation3D.Default, floorSize * 0.9));
            
            foreach (var b in floorBounds.Subdivide(gridSize, gridSize, 1))
            {
                var room = root.AddMesh(mesh, b.ToTransform3D(), null, "Room");
            }

            //var floor = root.AddMesh(floorMesh, tform, null, $"Floor {i}");
        }

        // Draw core 
        {
            var coreBox = new Bounds3D(
                (-CoreWidth / 2, -CoreWidth / 2, 0), 
                (CoreWidth / 2, CoreWidth / 2, NumFloors * FloorHeight));

            root.AddMesh(mesh, coreBox.ToTransform3D(), null, "Core");
        }

        // Draw columns 
        for (var i=0; i < 3; i++)
        for (var j = 0; j < 3; j++)
        {
            var elevation = i * FloorHeight;
            var coreBox = new Bounds3D(
                (-CoreWidth / 2, -CoreWidth / 2, 0),
                (CoreWidth / 2, CoreWidth / 2, NumFloors * FloorHeight));

            root.AddMesh(mesh, coreBox.ToTransform3D(), null, "Core");

        }

        if (GetResponse)
        {
            GetResponse = false;
            GetBackendResponse getReponse = new GetBackendResponse();
            getReponse.StartNow(Prompt);
        }

        var triMesh = scene.ToTriangleMesh();
        if (WriteObj)
        {
            WriteObj = false;
            string filePath = Path.Combine(Application.dataPath, "output.obj");
            triMesh.WriteObj(filePath);
        }

        if (ShouldLoadJson)
        {
            LoadJson();
        }


        return triMesh;
    }
}

public static class Extensions
{
    public static IEnumerable<Bounds3D> Subdivide(this Bounds3D bounds, int columns, int rows, int layers)
    {
        var size = bounds.Size / (columns, rows, layers);
        for (var i = 0; i < columns; i++)
        {
            for (var j = 0; j < rows; j++)
            {
                for (var k = 0; k < layers; k++)
                {
                    var min = bounds.Min + size * (i, j, k);
                    var max = min + size;
                    yield return new Bounds3D(min, max);
                }
            }
        }
    }
    
    public static ITransform3D ToTransform3D(this Bounds3D bounds)
        => new TRSTransform((bounds.Center, Rotation3D.Default, bounds.Size));

    public static TriangleMesh ToMesh(this Bounds3D bounds)
        => PlatonicSolids.Cube.ToTriangleMesh().Scale(bounds.Size);

    public static TriangleMesh ToBoxMesh(this Vector3D v)
        => PlatonicSolids.Cube.ToTriangleMesh().Scale(v);

    public static T MoveUp<T>(this T self, Number value) where T : ITransformable3D<T>
        => self.Translate((0, 0, value));

    public static Bounds3D ToBounds(this Vector3D v)
        => (-v.Half, v.Half);
}