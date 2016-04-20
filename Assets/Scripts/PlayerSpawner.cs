using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using Tiled2Unity;

public class PlayerSpawner : MonoBehaviour {

    public Object playerAsset;
    public ProCamera2D mainCamera;

    private GameObject spawnInstance;

    void Awake()
    {
        var tiledMap = GameObject.FindObjectOfType<TiledMap>();

        var boundariesCam = mainCamera.GetComponent<ProCamera2DNumericBoundaries>();
        boundariesCam.BottomBoundary = -tiledMap.MapHeightInPixels;
        boundariesCam.RightBoundary = tiledMap.MapWidthInPixels;

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        spawnInstance = (GameObject)Instantiate(playerAsset);

        var spawnList = GameObject.FindGameObjectsWithTag("spawn");
        if (spawnList.Length == 0)
        {
            // bad!
        }

        var pos = spawnList[0].transform.localPosition;
        spawnInstance.transform.localPosition = new Vector2(pos.x, pos.y);

        mainCamera.AddCameraTarget(spawnInstance.transform);
        mainCamera.MoveCameraInstantlyToPosition(pos);
    }
	
	// Update is called once per frame
	void Update() {
	    if (!spawnInstance && Input.GetKey(KeyCode.Z))
        {
            mainCamera.RemoveAllCameraTargets();
            SpawnPlayer();
        }
	}
}
