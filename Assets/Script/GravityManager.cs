using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public event EventHandler OnGravityChange;

    public enum GravityState {
        Left, Right, Up, Down
    }

    private GravityState gravityState;

    public static GravityManager Instance { get; private set; }

    private void Awake() {
        Instance = this; 
        gravityState = GravityState.Down;
    }

    public void ChangeGravityDirection(float zDegree) {
        Vector2 direction;
        GravityState newState;

        if (zDegree == 90) {
            direction = Vector2.right; //right
            newState = GravityState.Right;

        } else if (Mathf.Abs(zDegree) == 180) {
            direction = Vector2.up; //up
            newState = GravityState.Up;

        } else if (zDegree == (-90)) {
            direction = Vector2.left; //left
            newState = GravityState.Left;

        } else {
            direction = Vector2.down; //down
            newState = GravityState.Down;
        }

        Debug.Log(newState.ToString());

        if(gravityState != newState) {
            gravityState = newState;
            Physics2D.gravity = direction * 9.8f;
            Debug.Log(Physics2D.gravity);

            OnGravityChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public GravityState GetState() {
        return gravityState;
    }

    public void ResetGravity() {
        gravityState = GravityState.Down;
        Physics2D.gravity = Vector2.down * 9.8f;

        OnGravityChange?.Invoke(this, EventArgs.Empty);
    }
}
