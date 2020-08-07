using HT.Framework;
using UnityEngine;

/// <summary>
/// 常规主炮行为控制
/// </summary>
public class Weapon : MonoBehaviour
{
    private DataSetWeapon _dataSet;
    private bool _isFire = false;
    private bool _isExplode = false;
    private int _explodeIndex = 0;
    private float _explodeTimer = 0;
    private Vector3 _dir;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private bool _isPlaySound;

    /// <summary>
    /// 发射
    /// </summary>
    public void Fire(FSM tank, DataSetWeapon weapon, float rotate, bool isPlaySound)
    {
        _dataSet = weapon;
        _isFire = true;
        _isExplode = false;
        _dir = tank.transform.up;
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_boxCollider2D == null) _boxCollider2D = GetComponent<BoxCollider2D>();
        _isPlaySound = isPlaySound;

        _spriteRenderer.sprite = _dataSet.Picture;
        _boxCollider2D.size = _dataSet.Picture.rect.size * 0.01f;
        gameObject.layer = tank.CurrentData.Cast<TankData>().Side == TankSide.Player ? LayerMask.NameToLayer("PlayerWeapon") : LayerMask.NameToLayer("EnemyWeapon");
        transform.position = tank.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, rotate + 90);
        transform.localScale = Vector3.one;
        gameObject.SetActive(true);

        if (_isPlaySound) Main.m_Audio.PlayMultipleSound(_dataSet.ShootSound);
    }

    /// <summary>
    /// 还原
    /// </summary>
    public void ReSet()
    {
        _dataSet = null;
        _isFire = false;
        _isExplode = false;
        _boxCollider2D.enabled = true;
    }

    private void Update()
    {
        if (_isFire)
        {
            transform.position += _dir * _dataSet.Speed * Time.deltaTime;
        }
        else if (_isExplode)
        {
            if (_explodeTimer > 0)
            {
                _explodeTimer -= Time.deltaTime;
            }
            else
            {
                _explodeIndex += 1;
                _explodeTimer = Global.AnimationSpeed;
                if (_explodeIndex < _dataSet.ExplodePicture.Length)
                {
                    _spriteRenderer.sprite = _dataSet.ExplodePicture[_explodeIndex];
                }
                else
                {
                    _isExplode = false;
                    Main.m_ObjectPool.Despawn(Global.WeaponPoolName, gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isFire)
        {
            _isFire = false;
            _isExplode = true;
            _explodeIndex = 0;
            _explodeTimer = Global.AnimationSpeed;
            _spriteRenderer.sprite = _dataSet.ExplodePicture[_explodeIndex];
            _boxCollider2D.enabled = false;

            FSM fsm = collision.gameObject.GetComponent<FSM>();
            if (fsm)
            {
                fsm.CurrentData.Cast<TankData>().Hit(_dataSet.Attack);
            }

            if (_isPlaySound) Main.m_Audio.PlayMultipleSound(_dataSet.ExplodeSound);
        }
    }
}