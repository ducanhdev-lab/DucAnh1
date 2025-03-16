using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
     [SerializeField] public Animator playerAnimator;
    private bool isATK = true;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void HandleHPAnimation()
    {
        if (isATK)
        {
            StartCoroutine(AnimationComBat());
        }
    }
    private IEnumerator AnimationComBat()
{
    
        playerAnimator.SetBool("isATK", true);
        
        yield return new WaitForSeconds(0.5f);
        
        playerAnimator.SetBool("isATK", false);
    
    }

}
