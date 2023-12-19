using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ���̃N���X�́A�L�����N�^�[���ړ��ł���ꏊ���`���܂��B
/// </summary>
public class Walkable : MonoBehaviour
{

    //���̃u���b�N����ړ��\�ȃp�X�̃��X�g
    public List<WalkPath> possiblePaths = new List<WalkPath>();

    [Space]
    //�p�X�T�����ɉߋ��ɒʂ����u���b�N���L�^����ϐ�
    public Transform previousBlock;

    [Space]

    [Header("Bool")]
    //���̃I�u�W�F�N�g���K�i���ǂ����������t���O
    public bool isStair = false;
    // ���̃I�u�W�F�N�g�������n�ʁi�ړ�����I�u�W�F�N�g�j���ǂ����������t���O
    public bool movingGround = false;
    public bool isButton;
    //�v���C���[�̌�����ς��邩�ǂ����������t���O
    public bool dontRotate;

    [Space]

    [Header("Offsets")]
    [SerializeField, Tooltip("��{�̃I�t�Z�b�g�l")]
    private float walkPointOffset = .5f;
    [SerializeField, Tooltip("�K�i�̃I�t�Z�b�g")]
    private float stairOffset = .4f;

    /// <summary>
    /// �L�����N�^�[�̕��s���v�Z���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    public Vector3 GetWalkPoint()
    {
        //�I�u�W�F�N�g���K�i�ł���΁A�K�i�̃I�t�Z�b�g���g�p���܂��B
        float stair = isStair ? stairOffset : 0;
        //���s�ɂ��č��W��Ԃ�
        return transform.position + transform.up * walkPointOffset - transform.up * stair;
    }

    // Unity�G�f�B�^�ŃM�Y����`�悷�邽�߂̃��\�b�h�i�f�o�b�O�p�j
    private void OnDrawGizmos()
    {
        // �L���[�u�̏�ɖړI�n���������߂̃M�Y����`�悵�܂��B
        Gizmos.color = Color.white;
        Gizmos.DrawCube(GetWalkPoint(), new Vector3(0.1f, 0.1f, 0.1f));

        // �ړ��\�ȃp�X����ŕ`�悵�܂��B
        for (int i = 0; i < possiblePaths.Count; i++)
        {
            if (possiblePaths[i].active)
            {
                // �ړ��\�ȃp�X�͗ΐF�ŕ\�����܂��B
                Gizmos.color = Color.green;
                Gizmos.DrawLine(GetWalkPoint(), possiblePaths[i].target.GetComponent<Walkable>().GetWalkPoint());
            }
        }
    }
}
// ���s�\�ȃp�X���`���邽�߂̃N���X
[System.Serializable]
public class WalkPath
{
    // �ڕW�n�_
    public Transform target;
    // ���̃p�X���A�N�e�B�u���ǂ���
    public bool active = true;
}