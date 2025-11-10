using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    // This script is only for test
    // change it as you like.

    [SerializeField]
    GameObject bullet;

    [SerializeField]
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void attack1()
    {
        Debug.Log("PlayerAttack");
        if (bullet)
        {
            var bul = Instantiate(bullet, transform.position + transform.forward * 2 + transform.up * 2, transform.rotation);
            bul.GetComponent<Rigidbody>()?.AddForce(transform.forward * 20, ForceMode.Impulse);
            animator?.SetTrigger("Punch");
        }
    }
}
