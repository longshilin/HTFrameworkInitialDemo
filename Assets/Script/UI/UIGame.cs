using DG.Tweening;
using HT.Framework;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 游戏界面
/// </summary>
[UIResource(Global.ABUI, "Assets/Prefab/UI/UIGame.prefab", "null")]
public class UIGame : UILogicResident
{
    private Text _title;
    private Image _playerPicture;
    private Text _playerName;
    private Text _playerHP;
    private Text _playerShield;
    private Image _enemyPicture;
    private Text _enemyName;
    private Text _enemyHP;
    private Text _enemyShield;
    private Image _weaponCooling;
    private Image _superWeaponCooling;
    private Transform _panel_HurtBlood;
    private GameObject _panel_Beat;
    private GameObject _panel_Failed;
    private GameObject _loading;
    private FSM _player;
    private TankData _playerData;

    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();

        _title = UIEntity.GetComponentByChild<Text>("Title/Text");
        _playerPicture = UIEntity.GetComponentByChild<Image>("PlayerHead/Picture");
        _playerName = UIEntity.GetComponentByChild<Text>("PlayerHead/Name/Value");
        _playerHP = UIEntity.GetComponentByChild<Text>("PlayerHead/HP/Value");
        _playerShield = UIEntity.GetComponentByChild<Text>("PlayerHead/Shield/Value");
        _enemyPicture = UIEntity.GetComponentByChild<Image>("EnemyHead/Picture");
        _enemyName = UIEntity.GetComponentByChild<Text>("EnemyHead/Name/Value");
        _enemyHP = UIEntity.GetComponentByChild<Text>("EnemyHead/HP/Value");
        _enemyShield = UIEntity.GetComponentByChild<Text>("EnemyHead/Shield/Value");
        _weaponCooling = UIEntity.GetComponentByChild<Image>("Panel_Weapon/Button_Weapon/Cooling");
        _superWeaponCooling = UIEntity.GetComponentByChild<Image>("Panel_Weapon/Button_SuperWeapon/Cooling");
        _panel_HurtBlood = UIEntity.transform.Find("Panel_HurtBlood");
        _panel_Beat = UIEntity.FindChildren("Panel_Beat");
        _panel_Failed = UIEntity.FindChildren("Panel_Failed");
        _loading = UIEntity.FindChildren("Loading");

        _panel_Beat.FindChildren("Button_Close").rectTransform().AddEventListener(() =>
        {
            _panel_Beat.SetActive(false);
            Main.m_Procedure.SwitchProcedure<ProcedureChooseLevel>();
        });
        _panel_Failed.FindChildren("Button_Close").rectTransform().AddEventListener(() =>
        {
            _panel_Failed.SetActive(false);
            Main.m_Procedure.SwitchProcedure<ProcedureChooseLevel>();
        });
        UIEntity.FindChildren("Button_KillSelf").rectTransform().AddEventListener(() => { _player.SwitchState<TankStateDeath>(); });

