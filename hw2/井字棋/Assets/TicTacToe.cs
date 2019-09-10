using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
类: TicTacToe
功能: 完成一个简单井字棋游戏的全部功能
*/
public class TicTacToe : MonoBehaviour
{
    private int[,] board = new int[3, 3];               //棋盘3*3
    private int turn = 0;                               //轮次
    private int mode = 1;                               //模式，切换人人或人机对战
    private int initTurn = 0;                           //切换玩家1先手后手

    public Texture2D Backgroud;
    public Texture2D O;
    public Texture2D X;
    public Texture2D Space;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    private void OnGUI()
    {
        //小字体初始化
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.normal.background = null;
        style.fontSize = 20;

        //大字体初始化
        GUIStyle bigStyle = new GUIStyle();
        bigStyle.normal.textColor = Color.white;
        bigStyle.normal.background = null;
        bigStyle.fontSize = 50;

        //加载背景
        GUI.Label(new Rect(0, 0, 920, 540), Backgroud);

        //加载游戏状态
        int state = checkState();
        switch (state)
        {
            case 0:
                GUI.Label(new Rect(500, 50, 200, 50), "状态: 进行中, 玩家"+(turn+1)+"执棋", style);
                break;
            case 1:
                GUI.Label(new Rect(500, 50, 200, 50), "状态: 玩家1获胜", style);
                break;
            case 2:
                GUI.Label(new Rect(500, 50, 200, 50), "状态: 玩家2获胜", style);
                break;
            case 3:
                GUI.Label(new Rect(500, 50, 200, 50), "结果: 平局", style);
                break;
        }

        //加载标题
        GUI.Label(new Rect(135, 25, 200, 50), "简易井字棋", bigStyle);

        //加载重置按钮
        if (GUI.Button(new Rect(200, 350, 100, 50), "重置"))
            init();

        //加载玩家1选择先手与后手的选项及实现功能
        if (GUI.Button(new Rect(200, 100, 100, 50), "玩家1:先手"))
        {
            initTurn = 0;
            init();
        }
        if (GUI.Button(new Rect(200, 150, 100, 50), "玩家1:后手"))
        {
            initTurn = 1;
            init();
        }

        //加载选择与人或与AI对战模式的选项及实现其功能
        if (GUI.Button(new Rect(200, 225, 100, 50), "玩家2:玩家"))
        {
            mode = 1;
            init();
        }
        if (GUI.Button(new Rect(200, 275, 100, 50), "玩家2:AI"))
        {
            mode = 2;
            init();
        }

        //游戏运行逻辑
        //mode1: 人vs人
        //mode2: 人vsAI
        if (mode == 1)
            playerVsPlayer();
        else
            playerVsAIMode();
    }

    //游戏重置
    void init()
    {
        turn = initTurn;
        for(int temp1 = 0; temp1 < 3; temp1++)
        {
            for(int temp2 = 0; temp2 < 3; temp2++)
            {
                board[temp1, temp2] = 0;
            }
        }
    }

    //玩家对战游戏逻辑
    void playerVsPlayer()
    {
        //通过遍历所有棋格，判断玩家落子位置
        for (int temp1 = 0; temp1 < 3; temp1++)
        {
            for (int temp2 = 0; temp2 < 3; temp2++)
            {
                switch (board[temp1, temp2])
                {
                    case 0:
                        //玩家已落子，更新棋盘
                        if (GUI.Button(new Rect(450 + temp1 * 100, 100 + temp2 * 100, 100, 100), Space) && checkState() == 0)
                        {
                            board[temp1, temp2] = turn + 1;
                            turn = 1 - turn;
                        }
                        break;
                    case 1:
                        GUI.Button(new Rect(450 + temp1 * 100, 100 + temp2 * 100, 100, 100), O);
                        break;
                    case 2:
                        GUI.Button(new Rect(450 + temp1 * 100, 100 + temp2 * 100, 100, 100), X);
                        break;
                }
            }
        }
    }

