using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalamandraController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float impulse;
    [SerializeField] private float gravity;

    private float ground;
    private bool jump;
    private float ySpeed;

    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        ground = transform.position.y;
        jump = false;
        ySpeed = 0;
        direction = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        if(jump)
        {
            Jump();
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
            ySpeed = impulse;
        }

        float dx = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        if(dx<0)
        {
            direction.x = -1;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(dx>0)
        {
            direction.x = 1;
            GetComponent<SpriteRenderer>().flipX = false;
        }

        float xMax = Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect;
        float xMin = xMax - 2*(xMax - Camera.main.transform.position.x);
        if(transform.position.x + dx <= xMin || transform.position.x + dx >= xMax)
            dx = 0;
        
        transform.Translate(dx, ySpeed * Time.deltaTime, 0);
    }

    void Jump()
    {
        if(transform.position.y <= ground)
        {
            transform.position.Set(transform.position.x, ground, 0);
            jump = false;
            ySpeed = 0;
        }
        else
            ySpeed -= gravity * Time.deltaTime;
    }
}
