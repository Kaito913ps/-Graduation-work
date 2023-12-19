using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  このクラスは、キャラクターが移動できる場所を定義します。
/// </summary>
public class Walkable : MonoBehaviour
{

    //このブロックから移動可能なパスのリスト
    public List<WalkPath> possiblePaths = new List<WalkPath>();

    [Space]
    //パス探索時に過去に通ったブロックを記録する変数
    public Transform previousBlock;

    [Space]

    [Header("Bool")]
    //このオブジェクトが階段かどうかを示すフラグ
    public bool isStair = false;
    // このオブジェクトが動く地面（移動するオブジェクト）かどうかを示すフラグ
    public bool movingGround = false;
    public bool isButton;
    //プレイヤーの向きを変えるかどうかを示すフラグ
    public bool dontRotate;

    [Space]

    [Header("Offsets")]
    [SerializeField, Tooltip("基本のオフセット値")]
    private float walkPointOffset = .5f;
    [SerializeField, Tooltip("階段のオフセット")]
    private float stairOffset = .4f;

    /// <summary>
    /// キャラクターの歩行を計算するメソッド
    /// </summary>
    /// <returns></returns>
    public Vector3 GetWalkPoint()
    {
        //オブジェクトが階段であれば、階段のオフセットを使用します。
        float stair = isStair ? stairOffset : 0;
        //歩行について座標を返す
        return transform.position + transform.up * walkPointOffset - transform.up * stair;
    }

    // Unityエディタでギズモを描画するためのメソッド（デバッグ用）
    private void OnDrawGizmos()
    {
        // キューブの上に目的地を示すためのギズモを描画します。
        Gizmos.color = Color.white;
        Gizmos.DrawCube(GetWalkPoint(), new Vector3(0.1f, 0.1f, 0.1f));

        // 移動可能なパスを線で描画します。
        for (int i = 0; i < possiblePaths.Count; i++)
        {
            if (possiblePaths[i].active)
            {
                // 移動可能なパスは緑色で表示します。
                Gizmos.color = Color.green;
                Gizmos.DrawLine(GetWalkPoint(), possiblePaths[i].target.GetComponent<Walkable>().GetWalkPoint());
            }
        }
    }
}
// 歩行可能なパスを定義するためのクラス
[System.Serializable]
public class WalkPath
{
    // 目標地点
    public Transform target;
    // このパスがアクティブかどうか
    public bool active = true;
}