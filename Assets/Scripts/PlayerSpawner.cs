using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;
using Tiled2Unity;

public class PlayerSpawner : MonoBehaviour {

    public Object playerAsset;
    public ProCamera2D mainCamera;

    void Awake()
    {
        playerAsset = Resources.Load("KeenPlayer");

        var world = GameObject.Find("map");
        var tiledMap = world.GetComponent<TiledMap>();

        var boundariesCam = mainCamera.GetComponent<ProCamera2DNumericBoundaries>();
        boundariesCam.BottomBoundary = -tiledMap.MapHeightInPixels;
        boundariesCam.RightBoundary = tiledMap.MapWidthInPixels;

        GameObject spawnInstance = (GameObject) Instantiate(playerAsset);
        
        var spawnList = GameObject.FindGameObjectsWithTag("spawn");
        if (spawnList.Length == 0)
        {
            // bad!
        }

        var pos = spawnList[0].transform.localPosition;
        spawnInstance.transform.localPosition = new Vector2(pos.x, pos.y);

        var proCam = mainCamera.GetComponent<ProCamera2D>();
        proCam.AddCameraTarget(spawnInstance.transform);
        proCam.MoveCameraInstantlyToPosition(pos);
    }
	
	// Update is called once per frame
	void Update() {
	
	}
}
