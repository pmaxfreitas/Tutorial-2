using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    private GameObject canvas;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
    }

    void Update()
    {
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if(scoreValue == 4)
        {
            canvas.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
            canvas.GetComponent<HorizontalLayoutGroup>().padding.left = 0;
            canvas.GetComponent<HorizontalLayoutGroup>().padding.top = 0;

            score.fontSize = 30;
            score.alignment = TextAnchor.MiddleCenter;
            score.text = "You Win! \nMade by Max Freitas";
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Floor")
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }
}
