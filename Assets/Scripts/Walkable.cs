using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkable : MonoBehaviour
{
    //�ړ��ł���u���b�N���W
    public List<WalkPath> possiblePaths = new List<WalkPath>();

    [Space]
    //����T���Ƃ��ɒʂ�߂�����
    public Transform previousBlock;

    [Space]
    [Header("bool")]
    [SerializeField,Tooltip("�K�i���ǂ���")]
    private bool isStair = false;
    //�����I�u�W�F�N�g��
    public bool movingGround = false;
    [SerializeField, Tooltip("�����ύX�̗L��")]
    private bool dontRotate;

    [Space]

    [Header("Offsets")]
    public float walkPointOffset =0.5f;
    public float stairOffset = 0.4f;


    /// <summary>
    /// �������W�����߂�
    /// </summary>
    public Vector3 GetWalkPoint()
    {
        //�K�i���ǂ����H
        float stair = isStair ? stairOffset : 0;
        //���W��Ԃ�
        return transform.position + transform.up * walkPointOffset - transform.up * stair;
    }

    /// <summary>
    /// �L���[�u�̏�Ƀ^�[�Q�b�g��`��
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
