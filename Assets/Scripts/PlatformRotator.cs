using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotator : MonoBehaviour
{
   public void Rotate(Vector3 axis, float speed)
    {
        //プラットフォームの回転処理
        this.transform.Rotate(speed * Time.deltaTime * axis);
    }
}
