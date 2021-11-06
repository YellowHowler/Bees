using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] float FadeRate;
    [SerializeField] Animator transition;

    private float loadTime = 1;
    private Image image;
    private float targetAlpha;
    // Use this for initialization  
    // Update is called once per frame

    void Start()
    {
        transite();
    }
    
    void Update () {
        
    }

    public void FadeOut()
    {
        
    }

    IEnumerator TransitionCoroutine()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(loadTime);
    }

    public void transite()
    {
        StartCoroutine(TransitionCoroutine());
    }
}
