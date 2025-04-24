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
        // Tìm Player và Enemy trong Scene dựa trên tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");

        // Lấy component PlayerHealth và EnemyHealth
        if (playerObject != null)
        {
            plhealth = playerObject.GetComponent<PlayerHealth>();
            animP = playerObject.GetComponent<AnimationPlayer>();
        }
        else
        {
            Debug.LogError("Dmg: Player GameObject with tag 'Player' not found!");
        }

        if (enemyObject != null)
        {
            enhealth = enemyObject.GetComponent<EnemyHealth>();
            animE = enemyObject.GetComponent<AnimationEnemy>();
        }
        else
        {
            Debug.LogError("Dmg: Enemy GameObject with tag 'Enemy' not found!");
        }

        // Lấy Dot từ chính GameObject này
        dot = GetComponent<Dot>();
        if (dot == null)
        {
            Debug.LogError("Dmg: Dot component not found on this GameObject!");
        }
    }

    public void ProcessMatch()
    {
        if (dot != null && dot.isMatched)
        {
            HandleMatch();
        }
    }

    private void HandleMatch()
    {
        string dotTag = gameObject.tag;
        string debugMessage = "";

        // Đảm bảo các thành phần cần thiết đã được gán
        if (plhealth == null || enhealth == null)
        {
            Debug.LogError("Dmg: PlayerHealth or EnemyHealth is null!");
            return;
        }

        // Phân biệt lượt của Player hoặc Enemy dựa trên CombatManager
        if (!CombatManager.Instance.IsBotTurn) // Lượt của Player
        {
            switch (dotTag)
            {
                case "Symbol Attack":
                    debugMessage = $"Player attacks: Enemy HP -6";
                    enhealth.TakeDamage(6);
                    if (animP != null) animP.HandleHPAnimation(); // Player: Attack
                    if (animE != null) animE.HandleHPAnimation(); // Enemy: Hit
                    break;
                case "Symbol Health":
                    debugMessage = $"Player heals: HP +6";
                    plhealth.Heal(6);
                    break; // Không có animation cho Heal
                case "Symbol DEF":
                    debugMessage = $"Player gains defense: DEF +2";
                    plhealth.AddDEF(2);
                    break; // Không có animation cho DEF
                case "Symbol Mana":
                    debugMessage = $"Player gains mana: Mana +3";
                    plhealth.AddMana(3);
                    break; // Không có animation cho Mana
                default:
                    debugMessage = $"Player match at ({dot.col}, {dot.row}) with tag {dotTag}";
                    break;
            }
        }
        else // Lượt của Enemy (bot)
        {
            switch (dotTag)
            {
                case "Symbol Attack":
                    debugMessage = $"Enemy attacks: Player HP -6";
                    plhealth.TakeDamage(6);
                    if (animE != null) animE.HandleHPAnimation(); // Enemy: Attack
                    if (animP != null) animP.HandleHPAnimation(); // Player: Hit
                    break;
                case "Symbol Health":
                    debugMessage = $"Enemy heals: HP +6";
                    enhealth.Heal(6);
                    break; // Không có animation cho Heal
                case "Symbol DEF":
                    debugMessage = $"Enemy gains defense: DEF +2";
                    enhealth.AddDEF(2);
                    break; // Không có animation cho DEF
                case "Symbol Mana":
                    debugMessage = $"Enemy gains mana: Mana +3";
                    enhealth.AddMana(3);
                    break; // Không có animation cho Mana
                default:
                    debugMessage = $"Enemy match at ({dot.col}, {dot.row}) with tag {dotTag}";
                    break;
            }
        }

        Debug.Log(debugMessage);
        dot.isMatched = false; // Reset trạng thái match
    }
}