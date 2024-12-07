using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [System.Serializable]
    public struct StageInfo
    {
        public float bestTime;

        public bool isPuzzleClear;
        public bool isAllKill;
        public bool isTimeClear;

        public bool isClear;
    };

    //public StageInfo TutorialStage = new();
    //public StageInfo[] TriangleStage = new StageInfo[3];
    //public StageInfo[] SquareStage = new StageInfo[3];
    //public StageInfo[] CircleStage = new StageInfo[4];

    /// <summary>
    /// 0: Tutorial, 1 ~ 3: Triangle, 4 ~ 6: Square, 7: Cutscene 8 ~ 10: Circle
    /// </summary>
    public StageInfo[] Stages = new StageInfo[11];

    public bool IsTutorialClear = false;

    public bool UnlockSquare = false;

    public int leftStatPoint = 0;

    public const int maxStatPoint = 10;
    public int healthStat = 0;
    public int attackStat = 0;
    public int defenseStat = 0;
    public int speedStat = 0;
    public int cooldownStat = 0;

}
