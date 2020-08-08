using HT.Framework;
using UnityEngine;
/// <summary>
/// 登录界面
/// </summary>
[UIResource(Global.ABUI, "Assets/Prefab/UI/UILogin.prefab", "null")]
public class UILogin : UILogicResident
{
	/// <summary>
	/// 初始化
	/// </summary>
    public override void OnInit()
    {
        base.OnInit();

        UIEntity.FindChildren("Button_Play").rectTransform().AddEventListener(OnPlay);
        UIEntity.FindChildren("Button_Quit").rectTransform().AddEventListener(OnQuit);
    }

    private void OnPlay()
    {
        Main.m_Procedure.SwitchProcedure<ProcedureReady>();
    }

    private void OnQuit()
    {
        Application.Quit();
    }
}
