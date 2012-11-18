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
using mpost.WP7.Client;


namespace InARowGame.Lib
{
    public class GameController
    {
        private static GameController instance;
        public static string DefaultColor { get; set; }

        private GameController(string defaultColor) 
        {
            DefaultColor = defaultColor;
        }

        public static GameController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameController(DefaultColor);
                }
                return instance;
            }
        }

        public BlocksCollection BlocksCollection { get; set; }

        public int Players { get; set; }
        public int CurrentPlayer { get; set; }
        public event InARowGame.Lib.BlocksCollection.GameIsFinishedDelegate GameIsFinished;
        private int TurnNumber { get; set; }
        public bool IsFinished { get; set; }

        public int MaxInARow { get; set; }


        public Color CurrentColor
        {
            get
            {
                return colorList.Skip(CurrentPlayer - 1).First();
            }
        }

        public static List<Color> colorList = new List<Color>() { 
            ColorFromString.ToColor("LimeGreen"), 
            ColorFromString.ToColor("Red"),
            ColorFromString.ToColor("Blue"),
            ColorFromString.ToColor("Yellow")
        };

        public void Init(int rows, int columns)
        {
            if(Instance.BlocksCollection != null)
                Instance.BlocksCollection.Clear();

            Instance.BlocksCollection = new BlocksCollection(rows, columns, DefaultColor);
            Instance.BlocksCollection.GameIsFinished += new Lib.BlocksCollection.GameIsFinishedDelegate(Instance_GameIsFinished);
            
            Instance.Players = 2;
            Instance.CurrentPlayer = 1;
            Instance.TurnNumber = 0;
        }

        public void Reset()
        {
            Instance.BlocksCollection.Reset();

            Instance.TurnNumber = 0;
            Instance.CurrentPlayer = 1;
            IsFinished = false;
        }

        void Instance_GameIsFinished(Color color)
        {
            IsFinished = true;

            if (GameIsFinished != null)
                GameIsFinished(color);
        }

        public void SwitchPlayer()
        {
            TurnNumber++;

            if (Instance.Players == CurrentPlayer)
                Instance.CurrentPlayer = 1;
            else
            {
                CurrentPlayer++;
                var players = Instance.BlocksCollection.GetActivePlayers();
                if (TurnNumber >= 4 && !players.Contains(CurrentColor))
                    SwitchPlayer();
            }
        }
     
    }

}
