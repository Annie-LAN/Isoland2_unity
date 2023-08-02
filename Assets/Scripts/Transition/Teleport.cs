using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    // 用到 M tools（封装好的工具），将string转化成列表可以勾选场景，避免输错
    [SceneName] public string sceneFrom;
    [SceneName] public string sceneTo;
}
