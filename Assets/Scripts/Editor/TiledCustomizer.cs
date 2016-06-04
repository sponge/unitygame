using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Example custom importer:
[Tiled2Unity.CustomTiledImporter]
internal class MyCustomImporter : Tiled2Unity.ICustomTiledImporter {

    public void HandleCustomProperties(GameObject gameObject,
        IDictionary<string, string> keyValuePairs) {
        if (keyValuePairs.ContainsKey("trigger")) {
            var collider = gameObject.GetComponent<Collider2D>();
            collider.isTrigger = true;

            switch (keyValuePairs["trigger"]) {
                case "LevelEntrance":
                    var levelEntrance = gameObject.AddComponent<LevelEntrance>();
                    levelEntrance.destination = keyValuePairs["destination"];
                    levelEntrance.levelBit = (int)Mathf.Pow(2, int.Parse(keyValuePairs["id"]));
                    levelEntrance.image = AssetDatabase.LoadAssetAtPath<Sprite>(keyValuePairs["image"]);
                    levelEntrance.completedImage = AssetDatabase.LoadAssetAtPath<Sprite>(keyValuePairs["completedImage"]);
                    break;
            }
        }

        if (keyValuePairs.ContainsKey("hideIfCompleted")) {
            var barrier = gameObject.AddComponent<BarrierVisibility>();
            barrier.levelCompletedCondition = (int)Mathf.Pow(2, int.Parse(keyValuePairs["hideIfCompleted"]));
        }

        if (keyValuePairs.ContainsKey("prefab")) {
            Transform oldTileObject = gameObject.transform.Find("TileObject");
            if (oldTileObject != null) {
                Object.DestroyImmediate(oldTileObject.gameObject);
            }

            Object spawn = AssetDatabase.LoadAssetAtPath(keyValuePairs["prefab"], typeof(GameObject));
            if (spawn != null) {
                // Replace with new spawn object
                GameObject spawnInstance = (GameObject)Object.Instantiate(spawn);
                spawnInstance.name = spawn.name;

                // Use the position of the game object we're attached to
                spawnInstance.transform.parent = gameObject.transform;
                spawnInstance.transform.localPosition = Vector3.zero;

                var spr = spawnInstance.GetComponent<SpriteRenderer>();
                if (spr) {
                    // FIXME: this is still wrong because of differences between position anchor.
                    // i think i need height / 16 rounded up (eg 8,16 for a door, 8,8 for a coin)
                    // might need to be per-prefab (something thats not 2 tiles tall but should be placed on the bottom)
                    spawnInstance.transform.localPosition = new Vector2(spr.sprite.rect.width / 2 + (16 - spr.sprite.rect.width) / 2, spr.sprite.rect.height / 2);
                }
            }
        }
    }

    public void CustomizePrefab(GameObject prefab) {
        var colliders = prefab.GetComponentsInChildren<BoxCollider2D>();
        foreach (var col in colliders) {
            col.transform.localPosition = new Vector2(col.transform.localPosition.x + col.offset.x, col.transform.localPosition.y + col.offset.y);
            col.offset = Vector2.zero;
            Debug.Log(col.offset + "." + col.size);
        }
        Debug.Log("CustomizePrefab");
    }
}