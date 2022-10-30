using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    private GameObject canvas;
    public Text lives;
    private int livesValue = 3;
    public AudioClip bgMusic;
    public AudioClip winMusic;
    public AudioSource source;
    private Animator anim;
    private bool facingRight = true;
    private bool isOnGround;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask allGround;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        rd2d = GetComponent<Rigidbody2D>();
        
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();

        source.clip = bgMusic;
        source.Play();

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(isOnGround == true)
        {
            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                anim.SetInteger("State", 1);
            }
            else
            {
                anim.SetInteger("State", 0);
            }
        }
        else if(isOnGround == false)
        {
            anim.SetInteger("State", 2);
        }

        if(transform.position.y < -5 && 0 <= scoreValue && scoreValue <= 3)
        {
            transform.position = new Vector2(-8, 0);
        }
        else if(transform.position.y < -5 && 4 <= scoreValue && scoreValue <= 8)
        {
            transform.position = new Vector2(20, 0);
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, allGround);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if(scoreValue == 4)
            {
            transform.position = new Vector2(20, 0);
            livesValue = 3;
            lives.text = "Lives: " + livesValue.ToString();
            }

            if(scoreValue == 8)
            {
            canvas.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
            canvas.GetComponent<HorizontalLayoutGroup>().padding.top = 0;

            Destroy(GameObject.Find("Lives"));

            score.fontSize = 30;
            score.alignment = TextAnchor.MiddleCenter;
            score.text = "You Win! \nGame Created by Max Freitas";

            source.clip = winMusic;
            source.Play();
            }
        }

        if(collision.collider.tag == "enemy")
        {
            livesValue--;
            lives.text = "Lives: " + livesValue.ToString();

            Destroy(collision.collider.gameObject);

            if(livesValue == 0)
            {
            canvas.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
            canvas.GetComponent<HorizontalLayoutGroup>().padding.top = 0;

            Destroy(GameObject.Find("Score"));
            anim.SetInteger("State", 3);
            Destroy(this);

            lives.fontSize = 30;
            lives.alignment = TextAnchor.MiddleCenter;
            lives.text = "You Lose!";
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Floor" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
}
