# Unity Building Generator with Plato Libraries

This Unity project integrates **Plato Geometry Libraries** to automate the **generation, modification, and export of 3D building models**. The project communicates with a backend to receive building specifications via a prompt, updates the geometry dynamically, and exports **OBJ meshes** of the building for external use.

---

## **Features**

1. **Send a Prompt to Backend**  
   - User-defined prompt is sent to the backend to request building specifications.  
   - Example: `"Create a building with 4 floors, each with 2 offices."`

2. **Receive JSON Response Describing a Building**  
   - The backend responds with structured JSON data containing the building's layout, including:  
     - Number of floors  
     - Floor height  
     - Rooms with dimensions and names

3. **Update Building Geometry**  
   - Geometry is generated based on the backend's JSON response using **Plato Geometry** libraries.  
   - The building mesh is updated dynamically in the Unity scene.

4. **Export OBJ Meshes**  
   - Updated building geometry can be exported as **OBJ files** for external use, including architectural visualization and game assets.

---

## **Project Setup**

### **1. Prerequisites**

- **Unity**: Version 2021.3 or higher.  
- **Plato Geometry Libraries**: Ensure the Plato.Geometry NuGet package is available.  
- **Backend Server**: A backend capable of receiving prompts and returning JSON responses.  
- **.NET Core**: Required for Plato libraries.

### **2. Installation**

1. Clone this repository:
   ```bash
   git clone https://github.com/franmaranchello/llmto3d-geoengine.git
   cd llmto3d-geoengine
   ```

2. Open the project in Unity:
   - Use **Unity Hub** or **File > Open Project** from the Unity Editor.

3. Install Dependencies:
   - Make sure the **Plato.Geometry** package is installed. If needed, install it via the Unity Package Manager.

---

## **Usage**

### **1. Sending a Prompt to the Backend**

In your Unity C# script:

```csharp
string prompt = "Create a building with 4 floors, each with 2 offices.";
```

The **CallBackendAndUpdateGeometry()** function sends the prompt to the backend and waits for the response.

### **2. Receiving JSON and Updating Geometry**

The backend responds with a **JSON object** describing the building layout. Here’s a sample response:

```json
{
  "building": {
    "floors": [
      {
        "floorNumber": 1,
        "height": 12.0,
        "rooms": [
          { "name": "Office", "area": 30.0 },
          { "name": "Lobby", "area": 50.0 }
        ]
      }
    ]
  }
}
```

This data is used to **generate geometry** dynamically in the Unity scene using Plato libraries.

### **3. Exporting OBJ Meshes**

The generated building mesh can be **exported as an OBJ file** for use in other software. Use the following method:

```csharp
Plato.Geometry.IO.ObjExporter.WriteObj(mesh, "Assets/Exports/building.obj");
```

This exports the current building geometry to the specified path.

---

## **Architecture Overview**

1. **Backend Communication**:  
   - Uses **`HttpClient`** to send and receive data from the backend.

2. **Plato Geometry for Mesh Generation**:  
   - Converts JSON data into **3D meshes** inside Unity.

3. **OBJ Export**:  
   - Meshes are saved as **OBJ** files using Plato's `ObjExporter`.

---

## **Sample Workflow**

1. **Run the Backend Server**:
   - Ensure the backend service is running locally or on a remote server.

2. **Start Unity and Enter Play Mode**:
   - Enter play mode to send a prompt to the backend.

3. **View the Generated Building**:
   - The building will be generated dynamically based on the response.

4. **Export the Mesh**:
   - Export the mesh as an OBJ file using the `ObjExporter`.

---

## **Troubleshooting**

- **Backend Connection Error**:  
  Ensure the backend is running and the endpoint is reachable.

- **Plato Library Not Found**:  
  Verify that the Plato library is installed correctly in Unity’s Package Manager.

- **OBJ Export Issues**:  
  Make sure the export path is valid and the mesh is generated correctly before exporting.

---

## **Contributing**

Feel free to open issues or submit pull requests. Contributions are welcome!

---

## **License**

This project is licensed under the MIT License. See the `LICENSE` file for more details.

---

## **Contact**

For questions or feedback, contact us!.