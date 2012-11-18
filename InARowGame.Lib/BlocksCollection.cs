using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace InARowGame.Lib
{
    public class BlocksCollection : List<Block>
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public bool IsFinished { get; set; }
        public string DefaultColor { get; set; }

        public delegate void GameIsFinishedDelegate(Color color);
        public event GameIsFinishedDelegate GameIsFinished;
              
        public BlocksCollection(int rows, int columns, string defaultColor)
        {
            Rows = rows;
            Columns = columns;
            DefaultColor = defaultColor;

            FillCollection();
        }

        private void FillCollection()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    AddBlock(c, r);
                }
            }
        }

        private void AddBlock(int x, int y)
        {
            Block block = new Block(x, y, DefaultColor);
            block.IsTapped += new Block.IsTappedDelegate(block_IsTapped);
            //block.Explodes += new Block.ExplodesDelegate(block_Explodes);

            this.Add(block);
        }

        //void block_Explodes(Block block, Color color)
        //{
        //    var neighbours = GetNeighbours(block);

        //    Color? winnerColor = GetWinner();

        //    if (!winnerColor.HasValue)
        //    {
        //        foreach (Block neighbour in neighbours)
        //        {
        //            neighbour.Overtake(color);
        //        }
        //    }
        //}

        void block_IsTapped(Block block)
        {

            int hor = 1 + GetNeighboursCount(block, Enums.Direction.Left, 0) + GetNeighboursCount(block, Enums.Direction.Right, 0);
            int ver = 1 + GetNeighboursCount(block, Enums.Direction.Top, 0) + GetNeighboursCount(block, Enums.Direction.Bottom, 0);

            int dig1 = 1 + GetNeighboursCount(block, Enums.Direction.LeftTop, 0) + GetNeighboursCount(block, Enums.Direction.RightBottom, 0);
            int dig2 = 1 + GetNeighboursCount(block, Enums.Direction.RightTop, 0) + GetNeighboursCount(block, Enums.Direction.LeftBottom, 0);


            if (hor >= GameController.Instance.MaxInARow)
            {
                GetNeighboursCount(block, Enums.Direction.Left, 0, true);
                GetNeighboursCount(block, Enums.Direction.Right, 0, true);
            }

            if (ver >= GameController.Instance.MaxInARow)
            {

                GetNeighboursCount(block, Enums.Direction.Top, 0, true);
                GetNeighboursCount(block, Enums.Direction.Bottom, 0, true);

            }

            if (dig1 >= GameController.Instance.MaxInARow)
            {

                GetNeighboursCount(block, Enums.Direction.LeftTop, 0, true);
                GetNeighboursCount(block, Enums.Direction.RightBottom, 0, true);

            }

            if (dig2 >= GameController.Instance.MaxInARow)
            {
                GetNeighboursCount(block, Enums.Direction.RightTop, 0, true);
                GetNeighboursCount(block, Enums.Direction.LeftBottom, 0, true);

            }

            if (hor >= GameController.Instance.MaxInARow
                || ver >= GameController.Instance.MaxInARow
                || dig1 >= GameController.Instance.MaxInARow
                || dig2 >= GameController.Instance.MaxInARow)
            {
                block.ShowWinner();
                DeclareWinner(block.TapColor);
            }
        
        }


        private int GetNeighboursCount(Block block, Enums.Direction direction, int count, bool showWinner = false)
        {
           

            int x = block.X;
            int y = block.Y;
            
            int nX = 0;
            int nY = 0;

            switch (direction)
            {
                case Enums.Direction.Left:
                    nX = x - 1;
                    nY = y;
                    break;
                case Enums.Direction.Right:
                    nX = x + 1;
                    nY = y;
                    break;
                case Enums.Direction.Top:
                    nX = x;
                    nY = y - 1;
                    break;
                case Enums.Direction.Bottom:
                    nX = x;
                    nY = y + 1;
                    break;
                case Enums.Direction.LeftTop:
                    nX = x - 1;
                    nY = y - 1;
                    break;
                case Enums.Direction.RightTop:
                    nX = x + 1;
                    nY = y - 1;
                    break;
                case Enums.Direction.LeftBottom:
                    nX = x - 1;
                    nY = y + 1;
                    break;
                case Enums.Direction.RightBottom:
                    nX = x + 1;
                    nY = y + 1;
                    break;
                default:
                    break;
            }

            Block neighbour = GetBlock(nX, nY);

            if (neighbour != null && neighbour != block)
            {
                if (neighbour.TapColor.HasValue
                    && neighbour.TapColor == block.TapColor)
                {
                    if (showWinner)
                        neighbour.ShowWinner();

                    //Found a good neighbour
                    count = GetNeighboursCount(neighbour, direction, count + 1, showWinner);
                }
            }

            return count;
        }

        private Block GetBlock(int x, int y)
        {
            return this.Where(b => b.X == x).Where(b => b.Y == y).FirstOrDefault();
        }


        //public Color? GetWinner()
        //{
        //    Color? color = null;

        //    var colors = GetActivePlayers();

        //    if (colors.Count() >= 2)
        //    {
        //        var distinct = colors.Distinct();
                    
        //        if (distinct.Count() == 1)
        //        {
        //            color = distinct.First();

        //            DeclareWinner(color);
        //        }
        //    }

        //    return color;
        //}

        private void DeclareWinner(Color? color)
        {
            if (!IsFinished)
            {
                IsFinished = true;

                foreach (Block b in this)
                {
                    b.IsFinished = true;
                }

                if (GameIsFinished != null)
                    GameIsFinished(color.Value);
            }
        }

        public IEnumerable<Color> GetActivePlayers()
        {
            var colors = this.Where(b => b.TapColor.HasValue).Select(b => b.TapColor.Value);
            return colors;
        }


        public void Reset()
        {
            IsFinished = false;

            foreach (Block b in this)
            {
                b.Reset();
            }
        }
    }
}
