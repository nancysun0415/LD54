using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    private const string CONTAINER = "Container";

    private int safeZoneCount = 0;
    private Rigidbody2D rb;
    private Animator animator;
    private const string LIGHT = "Light";
    private const string ISDEAD = "isDead";

    [SerializeField] private AudioSource playerDie;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.CompareTag(CONTAINER)) {
            Container container = other.gameObject.GetComponent<Container>();

            if(!container.IsOn())
                safeZoneCount++;

            container.Interact();
            RotatePlayer(other.gameObject);
        }
    }

    private void RotatePlayer(GameObject gameObject) {
        if(gameObject.transform.eulerAngles != transform.eulerAngles) {
            transform.rotation = Quaternion.Euler(gameObject.transform.eulerAngles);
            //also play animation
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag(LIGHT)) {
            Debug.Log("Exiting");
            safeZoneCount--;
            Debug.Log("-1 Current: " + safeZoneCount);
        }

        if (safeZoneCount <= 0) {
            Die();
        }
    }

    private void Die() {
        Debug.Log("YOU SHALL DIE.mp3");
        animator.SetTrigger(ISDEAD);
        playerDie.Play();
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void RestartLevel() {
        GravityManager.Instance.ResetGravity();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
