using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotator : MonoBehaviour
{
   public void Rotate(Vector3 axis, float speed)
    {
        //�v���b�g�t�H�[���̉�]����
        this.transform.Rotate(speed * Time.deltaTime * axis);
    }
}
