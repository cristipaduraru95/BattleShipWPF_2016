using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BattleShipWPF.BattleShipClasses
{
    public class BattleshipBoard : AbstractClasses.Board
    {
        public enum BoardValues
        {
            Empty,
            Body,
            DamagedBody,
            Head
        }
        private Button[,] buttons;

        public Button[,] Buttons { get => buttons; set => buttons = value; }

        public BattleshipBoard(int value)
        {
            Length = value;
            Buttons = new Button[Length, Length];
            Field = new int[Length, Length];
            for (var i = 0; i < Length; i++)
            {
                for (var j = 0; j < Length; j++)
                {
                    Buttons[i, j] = new Button
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Background = Brushes.White
                    };
                    Field[i, j] = (int) BoardValues.Empty;
                }
            }
        }

    }
}