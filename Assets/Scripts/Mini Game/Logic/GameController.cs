using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameH2A_SO gameData;
    public GameObject lineParent;
    public LineRenderer linePrefab;
    public Ball ballPrefab;
    public Transform[] holderTransforms;

    private void Start()
    {
        DrawLine();
        CreateBall();
    }
    public void DrawLine()
    {
        foreach(var connections in gameData.lineConnections)
        {
            var line = Instantiate(linePrefab, lineParent.transform);
            line.SetPosition(0, holderTransforms[connections.from].position);
            line.SetPosition(1, holderTransforms[connections.to].position);
        }
    }

    public void CreateBall()
    {
        for (int i = 0; i < gameData.startBallOrder.Count; i++)
        {
            if (gameData.startBallOrder[i] == BallName.None)
            {
                holderTransforms[i].GetComponent<Holder>().isEmpty = true;
                continue;
            }
            Ball ball = Instantiate(ballPrefab, holderTransforms[i]);
            holderTransforms[i].GetComponent<Holder>().isEmpty = false;
            ball.SetupBall(gameData.GetBallDetails(gameData.startBallOrder[i]));
        }
    }
}
