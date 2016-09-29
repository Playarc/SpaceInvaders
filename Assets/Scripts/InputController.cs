using UnityEngine;

public class InputController
{
    public delegate void SpacePressed();
    public delegate void ArrowPressed(bool right);

    public event SpacePressed OnSpacePressed;
    public event ArrowPressed OnArrowPressed;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            InvokeArrowPressedEvent(false);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            InvokeArrowPressedEvent(true);
        }
    }

    private void InvokeSpacePressedEvent()
    {
        if (OnSpacePressed != null)
        {
            OnSpacePressed();
        }
    }

    private void InvokeArrowPressedEvent(bool right)
    {
        if (OnArrowPressed != null)
        {
            OnArrowPressed(right);
        }
    }
}