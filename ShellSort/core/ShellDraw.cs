using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Timers;

namespace ShellSort
{

    public class NumDrawElement
    {
       public Grid Grid { get; set; }
       public double Num { get; set; }
        public NumDrawElement(double Num, double size)
        {
            this.Num = Num;
            Grid = new Grid() { Background = Brushes.White, Margin = new Thickness(1), Height = ((450D / 255D) * Num), Width = (450 / size + 1), VerticalAlignment = VerticalAlignment.Bottom };
            Border border = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            Grid.Children.Add(border);
        }
    }

    public class ShellAlgo
    {
        static int size = 15;
        public List<NumDrawElement> Numbers { get; private set; }
        Random ran;
        StackPanel SPanel;

        public ShellAlgo(StackPanel SPanel)
        {
            Numbers = new List<NumDrawElement>();
            ran = new Random();
            this.SPanel = SPanel;
        }

        public void Generate()
        {
            for(int i = 0; i < (int)size; i++)   Numbers.Add(new NumDrawElement((byte)ran.Next(255), size));
        }

        public void Visualize()
        {
            for (int i = 0; i < Numbers.Count; i++)   SPanel.Children.Add(Numbers[i].Grid);
        }

        static Grid lastGrid;

        int l, j, inc = 3;
        double temp;
        public void initVar()
        {
            l = 0;
        }

        public void NextIteration()
        {

            int l, j, inc;
            double temp;
            inc = 3;
            while (inc > 0)
            {
                for (l = 0; l < Numbers.Count; l++)
                {
                    j = l;
                    temp = Numbers[l].Grid.Height;
                    while ((j >= inc) && (Numbers[j - inc].Grid.Height > temp))
                    {
                        Numbers[j].Grid.Height = Numbers[j - inc].Grid.Height;
                        j = j - inc;
                    }
                    Numbers[j].Grid.Height = temp;
                }
                if (inc / 2 != 0) inc = inc / 2;
                else if (inc == 1) inc = 0;
                else inc = 1;
            }
        }

        public  void NextIterationForGnome()
        {
            SPanel.Dispatcher.Invoke(() =>
            {
                if (inc > 0)
                { 
                    if ((l < Numbers.Count))
                    {
                        j = l;
                        temp = Numbers[l].Grid.Height;
                        while ((j >= inc) && (Numbers[j - inc].Grid.Height > temp))
                        {
                            Numbers[j].Grid.Height = Numbers[j - inc].Grid.Height;
                            j = j - inc;
                        }

                        Numbers[j].Grid.Height = temp;
                        if (lastGrid != null)
                        {
                            lastGrid.Background = Brushes.White;
                        }
                        Numbers[j].Grid.Background = Brushes.Beige;
                        lastGrid = Numbers[j].Grid;
                        l++;

                    }
                    else
                    {
                        l = 0;
                        if (inc / 2 != 0) inc = inc / 2;
                        else if (inc == 1) inc = 0;
                        else inc = 1;
                    }
                    return;
                }
                else return;
            });
        }
    }

    public class ShellDraw
    {
        ShellAlgo Gnome;
        Timer timer;
        StackPanel SPanel;
        public ShellDraw(StackPanel SPanel)
        {
            this.SPanel = SPanel;
            Gnome = new ShellAlgo(SPanel);
            timer = new System.Timers.Timer(200);
            timer.Elapsed += Timer_Elapsed;
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Gnome.NextIterationForGnome();
            SPanel.Dispatcher.Invoke(() => SPanel.UpdateLayout());
        }
        public void Run()
        {
            Gnome.Generate();
            Gnome.Visualize();
            Gnome.initVar();
            timer.Start();
        }
    }
}
