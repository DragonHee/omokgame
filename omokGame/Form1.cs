using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace omokGame
{
    public partial class Form1 : Form
    {
        
        private int margin = 40;
        private int 눈size = 30;
        private int 돌size = 28;
        private int 화점size = 10;

        private Graphics g;
        private Pen pen;
        private Brush wBrush, bBrush;

        enum STONE { none, black, white};
        STONE[,] 바둑판 = new STONE[19, 19];
        bool flag = false; // false -> 검은돌, true -> 흰색돌

        bool imageFlag = true; // false -> 그리기, true -> 이미지

        public Form1()
        {
            InitializeComponent();

            pen = new Pen(Color.Black);
            bBrush = new SolidBrush(Color.Black);
            wBrush = new SolidBrush(Color.White);
           
            this.ClientSize = new Size(2 * margin + 18 * 눈size, 2 * margin + 18 * 눈size + menuStrip.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBoard();
            DrawStone();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            int x = (e.X - margin + 눈size / 2) / 눈size;
            int y = (e.Y - margin + 눈size / 2) / 눈size;

            if (바둑판[x, y] != STONE.none)
                return;

            Rectangle r = new Rectangle(
                margin + 눈size * x - 돌size / 2,
                margin + 눈size * y - 돌size / 2,
                돌size, 돌size);

            if(flag == false)
            {
                if (imageFlag == false)
                    g.FillEllipse(bBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../images/black.png");
                    g.DrawImage(bmp, r);
                }
       
                flag = true;
                바둑판[x, y] = STONE.black;
            }
            else
            {
                if (imageFlag == false)
                    g.FillEllipse(wBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../images/white.png");
                    g.DrawImage(bmp, r);
                }

                flag = false;
                바둑판[x, y] = STONE.white;
            }

            checkOmok(x, y);
        }

        private void 이미지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageFlag = true;
        }

        private void 그리기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageFlag = false;
        }

        private void DrawBoard()
        {
            g = panel1.CreateGraphics();

            // 세로 선
            for(int i = 0; i < 19; i++)
            {
                g.DrawLine(pen, new Point(margin + i * 눈size, margin), new Point(margin + i * 눈size, margin + 18 * 눈size));
            }

            // 가로 선
            for(int i = 0; i < 19; i++)
            {
                g.DrawLine(pen, new Point(margin, margin + i * 눈size), new Point(margin + 18 * 눈size, margin + i * 눈size));
            }

            // 화점
            for(int x = 3; x <= 15; x += 6)
            {
                for(int y = 3; y <= 15; y += 6)
                {
                    g.FillEllipse(bBrush, margin + 눈size * x - 화점size / 2, margin + 눈size * y - 화점size / 2, 화점size, 화점size);
                }
            }
        }

        private void DrawStone()
        {
            g = panel1.CreateGraphics();

            for(int x = 0; x < 19; x++)
            {
                for (int y = 0; y < 19; y++)
                {
                    Rectangle r = new Rectangle(
                        margin + 눈size * x - 돌size / 2,
                        margin + 눈size * y - 돌size / 2,
                        돌size, 돌size);

                    if (바둑판[x, y] == STONE.black)
                    {
                        if (imageFlag == false)
                            g.FillEllipse(bBrush, r);
                        else
                        {
                            Bitmap bmp = new Bitmap("../../images/black.png");
                            g.DrawImage(bmp, r);
                        }
                    }
                    else if(바둑판[x,y] == STONE.white)
                    {
                        if (imageFlag == false)
                            g.FillEllipse(bBrush, r);
                        else
                        {
                            Bitmap bmp = new Bitmap("../../images/white.png");
                            g.DrawImage(bmp, r);
                        }
                    }
                }
            }
        }

        private void checkOmok(int x, int y)
        {
            int cnt = 1;

            // 오른쪽 방향
            for(int i = x + 1; i <= 18; i++)
            {
                if (바둑판[i, y] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            }

            // 왼쪽 방향
            for (int i = x - 1; i >= 0; i--)
            {
                if (바둑판[i, y] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            }

            if(cnt >= 5)
            {
                OmokComplete(x, y);
                return;
            }

            cnt = 1;

            // 위 방향
            for (int i = y + 1; i <= 18; i++)
            {
                if (바둑판[x, i] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            }

            // 아래 방향
            for (int i = y - 1; i >= 0; i--)
            {
                if (바둑판[x, i] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            }

            if (cnt >= 5)
            {
                OmokComplete(x, y);
                return;
            }

            cnt = 1;

            // 대각선 오른쪽 위 방향
            for(int i = x + 1, j = y + 1; i <= 18 && j <= 18; i++, j++)
            {
                if (바둑판[i, j] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            }

            // 대각선 왼쪽 아래 방향
            for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (바둑판[i, j] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            }

            if (cnt >= 5)
            {
                OmokComplete(x, y);
                return;
            }

            cnt = 1;

            // 대각선 왼쪽 위 방향
            for (int i = x - 1, j = y + 1; i >= 0 && j <= 18; i--, j++)
            {
                if (바둑판[i, j] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            }

            // 대각선 오른쪽 아래 방향
            for (int i = x + 1, j = y - 1; i <= 18 && j >= 0; i++, j--)
            {
                if (바둑판[i, j] == 바둑판[x, y])
                    cnt++;
                else
                    break;
            }

            if (cnt >= 5)
            {
                OmokComplete(x, y);
                return;
            }
        }

        private void OmokComplete(int x, int y)
        {
            DialogResult res = MessageBox.Show(바둑판[x, y].ToString().ToUpper() + 
                " Wins!\n새로운 게임을 시작할까요?", "게임 종료", MessageBoxButtons.YesNo);

            if(res == DialogResult.Yes)
            {
                NewGame();
            }
            else if(res == DialogResult.No)
            {
                this.Close();
            }
        }
       
        private void NewGame()
        {
            flag = false;
            
            for(int x = 0; x < 19; x++)
            {
                for(int y = 0; y < 19; y++)
                {
                    바둑판[x, y] = STONE.none;
                }
            }

            panel1.Refresh();
            DrawBoard();
        }
    }
}
