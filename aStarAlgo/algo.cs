using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aStarAlgo
{

    using compareCell = Tuple<double, cell>;
    using nextCell = Tuple<double, cell>;
    static class myDefine
    {
        public const int ROW = 9;
        public const int COL = 10;
    }
    public class cell
    {
        public int x, y;
        public double f, g, h;
        public bool bWall;
        public int parent_x;
        public int parent_y;
        public cell(int _x = -1, int _y = -1)
        {
            x = _x;
            y = _y;
            f = g = h = int.MaxValue;
            parent_x = -1;
            parent_y = -1;
        }
        public static cell createCell(int x = -1, int y = -1)
        {
            cell a = new cell();
            a.x = x;
            a.y = y;
            a.f = a.g = a.h = 0;
            return a;
        }
    }

    public class algo
    {
        public algo() { }
        public double manhattanDistance(cell currCell, cell tar)
        {            
            return Math.Abs(currCell.x - tar.x) + Math.Abs(currCell.y - tar.y);
        }
        public double euclideanDistance(cell currCell, cell tar)
        {
            return Math.Sqrt(Math.Pow(currCell.x - tar.x, 2) + Math.Pow(currCell.y - tar.y, 2));
        }
        public double calculateHeuristics(cell currCell, cell tar)
        {
            //return euclideanDistance(currCell, tar);
            return manhattanDistance(currCell, tar);
        }

        public bool isBlocked(int p)
        {
            if (p == 0)
                return true;
            else
                return false;
        }
        
        public bool isDest(cell dest, cell currCell)
        {
            if (currCell.x == dest.x && currCell.y == dest.y)
                return true;
            else
                return false;
        }

        public bool isValid(int x, int y, int borderX, int borderY)
        {
            if (x < 0 || y < 0 || x > borderX - 1 || y > borderY - 1)
            {
                return false;
            }
            return true;
        }

        public List<cell> tracePath(cell[,] ans, cell dest)
        {
            int row = dest.x;
            int col = dest.y;
            List<cell> path = new List<cell>();
            Console.WriteLine("path add cell: " + row + ", " + col);
            
            while (!(ans[row, col].parent_x == row && ans[row, col].parent_y == col))
            {
                path.Add(new cell(row, col));
                Console.WriteLine("path add cell: " + row + ", " + col);
                int tmp_row = ans[row, col].parent_x;
                int tmp_col = ans[row, col].parent_y;
                row = tmp_row;
                col = tmp_col;
                Console.WriteLine("path add parent cell: " + row + ", " + col);
            }
            
            return path;
        }

        public List<cell> aStarSearch(int[,] myMap, int row, int col, cell src, cell tar)
        {
            List<cell> ans = new List<cell>();
            SortedSet<nextCell> openList = new SortedSet<nextCell>(new CellComparer());
            bool[,] closedList = new bool[row, col];
            cell[,] cellDetails = new cell[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    closedList[i, j] = false;
                    cellDetails[i, j] = new cell(i, j);                    
                }
            }

            

            cellDetails[src.x, src.y].parent_x = src.x;
            cellDetails[src.x, src.y].parent_y = src.y;            
            cellDetails[src.x, src.y].g = 0;
            cellDetails[src.x, src.y].h = calculateHeuristics(src, tar);
            cellDetails[src.x, src.y].f = cellDetails[src.x, src.y].g + cellDetails[src.x, src.y].h;
            openList.Add(new nextCell(0, cellDetails[src.x, src.y]));

            while (openList.Any())
            {                
                String routeStr = $"openlist cell count: {openList.Count}\n";
                foreach (var path in openList)
                {
                    routeStr += $"openlist cell: f:{path.Item1}, ({path.Item2.x}, {path.Item2.y})\n";
                }
                Console.WriteLine(routeStr);
                nextCell currData = openList.First();
                int x = currData.Item2.x;
                int y = currData.Item2.y;
                double f = 0, g, h;
                    //new Tuple<int, cell>(openList.First().Item1, new cell(openList.First().Item2.x, openList.First().Item2.y));
                openList.Remove(openList.First());
                closedList[x, y] = true;
                Console.WriteLine("current cell: " + x + ", "+ y + ", f:" + currData.Item1);
                Console.WriteLine("parrent cell: " + currData.Item2.parent_x + ", " + currData.Item2.parent_y + ", f:" + currData.Item2.f);
                
                // up
                if (isValid(x, y - 1, row, col))
                {
                    if (isDest(tar, cellDetails[x, y - 1]))
                    {
                        cellDetails[x, y - 1].parent_x = x;
                        cellDetails[x, y - 1].parent_y = y;
                        ans = tracePath(cellDetails, tar);
                        return ans;
                    }
                    else if( !closedList[x, y - 1] && !isBlocked(myMap[x, y - 1]))
                    {
                        g = cellDetails[x, y].g + 1;
                        h = calculateHeuristics(new cell(x, y - 1), tar);
                        f = g + h;
                        Console.WriteLine("try up cell: " + x + ", " + (y - 1) + ", f:" + f + ", g:" + g + ", h:" + h);
                        if (cellDetails[x, y - 1].f == int.MaxValue || cellDetails[x, y - 1].f > f)
                        {
                            cellDetails[x, y - 1].f = f;
                            cellDetails[x, y - 1].g = g;
                            cellDetails[x, y - 1].h = h;
                            cellDetails[x, y - 1].parent_x = x;
                            cellDetails[x, y - 1].parent_y = y;
                            openList.Add(new nextCell(f, cellDetails[x, y - 1]));
                        }
                    }
                }

                // down
                if (isValid(x, y + 1, row, col))
                {
                    if (isDest(tar, cellDetails[x, y + 1]))
                    {
                        cellDetails[x, y + 1].parent_x = x;
                        cellDetails[x, y + 1].parent_y = y;
                        ans = tracePath(cellDetails, tar);
                        return ans;
                    }
                    else if (!closedList[x, y + 1] && !isBlocked(myMap[x, y + 1]))
                    {
                        g = cellDetails[x, y].g + 1;
                        h = calculateHeuristics(new cell(x, y + 1), tar);
                        f = g + h;
                        Console.WriteLine("try down cell: " + x + ", " + (y + 1) + ", f:" + f + ", g:" + g + ", h:" + h);
                        if (cellDetails[x, y + 1].f == int.MaxValue || cellDetails[x, y + 1].f > f)
                        {
                            cellDetails[x, y + 1].f = f;
                            cellDetails[x, y + 1].g = g;
                            cellDetails[x, y + 1].h = h;
                            cellDetails[x, y + 1].parent_x = x;
                            cellDetails[x, y + 1].parent_y = y;
                            openList.Add(new nextCell(f, cellDetails[x, y + 1]));
                        }
                    }
                }

                // left
                if (isValid(x - 1, y, row, col))
                {
                    if (isDest(tar, cellDetails[x - 1, y]))
                    {
                        cellDetails[x - 1, y].parent_x = x;
                        cellDetails[x - 1, y].parent_y = y;
                        ans = tracePath(cellDetails, tar);
                        return ans;
                    }
                    else if (!closedList[x - 1, y] && !isBlocked(myMap[x - 1, y]))
                    {
                        g = cellDetails[x, y].g + 1;
                        h = calculateHeuristics(new cell(x - 1, y), tar);
                        f = g + h;
                        Console.WriteLine("try left cell: " + (x - 1) + ", " + (y) + ", f:" + f + ", g:" + g + ", h:" + h);
                        if (cellDetails[x - 1, y].f == int.MaxValue || cellDetails[x - 1, y].f > f)
                        {
                            cellDetails[x - 1, y].f = f;
                            cellDetails[x - 1, y].g = g;
                            cellDetails[x - 1, y].h = h;
                            cellDetails[x - 1, y].parent_x = x;
                            cellDetails[x - 1, y].parent_y = y;
                            openList.Add(new nextCell(f, cellDetails[x - 1, y]));
                        }
                    }
                }

                // right
                if (isValid(x + 1, y, row, col))
                {
                    if (isDest(tar, cellDetails[x + 1, y]))
                    {
                        cellDetails[x + 1, y].parent_x = x;
                        cellDetails[x + 1, y].parent_y = y;
                        ans = tracePath(cellDetails, tar);
                        return ans;
                    }
                    else if (!closedList[x + 1, y] && !isBlocked(myMap[x + 1, y]))
                    {
                        g = cellDetails[x, y].g + 1;
                        h = calculateHeuristics(new cell(x + 1, y), tar);
                        f = g + h;
                        Console.WriteLine("try right cell: " + (x + 1) + ", " + (y) + ", f:" + f + ", g:" + g + ", h:" + h);
                        if (cellDetails[x + 1, y].f == int.MaxValue || cellDetails[x + 1, y].f > f)
                        {
                            cellDetails[x + 1, y].f = f;
                            cellDetails[x + 1, y].g = g;
                            cellDetails[x + 1, y].h = h;
                            cellDetails[x + 1, y].parent_x = x;
                            cellDetails[x + 1, y].parent_y = y;
                            Console.WriteLine();
                            openList.Add(new nextCell(f, cellDetails[x + 1, y]));
                        }
                    }
                }
            }

            return ans;
        }
    }

    public class CellComparer : IComparer<compareCell>
    {
        public int Compare(compareCell a, compareCell b)
        {
            int compare = a.Item1.CompareTo(b.Item1);
            if (compare == 0)
            {
                int compare2 = a.Item2.x.CompareTo(b.Item2.x);
                if (compare2 == 0)
                {
                    return a.Item2.y.CompareTo(b.Item2.y);
                }
                else
                {
                    return compare2;
                }
            }
            else
            {
                return compare;
            }
            
        }
    }
}
