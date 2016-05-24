using UnityEngine;
using System.Collections;

public class DamageNumber : MonoBehaviour {

    public float lifetime;
    public string value;

    private TextMesh textMesh;

	void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        textMesh.color = new Color(0, 0, 0, 0);
    }

    public void Trigger()
    {
        textMesh.color = Color.white;
        textMesh.text = value;
        iTween.MoveBy(gameObject, new Vector3(0, 8, 0), lifetime);
        iTween.FadeTo(gameObject, iTween.Hash("alpha", 0, "time", lifetime));
        Destroy(gameObject, lifetime);
    }
}
