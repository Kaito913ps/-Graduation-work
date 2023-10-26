using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Space]
    [Header("�L���[�u�̏��")]
    [SerializeField, Tooltip("���݈ʒu����L���[�u")]
    private Transform currentCube;
    [SerializeField, Tooltip("�}�E�X�N���b�N�����L���[�u")]
    private Transform clickedCube;

    [Space]
    [Header("�p�X�t�@�C���f�B���O")]
    //�v���C���[�����ۂɈړ�����p�X
    public List<Transform> finalPath = new List<Transform>();



    // Start is called before the first frame update
    void Start()
    {
        RayCastDown();
    }

    // Update is called once per frame
    void Update()
    {
        RayCastDown();

        //���ݓ���ł���L���[�u�������ꍇ
        if(currentCube.GetComponent<Walkable>().movingGround)
        {
            //�v���C���[�����̎q�ɓ����
            transform.parent = currentCube.parent;
        }
        else
        {
            transform .parent = null;
        }

        //�}�E�X�N���b�N�`�F�b�N
        if(Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            //���C�ł��グ��
            if(Physics.Raycast(mouseRay, out mouseHit))
            {
                //�N���b�N�����ꏊ��Path�̏ꍇ
                if(mouseHit.transform.GetComponent<Walkable>() != null)
                {
                    //�N���b�N�����L���[�u�̈ʒu��ݒ�
                }
            }
        }
    }

    /// <summary>
    /// ����T��
    /// </summary>
    private void FindPath()
    {
        //�����ړ�����L���[�u
        List<Transform> nextCubes = new List<Transform>();
        //�O�̃L���[�u
        List<Transform> pastCubes = new List<Transform> ();

        

        pastCubes.Add(currentCube);

    }

    private void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes)
    {
        Transform current = nextCubes.First();
        nextCubes.Remove(current);

        //�N���b�N�����L���[�u�ƌ��݂̃L���[�u�������ꍇ
        //�ڕW���W�ɓ�����������
        if(current == clickedCube)
        {
            return;
        }
        //�K�₵���L���[�u���X�g�Ɍ��݂̃L���[�u��ǉ�����
        visitedCubes.Add(current);
    }

    /// <summary>
    /// �p�X�̍쐬
    /// </summary>
    private void BuildPath()
    {
        Transform cube = clickedCube;

        //�N���b�N�����L���[�u�����݂̃L���[�u�Ɠ������Ȃ��܂�
        while(cube != currentCube)
        {
            //���ۂɈړ�����p�X�ɑ}��
            finalPath.Add(cube);
            //�N���b�N�����L���[�u�̑O�̃L���[�u��Null�̎�
            if(cube.GetComponent<Walkable>().previousBlock != null)
            {
                cube = cube.GetComponent<Walkable>().previousBlock;
            }
            else
            {
                return;
            }
        }
    }

    private void FollowPath()
    {

    }

    /// <summary>
    /// ���݃v���C���[������ł���L���[�u��T���֐�
    /// </summary>
    public void RayCastDown()
    {

    }
}
