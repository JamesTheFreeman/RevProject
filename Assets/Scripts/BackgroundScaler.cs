using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    public GameObject background;
    public GameObject cam;
    private float x = 20 / 5;
    private float y = 12 / 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float camSize = cam.GetComponent<Camera>().orthographicSize;
        background.transform.localScale = new Vector3(x * camSize, y * camSize, 1);
    }
}
