using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dmg : MonoBehaviour
{
    [SerializeField] private Dot dot;
    [SerializeField] private EnemyHealth enhealth;
    [SerializeField] private PlayerHealth plhealth;
    [SerializeField] private AnimationEnemy animE;
    [SerializeField] private AnimationPlayer animP;
    
    void Start()
    {
        enhealth = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyHealth>();
        plhealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        dot = GetComponent<Dot>();
        animE = GameObject.FindGameObjectWithTag("Enemy")?.GetComponent<AnimationEnemy>();
        animP = GameObject.FindGameObjectWithTag("Player")?.GetComponent<AnimationPlayer>();
    }

    public void ProcessMatch()
    {
        if (dot != null && dot.isMatched)
        {
            HandleMatch();
        }
    }

    private void HandleMatch()      //
    {
        string dotTag = gameObject.tag;
        string debugMessage = "";
        
        switch (dotTag)
        {
            case "Symbol Attack":
                debugMessage = $"HP -6";
                enhealth.TakeDamage(6);
                animP.HandleHPAnimation();
                animE.HandleHPAnimation();
                break;
            case "Symbol Health":
                debugMessage = $"HP +4";
                plhealth.Heal(6);
                break;
            case "Symbol DEF":
                debugMessage = $"Defense +3";
                plhealth.AddDEF(2);
                break;
            case "Symbol Mana":
                debugMessage = $"Mana +2";
                plhealth.AddMana(3);
                break;
            default:
                debugMessage = $"Match found at position ({dot.col}, {dot.row}) with tag {dotTag}";
                break;
        }
        
        Debug.Log(debugMessage);
        dot.isMatched = false;
    }
}