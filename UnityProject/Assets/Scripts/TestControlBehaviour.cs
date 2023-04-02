using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestControlBehaviour : MonoBehaviour
{
	private const string BringTrigger = "bring";
	private const string IdleTwiddleTrigger = "idle_twiddle";
	private const string PutDownTrigger = "put_down";

	Animator anim;
    float a = 0.5f;
    float f = 0.0f;
    int i = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void IdleFluc()
    {
        if (f < 1.0f)
        {
            a += i == 0 ? 0.03f : -0.03f; // add if i is 0, otherwise substract

            if (a > 0.9f) i = 1;
            if (a < 0.1f) i = 0;

            anim.SetFloat("idle_random", a);

            f += 0.1f;
        }
        else
        {
            i = Random.Range(0, 2);
            f = 0;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(BringTrigger);
            anim.SetTrigger(BringTrigger);
            InvokeRepeating(nameof(IdleFluc), 0.0f, 0.1f);
        }
        /*else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            
        }*/
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            anim.SetTrigger(IdleTwiddleTrigger);
            Debug.Log(IdleTwiddleTrigger + anim.GetBool(IdleTwiddleTrigger).ToString());
        }
        /*else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            
        }*/
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            anim.SetTrigger(PutDownTrigger);
            Debug.Log(PutDownTrigger);
        }
    }
}
