using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance;

    public InputSO actualInputConfig;

    private void Start()
    {
        Instance = this;
    }

    public float HorizontalMovement()
    {
        return Input.GetAxis("Horizontal");
    }

    public float VerticalMovement()
    {
        return Input.GetAxis("Vertical");
    }

    public bool Jump()
    {
        return Input.GetKeyDown(actualInputConfig.jump);
    }

    public bool RunInput()
    {
        return Input.GetKey(actualInputConfig.run);
    }
    public bool CrouchInput()
    {
        return Input.GetKey(actualInputConfig.crouch);
    }

    public bool PickUpItem()
    {
        return Input.GetKeyDown(actualInputConfig.pickup);
    }

    public bool MainAttack()
    {
        return Input.GetKeyDown(actualInputConfig.mainAttack);
    }

    public bool SecondaryAttack()
    {
        return Input.GetKeyDown(actualInputConfig.secondaryAttack);
    }

    public bool SpecialAttack()
    {
        return Input.GetKeyDown(actualInputConfig.specialAttack);
    }

    public bool Dash()
    {
        return Input.GetKeyDown(actualInputConfig.dash);
    }

    public Vector2 MousePos()
    {
        return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    }
}
