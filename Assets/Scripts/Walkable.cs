using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkable : MonoBehaviour
{
    //移動できるブロック座標
    public List<WalkPath> possiblePaths = new List<WalkPath>();

    [Space]
    //道を探すときに通り過ぎた道
    public Transform previousBlock;

    [Space]
    [Header("bool")]
    [SerializeField,Tooltip("階段かどうか")]
    private bool isStair = false;
    //動くオブジェクトか
    public bool movingGround = false;
    [SerializeField, Tooltip("方向変更の有無")]
    private bool dontRotate;

    [Space]

    [Header("Offsets")]
    public float walkPointOffset =0.5f;
    public float stairOffset = 0.4f;


    /// <summary>
    /// 歩く座標を求める
    /// </summary>
    public Vector3 GetWalkPoint()
    {
        //階段かどうか？
        float stair = isStair ? stairOffset : 0;
        //座標を返す
        return transform.position + transform.up * walkPointOffset - transform.up * stair;
    }

    /// <summary>
    /// キューブの上にターゲットを描く
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        float stair = isStair ? 0.4f : 0;
        Gizmos.DrawSphere(GetWalkPoint(), 0.1f);

        //if (possiblePaths == null)
        //    return;

        //foreach (WalkPath p in possiblePaths)
        //{
        //    if (p.target == null)
        //        return;
        //    Gizmos.color = p.active ? Color.black : Color.clear;
        //    Gizmos.DrawLine(GetWalkPoint(), p.target.GetComponent<Walkable>().GetWalkPoint());
        //}
    }

    [System.Serializable]
    public class WalkPath
    {
        public Transform target;
        public bool active = true;
    }
}
