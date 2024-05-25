using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aStarAlgo
{

    public partial class Form1 : Form
    {
        cell startCell = new cell(0, 0);
        cell endCell = new cell(0, 0);
        Button[,] myBtns = new Button[myDefine.ROW, myDefine.COL];
        int[,] grid = new int[myDefine.ROW, myDefine.COL]
{
                { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1 },
                { 1, 1, 1, 0, 1, 1, 1, 0, 1, 1 },
                { 1, 1, 1, 0, 1, 1, 0, 1, 1, 1 },
                { 0, 0, 1, 0, 1, 0, 0, 0, 0, 1 },
                { 1, 1, 1, 0, 1, 1, 1, 0, 1, 1 },
                { 1, 0, 1, 1, 1, 1, 0, 1, 0, 1 },
                { 1, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
                { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 1, 1, 0, 0, 0, 1, 0, 0, 1 }
};
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < myDefine.ROW; i++)
            {
                for (int j = 0; j < myDefine.COL; j++)
                {
                    myBtns[i, j] = new Button();
                    myBtns[i, j].Size = new Size(60, 60);
                    myBtns[i, j].Location = new Point(20 + j * myBtns[i, j].Width, 20 + i * myBtns[i, j].Height);
                    myBtns[i, j].Name = String.Format("{0},{1}", i, j);

                    myBtns[i, j].TabIndex = 0;
                    myBtns[i, j].Text = String.Format("path[{0}, {1}]", i, j);
                    myBtns[i, j].UseVisualStyleBackColor = true;
                    myBtns[i, j].Click += new EventHandler(myBtns_Click);
                    if (grid[i, j] == 0)
                    {
                        myBtns[i, j].Enabled = false;
                    }
                    Controls.Add(myBtns[i, j]);
                }
            }
            resetMap();
            myBtns[startCell.x, startCell.y].BackColor = Color.Red;
        }

        void resetMap()
        {
            for (int i = 0; i < myDefine.ROW; i++)
            {
                for (int j = 0; j < myDefine.COL; j++)
                {
                    if (grid[i, j] == 0)
                    {
                        myBtns[i, j].BackColor = Color.Black;
                    }
                    else
                    {
                        myBtns[i, j].BackColor = Color.White;
                    }
                }
            }
        }
        void myBtns_Click(object sender, EventArgs e)
        {
            resetMap();

            Button button = sender as Button;
            int x = Int32.Parse(button.Name.Split(',')[0]);
            int y = Int32.Parse(button.Name.Split(',')[1]);
            endCell.x = x;
            endCell.y = y;
            Console.WriteLine(new string('-', 30));
            Console.WriteLine("end cell: " + endCell.x + ", " + endCell.y);
            List<cell> result = (new algo()).aStarSearch(grid, myDefine.ROW, myDefine.COL, startCell, endCell);

            myBtns[startCell.x, startCell.y].BackColor = Color.Red;
            if (result.Count == 0)
            {
                MessageBox.Show("path not found");
            }
            else
            {
                
                foreach (var p in result)
                {
                    Console.WriteLine("path cell: " + p.x + ", " + p.y);
                    myBtns[p.x, p.y].BackColor = Color.Red;
                }
                myBtns[endCell.x, endCell.y].BackColor = Color.Yellow;
                startCell.x = endCell.x;
                startCell.y = endCell.y;
            }
            

            Console.WriteLine(new string('-', 30));
        }
    }
}
