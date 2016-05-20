using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;
using Tiled2Unity;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour {

    public Object playerAsset;
    public ProCamera2D mainCamera;
    public bool spawnerUsesSavedPosition;

    private GameObject spawnInstance;
	private TiledMap tiledMap;
    private GameSession session;

    void Awake()
    {
        tiledMap = FindObjectOfType<TiledMap>();
        session = FindObjectOfType<GameSession>();

        var boundariesCam = mainCamera.GetComponent<ProCamera2DNumericBoundaries>();
        boundariesCam.BottomBoundary = -tiledMap.MapHeightInPixels;
        boundariesCam.RightBoundary = tiledMap.MapWidthInPixels;

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        spawnInstance = (GameObject)Instantiate(playerAsset);

        Vector3 pos;

        if (spawnerUsesSavedPosition && session != null && session.useSessionPosition)
        {
            pos = session.overworldPosition;
        } else
        {
            var spawnList = GameObject.FindGameObjectsWithTag("spawn");
            if (spawnList.Length == 0)
            {
                // FIXME: bad!
            }

            pos = spawnList[0].transform.position;
            // FIXME: hardcoded numbers
            pos.x += 8;
            pos.y += 11;
        }

        spawnInstance.transform.localPosition = pos;

        if (session != null)
        {
            var inv = spawnInstance.GetComponent<Inventory>();
            inv.items = session.inventoryItems;
        }


        mainCamera.AddCameraTarget(spawnInstance.transform);
        mainCamera.MoveCameraInstantlyToPosition(pos);
    }
	
	// Update is called once per frame
	void Update() {
		if (spawnInstance) {
			// fell out of the map
			if (spawnInstance.gameObject.transform.position.y + 100 < -tiledMap.MapHeightInPixels) {
				spawnInstance.gameObject.GetComponent<Hurtable>().InstaGib();
			}
		}
	    if (!spawnInstance && Input.GetKey(KeyCode.Z))
        {
            //mainCamera.RemoveAllCameraTargets();
            //SpawnPlayer();
            var currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }
	}
}
