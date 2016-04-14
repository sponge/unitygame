using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

// Example custom importer:
[Tiled2Unity.CustomTiledImporter]
class MyCustomImporter : Tiled2Unity.ICustomTiledImporter
{
    public void HandleCustomProperties(GameObject gameObject,
        IDictionary<string, string> keyValuePairs)
    {
        if (!keyValuePairs.ContainsKey("prefab"))
        {
            Debug.Log("No prefab key found");
            return;
        }

        // Remove old tile object
        Transform oldTileObject = gameObject.transform.Find("TileObject");

        if (oldTileObject != null)
        {
            Object.DestroyImmediate(oldTileObject.gameObject);
        }

        Object spawn = AssetDatabase.LoadAssetAtPath(keyValuePairs["prefab"], typeof(GameObject));
        if (spawn != null)
        {
            // Replace with new spawn object
            GameObject spawnInstance = (GameObject) Object.Instantiate(spawn);
            spawnInstance.name = spawn.name;

            // Use the position of the game object we're attached to
            spawnInstance.transform.parent = gameObject.transform;
            spawnInstance.transform.localPosition = Vector3.zero;
        }
    }

    public void CustomizePrefab(GameObject prefab)
    {
        Debug.Log("CustomizePrefab");
    }
}