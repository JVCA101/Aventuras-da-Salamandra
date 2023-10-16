using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalamandraController : MonoBehaviour
{
    public float speed;
    [SerializeField] private float impulse;
    [SerializeField] private float gravity;
    public bool player1;

    public float ground;
    private float playerGround;
    private bool jump;
    private float ySpeed;

    private Vector2 direction;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        ground = transform.position.y;
        playerGround = ground;
        jump = false;
        ySpeed = 0;

        direction = Vector2.right;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        float dx = 0;
        if(player1)
            dx = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        else if(Input.GetKey(KeyCode.J))
            dx = -speed * Time.deltaTime;
        else if(Input.GetKey(KeyCode.L))
            dx = speed * Time.deltaTime;
        
        if(dx<0)
        {
            direction.x = -1;
            spriteRenderer.flipX = true;
        }
        else if(dx>0)
        {
            direction.x = 1;
            spriteRenderer.flipX = false;
        }

        if(Mathf.Abs(dx) > 0)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);

        float xMax = Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect;
        float xMin = xMax - 2*(xMax - Camera.main.transform.position.x);
        if(transform.position.x + dx <= xMin || transform.position.x + dx >= xMax)
            dx = 0;
        
        transform.Translate(dx, ySpeed * Time.deltaTime, 0);
    }

    void Jump()
    {
        if(transform.position.y <= playerGround)
        {
            if(transform.position.y <= ground)
                playerGround = ground;
            
            transform.position.Set(transform.position.x, playerGround, 0);
            jump = false;
            ySpeed = 0;
        }
        else
            ySpeed -= gravity * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Platform" && ySpeed < 0)
        {
            playerGround = other.transform.position.y;
            playerGround += transform.localScale.y*0.5f + other.transform.localScale.y;
            Debug.Log("Colidiu");
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        playerGround = ground;
        jump = true;
    }
}
