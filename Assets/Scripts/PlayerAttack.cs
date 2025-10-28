using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    // This script is only for test
    // change it as you like.

    InputSystem_Actions inputActions;

    [SerializeField]
    GameObject bullet;
    void Start()
    {
        inputActions = new InputSystem_Actions();
        var playerMap = inputActions.Player;
        playerMap.Enable();
        playerMap.Attack.performed += ctx => attack();

    }

    // Update is called once per frame
    void attack()
    {
        Debug.Log("PlayerAttack");
        if (bullet)
        {
            var bul = Instantiate(bullet, transform.position + transform.forward * 2, transform.rotation);
            bul.GetComponent<Rigidbody>()?.AddForce(transform.forward*20,ForceMode.Impulse);
        }
    }
}
