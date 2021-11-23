using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Credit
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool Round;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        
        public void Counting()
        {
            double Money = Convert.ToDouble(MoneyTB.Text);
            int Month = Convert.ToInt32(MonthTB.Text);
            double Percent = Convert.ToDouble(YearPerTB.Text);
            double PM = Percent / 12 / 100;
            double KA = (PM * Math.Pow(1 + PM, Month)) / (Math.Pow(1 + PM, Month) - 1);
            double PLT = KA * Money;
          
            MonthPerTB.Text = Convert.ToString(Rounding(PLT));
            KATB.Text = Convert.ToString(Rounding(PLT * Month));
            PlatiTB.Text = Convert.ToString(Rounding(Convert.ToDouble(KATB.Text) - Money));

            DatagridFill();
        }

        void DatagridFill()
        {
            int Month = Convert.ToInt32(MonthTB.Text);
            double Money = Convert.ToDouble(MoneyTB.Text);
            double Percent = Convert.ToDouble(YearPerTB.Text);
            double PM = Percent / 12 / 100;
            double PLT = Convert.ToDouble(MonthPerTB.Text);

            double Summ = 0;

            double[,] valuesForDG = new double[Month, 5];

            try
            {
                dataGrid.Items.Clear();

                for (int i = 0; i < Month; i++)
                {
                    valuesForDG[i, 0] = i+1; // период
                    valuesForDG[i, 1] = Rounding((PLT - Money * PM) * Math.Pow(1 + PM, i)); //ОСПЛТ
                    valuesForDG[i, 2] = Rounding(PLT*(1-Math.Pow((1+ PM),i-Month)));//ПРПЛТ
                    valuesForDG[i, 3] = Rounding(valuesForDG[i, 1] + valuesForDG[i, 2]); // сумма ОСПЛТ и ПРПЛТ
                    Summ += valuesForDG[i, 1];
                    valuesForDG[i, 4] = Rounding(Money - Summ); //сумма кредита + (первое значение ОСПЛТ + ОСПЛТ)

                    dataGrid.Items.Add(new Columns { Column1 = valuesForDG[i, 0], Column2 = valuesForDG[i, 1], Column3 = valuesForDG[i, 2], Column4 = valuesForDG[i, 3], Column5 = valuesForDG[i, 4] });
                }

                dataGrid.AutoGenerateColumns = true;
                dataGrid.CanUserAddRows = false;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public double Rounding(double obj)
        {
            if(Round)
            {
                return Math.Round(obj);
            }
            else
            {
                return Math.Round(obj, 2);
            }
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            Counting();
        }

        private void RoundCB_Click(object sender, RoutedEventArgs e)
        {
            Round = Convert.ToBoolean(RoundCB.IsChecked);
            Counting();
        }
    }

    public class Columns
    {
        public double Column1 { get; set; }
        public double Column2 { get; set; }
        public double Column3 { get; set; }
        public double Column4 { get; set; }
        public double Column5 { get; set; }
    }
}
