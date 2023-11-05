using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    private float speedX;
    private float speedY;
    private GameObject[] players;
    private Transform player;
    private float halfWidth;
    private float halfHeight;

    // Start is called before the first frame update
    void Start()
    {
        speedX = 1.8f;
        speedY = 4f;

        players = GameObject.FindGameObjectsWithTag("Player");
        player = players[0].GetComponent<Transform>();

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        float x;
        if(players.Length == 1)
            x = player.position.x+halfWidth/2f;
        else if((players[0].transform.position.x+players[1].transform.position.x)/2f > leftEdge.position.x + halfWidth)
            x = (players[0].transform.position.x+players[1].transform.position.x)/2f;
        else
            x = leftEdge.position.x + halfWidth;
        
        transform.position = new Vector3(x, leftEdge.position.y+halfHeight, transform.position.z);
    }


    // Single Player

    void FollowPlayerCenteredInOneSideX()
    {
        float cameraLeft = transform.position.x - halfWidth;
        float posXMin = leftEdge.position.x + halfWidth;
        float posXMax = rightEdge.position.x - halfWidth;
        
        
        float posX;
        if(players[0].GetComponent<SpriteRenderer>().flipX == false)
            posX = player.position.x + halfWidth/2f;
        else
            posX = player.position.x - halfWidth/2f;

        posX = Mathf.Clamp(posX, posXMin, posXMax);

        float x = Mathf.Lerp(transform.position.x, posX, speedX * Time.deltaTime);
        float y = transform.position.y;
        float z = transform.position.z;
        transform.position = new Vector3(x, y, z);
    }

    void FollowPlayerY()
    {
        float x = transform.position.x;
        float yMin = leftEdge.position.y;
        float y;
        if(transform.position.y <= yMin + halfHeight && player.position.y <= yMin + halfHeight/2f)
            y = yMin + halfHeight;
        else
            y = Mathf.Lerp(transform.position.y, player.position.y + halfHeight/2f, speedY * Time.deltaTime);
        float z = transform.position.z;
        transform.position = new Vector3(x, y, z);
    }

    void Camera1()
    {
        FollowPlayerCenteredInOneSideX();
        FollowPlayerY();
    }


    // Multiplayer

    void Camera2()
    {
        float x = 0;
        float y = 0;

        for(int i = 0; i < players.Length; i++)
        {
            x += players[i].transform.position.x;
            y += players[i].transform.position.y;
        }

        x /= players.Length;
        y /= players.Length;

        x = Mathf.Clamp(x, leftEdge.position.x + halfWidth, rightEdge.position.x - halfWidth);
        y = Mathf.Clamp(y, leftEdge.position.y + halfHeight, rightEdge.position.y - halfHeight);

        x = Mathf.Lerp(transform.position.x, x, speedX * Time.deltaTime);
        y = Mathf.Lerp(transform.position.y, y, speedY * Time.deltaTime);

        float z = transform.position.z;
        transform.position = new Vector3(x, y, z);
    }


    // Late Update is called once per frame after Update
    void LateUpdate()
    {
        if(players.Length == 1)
            Camera1();
        else if(players.Length == 2)
            Camera2();
        else
            Application.Quit();
    }

}
