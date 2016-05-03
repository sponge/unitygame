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

        if (keyValuePairs.ContainsKey("trigger"))
        {
            var collider = gameObject.GetComponent<Collider2D>();
            collider.isTrigger = true;

			switch (keyValuePairs ["trigger"]) {
			case "LevelEntrance":
				var levelEntrance = gameObject.AddComponent<LevelEntrance> ();
				levelEntrance.destination = keyValuePairs ["destination"];
				break;
			}
        }

        if (keyValuePairs.ContainsKey("prefab"))
        {
            Transform oldTileObject = gameObject.transform.Find("TileObject");
            if (oldTileObject != null)
            {
                Object.DestroyImmediate(oldTileObject.gameObject);
            }

            Object spawn = AssetDatabase.LoadAssetAtPath(keyValuePairs["prefab"], typeof(GameObject));
            if (spawn != null)
            {
                // Replace with new spawn object
                GameObject spawnInstance = (GameObject)Object.Instantiate(spawn);
                spawnInstance.name = spawn.name;

                // Use the position of the game object we're attached to
                spawnInstance.transform.parent = gameObject.transform;
                spawnInstance.transform.localPosition = Vector3.zero;

                var spr = spawnInstance.GetComponent<SpriteRenderer>();
                if (spr)
                {
                    spawnInstance.transform.localPosition = new Vector2(spr.sprite.rect.width / 2, spr.sprite.rect.height / 2);
                }
            }
        }

    }

    public void CustomizePrefab(GameObject prefab)
    {
        Debug.Log("CustomizePrefab");
    }
}