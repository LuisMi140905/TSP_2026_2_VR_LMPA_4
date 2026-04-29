using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Anim : MonoBehaviour
{
    public Animator Animator;

    void Start()
    {
        Animator = GetComponent<Animator>();
        //Ejemplo
        Animator=GameObject.FindGameObjectWithTag("Dragon").GetComponent<Animator>();
    }

    public void PayAnim() 
    {
        Animator.enabled = true;
        Animator.Play("PayAnim");
    }

    public void StopAnim() 
    {
        Animator.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
