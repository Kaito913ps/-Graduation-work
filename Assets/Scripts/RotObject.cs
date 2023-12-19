using UnityEngine;

public class RotObject : MonoBehaviour
{
    public Transform objectToRotate;
    public Vector3 rotationAxis = Vector3.up;
    public float targetRotationAngle = 90f;
    public float rotationSpeed = 100f;

    private float currentRotationAngle = 0f;
    private Camera mainCamera;
    private bool isRotating = false;

    //ここで関連するWalkPathのリストを定義します
    public WalkPath[] relatedWalkPaths;

    void Start()
    {
        mainCamera = Camera.main; // メインカメラの参照を取得
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == this.transform || IsChildTransform(hit.transform))
                {
                    StartRotating();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopRotating();
        }

        if (isRotating)
        {
            RotateObjectTowardsTargetAngle();
        }
    }

    private void StartRotating()
    {
        if (CanRotate())
        {
            isRotating = true;
        }
    }

    private void StopRotating()
    {
        isRotating = false;
        // ハンドルの回転が停止した後、WalkPathを更新
    }

    private bool IsChildTransform(Transform transform)
    {
        return transform.IsChildOf(this.transform);
    }

    private void RotateObjectTowardsTargetAngle()
    {
        if (!CanRotate())
        {
            return; // 回転できない場合は処理を中断
        }

        if (Mathf.Abs(currentRotationAngle - targetRotationAngle) > 0.01f)
        {
            float step = rotationSpeed * Time.deltaTime;
            float angleToRotate = Mathf.MoveTowards(currentRotationAngle, targetRotationAngle, step);
            objectToRotate.Rotate(rotationAxis, angleToRotate - currentRotationAngle);
            currentRotationAngle = angleToRotate;
        }
    }

    private void UpdateWalkablePaths()
    {
        foreach(var path in relatedWalkPaths)
        {
            //ハンドルの現在の角度に応じて、パスのアクティブ状態を設定
            if(Mathf.Abs(currentRotationAngle - targetRotationAngle) < 5.0f)
            {
                path.active = true;
            }
            else
            {
                path.active = false;
            }
        }
        
    }

    //private void UpdatePathActiveStatus()
    //{
    //    // ここで、回転したハンドルの角度に基づいて、
    //    // 各WalkPathのactiveプロパティを更新します。
    //    // 例えば、特定の角度で特定のパスがアクティブになるようにします。
    //    foreach (var pathCondition in pathCubes)
    //    {
    //        foreach (var singlePath in pathCondition.path)
    //        {
    //            singlePath.block.possiblePaths[singlePath.index].active =
    //                Mathf.Abs((rotateObj.eulerAngles - pathCondition.angle).magnitude) < someThreshold;
    //        }
    //    }
    //}

    // 新しいメソッド: 回転可能かどうかをチェック
    private bool CanRotate()
    {
        // ここに回転可能かどうかのロジックを実装
        // 例: プレイヤーがオブジェクト上にいない、他のオブジェクトと衝突していない等
        return true; // この条件はプロジェクトの要件に応じて変更
    }
}
