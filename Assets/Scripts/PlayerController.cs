using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Walkable;

public class PlayerController : MonoBehaviour
{
    public bool walking = false;
    [Space]
    [Header("�L���[�u�̏��")]
    [SerializeField, Tooltip("���݂̈ʒu�ɂ���L���[�u")]
    private Transform currentCube;
    [SerializeField, Tooltip("�}�E�X�N���b�N�����L���[�u")]
    private Transform clickedCube;

    [Space]
    [Header("�o�H")]
    //�v���C���[�����ۂɈړ�����o�H
    public List<Transform> finalPath = new List<Transform>();



    // Start is called before the first frame update
    void Start()
    {
        //�v���C���[������ł���L���[�u�̐ݒ�
        RayCastDown();
    }

    // Update is called once per frame

    void Update()
    {
        //�v���C���[������ł���L���[�u�̐ݒ�
        RayCastDown();

        ////���ݓ���ł���L���[�u�������ꍇ
        if (currentCube.GetComponent<Walkable>().movingGround)
        {

            //�v���C���[�����̎q�ɓ����
            transform.parent = currentCube.parent;
        }
        else
        {
            transform.parent = null;
        }

        //�}�E�X�N���b�N�`�F�b�N
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            //���C�C�𔭎�!!
            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                //�N���b�N�����ꏊ��Path�̏ꍇ
                if (mouseHit.transform.GetComponent<Walkable>() != null)
                {
                    //�N���b�N�����L���[�u�̈ʒu��ݒ�
                    clickedCube = mouseHit.transform;
                    FindPath();
                }
            }
        }
    }

    /// <summary>
    ///�o�H�T��
    /// </summary>
    private void FindPath()
    {
        //���Ɉړ�����L���[�u
        List<Transform> nextCubes = new List<Transform>();
        //�O�̃L���[�u
        List<Transform> pastCubes = new List<Transform>();

        //���݂̃L���[�u�ɐڑ����ꂽ�L���[�u�̐��������[�v
        foreach (WalkPath path in currentCube.GetComponent<Walkable>().possiblePaths)
        {
            if (path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = currentCube;
            }
        }

        pastCubes.Add(currentCube);

        ExploreCube(nextCubes, pastCubes);
        BuildPath();
    }

    /// <summary>
    /// �o�H�T���̂��߂̕⏕���\�b�h�B
    /// </summary>
    /// <param name="nextCubes"></param>
    /// <param name="visitedCubes"></param>
    private void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes)
    {
        Transform current = nextCubes.First();
        nextCubes.Remove(current);

        //�N���b�N�����L���[�u�ƌ��݂̃L���[�u�������ꍇ
        //�ڕW���W�ɓ��B�����ꍇ
        if (current == clickedCube)
        {
            return;
        }

        //���݂̃L���[�u�̈ړ��\�ȃL���[�u�̐������J��Ԃ�
        foreach (WalkPath path in current.GetComponent<Walkable>().possiblePaths)
        {
            //���ɒʉ߂������łȂ��A�������ڑ�����Ă���ꍇ
            if (!visitedCubes.Contains(path.target) && path.active)
            {
                //���̌����L���[�u�Ɉړ��o�H�ɒǉ��v
                nextCubes.Add(path.target);
                //���ɒT������L���[�u���ړ��o�H�ɒǉ�
                path.target.GetComponent<Walkable>().previousBlock = current;
            }
        }
        //�K�ꂽ�L���[�u�̃��X�g�Ɍ��݂̃L���[�u��ǉ�
        visitedCubes.Add(current);

        //���X�g��1�ł�����ꍇ
        if (nextCubes.Any())
        {
            ExploreCube(nextCubes, visitedCubes);
        }
    }

    /// <summary>
    /// �o�H�̐���
    /// </summary>
    private void BuildPath()
    {
       
        Transform cube = clickedCube;

        //���b�N�����L���[�u�����݂̃L���[�u�Ɠ����łȂ�����v
        while (cube != currentCube)
        {
            
            //���ۂɈړ�����o�H�ɑ}��
            finalPath.Add(cube);
            //�N���b�N�����L���[�u�̑O�̃L���[�u��Null�̏ꍇ�v
            if (cube.GetComponent<Walkable>().previousBlock != null)
            {
               
                cube = cube.GetComponent<Walkable>().previousBlock;
            }
            else
            {
                return;
            }
        }

        FollowPath();
    }

    private void FollowPath()
    {

        Sequence s = DOTween.Sequence();

        walking = true;

        for (int i = finalPath.Count - 1; i > 0; i--)
        {
            float time = finalPath[i].GetComponent<Walkable>().isStair ? 1.5f : 1;

            s.Append(transform.DOMove(finalPath[i].GetComponent<Walkable>().GetWalkPoint(), .2f * time).SetEase(Ease.Linear));
        }
        s.Append(transform.DOMove(clickedCube.GetComponent<Walkable>().GetWalkPoint(), .2f).SetEase(Ease.Linear));
        s.AppendCallback(() => Clear());
    }

    void Clear()
    {
        foreach (Transform t in finalPath)
        {
            t.GetComponent<Walkable>().previousBlock = null;
        }
        finalPath.Clear();
        walking = false;
    }

    /// <summary>
    ///�v���C���[�����ݓ���ł���L���[�u��������֐�
    /// </summary>
    public void RayCastDown()
    {
        //�v���C���[�̒��S���W�𐶐�
        Vector3 rayPos = transform.position;
        rayPos.y += transform.localScale.y * 0.5f;

        ////���C���쐬���A�����͉�����
        Ray playerRay = new Ray(rayPos, -transform.up);
        RaycastHit playerHit;

        //Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        //RaycastHit playerHit;

        //���C�𔭎�!!
        if (Physics.Raycast(playerRay, out playerHit))
        {
            if (playerHit.transform.GetComponent<Walkable>() != null)
            {
                //����𓥂�ł���ꍇ
                currentCube = playerHit.transform;
            }
        }
    }
}
