using HT.Framework;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 选择关卡界面
/// </summary>
[UIResource(Global.ABUI, "Assets/Prefab/UI/UIChooseLevel.prefab", "null")]
public class UIChooseLevel : UILogicResident
{
    //关卡列表的ScrollRect组件
    private ScrollRect _scrollView_Levels;
    //下方的进入游戏按钮
    private Button _button_Start;
    //当前的所有可选关卡按钮（这里其实是Toggle组件）
    private Toggle[] _levelToggles;
    //当前的所有可选关卡选项
    private UILevelItem[] _levelItems;
    //用户当前是否改变了选择的关卡
    private bool _isChangeChoose;
    //当前选择的关卡
    private UILevelItem _currentChoose;

    /// <summary>
    /// 当前选择的关卡
    /// </summary>
    public UILevelItem CurrentChoose
    {
        get
        {
            return _currentChoose;
        }
        set
        {
            if (_currentChoose != value)
            {
                _currentChoose = value;
                _button_Start.interactable = _currentChoose != null;
            }
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();

        _scrollView_Levels = UIEntity.GetComponentByChild<ScrollRect>("ScrollView_Levels");
        _button_Start = UIEntity.GetComponentByChild<Button>("Panel_Start/Button_Start");
        _levelToggles = _scrollView_Levels.content.GetComponentsInChildren<Toggle>();
        _levelItems = _scrollView_Levels.content.GetComponentsInChildren<UILevelItem>();
        _isChangeChoose = false;

        for (int i = 0; i < _levelToggles.Length; i++)
        {
            _levelToggles[i].onValueChanged.AddListener((value) => { _isChangeChoose = true; });
        }
        UIEntity.FindChildren("Button_Back").rectTransform().AddEventListener(() => { Main.m_Procedure.SwitchProcedure<ProcedureReady>(); });

        _button_Start.onClick.AddListener(() =>
        {
            Main.m_Procedure.GetProcedure<ProcedureGame>().ChooseLevel = CurrentChoose.Level;
            Main.m_Procedure.SwitchProcedure<ProcedureGame>();
        });
    }

    /// <summary>
    /// 打开UI
    /// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);

        ReadLevelData();
    }
    
    /// <summary>
    /// UI逻辑刷新
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (_isChangeChoose)
        {
            _isChangeChoose = false;
            RefreshChoose();
        }
    }

    /// <summary>
    /// 刷新选择
    /// </summary>
    private void RefreshChoose()
    {
        for (int i = 0; i < _levelToggles.Length; i++)
        {
            if (_levelToggles[i].isOn)
            {
                CurrentChoose = _levelItems[i];
                return;
            }
        }
        CurrentChoose = null;
    }

    /// <summary>
    /// 读取关卡数据
    /// </summary>
    private void ReadLevelData()
    {
        //目前只有二关
        string score;
        string number;
        bool isEnable = true;
        for (int i = 0; i < _levelItems.Length; i++)
        {
            string levelData = PlayerPrefs.GetString("TankWar_Level_" + _levelItems[i].Level.ToString(), "");
            if (levelData != "")
            {
                JsonData json = GlobalTools.StringToJson(levelData);
                score = json["Score"].ToString();
                number = json["Number"].ToString();
                _levelItems[i].ApplyData(score, number, isEnable);
                //上一关通关次数大于0，本关才激活
                isEnable = int.Parse(number) > 0;
            }
            else
            {
                score = "0";
                number = "0";
                _levelItems[i].ApplyData(score, number, isEnable);
                //上一关通关次数大于0，本关才激活
                isEnable = int.Parse(number) > 0;
            }
        }
    }
}