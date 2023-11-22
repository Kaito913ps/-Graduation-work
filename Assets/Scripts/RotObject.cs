using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// オブジェクトの回転を管理するクラス
public class RotObject : MonoBehaviour
{
    // 回転軸を定義する列挙型
    public enum AxisOfRotate
    {
        X, Y, Z
    }

    [Header("Rotate Object")]
    [SerializeField,Tooltip("回転するオブジェクト")]
    private Transform rotateObj;
    // 各パスの状態を管理するリスト
    public List<PathCondition> pathCubes = new List<PathCondition>();

    [SerializeField, Tooltip("回転する軸")]
    private AxisOfRotate axisOfRotate;

    [SerializeField, Tooltip("回転速度")]
    private float rotSpeed;

    // 回転中かどうかのフラグ
    private bool isRotate;
    // 前回の角度を記録
    private float pastAngle;
    // サウンドナンバー
    private int soundNum;

    void Start()
    {
        isRotate = false;
        soundNum = 1;
    }

    void Update()
    {
        // マウスボタンのクリックを検出
        if (Input.GetMouseButtonDown(0) /*&& GameManager.instance.Ready*/)
        {
            // マウス位置に応じたレイキャストを作成
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;

            // レイキャストを実行
            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                // 当たったオブジェクトがレバーかどうかをチェック
                if (mouseHit.transform == transform)
                {
                    isRotate = true;

                    pastAngle = getAngle();

                    soundNum = 0;
                }
                else
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (mouseHit.transform == transform.GetChild(i))
                        {
                            isRotate = true;

                            pastAngle = getAngle();

                            soundNum = 0;

                            break;
                        }
                    }
                }
            }
        }

        // 回転中の処理です。
        if (isRotate)
        {
            // マウスの移動量に基づいて回転量を計算
            Vector2 rot = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            rot *= rotSpeed;

            // レバーの回転
            transform.Rotate(((axisOfRotate == AxisOfRotate.X) ? (rot.x) : (0)),
                    ((axisOfRotate == AxisOfRotate.Y) ? (rot.x) : (0)),
                    ((axisOfRotate == AxisOfRotate.Z) ? (rot.x) : (0)));

            // オブジェクトの回転
            rotateObj.Rotate(((axisOfRotate == AxisOfRotate.X) ? (rot.x) : (0)),
                ((axisOfRotate == AxisOfRotate.Y) ? (rot.x) : (0)),
                ((axisOfRotate == AxisOfRotate.Z) ? (rot.x) : (0)));

            // 回転角度に応じてサウンドを再生
            if (Mathf.Abs(getAngle() - pastAngle) >= 30.0f)
            {
                pastAngle = getAngle();

               // SoundManager.instance.play("RotateSound_" + soundNum.ToString());

                soundNum = (soundNum > 5) ? (0) : (soundNum + 1);
            }

            // 回転角度をチェックしてパスの状態を更新
            for (int i = 0; i < pathCubes.Count; i++)
            {
                for (int j = 0; j < pathCubes[i].path.Count; j++)
                {
                    pathCubes[i].path[j].block.possiblePaths[pathCubes[i].path[j].index - 1].active =
                        transform.eulerAngles.Equals(pathCubes[i].angle);
                }
            }

            // マウスボタンを離すと回転を停止
            if (Input.GetMouseButtonUp(0))
            {
                isRotate = false;
                // 角度を自動調整
                AutoAdjustAngle();
            }
        }
    }
    /// <summary>
    /// 角度を自動で調整するメソッド
    /// </summary>
    private void AutoAdjustAngle()
    {
        float currentAngle = getAngle();
        float targetAngle;

        // 角度を90度単位で自動調整
        for (int i = 270; i >= 0; i -= 90)
        {
            if (currentAngle > i)
            {
                targetAngle = (currentAngle > i + 45) ? i + 90 : i;
                StartCoroutine(Rotate(currentAngle, targetAngle, true));
                break;
            }
        }
    }

    // 回転コルーチン
    IEnumerator Rotate(float startAngle, float finalAngle, bool outBack)
    {
        float timing = 0;

        // 弾む角度
        float firstAngle = (finalAngle > startAngle) ? (finalAngle + 12.5f) : (finalAngle - 12.5f);
        // 次の角度
        float nextAngle = (outBack) ? (firstAngle) : (finalAngle);

        int soundNum = 1;

        // 回転
        while (timing < 1.0f)
        {
            float angle = Mathf.Lerp(startAngle, nextAngle, timing);

            setAngle(angle);

            timing += Time.deltaTime * rotSpeed;

            if (timing >= 1.0f)
            {
                setAngle(nextAngle);

                //SoundManager.instance.play("RotateSound_" + soundNum.ToString(), 0.5f);
                soundNum++;

                // 一度跳ねた場合
                if (nextAngle != finalAngle)
                {
                    startAngle = nextAngle;
                    nextAngle = finalAngle;
                    timing = 0;
                }
            }

            yield return null;
        }

        // 360度を超える場合
        setAngle((finalAngle >= 360) ? (finalAngle - 360) : (finalAngle));

        // 角度を確認した後、キューブの経路を設定
        for (int i = 0; i < pathCubes.Count; i++)
        {
            for (int j = 0; j < pathCubes[i].path.Count; j++)
            {
                pathCubes[i].path[j].block.possiblePaths[pathCubes[i].path[j].index - 1].active =
                    transform.eulerAngles.Equals(pathCubes[i].angle);
            }
        }

        yield return null;
    }

    public void autoRotate(float targetAngle, bool outBack)
    {
        StartCoroutine(Rotate(getAngle(), targetAngle, outBack));
    }

    /// <summary>
    ///  指定された角度にオブジェクトを設定するメソッド
    /// </summary>
    /// <param name="angle"></param>
    private void setAngle(float angle)
    {
        transform.rotation = Quaternion.Euler(new Vector3(
            (axisOfRotate == AxisOfRotate.X) ? angle : 0,
            (axisOfRotate == AxisOfRotate.Y) ? angle : 0,
            (axisOfRotate == AxisOfRotate.Z) ? angle : 0));

        rotateObj.rotation = Quaternion.Euler(new Vector3(
            (axisOfRotate == AxisOfRotate.X) ? angle : 0,
            (axisOfRotate == AxisOfRotate.Y) ? angle : 0,
            (axisOfRotate == AxisOfRotate.Z) ? angle : 0));
    }

    /// <summary>
    /// 現在の角度を取得するメソッド
    /// </summary>
    /// <returns></returns>
    private float getAngle()
    {
        float ret = 0;

        switch (axisOfRotate)
        {
            case AxisOfRotate.X: ret = transform.eulerAngles.x; break;
            case AxisOfRotate.Y: ret = transform.eulerAngles.y; break;
            case AxisOfRotate.Z: ret = transform.eulerAngles.z; break;
        }

        return ret;
    }
}

// 回転することで影響を受けるパスの条件を定義するクラス。
[System.Serializable]
public class PathCondition
{
    // この角度のときにパスがアクティブになる
    public Vector3 angle;
    // このパスの条件が適用されるシングルパスのリスト
    public List<SinglePath> path;
}

// 単一のパスの情報を持つクラス
[System.Serializable]
public class SinglePath
{
    // このパスの情報を持つWalkableオブジェクト
    public Walkable block;
    // このパスのインデックス
    public int index;
}