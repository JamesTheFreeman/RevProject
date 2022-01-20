using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private Transform _Player;

    private float smoothTime = 0.3f;
    private float yVelocity = 0.0f;
    private float xVelocity = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _Player = player.transform;
    }

    private void FixedUpdate()
    {
        float newPositionX = Mathf.SmoothDamp(transform.position.x, _Player.position.x, ref xVelocity, smoothTime);
        float newPositionY = Mathf.SmoothDamp(transform.position.y, _Player.position.y, ref yVelocity, smoothTime);
        transform.position = new Vector3(newPositionX, newPositionY, transform.position.z);
    }
}
