using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySprite : MonoBehaviour
{
    // sprite speed
    public float speed = 5;
    // threshhold for double jumping
    public float velThreshhold = 0.01f;

    // the sprite states and animations
    public Sprite fireSprite;
    public Sprite waterSprite;
    public Sprite airSprite;
    public RuntimeAnimatorController fireAnimate;
    public RuntimeAnimatorController waterAnimate;
    public RuntimeAnimatorController airAnimate;
    // how often player can switch sprites
    private float spriteDelay = 0.3f;
    // 0 is fire, 1 is water, 2 is air
    private float spriteState = 0;

    // water shield
    public static bool shielded = false;
    public GameObject shieldPrefab;
    private GameObject waterShield;

    // fireball
    public GameObject ballPrefab;
    private float ballDelay = 0.25f;

    // time tracker
    private float spriteTime;
    private float abilityTime;

    // sprite components
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    //private Transform player;
    public static float health = 100;

    // call start
    private void Start()
    {
        // initialize sprite components
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        //transform = GetComponent<Transform>();

        // get current time
        spriteTime = Time.time;
        abilityTime = Time.time;
    }

    // frame update
    private void Update()
    {
        // change state of sprite if necessary
        // implement a sprite switch cooldown
        if (Input.GetButton("Switch") && (Time.time - spriteTime > spriteDelay))
        {
            ChangeSprite();
            spriteTime = Time.time;
        }
        // use ability
        if (Input.GetButton("Fire"))
        {
            UseAbility();
        }
        // destroy shield if necessary
        if (spriteState != 1 || Input.GetButtonUp("Fire"))
        {
            // destroy the shield
            if (waterShield != null)
            {
                Destroy(waterShield);
            }
        }
    }

    // physics update
    private void FixedUpdate()
    {
        // move the sprite
        Move();
    }

    private void Move()
    {
        // horizontal input axis
        float horizontal = Input.GetAxis("Horizontal");

        // allow double jumps only if it's the air sprite
            // infinite double jumps?
        // other sprites are only allowed single jumps
        //if (Input.GetButton("Vertical") && Mathf.Abs(rb.velocity.y) < velThreshhold && (spriteState == 2 || Mathf.Abs(rb.velocity.y) < 0.01f))
        if (Input.GetButton("Vertical") && (spriteState == 2 || Mathf.Abs(rb.velocity.y) < velThreshhold))
        {
            rb.velocity = new Vector2(horizontal * speed, speed);
        }
        else
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        // fix the orientation of the sprite
        if (rb.velocity.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (rb.velocity.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void UseAbility()
    {
        // fire ability - shoot
        if (spriteState == 0 && ((Time.time - abilityTime) > ballDelay))
        {
            // shoot
            // instantiate ball
            GameObject ball = Instantiate(ballPrefab, transform.position, transform.rotation);

            // ability time cooldown
            abilityTime = Time.time;
        }
        // water ability - shield
        else if (spriteState == 1 && waterShield == null)
        {
            //shield
            waterShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        }
        // air ability - jump/double jump
        else if (spriteState == 2)
        {
            // jump
            rb.velocity = new Vector2(rb.velocity.x, speed);
        }
    }

    private void ChangeSprite()
    {
        // change the sprite state
        if (spriteState == 2)
        {
            spriteState = 0;
        }
        else
        {
            spriteState++;
        }

        // if it's fire sprite
        if (spriteState == 0)
        {
            spriteRenderer.sprite = fireSprite;
            animator.runtimeAnimatorController = fireAnimate;
        }
        // if it's water sprite
        else if (spriteState == 1)
        {
            spriteRenderer.sprite = waterSprite;
            animator.runtimeAnimatorController = waterAnimate;
        }
        // if it's air sprite
        else if (spriteState == 2)
        {
            spriteRenderer.sprite = airSprite;
            animator.runtimeAnimatorController = airAnimate;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if it collides with a vorax shot
        // if it collides with a vorax
        // if it collides with a crystal
    }
}