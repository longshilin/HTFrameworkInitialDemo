using HT.Framework;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 选择界面关卡选项
/// </summary>
public class UILevelItem : MonoBehaviour
{
    /// <summary>
    /// 关卡序号
    /// </summary>
    public int Level;

    /// <summary>
    /// 应用关卡数据
    /// </summary>
    /// <param name="score">通关得分</param>
    /// <param name="number">通关次数</param>
    /// <param name="isEnable">是否激活</param>
    public void ApplyData(string score, string number, bool isEnable)
    {
        transform.GetComponentByChild<Text>("Score/Value").text = score;
        transform.GetComponentByChild<Text>("Number/Value").text = number;
        transform.GetComponent<Toggle>().interactable = isEnable;
    }
}
