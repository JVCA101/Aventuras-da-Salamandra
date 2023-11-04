using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorceguinhoController : MonoBehaviour
{
    [SerializeField] private float vSpeed = 2f;
    [SerializeField] private float hSpeed = 3f;
    [SerializeField] private float oscilationSpeed = 2.0f;
    [SerializeField] private bool isFacingLeft = true;
    [SerializeField] private moveSet movement = moveSet.still;
    [SerializeField] private float halfHDistance = 3.7f;
    private float initialY;
    private float initialX;
    private float leftXLimit;
    private float rightXLimit;
    private SpriteRenderer spriteRenderer;
    private bool isOffCam = false;

    public enum moveSet{
        vOscilation, hOscilation, circular, still,
    };
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = isFacingLeft;
        initialY = transform.position.y;
        initialX = transform.position.x;
        leftXLimit = initialX - halfHDistance;
        rightXLimit = initialX + halfHDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if(isOffCam)
            return;
        switch(movement){
            case moveSet.vOscilation:
                vOscilationMovement();
                break;
            case moveSet.hOscilation:
                hOscilationMovement();
                break;
            case moveSet.circular:
                CircularMovement();
                break;
            default:
                break;
        }
    }

    void LateUpdate()
    {
        isOffCam = Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > 15f;
    }

    private void vOscilationMovement(){
        Vector2 newPos = transform.position;
        float y = Mathf.Sin(Time.time*oscilationSpeed)*vSpeed;
        newPos.y = initialY + y;
        transform.position = newPos;
    }

    private void hOscilationMovement(){
        Vector2 newPos = transform.position;
        float dx = Time.deltaTime * hSpeed * (isFacingLeft ? -1 : 1);
        newPos.x = Mathf.Clamp(newPos.x + dx, leftXLimit, rightXLimit);
        if(newPos.x == leftXLimit){
            isFacingLeft = false;
            spriteRenderer.flipX = false;
        }
        else if(newPos.x == rightXLimit){
            isFacingLeft = true;
            spriteRenderer.flipX = true;
        }
        transform.position = newPos;
    }

    private void CircularMovement(){
        float x = Mathf.Cos(Time.time*oscilationSpeed)*hSpeed;
        float y = Mathf.Sin(Time.time*oscilationSpeed)*vSpeed;
        Vector2 newPos = transform.position;
        newPos.x = initialX + x;
        newPos.y = initialY + y;
        transform.position = newPos;
    }
}
