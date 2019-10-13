using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    void SetFlyMode(bool isPhysis);
    void Hit(Vector3 position);
    void Restart();
    void SetMode(bool isInfinite);
}
