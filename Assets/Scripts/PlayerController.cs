using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;


public class PlayerController : MonoBehaviour
{
    InputSystem_Actions inputActions;
    Vector2 moveVector = new Vector2(0, 0);
    float speed = 10;
    void Start()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => { moveVector = ctx.ReadValue<Vector2>(); };
        inputActions.Player.Move.canceled += ctx => { moveVector = Vector2.zero; };

    }

    void Update()
    {
        Move();
    }
    void Move()
    {
        Vector3 newPosition = transform.position + new Vector3(moveVector.x * speed * Time.deltaTime, moveVector.y * speed * Time.deltaTime, 0);
        transform.position = newPosition;
    }
}
