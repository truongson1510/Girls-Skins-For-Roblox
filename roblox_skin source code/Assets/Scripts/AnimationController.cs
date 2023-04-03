using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{


    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToRamba_dance()
    {

        animator.SetBool("ToRamba", true);
        animator.SetBool("ToHipHopDance", false);
        animator.SetBool("TowaveDance", false);

    }


    public void ToHipHop_Dance()
    {

        animator.SetBool("ToHipHopDance", true);
        animator.SetBool("TowaveDance", false);
        animator.SetBool("ToRamba", false);
       
    }


    public void ToWave_Dance()
    {
        animator.SetBool("TowaveDance", true);
        animator.SetBool("ToRamba", false);
        animator.SetBool("ToHipHopDance", false);
    }

   
}

