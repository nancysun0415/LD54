using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    private AudioSource finishSound;
    private bool levelCompleted = false;

    private void Start() {
        finishSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.name == "Player" && !levelCompleted) {
            finishSound.Play();
            Invoke("CompleteLevel", 1f);
            levelCompleted = true;
            //CompleteLevel();
        }
    }

    private void CompleteLevel() {
        Debug.Log("Level Complete");
        GravityManager.Instance.ResetGravity();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
