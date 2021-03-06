﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [Header("Player attributes")]
    public float speed = 20f;
    public float jumpForce = 10f;

    [Header("Slide attributes")]
    public float scaleValue = .2f;
    public float slideTime = 1f;
    public float shrinkTime = 0.005f;

    private bool isRunning = false;
    private bool isGrounded = false;
    private bool canSlide = false;


    private Rigidbody2D rb;

    private Button runButton;

    [Header("Public gameobjects")]
    public HealthSystem healthScr;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        runButton = GameObject.FindGameObjectWithTag("RunButton").GetComponent<Button>();

	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

        if (col.gameObject.tag == "DamageObject")
        {
            healthScr.IsHit();
            SlideButton();

            Debug.Log("Lose health and slide");
        }
    }

    void FixedUpdate()
    {
        if (isRunning)
        {
            canSlide = true;
        }
    }

    public void Run()
    {
        if (isGrounded)
		{
			rb.velocity = new Vector2(speed * 0.02f, rb.velocity.y);
			isRunning = true;
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
			rb.AddForce(Vector2.up * jumpForce);
            isGrounded = false;
        }
    }

    public void SlideButton()
    {
        StartCoroutine(Slide());
    }

    public IEnumerator Slide()
    {
        if (isRunning)
        {
            canSlide = false;
            float defaultScale = transform.localScale.y;
            while (transform.localScale.y > defaultScale / 2)
            {
                transform.localScale -= new Vector3(0, scaleValue, 0);
                yield return new WaitForSeconds(shrinkTime);
            }

            yield return new WaitForSeconds(slideTime);

            while (transform.localScale.y < defaultScale)
            {
                transform.localScale += new Vector3(0, scaleValue, 0);
                yield return new WaitForSeconds(shrinkTime);
            }
            canSlide = true;
            Run();
            yield return null;
        }
        yield return null;
    }

    public void Stop()
    {
        isRunning = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
}

