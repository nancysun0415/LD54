using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.U2D;

public class Container : MonoBehaviour
{
    private bool isOn = false;
    private Animator animator;
    private const string IS_ON = "isOn";

    [SerializeField] private LightZone lightZone;
    [SerializeField] private Light2DBase zoneLight2D;
    [SerializeField] private Light2DBase containerLight2D;
    [SerializeField] private AudioSource lightUp;
    [SerializeField] private AudioSource lightOff;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {

        isOn = false;
    }

    public void Interact() {

        if(!isOn) {
            TurnOn();
            TurnOffOther();
        }
    }

    public void TurnOffOther() {
        Container[] containers = FindObjectsOfType<Container>();
        foreach(Container container in containers) {
            if(container != this) {
                container.TurnOff();
                
            }
        }
    }

    private void TurnOff() {
        if(isOn) {
            isOn = false;
            //add animation here
            animator.SetBool(IS_ON, false);

            lightOff.Play();

            lightZone.gameObject.SetActive(false);
            zoneLight2D.gameObject.SetActive(false);
            containerLight2D.gameObject.SetActive(false);
            Debug.Log("Turning off: " + this.name);
        }
    }

    private void TurnOn() {
        isOn = true;
        //add animation here
        animator.SetBool(IS_ON, true);

        lightUp.Play();

        lightZone.gameObject.SetActive(true);
        zoneLight2D.gameObject.SetActive(true);
        containerLight2D.gameObject.SetActive(true);

        Debug.Log("Turning on" + this.name);

        GravityManager.Instance.ChangeGravityDirection(transform.eulerAngles.z);
    }

    public bool IsOn() { return isOn; }
}
