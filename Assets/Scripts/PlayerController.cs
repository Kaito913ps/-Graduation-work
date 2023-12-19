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
    [SerializeField]
    private float moveSpeed;

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
        if (Input.GetMouseButtonDown(0) && !walking)
        {
            HandleMouseClick();
        }
    }

    private void HandleMouseClick()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out RaycastHit mouseHit))
        {
            if (mouseHit.transform.GetComponent<Walkable>() != null)
            {
                clickedCube = mouseHit.transform;
                FindPath();
            }
        }
    }

    /// <summary>
    ///経路探索
    /// </summary>
    private void FindPath()
    {
        finalPath.Clear();
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
        // 再帰的に経路を探索
        ExploreCube(nextCubes, pastCubes);

        //探索が完了した後,経路が有効かどうかを確認
        if(IsPathValid(clickedCube))
        {
            //有効な経路がある場合,
            BuildPath();
            Debug.Log("FindPath: 有効な経路が見つかりました");
        }
        else
        {
            // 有効な経路がない場合はクリア
            finalPath.Clear();
            Debug.Log("FindPath: 有効な経路が見つかりませんでした");
        }

    }


    private bool IsPathValid(Transform destination)
    {
        // 目的地への経路が存在するかどうかをチェックするロジック
        // ここでは、単純な例として、目的地が直接的にアクセス可能かどうかをチェックします。
        foreach (WalkPath path in currentCube.GetComponent<Walkable>().possiblePaths)
        {
            if (path.active && path.target == destination)
            {
                Debug.Log("直接の経路が存在: " + destination.name);
                return true; // 目的地へ直接移動できる経路がある
            }
        }
        // 間接的な経路の存在をログに出力
        bool pathExists = destination.GetComponent<Walkable>().previousBlock != null;
        Debug.Log("目的地への経路が " + (pathExists ? "存在します: " : "存在しません: ") + destination.name);
        return pathExists;
    }



    /// <summary>
    /// 経路探索のための補助メソッド。
    /// </summary>
    /// <param name="nextCubes"></param>
    /// <param name="visitedCubes"></param>
    private void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes)
    {
        while (nextCubes.Count > 0)
        {
            Transform current = nextCubes[0];
            nextCubes.RemoveAt(0);

            // すでに訪れたキューブはスキップ
            if (visitedCubes.Contains(current))
            {
                continue;
            }

            // 現在のキューブがクリックしたキューブと同じなら、探索終了
            if (current == clickedCube)
            {
                return;
            }

            foreach (WalkPath path in current.GetComponent<Walkable>().possiblePaths)
            {
                if (path.active && !visitedCubes.Contains(path.target))
                {
                    nextCubes.Add(path.target);
                    path.target.GetComponent<Walkable>().previousBlock = current;
                }
            }
            visitedCubes.Add(current);
        }
    }



    /// <summary>
    /// 経路の生成
    /// </summary>
    private void BuildPath()
    {
        Transform cube = clickedCube;

        while (cube != null && cube != currentCube)
        {
            // 経路リストの先頭に追加
            finalPath.Insert(0, cube);

            cube = cube.GetComponent<Walkable>().previousBlock;
        }

        // 経路を逆順にして正しい順序に
        finalPath.Reverse();

        FollowPath();
    }



    private void FollowPath()
    {
        if (finalPath.Count == 0 || walking)
        {
            return;
        }

        walking = true;
        Sequence sequence = DOTween.Sequence();

        foreach (Transform waypoint in finalPath)
        {
            sequence.Append(transform.DOMove(waypoint.position, moveSpeed).SetEase(Ease.Linear))
                    .AppendCallback(() => Debug.Log("到達: " + waypoint.name)); // 各移動後にログ出力
        }

        sequence.AppendCallback(() =>
        {
            walking = false;
            OnReachedDestination();
        });
    }

    private void ClearPath()
    {
        foreach (Transform t in finalPath)
        {
            t.GetComponent<Walkable>().previousBlock = null;
        }
        finalPath.Clear();
    }


    // 移動終了時の処理
    private void OnReachedDestination()
    {
        Debug.Log("目的地に到達しました。最終的なキューブ: " + currentCube.name);

        // その他の必要な処理...
    }



    // 移動終了時の処理



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
               // Debug.Log("RayCastDown: 現在のキューブ " + currentCube.name);
            }
        }
    }
}