        Main.m_Event.Subscribe<EventGameLoading>(OnGameLoading);
        Main.m_Event.Subscribe<EventGameLoaded>(OnGameLoaded);
        Main.m_Event.Subscribe<EventGameStart>(OnGameStart);
        Main.m_Event.Subscribe<EventGameFailed>(OnGameFailed);
        Main.m_Event.Subscribe<EventGameBeat>(OnGameBeat);
        Main.m_Event.Subscribe<EventWeaponCooling>(OnWeaponCooling);
        Main.m_Event.Subscribe<EventSuperWeaponCooling>(OnSuperWeaponCooling);
        Main.m_Event.Subscribe<EventHitPlayer>(OnHitPlayer);
        Main.m_Event.Subscribe<EventHitEnemy>(OnHitEnemy);
    }
    
    /// <summary>
    /// 打开UI
    /// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);

        //注册伤害飘血显示框的对象池
        Main.m_ObjectPool.RegisterSpawnPool(Global.HurtBloodPoolName, _panel_HurtBlood.FindChildren("HurtBloodTem"), null, null, 200);
    }
    
	/// <summary>
	/// 关闭UI
	/// </summary>
    public override void OnClose()
    {
        base.OnClose();

        //清理并移除伤害飘血显示框对象池
        Main.m_ObjectPool.Clear(Global.HurtBloodPoolName);
        Main.m_ObjectPool.UnRegisterSpawnPool(Global.HurtBloodPoolName);
    }

	/// <summary>
	/// 销毁UI
	/// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();

        Main.m_Event.Unsubscribe<EventGameLoading>(OnGameLoading);
        Main.m_Event.Unsubscribe<EventGameLoaded>(OnGameLoaded);
        Main.m_Event.Unsubscribe<EventGameStart>(OnGameStart);
        Main.m_Event.Unsubscribe<EventGameFailed>(OnGameFailed);
        Main.m_Event.Unsubscribe<EventGameBeat>(OnGameBeat);
        Main.m_Event.Unsubscribe<EventWeaponCooling>(OnWeaponCooling);
        Main.m_Event.Unsubscribe<EventSuperWeaponCooling>(OnSuperWeaponCooling);
        Main.m_Event.Unsubscribe<EventHitPlayer>(OnHitPlayer);
        Main.m_Event.Unsubscribe<EventHitEnemy>(OnHitEnemy);
    }
    
    private void OnGameLoading()
    {
        _loading.SetActive(true);
    }

    private void OnGameLoaded()
    {
        _loading.SetActive(false);
    }

    private void OnGameStart(EventHandlerBase arg)
    {
        EventGameStart e = arg.Cast<EventGameStart>();
        _title.text = "第" + e.Level.ToString() + "关";
        _player = e.Player;
        _playerData = _player.CurrentData.Cast<TankData>();
        _playerPicture.sprite = _playerData.Picture;
        _playerName.text = _playerData.Name;
        _playerHP.text = _playerData.HP.ToString() + "/" + _playerData.MaxHP.ToString();
        _playerShield.text = _playerData.Shield.ToString() + "/" + _playerData.MaxShield.ToString();
    }

    private void OnGameFailed()
    {
        Main.Current.DelayExecute(() => { _panel_Failed.SetActive(true); }, 3);
    }

    private void OnGameBeat(EventHandlerBase arg)
    {
        ProcedureGame procedureGame = Main.m_Procedure.CurrentProcedure.Cast<ProcedureGame>();
        int timeScore = 1800 - procedureGame.GameTime;
        int fightScore = (int)(1800 * ((float)_playerData.HP / _playerData.MaxHP));
        int totalScore = timeScore + fightScore;

        Main.Current.DelayExecute(() =>
        {
            _panel_Beat.SetActive(true);
            _panel_Beat.GetComponentByChild<Text>("TimeScore").text = timeScore.ToString();
            _panel_Beat.GetComponentByChild<Text>("FightScore").text = fightScore.ToString();
            _panel_Beat.GetComponentByChild<Text>("TotalScore").text = totalScore.ToString();
            string key = "TankWar_Level_" + procedureGame.ChooseLevel.ToString();
            string levelData = PlayerPrefs.GetString(key, "");
            if (levelData != "")
            {
                JsonData json = GlobalTools.StringToJson(levelData);
                if (int.Parse(json["Score"].ToString()) < totalScore) json["Score"] = totalScore;
                json["Number"] = int.Parse(json["Number"].ToString()) + 1;
                PlayerPrefs.SetString(key, GlobalTools.JsonToString(json));
            }
            else
            {
                JsonData json = new JsonData();
                json["Score"] = new JsonData();
                json["Score"] = totalScore;
                json["Number"] = new JsonData();
                json["Number"] = 1;
                PlayerPrefs.SetString(key, GlobalTools.JsonToString(json));
            }
        }, 3);
    }

    private void OnSuperWeaponCooling(EventHandlerBase arg)
    {
        _superWeaponCooling.fillAmount = arg.Cast<EventSuperWeaponCooling>().Percen;
    }

    private void OnWeaponCooling(EventHandlerBase arg)
    {
        _weaponCooling.fillAmount = arg.Cast<EventWeaponCooling>().Percen;
    }

    private void OnHitPlayer(EventHandlerBase arg)
    {
        EventHitPlayer eventHit = arg.Cast<EventHitPlayer>();
        _playerHP.text = _playerData.HP.ToString() + "/" + _playerData.MaxHP.ToString();
        _playerShield.text = _playerData.Shield.ToString() + "/" + _playerData.MaxShield.ToString();
        HurtBlood(_player.transform.position, "-" + eventHit.Hurt.ToString() + eventHit.BuffName);
    }

    private void OnHitEnemy(EventHandlerBase arg)
    {
        EventHitEnemy eventHit = arg.Cast<EventHitEnemy>();
        TankData tankData = eventHit.Enemy.CurrentData.Cast<TankData>();
        if (tankData.HP > 0)
        {
            _enemyPicture.transform.parent.gameObject.SetActive(true);
            _enemyPicture.sprite = tankData.Picture;
            _enemyName.text = tankData.Name;
            _enemyHP.text = tankData.HP.ToString() + "/" + tankData.MaxHP.ToString();
            _enemyShield.text = tankData.Shield.ToString() + "/" + tankData.MaxShield.ToString();
        }
        else
        {
            _enemyPicture.transform.parent.gameObject.SetActive(false);
        }
        HurtBlood(eventHit.Enemy.transform.position, "-" + eventHit.Hurt.ToString() + eventHit.BuffName);
    }

    private void HurtBlood(Vector3 pos, string show)
    {
        RectTransform hurtBlood = Main.m_ObjectPool.Spawn(Global.HurtBloodPoolName).rectTransform();
        hurtBlood.anchoredPosition = pos.WorldToUGUIPosition();
        hurtBlood.localScale = new Vector3(0.6f, 0.6f, 1);
        hurtBlood.DOAnchorPos(hurtBlood.anchoredPosition + new Vector2(0, 50), 1);
        hurtBlood.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

        Text text = hurtBlood.GetComponent<Text>();
        text.color = Color.red;
        text.text = show;
        text.DOColor(new Color(1, 0, 0, 0), 1).OnComplete(() =>
        {
            Main.m_ObjectPool.Despawn(Global.HurtBloodPoolName, hurtBlood.gameObject);
        });
    }
}