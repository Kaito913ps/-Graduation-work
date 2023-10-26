using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Space]
    [Header("キューブの情報")]
    [SerializeField, Tooltip("現在位置するキューブ")]
    private Transform currentCube;
    [SerializeField, Tooltip("マウスクリックしたキューブ")]
    private Transform clickedCube;

    [Space]
    [Header("パスファインディング")]
    //プレイヤーが実際に移動するパス
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

        //現在踏んでいるキューブが動く場合
        if(currentCube.GetComponent<Walkable>().movingGround)
        {
            //プレイヤーをその子に入れる
            transform.parent = currentCube.parent;
        }
        else
        {
            transform .parent = null;
        }

        //マウスクリックチェック
        if(Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            //レイ打ち上げる
            if(Physics.Raycast(mouseRay, out mouseHit))
            {
                //クリックした場所がPathの場合
                if(mouseHit.transform.GetComponent<Walkable>() != null)
                {
                    //クリックしたキューブの位置を設定
                }
            }
        }
    }

    /// <summary>
    /// 道を探す
    /// </summary>
    private void FindPath()
    {
        //次を移動するキューブ
        List<Transform> nextCubes = new List<Transform>();
        //前のキューブ
        List<Transform> pastCubes = new List<Transform> ();

        

        pastCubes.Add(currentCube);

    }

    private void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes)
    {
        Transform current = nextCubes.First();
        nextCubes.Remove(current);

        //クリックしたキューブと現在のキューブが同じ場合
        //目標座標に到着したもの
        if(current == clickedCube)
        {
            return;
        }
        //訪問したキューブリストに現在のキューブを追加する
        visitedCubes.Add(current);
    }

    /// <summary>
    /// パスの作成
    /// </summary>
    private void BuildPath()
    {
        Transform cube = clickedCube;

        //クリックしたキューブが現在のキューブと等しくないまで
        while(cube != currentCube)
        {
            //実際に移動するパスに挿入
            finalPath.Add(cube);
            //クリックしたキューブの前のキューブがNullの時
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
    /// 現在プレイヤーが踏んでいるキューブを探す関数
    /// </summary>
    public void RayCastDown()
    {

    }
}
