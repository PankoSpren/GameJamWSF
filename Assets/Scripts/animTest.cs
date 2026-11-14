using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class animTest : MonoBehaviour
{
    Animator animator;
    int number = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }
    private void Update()
    {
        if (true == Input.GetKeyDown(KeyCode.Space))
        {
            Transition();
        }
    }
    void Transition()
    {
        number += 1;
        animator.SetInteger("TransitionInt", number);
    }

}

