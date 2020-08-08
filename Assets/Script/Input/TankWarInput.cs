using HT.Framework;
using UnityEngine;

public class TankWarInput : InputDeviceBase
{
    public override void OnStartUp()
    {
        Main.m_Input.RegisterVirtualButton(InputButtonType.MouseLeft);
        Main.m_Input.RegisterVirtualButton(InputButtonType.MouseRight);
        Main.m_Input.RegisterVirtualButton(InputButtonType.MouseMiddle);

        Main.m_Input.RegisterVirtualAxis(InputAxisType.MouseX);
        Main.m_Input.RegisterVirtualAxis(InputAxisType.MouseY);
        Main.m_Input.RegisterVirtualAxis(InputAxisType.MouseScrollWheel);
        Main.m_Input.RegisterVirtualAxis(InputAxisType.Horizontal);
        Main.m_Input.RegisterVirtualAxis(InputAxisType.Vertical);

        Main.m_Input.RegisterVirtualButton("Shoot");
        Main.m_Input.RegisterVirtualButton("ShootSuperWeapon");
    }

    public override void OnRun()
    {
        if (Input.GetMouseButtonDown(0)) Main.m_Input.SetButtonDown(InputButtonType.MouseLeft);
        else if (Input.GetMouseButtonUp(0)) Main.m_Input.SetButtonUp(InputButtonType.MouseLeft);

        if (Input.GetMouseButtonDown(1)) Main.m_Input.SetButtonDown(InputButtonType.MouseRight);
        else if (Input.GetMouseButtonUp(1)) Main.m_Input.SetButtonUp(InputButtonType.MouseRight);

        if (Input.GetMouseButtonDown(2)) Main.m_Input.SetButtonDown(InputButtonType.MouseMiddle);
        else if (Input.GetMouseButtonUp(2)) Main.m_Input.SetButtonUp(InputButtonType.MouseMiddle);
        
        Main.m_Input.SetAxis(InputAxisType.MouseX, Input.GetAxis("Mouse X"));
        Main.m_Input.SetAxis(InputAxisType.MouseY, Input.GetAxis("Mouse Y"));
        Main.m_Input.SetAxis(InputAxisType.MouseScrollWheel, Input.GetAxis("Mouse ScrollWheel"));
        Main.m_Input.SetAxis(InputAxisType.Horizontal, Input.GetAxis("Horizontal"));
        Main.m_Input.SetAxis(InputAxisType.Vertical, Input.GetAxis("Vertical"));
        
        if (Input.GetKeyDown(KeyCode.J)) Main.m_Input.SetButtonDown("Shoot");
        else if (Input.GetKeyUp(KeyCode.J)) Main.m_Input.SetButtonUp("Shoot");

        if (Input.GetKeyDown(KeyCode.K)) Main.m_Input.SetButtonDown("ShootSuperWeapon");
        else if (Input.GetKeyUp(KeyCode.K)) Main.m_Input.SetButtonUp("ShootSuperWeapon");

        Main.m_Input.SetVirtualMousePosition(Input.mousePosition);
    }

    public override void OnShutdown()
    {
        Main.m_Input.UnRegisterVirtualButton(InputButtonType.MouseLeft);
        Main.m_Input.UnRegisterVirtualButton(InputButtonType.MouseRight);
        Main.m_Input.UnRegisterVirtualButton(InputButtonType.MouseMiddle);

        Main.m_Input.UnRegisterVirtualAxis(InputAxisType.MouseX);
        Main.m_Input.UnRegisterVirtualAxis(InputAxisType.MouseY);
        Main.m_Input.UnRegisterVirtualAxis(InputAxisType.MouseScrollWheel);
        Main.m_Input.UnRegisterVirtualAxis(InputAxisType.Horizontal);
        Main.m_Input.UnRegisterVirtualAxis(InputAxisType.Vertical);

        Main.m_Input.UnRegisterVirtualButton("Shoot");
        Main.m_Input.UnRegisterVirtualButton("ShootSuperWeapon");
    }
}
