using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Walkable;

public class PlayerController : MonoBehaviour
{
    public bool walking = false;
    [Space]
    [Header("キューブの情報")]
    [SerializeField, Tooltip("現在の位置にあるキューブ")]
    private Transform currentCube;
    [SerializeField, Tooltip("マウスクリックしたキューブ")]
    private Transform clickedCube;

    [Space]
    [Header("経路")]
    //プレイヤーが実際に移動する経路
    public List<Transform> finalPath = new List<Transform>();



    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーが踏んでいるキューブの設定
        RayCastDown();
    }

    // Update is called once per frame

    void Update()
    {
        //プレイヤーが踏んでいるキューブの設定
        RayCastDown();

        ////現在踏んでいるキューブが動く場合
        if (currentCube.GetComponent<Walkable>().movingGround)
        {

            //プレイヤーをその子に入れる
            transform.parent = currentCube.parent;
        }
        else
        {
            transform.parent = null;
        }

        //マウスクリックチェック
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            //レイイを発射!!
            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                //クリックした場所がPathの場合
                if (mouseHit.transform.GetComponent<Walkable>() != null)
                {
                    //クリックしたキューブの位置を設定
                    clickedCube = mouseHit.transform;
                    FindPath();
                }
            }
        }
    }

    /// <summary>
    ///経路探索
    /// </summary>
    private void FindPath()
    {
        //次に移動するキューブ
        List<Transform> nextCubes = new List<Transform>();
        //前のキューブ
        List<Transform> pastCubes = new List<Transform>();

        //現在のキューブに接続されたキューブの数だけループ
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
    /// 経路探索のための補助メソッド。
    /// </summary>
    /// <param name="nextCubes"></param>
    /// <param name="visitedCubes"></param>
    private void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes)
    {
        Transform current = nextCubes.First();
        nextCubes.Remove(current);

        //クリックしたキューブと現在のキューブが同じ場合
        //目標座標に到達した場合
        if (current == clickedCube)
        {
            return;
        }

        //現在のキューブの移動可能なキューブの数だけ繰り返す
        foreach (WalkPath path in current.GetComponent<Walkable>().possiblePaths)
        {
            //既に通過した道でなく、かつ道が接続されている場合
            if (!visitedCubes.Contains(path.target) && path.active)
            {
                //次の検索キューブに移動経路に追加」
                nextCubes.Add(path.target);
                //次に探索するキューブを移動経路に追加
                path.target.GetComponent<Walkable>().previousBlock = current;
            }
        }
        //訪れたキューブのリストに現在のキューブを追加
        visitedCubes.Add(current);

        //リストが1つでもある場合
        if (nextCubes.Any())
        {
            ExploreCube(nextCubes, visitedCubes);
        }
    }

    /// <summary>
    /// 経路の生成
    /// </summary>
    private void BuildPath()
    {
       
        Transform cube = clickedCube;

        //リックしたキューブが現在のキューブと同じでない限り」
        while (cube != currentCube)
        {
            
            //実際に移動する経路に挿入
            finalPath.Add(cube);
            //クリックしたキューブの前のキューブがNullの場合」
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
    ///プレイヤーが現在踏んでいるキューブを見つける関数
    /// </summary>
    public void RayCastDown()
    {
        //プレイヤーの中心座標を生成
        Vector3 rayPos = transform.position;
        rayPos.y += transform.localScale.y * 0.5f;

        ////レイを作成し、方向は下向き
        Ray playerRay = new Ray(rayPos, -transform.up);
        RaycastHit playerHit;

        //Ray playerRay = new Ray(transform.GetChild(0).position, -transform.up);
        //RaycastHit playerHit;

        //レイを発射!!
        if (Physics.Raycast(playerRay, out playerHit))
        {
            if (playerHit.transform.GetComponent<Walkable>() != null)
            {
                //足場を踏んでいる場合
                currentCube = playerHit.transform;
            }
        }
    }
}
