using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SalamandraController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float impulse;
    [SerializeField] private float gravity;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float damageCooldown = 1.5f;
    [SerializeField] private bool player1;
    private AudioSource damageTakenSound;
    private AudioSource jumpSound;


    public float ground;
    private float playerGround;
    private bool jump;
    private float ySpeed;
    private int health;
    private float timeDmg;
    private bool gotDamaged;

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
        damageTakenSound = GetComponents<AudioSource>()[0];
        jumpSound = GetComponents<AudioSource>()[1];
        health = maxHealth;
        timeDmg = 0f;
        gotDamaged = false;

        if(PlayerPrefs.GetInt("player2") == 0 && !player1)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(jump)
        {
            Jump();
        }
        else if((player1 && Input.GetKeyDown(KeyCode.Space)) || (!player1 && Input.GetKeyDown(KeyCode.I)))
        {
            jumpSound.Play();
            jump = true;
            animator.SetBool("isJumping", true);
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

        if(gotDamaged)
            timeDmg += Time.deltaTime;
        if(timeDmg >= damageCooldown){
            timeDmg = 0f;
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            gotDamaged = false;
        }
        
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
            animator.SetBool("isJumping", false);
            ySpeed = 0;
        }
        else
            ySpeed -= gravity * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Platform" && ySpeed <= 0)
        {
            playerGround = other.transform.position.y + other.transform.localScale.y*0.5f;
            // playerGround += transform.localScale.y*0.5f + other.transform.localScale.y*0.5f;
        }
        if(other.gameObject.tag == "Enemy" && !gotDamaged)
        {
            Debug.Log("Colidiu");
            damageTakenSound.Play();
            health--;
            if(health<=0){
                SceneManager.LoadScene("DeathScene");
            }
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.9f);
            gotDamaged = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        playerGround = ground;
        jump = true;
        animator.SetBool("isJumping", true);
    }

}
