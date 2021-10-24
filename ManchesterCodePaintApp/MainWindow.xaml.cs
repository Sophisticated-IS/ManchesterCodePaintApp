using System;
using System.Linq;
using System.Windows;
using OxyPlot;
using OxyPlot.Legends;

namespace ManchesterCodePaintApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlotManchesterCode(object sender, RoutedEventArgs e)
        {
            if (HexTextCode.Text.Length > 1)
            {
                HexTextCode.Text = HexTextCode.Text.TrimStart('0');
            }

            var text = HexTextCode.Text;
            var isDecoded = GetBinaryCode(text, out var binaryCode);
            if (!isDecoded)
            {
                BinTextCode.Text = string.Empty;
                MessageBox.Show("Number does not correspond to HEX format!!!");
                return;
            }

            try
            {
                BinTextCode.Text = binaryCode;
                var plotter = new ManchesterCodePlotter();
                var bytes = binaryCode.Split(' ');
                var plotViewModel = plotter.CreateModel(bytes);
                plotViewModel.Legends.Add(new Legend
                {
                    LegendFontSize = double.NaN,
                    LegendTitle = "Legend with groups of Series",
                    LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                    LegendBorder = OxyColors.Black,
                });

                PlotView.Model = plotViewModel;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error when plotting the graph:\n" + exception);
            }
        }

        private bool GetBinaryCode(string hexCode, out string code)
        {
            code = null;
            var result = false;
            try
            {
                if (string.IsNullOrEmpty(hexCode)) throw new ArgumentOutOfRangeException(nameof(hexCode));
                code = string.Join(" ",
                    hexCode.Select(
                        c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2)
                            .PadLeft(4, '0')
                    )
                );
                result = true;
            }
            catch (Exception)
            {
                // ignored
            }

            return result;
        }
    }
}