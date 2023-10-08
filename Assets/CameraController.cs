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
    FollowPlayerCenteredInOneSide,
    BoxCentered
}

public class CameraController : MonoBehaviour
{
    // [SerializeField] private float speed;
    [SerializeField] private CameraTypeX cameraTypeX;
    [SerializeField] private CameraTypeY cameraTypeY;
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    private Transform player;
    private float halfWidth;
    private float halfHeight;

    private float t = 0;
    private int cameraFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

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

    void FollowPlayerY()
    {
        float x = transform.position.x;
        // float y = Mathf.Clamp(player.position.y, yMin.position.y + halfHeight, yMax.position.y - halfHeight);
        float y = player.position.y + halfHeight/2f;
        float z = transform.position.z;
        transform.position = new Vector3(x, y, z);
    }

    void FollowPlayerCenteredInOneSideX()
    {

    }

    void FollowPlayerCenteredInOneSideY()
    {

    }

    void BoxCenteredY()
    {
        float cameraLeft = transform.position.y - halfHeight;
        float regionLeft = cameraLeft + halfHeight * 0.5f;
        float regionRight = cameraLeft + halfHeight * 1.5f;
        float dy = 0;
        if(player.transform.position.y < regionLeft)
        {
            dy = player.transform.position.y - regionLeft;
        }
        else if(player.transform.position.y > regionRight)
        {
            dy = player.transform.position.y - regionRight;
        }
        float x = transform.position.x;
        float y = Mathf.Clamp(transform.position.y + dy, leftEdge.position.y + halfHeight, rightEdge.position.y - halfHeight);
        float z = transform.position.z;
        transform.position = new Vector3(x, y, z);
    }

    void LateUpdate()
    {
        switch(cameraTypeX)
        {
            case CameraTypeX.SmoothTransition: SmoothTransition(); break;
            case CameraTypeX.FollowPlayerCenteredInOneSide: FollowPlayerCenteredInOneSideX(); break;
            case CameraTypeX.BoxCentered: BoxCenteredX(); break;
        }

        switch(cameraTypeY)
        {
            case CameraTypeY.FollowPlayer: FollowPlayerY(); break;
            case CameraTypeY.FollowPlayerCenteredInOneSide: FollowPlayerCenteredInOneSideY(); break;
            case CameraTypeY.BoxCentered: BoxCenteredY(); break;
        }
    }
}
