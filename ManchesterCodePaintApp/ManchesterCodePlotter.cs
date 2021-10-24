using System;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;

namespace ManchesterCodePaintApp
{
    public sealed class ManchesterCodePlotter : IManchesterCodePlotter
    {
        private uint _x;
        private byte _y;
        private LineSeries _lineSeries;
        private readonly PlotModel _plotModel;
        public ManchesterCodePlotter()
        {
            _x = 0;
            _y = 0;
            _plotModel = new PlotModel();
        }

        public PlotModel CreateModel(string[] binaryCodes)
        {
            foreach (var code in binaryCodes)
            {
                DrawOneByte(code);
            }
            
            return _plotModel;
        }

        public void DrawOneByte(string byteCode)
        {
            //Каждый байт должен предваряться последовательностью jjkjk 
            _lineSeries = new LineSeries{Color = OxyColors.Red,MarkerType = MarkerType.Circle,SeriesGroupName = "jjKjk"};
            _lineSeries.Points.Add(new DataPoint(_x,_y));
            DrawJ();
            DrawJ();
            DrawK();
            DrawJ();
            DrawK();
            _plotModel.Series.Add(_lineSeries);

            _lineSeries = new LineSeries{Color = OxyColors.Green,MarkerType = MarkerType.Circle};
            _lineSeries.Points.Add(new DataPoint(_x,_y));
            var binaryCodes = byteCode.ToArray();
            foreach (var binaryCode in binaryCodes)
            {
                switch (binaryCode)
                {
                    case '0':
                        DrawZero();
                        break;
                    case '1': 
                        DrawOne();
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(byteCode));
                }
            }
            _plotModel.Series.Add(_lineSeries);

            //Каждый байт должен оканчиваться jjj
            _lineSeries = new LineSeries{Color = OxyColors.Blue,MarkerType = MarkerType.Circle,SeriesGroupName = "jjj"};
            _lineSeries.Points.Add(new DataPoint(_x,_y));
            DrawJ();
            DrawJ();
            DrawJ();
            _plotModel.Series.Add(_lineSeries);
        }

        /// <summary>
        /// Рисует сохранение сигнала, т.е. сигнал остался прежним
        /// </summary>
        private void DrawNoSignal()
        {
            _x++;
            var dataPoint = new DataPoint(_x,_y);
            _lineSeries.Points.Add(dataPoint);
        }

        /// <summary>
        /// Рисует изменение сигнала 
        /// </summary>
        private void DrawSignal()
        {
            _x++;
            _y = InverseYCoordinate(_y);
            var dataPoint = new DataPoint(_x,_y);
            _lineSeries.Points.Add(dataPoint);
        }

        private byte InverseYCoordinate(byte y)
        {
            switch (y)
            {
                case 0: return 1;
                case 1: return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(InverseYCoordinate) + "Y=" + y);
            }
        }
        
        
        public void DrawJ()
        {
            DrawNoSignal();
            DrawNoSignal();
        }

        public void DrawK()
        {
            DrawSignal();
            DrawNoSignal();
        }

        public void DrawZero()
        {
            DrawSignal();
            DrawSignal();
        }

        public void DrawOne()
        {
            DrawNoSignal();
            DrawSignal();
        }
    }
}