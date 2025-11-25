using UnityEngine;

public class PlayerStateController : MonoBehaviour
{

    public float gpa = 3.0f;
    public int currentHealth;
    public int maxHealth = 50;

    [Range(0,50)]
    public int atk = 5;

    [Range(0,50)]
    public int def = 5;

    void Start()
    {
        currentHealth = maxHealth;
    }
}
