using TMPro;
using UnityEngine;
public class PlayerInfoDisplayer : MonoBehaviour {
    

    [SerializeField]
    GameObject player;
    PlayerStateController playerStateController;

    [SerializeField]
    GameObject hpBar;
    Vector3 hpBarInitialSize;

    [SerializeField]
    GameObject hpTextObj;
    TextMeshProUGUI hpText;
    int preHP, preMAX;

    [SerializeField]
    GameObject atkNum;
    TextMeshProUGUI atk;
    int preAtk;

    [SerializeField]
    GameObject defNum;
    TextMeshProUGUI def;
    int preDef;

    [SerializeField]
    Color buffColor;

    void Start() {
        player = GameObject.Find("Player");
        playerStateController = player?.GetComponent<PlayerStateController>();

        atk = atkNum?.GetComponent<TextMeshProUGUI>();
        def = defNum?.GetComponent<TextMeshProUGUI>();

        hpBarInitialSize = hpBar.transform.localScale;
        hpText = hpTextObj?.GetComponent<TextMeshProUGUI>();
        
        if (!playerStateController) {
            this.enabled = false;
        }
    }
    void Update() {
        updateAtk();
        updateDef();
        updateHpBar();
    }

    void updateAtk() {
        if (!atk || playerStateController.atk == preAtk) {
            return;
        }
        preAtk = playerStateController.atk;
        atk.SetText("{}",playerStateController.atk);
    }
    void updateDef() {
        if (!def || playerStateController.def == preDef) {
            return;
        }
        preDef = playerStateController.def;
        def.SetText("{}",playerStateController.def);
    }
    void updateHpBar() {
        if (!hpBar || (preHP == playerStateController.currentHealth && playerStateController.maxHealth == preMAX))
        {
            return;
        }
        preMAX = playerStateController.maxHealth;
        preHP = playerStateController.currentHealth;
        float ration = (float)playerStateController.currentHealth / (float)playerStateController.maxHealth;
        hpBar.transform.localScale = new Vector3(hpBarInitialSize.x * ration, hpBarInitialSize.y, hpBarInitialSize.z);

        if(!hpText) {
            return;
        }
        hpText.SetText("{0}/{1}",playerStateController.currentHealth, playerStateController.maxHealth);
        
    }
}
