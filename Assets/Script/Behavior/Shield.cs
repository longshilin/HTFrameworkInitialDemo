using UnityEngine;

/// <summary>
/// 护盾行为控制
/// </summary>
public class Shield : MonoBehaviour
{
    /// <summary>
    /// 最大护盾值
    /// </summary>
    public int MaxValue { get; set; }
    /// <summary>
    /// 护盾值
    /// </summary>
    public int Value { get; set; }

    private DataSetShield _dataSet;
    private int _shieldIndex = 0;
    private float _shieldTimer = 0;
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// 生成护盾
    /// </summary>
    public void Generate(DataSetShield shield)
    {
        MaxValue = Value = shield.ShieldValue;
        _dataSet = shield;
        _shieldIndex = 0;
        _shieldTimer = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _dataSet.Picture[_shieldIndex];
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 护盾破灭
    /// </summary>
    public void Destroy()
    {
        Value = 0;
        _dataSet = null;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_shieldTimer > 0)
        {
            _shieldTimer -= Time.deltaTime;
        }
        else
        {
            _shieldTimer = Global.AnimationSpeed;
            _shieldIndex += 1;
            if(_shieldIndex >= _dataSet.Picture.Length) _shieldIndex = 0;
            _spriteRenderer.sprite = _dataSet.Picture[_shieldIndex];
        }
    }
}
