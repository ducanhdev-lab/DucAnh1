using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnemy : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public Animator enemyAnimator;
    private bool isHurt = true;
    private bool isDead = false;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void HandleHPAnimation()
    {
        if (isHurt)
        {
            StartCoroutine(AnimationComBat());
        }
    }
    private IEnumerator AnimationComBat()
{
    
        enemyAnimator.SetBool("isHurt", true);
        
        yield return new WaitForSeconds(0.9f);
        
        enemyAnimator.SetBool("isHurt", false);
    
}
    public void HandleDeathAnimation()
    {
        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("isDead", true);
        }
    }

}
