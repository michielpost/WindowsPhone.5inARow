using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using InARowGame.Lib;
using Microsoft.Phone.Controls;

namespace InARowGame
{
    public partial class BlockControl : UserControl
    {
        public InARowGame.Lib.Block Block { get; set; }

        public BlockControl(InARowGame.Lib.Block block)
        {
            Block = block;
            DataContext = Block;

            Block.BeginIsTapped += new InARowGame.Lib.Block.BeginIsTappedDelegate(Block_BeginIsTapped);
            Block.BeginShowWinner += new Lib.Block.BeginShowWinnerDelegate(Block_BeginShowWinner);
            Block.ResetBlock += new Lib.Block.ResetDelegate(Block_Reset);

            InitializeComponent();

            ToBigAnimation.Completed += new EventHandler(ToBigAnimation_Completed);
            //ExplodeAnimation.Completed += new EventHandler(ExplodeAnimation_Completed);

        }

        void Block_Reset(InARowGame.Lib.Block block)
        {
            LayoutRoot.Background = new SolidColorBrush(Colors.Transparent);

        }

        void Block_BeginShowWinner(InARowGame.Lib.Block block)
        {
            LayoutRoot.Background = new SolidColorBrush(Colors.Yellow);
        }



        //void Block_BeginExplodes(Block block, Color color)
        //{
        //    ExplodeAnimation.Begin();
        //}

        //void ExplodeAnimation_Completed(object sender, EventArgs e)
        //{
        //    ForegroundGrid.Opacity = 0;
        //    Block.ExplodeFinished();
        //}

        void Block_BeginIsTapped()
        {
            ToBigAnimation.Begin();
        }

        void ToBigAnimation_Completed(object sender, EventArgs e)
        {
            Block.TapFinished();
        }
      
        
        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GameController.Instance.IsFinished)
                GameController.Instance.Reset();
            else
                Block.Tap(GameController.Instance.CurrentColor, true);
        }
    }
}
