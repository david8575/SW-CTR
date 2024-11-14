using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [System.Serializable]
    public struct StageInfo
    {
        public bool isClear;
        public float bestTime;
        public bool puzzleClear;
    };

    public StageInfo TutorialStage = new();
    public StageInfo[] TriangleStage = new StageInfo[3];
    public StageInfo[] SquareStage = new StageInfo[3];
    public StageInfo[] PentagonStage = new StageInfo[3];
    public StageInfo[] CircleStage = new StageInfo[4];

    public bool UnlockSquare = false;

    public int leftStatPoint = 0;
    public int healthStat = 0;
    public int attackStat = 0;
    public int defenseStat = 0;
    public int speedStat = 0;
    public int cooldownStat = 0;

}
