using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraTypeX
{
    FollowPlayerCenteredInOneSide,
    SmoothTransition,
    BoxCentered
}

public enum CameraTypeY
{
    FollowPlayer,
    BoxCentered
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraTypeX cameraTypeX;
    [SerializeField] private CameraTypeY cameraTypeY;
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    private float speedX;
    private float speedY;
    private GameObject[] players;
    private Transform player;
    private float halfWidth;
    private float halfHeight;
    private float t = 0;
    private int cameraFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        speedX = 1.8f;
        speedY = 4f;

        players = GameObject.FindGameObjectsWithTag("Player");
        player = players[0].GetComponent<Transform>();

        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    // Single Player
    void SmoothTransition()
    {
        int frame = Mathf.FloorToInt((player.position.x - leftEdge.position.x) / (2*halfWidth));

        float smoothX = transform.position.x;
        if(frame != cameraFrame)
        {
            float initialPos = leftEdge.position.x + halfWidth + 2*cameraFrame*halfWidth;
            float finalPos = leftEdge.position.x + halfWidth + 2*frame*halfWidth;
            smoothX = Mathf.Lerp(initialPos, finalPos, t);
            t += 2 * Time.deltaTime;
            if(t >= 1)
            {
                cameraFrame += frame-cameraFrame;
                t = 0;
            }
        }

        float x = Mathf.Clamp(smoothX, leftEdge.position.x + halfWidth, rightEdge.position.x - halfWidth);
        float y = transform.position.y;
        float z = transform.position.z;
        transform.position = new Vector3(x, y, z);
    }

    void BoxCenteredX()
    {
        float cameraLeft = transform.position.x - halfWidth;
        float regionLeft = cameraLeft + halfWidth * 0.5f;
        float regionRight = cameraLeft + halfWidth * 1.5f;
        float dx = 0;
        if(player.transform.position.x < regionLeft)
        {
            dx = player.transform.position.x - regionLeft;
        }
        else if(player.transform.position.x > regionRight)
        {
            dx = player.transform.position.x - regionRight;
        }
        float x = Mathf.Clamp(transform.position.x + dx, leftEdge.position.x + halfWidth, rightEdge.position.x - halfWidth);
        float y = transform.position.y;
        float z = transform.position.z;
        transform.position = new Vector3(x, y, z);
    }

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

    void BoxCenteredY()
    {
        float cameraDown = transform.position.y - halfHeight;
        float regionDown = cameraDown + halfHeight * 0.5f;
        float regionUp = cameraDown + halfHeight * 1.5f;
        float dy = 0;
        if(player.transform.position.y < regionDown)
            dy = player.transform.position.y - regionDown;
        else if(player.transform.position.y > regionUp)
            dy = player.transform.position.y - regionUp;
        float x = transform.position.x;
        float y = transform.position.y + dy;
        float z = transform.position.z;
        transform.position = new Vector3(x, y, z);
    }

    void Camera1()
    {
        Debug.Log("1 player");
        switch(cameraTypeX)
        {
            case CameraTypeX.SmoothTransition: SmoothTransition(); break;
            case CameraTypeX.FollowPlayerCenteredInOneSide: FollowPlayerCenteredInOneSideX(); break;
            case CameraTypeX.BoxCentered: BoxCenteredX(); break;
        }

        switch(cameraTypeY)
        {
            case CameraTypeY.FollowPlayer: FollowPlayerY(); break;
            case CameraTypeY.BoxCentered: BoxCenteredY(); break;
        }
    }


    // 2 Players
    void Players2()
    {
        // X
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

    void Camera2()
    {
        Debug.Log("2 players");
        Players2();
    }


    // Late Update is called once per frame after Update
    void LateUpdate()
    {
        if(players.Length == 1)
        {
            Camera1();
        }
        else if(players.Length == 2)
        {
            Camera2();
        }
        else
            Application.Quit();
    }

}