    //人机对战游戏逻辑
    void playerVsAIMode()
    {
        //遍历所有棋格，在玩家回合判断玩家落子位置，在AI回合唤醒AI进行落子
        for (int temp1 = 0; temp1 < 3; temp1++)
        {
            for (int temp2 = 0; temp2 < 3; temp2++)
            {
                switch (board[temp1, temp2])
                {
                    case 0:
                        if (turn == 0)
                        {
                            //玩家回合: 
                            if (GUI.Button(new Rect(450 + temp1 * 100, 100 + temp2 * 100, 100, 100), Space) && checkState() == 0)
                            {
                                board[temp1, temp2] = 1;
                                turn = 1;
                            }
                        }
                        else
                        {
                            //AI回合
                            AITurn();
                            turn = 0;
                        }
                        break;
                    case 1:
                        GUI.Button(new Rect(450 + temp1 * 100, 100 + temp2 * 100, 100, 100), O);
                        break;
                    case 2:
                        GUI.Button(new Rect(450 + temp1 * 100, 100 + temp2 * 100, 100, 100), X);
                        break;
                }
            }
        }
    }

    //AI回合，AI视棋盘情况进行落子
    void AITurn()
    {
        //游戏不在进行中，不进行落子
        if (checkState() != 0)
            return;

        /*
         * (tarLoseX, tarLoseY)  玩家下一回合如下此处，玩家将胜利
         * cnt                   棋盘空闲格数量
         * mp                    储存棋盘空闲格位置
         */
        int tarLoseX, tarLoseY, cnt;
        int[] mp = new int[9];
        cnt = 0;
        tarLoseX = tarLoseY = -1;

        //遍历棋盘，计算下一次落子位置
        for(int temp1 = 0; temp1 < 3; temp1++)
        {
            for(int temp2 = 0; temp2 < 3; temp2++)
            {
                if(board[temp1, temp2] == 0)
                {
                    //判断AI落子此处是否会胜利，若胜利，则落子
                    board[temp1, temp2] = 2;
                    if (checkState() == 2)
                        return;

                    //判断玩家落子此处是否会胜利，若胜利，则记下当前位置(玩家已将军)
                    board[temp1, temp2] = 1;
                    if (checkState() == 1)
                    {
                        tarLoseX = temp1;
                        tarLoseY = temp2;
                    }

                    //恢复棋盘，记下当前空白格位置
                    board[temp1, temp2] = 0;
                    mp[cnt++] = temp1*3+temp2;
                }
            }
        }

        //若存在玩家将军，则落子
        if (tarLoseX != -1) {
            board[tarLoseX, tarLoseY] = 2;
            return;
        }

        //AI落子后既不会胜利，也不存在玩家将军，则随机选择一个空白格落子
        int rd = (int)Random.Range(0, cnt);
        board[mp[rd] / 3, mp[rd] % 3] = 2;
    }

    //检测游戏状态
    //返回值: 0-进行中, 1-玩家1获胜, 2-玩家2获胜, 3-平局
    int checkState()
    {
        //判断交叉线是否符合获胜条件
        if (board[0, 0] != 0 && board[0, 0] == board[1, 1] && board[0, 0] == board[2, 2])
            return board[0, 0];
        if (board[2, 0] != 0 && board[2, 0] == board[1, 1] && board[2, 0] == board[0, 2])
            return board[2, 0];

        int cnt = 0;
        for (int temp1 = 0; temp1 < 3; temp1++)
        {   
            //判断temp1行是否符合获胜条件
            if (board[temp1, 0] == board[temp1, 1] && board[temp1, 0] == board[temp1, 2] && board[temp1, 0] != 0)
                return board[temp1, 0];
            //判断temp1列是否符合获胜条件
            if (board[0, temp1] == board[1, temp1] && board[0, temp1] == board[2, temp1] && board[0, temp1] != 0)
                return board[0, temp1];
            //统计temo1行的空白格数量
            for (int temp2 = 0; temp2 <3; temp2++)
            {
                if (board[temp1, temp2] == 0)
                    cnt++;
            }
        }
        //若空白格已下完，则平局，否则游戏继续进行
        return cnt==0?3:0;
    }
}
