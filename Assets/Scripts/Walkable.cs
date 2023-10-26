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
    [SerializeField,Tooltip("�����I�u�W�F�N�g��")]
    private bool movingGround = false;
    [SerializeField, Tooltip("�����ύX�̗L��")]
    private bool dontRotate;

    [Space]

    [Header("Offsets")]
    private float walkPointOffset = 5f;
    private float stairOffset = .4f;

    /// <summary>
    /// �������W�����߂�
    /// </summary>
    public Vector3 GetWalkPoint()
    {
        //�K�i���ǂ����H
        float stair = (isStair) ? (stairOffset) : 0f;

        //���W��Ԃ�
        return transform.position + transform.up * 0.5f - transform.up * stair;
    }

    /// <summary>
    /// �L���[�u�̏�Ƀ^�[�Q�b�g��`��
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        float stair = isStair ? .4f : 0;
        Gizmos.DrawSphere(GetWalkPoint(), .1f);

        if (possiblePaths == null)
            return;

        foreach (WalkPath p in possiblePaths)
        {
            if (p.target == null)
                return;
            Gizmos.color = p.active ? Color.black : Color.clear;
            Gizmos.DrawLine(GetWalkPoint(), p.target.GetComponent<Walkable>().GetWalkPoint());
        }
    }

    [System.Serializable]
    public class WalkPath
    {
        public Transform target;
        public bool active = true;
    }
}
