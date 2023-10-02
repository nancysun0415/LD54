using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightZone : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    [SerializeField] private Vector2 defaultSize = new Vector2(5f, 2f);
    [SerializeField] private Vector2 defaultOffset = new Vector2(0f, 1f);

    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

}
