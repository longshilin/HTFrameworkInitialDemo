using HT.Framework;
using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// 闯关流程
/// </summary>
public class ProcedureGame : ProcedureBase
{
    /// <summary>
    /// 已选坦克数据集
    /// </summary>
    public DataSetTank ChooseDataSet;
    /// <summary>
    /// 已选关卡
    /// </summary>
    public int ChooseLevel = 1;
    /// <summary>
    /// 本次关卡耗时（秒）
    /// </summary>
    public int GameTime = 0;

    //当前玩家
    private FSM _player;
    //当前关卡
    private ILevel _level;
    //当前关卡逻辑类
    private EntityLogicBase _levelLogic;
    //当前常规主炮模板
    private GameObject _weaponTem;
    //当前超级武器模板
    private GameObject _superWeaponTem;

    /// <summary>
    /// 流程初始化
    /// </summary>
    public override void OnInit()
    {
        Main.m_Controller.EnableRotationControl = false;
        Main.m_Controller.EnablePositionControl = false;
    }

    /// <summary>
    /// 进入流程
    /// </summary>
    /// <param name="lastProcedure">上一个离开的流程</param>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
        Main.Current.StartCoroutine(InitGame());
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        Main.Current.StartCoroutine(ClearGame());
    }

    /// <summary>
    /// 流程帧刷新
    /// </summary>
    public override void OnUpdate()
    {
        
    }

    /// <summary>
    /// 流程帧刷新（秒）
    /// </summary>
    public override void OnUpdateSecond()
    {
        GameTime += 1;
    }

    /// <summary>
    /// 初始化游戏
    /// </summary>
    private IEnumerator InitGame()
    {
        //打开游戏界面
        yield return Main.m_UI.OpenResidentUI<UIGame>();

        //抛出加载中事件
        Main.m_Event.Throw<EventGameLoading>();
        
        //加载已选关卡
        Type type = ReflectionToolkit.GetTypeInRunTimeAssemblies("Level" + ChooseLevel.ToString());
        yield return Main.m_Entity.CreateEntity(type, "Level", null, (level) =>
        {
            _level = level as ILevel;
            _levelLogic = level;
        });

        //创建玩家
        PrefabInfo prefabInfo = new PrefabInfo(Global.ABTem, "Assets/Prefab/Tem/玩家坦克模板.prefab", null);
        yield return Main.m_Resource.LoadPrefab(prefabInfo, null, null, (obj) =>
        {
            _player = obj.GetComponent<FSM>();
            _player.name = "Player";
            _player.transform.SetParent(_levelLogic.Entity.transform);
            _player.transform.localPosition = _level.Birthplace.localPosition;
            _player.transform.localRotation = _level.Birthplace.localRotation;
            _player.transform.localScale = _level.Birthplace.localScale;
            _player.CurrentData.Cast<TankData>().SetData(ChooseDataSet, ChooseDataSet.name, TankSide.Player);
            _player.gameObject.SetActive(true);
            Main.m_FSM.RegisterFSM(_player);
        });
        
        //加载常规主炮模板
        prefabInfo = new PrefabInfo(Global.ABTem, "Assets/Prefab/Tem/常规主炮模板.prefab", null);
        yield return Main.m_Resource.LoadPrefab(prefabInfo, null, null, (obj) =>
        {
            _weaponTem = obj;
            _weaponTem.transform.SetParent(_levelLogic.Entity.transform);
            _weaponTem.SetActive(false);
        });

        //加载超级武器模板
        prefabInfo = new PrefabInfo(Global.ABTem, "Assets/Prefab/Tem/超级武器模板.prefab", null);
        yield return Main.m_Resource.LoadPrefab(prefabInfo, null, null, (obj) =>
        {
            _superWeaponTem = obj;
            _superWeaponTem.transform.SetParent(_levelLogic.Entity.transform);
            _superWeaponTem.SetActive(false);
        });

        //卸载模板AB包，因为后续短时间内不会再使用
        Main.m_Resource.UnLoadAsset(Global.ABTem);

        //注册常规主炮对象池
        Main.m_ObjectPool.RegisterSpawnPool(Global.WeaponPoolName, _weaponTem, (obj)=>
        {
            obj.transform.SetParent(_levelLogic.Entity.transform);
            obj.name = "Weapon";
        }, (obj) =>
        {
            obj.GetComponent<Weapon>().ReSet();
        }, 200);
        //注册超级武器对象池
        Main.m_ObjectPool.RegisterSpawnPool(Global.SuperWeaponPoolName, _superWeaponTem, (obj) =>
        {
            obj.transform.SetParent(_levelLogic.Entity.transform);
            obj.name = "SuperWeapon";
        }, (obj) =>
        {
            obj.GetComponent<SuperWeapon>().ReSet();
        }, 200);
        //设置主摄像机视角并固定视角（由于主摄像机视角自由控制已被关闭）
        Main.m_Controller.SetLookAngle(new Vector3(360, 0, 3.5f), false);
        //设置主摄像机视角一直追踪玩家
        Main.m_Controller.EnterKeepTrack(_player.transform);

        //抛出加载完成事件
        Main.m_Event.Throw<EventGameLoaded>();

        //抛出游戏开始事件
        Main.m_Event.Throw(Main.m_ReferencePool.Spawn<EventGameStart>().Fill(_player, ChooseLevel));

        //重置关卡计时
        GameTime = 0;
    }

    /// <summary>
    /// 清理游戏
    /// </summary>
    private IEnumerator ClearGame()
    {
        yield return null;

        //关闭游戏界面
        Main.m_UI.CloseUI<UIGame>();

        //销毁关卡实体
        Main.m_Entity.DestroyEntity(_levelLogic);
        _level = null;
        _levelLogic = null;
        
        //清理并移除常规主炮对象池
        Main.m_ObjectPool.Clear(Global.WeaponPoolName);
        Main.m_ObjectPool.UnRegisterSpawnPool(Global.WeaponPoolName);
        //清理并移除超级武器对象池
        Main.m_ObjectPool.Clear(Global.SuperWeaponPoolName);
        Main.m_ObjectPool.UnRegisterSpawnPool(Global.SuperWeaponPoolName);
        
        //主摄像机视角退出追踪模式
        Main.m_Controller.LeaveKeepTrack();
    }
}