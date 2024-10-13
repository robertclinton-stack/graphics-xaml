using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpfEllipseTranslate8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int digit1;
        private int stepB;
        private int stepC;
        private int x1;        // assigned values in if(s == 'A'){
        private int x2;        // assigned values in if(s == 'A'){
        private int oldX2;
        private int ellipseN;  // assigned values in if(s == 1, 2, 3, 4){
//      private int ellipseRS;
        private double y1;     // assigned values in (s == 'B'){
        private double y2;     // assigned values in (s == 'B'){
        private double x3;     // assigned values in (s == 'B'){
        private double x4;     // assigned values in if(s == 'B'){
//      private double ellW;
        private double localT;
        private bool init;
        private bool tFParse;
        private bool inCircle;
        private char charInput0;
        private char charInput1;
        private char charD;
        private string? stringInput;
        private string? str1;
        private string? str2;
        private string? str3;
//      private string? s4;
        private delegate void Cir();
        private Cir moveInCircle;
        private double[] rS = new double[4] {0.0, 0.0, 0.0, 0.0};
        private Ellipse[] ell = new Ellipse[4];
        public MainWindow()
        {
            InitializeComponent();
            digit1 = 0;
            stepB = 0;
            stepC = 0;
            x1 = 0;
            x2 = 0;
            oldX2 = 0;
//          ellW = 0;
            ellipseN = 0;
//          ellipseRS = 0;
            y1 = 300.0;
            y2 = 0.0;
            x3 = 0.0;
            x4 = 0.0;
//          ellW = 0.0;
            localT = 0.0;
            init = true;
            tFParse = false;
            charInput0 = '\0';
            charInput1 = '\0';
            charD = 'D';
            stringInput = null;
            str1 = null;
            str2 = null;
            str3 = null;
//          s4 = null;
            LabelWPF1.Content = "1: Red,   2: Yellow,   3: Green,   4: Blue,    ^: Up,        pg dn: Down,\n" +
            "<: Left,        >: Right,    BackGround:    5: Red,      6: Yellow,      7: Green,\n" +
            "8: Blue,      9: Black,      0: White,      Press A to move circles across the monitor\n" +
            "Press B to resize an individual circle    Press C to resize all the circles   Press\n" +
            "Z to reset";
            moveInCircle = new Cir(funcMoveInCircle);
            for(int i = 0; i < 4; i++)
               ell[i] = new Ellipse();
            ell[0] = ellipseR;
            ell[1] = ellipseY;
            ell[2] = ellipseG;
            ell[3] = ellipseB;
        }

        private void mW_KD(object sender, KeyEventArgs e)
        {
            stringInput = e.Key.ToString() + "a";
            charInput0 = stringInput[0];
            charInput1 = stringInput[1];

            if((stepB == 0) && (stepC == 0))
               LabelWPF3.Content = null;

            if((String.Compare(charD.ToString() /* "D" */ , charInput0.ToString()) == 0) &&
            (String.Compare(charD.ToString(), charInput1.ToString()) != 0))
            {
                                                       // digit entry

                if((stepB == 0) && (stepC == 0))       // cannot change the value of ellipseN while
                {                                      // entering a value to resize ellipse
                    if(charInput1.ToString() == "1")
                    {
                        ellipseN = 0;
                        str1 = "red";
                    }
                    if(charInput1.ToString() == "2")
                    {
                        ellipseN = 1;
                        str1 = "yellow";
                    }
                    if(charInput1.ToString() == "3")
                    {
                        ellipseN = 2;
                        str1 = "green";
                    }
                    if(charInput1.ToString() == "4")
                    {
                        ellipseN = 3;
                        str1 = "blue";
                    }
                    if(charInput1.ToString() == "5")
                       mW.Background = Brushes.Red;
                    if(charInput1.ToString() == "6")
                       mW.Background = Brushes.Yellow;
                    if(charInput1.ToString() == "7")
                       mW.Background = Brushes.Green;
                    if(charInput1.ToString() == "8")
                       mW.Background = Brushes.Blue;
                    if(charInput1.ToString() == "9")
                       mW.Background = Brushes.Black;
                    if(charInput1.ToString() == "0")
                       mW.Background = Brushes.White;
                    LabelWPF2.Content = str1;
                }
                if((stepB >= 1) && (stepB <= 4))     // resizes individual circles
                {                                    // in order for the stepB condition to be met,
                                                     // the 'B' key must have been pressed.
                    tFParse = Int32.TryParse(charInput1.ToString(), out digit1);
                    if(tFParse == true)
                    {
                        str2 = charInput1.ToString();
                        str3 += str2;
                        Int32.TryParse(str3, out digit1);
                        if(stepB == 4)
                        {
                            scaleEllipses(ellipseN, digit1);
                            str2 = "Circle " + str1 + " is resized at " + str3;
                            LabelWPF3.Content = str2;
                            str2 = null;
                            str3 = null;
                            stepB = 0;
                        }
                        else
                        {
                            LabelWPF3.Content = str3;
                            ++stepB;                     // stepB is not incremented if the parse
                        }                                // fails.
                    }
                    else
                       LabelWPF3.Content = "cannot enter non-digits.";
                }
                if((stepC >= 1) && (stepC <= 4))         // resizes all the circles
                {                                        // in order for the sepC condition to be met,
                                                         // the 'C' key must have been pressed.

                    tFParse = Int32.TryParse(charInput1.ToString(), out digit1);
                    if(tFParse == true)
                    {
                        str2 = charInput1.ToString();
                        str3 += str2;
                        Int32.TryParse(str3, out digit1);
                        if(stepC == 4)
                        {
                            scaleEllipses(0, 10000, digit1);
                            str2 = "Circles are resized at " + str3;
                            LabelWPF3.Content = str2;
                            str2 = null;
                            str3 = null;
                            stepC = 0;
                        }
                        else
                        {
                            LabelWPF3.Content = str3;
                            ++stepC;                     // stepC is not incremented if the parse
                        }                                // fails
                    }
                    else
                       LabelWPF3.Content = "cannot enter non-digits.";
                }
            }
            else                                 // character entry
            {
                if(charInput0.ToString() == "A")
                {
                    if(init == true)
                    {
                        for(int i = 0; i < 4; i++)
                        {
                            Canvas.SetLeft(ell[i], 5);
                            Canvas.SetTop(ell[i], 140 + (i * 150));
                        }
                        init = false;
                    }
                    Canvas.SetLeft(ell[x1], (5 + (x2 * 200)) - rS[x1]);
                    ++x1;
                    if(x1 == 4)
                    {
                        x1 = 0;
                        oldX2 = x2;
                        ++x2;
                    }
                    if(x2 == 7)
                    {
                        oldX2 = x2;
                        x2 = 0;
                    }
                }
                if((stepB == 0) && (stepC == 0))
                {
                    if(charInput0.ToString() == "B")
                    {
                        LabelWPF3.Content = "Enter four digits to determine the \n" +
                        "size of an individual circle.\n";
                        stepB = 1;
                    }
                    if(charInput0.ToString() == "C")
                    {
                        LabelWPF3.Content = "Enter four digits to determine the size\n" +
                        "of the all the circles\n";
                        stepC = 1;
                    }
                }
                if(charInput0.ToString() == "Z")
                {
                    for(int i = 0; i < 4; i++)
                       Canvas.SetTop(ell[i], (140 + (i * 150)) - rS[i]);
                    stepB = 0;
                    stepC = 0;
                    str2 = null;
                    str3 = null;
                    LabelWPF3.Content = str3;
                }
                if(stringInput == "Righta")
                   moveEllipse(ell[ellipseN], 6, 0);
                if(stringInput == "Lefta")
                   moveEllipse(ell[ellipseN], -6, 0);
                if(stringInput == "Upa")
                   moveEllipse(ell[ellipseN], 0, -6);
                if(stringInput == "Nexta")
                   moveEllipse(ell[ellipseN], 0, 6);
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if(inCircle)
            {
                inCircle = false;
                startStopButton.Content = "MoveInCircle";
                init = true;
            }
            else
            {
                inCircle = true;
                startStopButton.Content = "Stop";
                startStopButton.Dispatcher.BeginInvoke(DispatcherPriority.Normal, moveInCircle);
            }
        }

        private void funcMoveInCircle()
        {
            x4 = x3 * Math.Cos(localT) - y1 * Math.Sin(localT);
            y2 = x3 * Math.Sin(localT) + y1 * Math.Cos(localT);
            Canvas.SetLeft(ell[ellipseN], (int)x4 + 630);
            Canvas.SetTop(ell[ellipseN], (int)y2 + 350);
            localT += .0002;
            if(localT > 6.26)
               localT = 0.0;
            for(int j = 0; j < 100000; j++)
               ;
            if(inCircle)
               startStopButton.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, moveInCircle);
        }

        private void moveEllipse(Ellipse ell, int DeltaLeft, int DeltaTop)
        {
            int localLeft = 0;
            int localTop = 0;
            localLeft = (int)Canvas.GetLeft(ell);
            localTop = (int)Canvas.GetTop(ell);
            localLeft += DeltaLeft;
            localTop += DeltaTop;
            Canvas.SetLeft(ell, localLeft);
            Canvas.SetTop(ell, localTop);
        }

        private void scaleEllipses(int e, int sizeOne = 10000, int sizeAll = 10000)
        {
            int z0   = 0;
            int z1   = 1;
            int z2   = 2;
            int z3   = 3;
            int size = 0;
            int localLeft = 0;
            int localTop = 0;

            if(sizeAll != 10000)            // all the ellipses are resized
               size = sizeAll;

            if(sizeOne != 10000)            // only ellipse[e] is resized
            {
                z0 = z1 = z2 = z3 = e;
                size = sizeOne;
            }

            ell[z0].Width   = size;
            ell[z1].Width   = size;
            ell[z2].Width   = size;
            ell[z3].Width   = size;
            ell[z0].Height  = size;
            ell[z1].Height  = size;
            ell[z2].Height  = size;
            ell[z3].Height  = size;
            if(sizeOne != 10000)
            {
                rS[e] = ell[e].Width;
                rS[e] /= 2.0;
                rS[e] -= 30.0;
                localLeft = (int)Canvas.GetLeft(ell[e]);
                localTop = (int)Canvas.GetTop(ell[e]);
                localLeft -= (int)rS[e];
                localTop -= (int)rS[e];
                Canvas.SetLeft(ell[e], localLeft);
                Canvas.SetTop(ell[e], localTop);
//              Canvas.SetTop(ell[e], (140 + (e * 150)) - rS[e]);      // permanently resets
//              if(e < x1)                                             // Canvas.SetTop()
//                 oldX2 = x2;
//              Canvas.SetLeft(ell[e], (5 + (oldX2 * 200)) - rS[e]);
            }
            if(sizeAll != 10000)
            {
                for(int i = 0; i < 4; i++)
                {
                    rS[i] = ell[0].Width;
                    rS[i] /= 2.0;
                    rS[i] -= 30.0;
                    localLeft = (int)Canvas.GetLeft(ell[i]);
                    localTop = (int)Canvas.GetTop(ell[i]);
                    localLeft -= (int)rS[0];
                    localTop -= (int)rS[0];
                    Canvas.SetLeft(ell[i], localLeft);
                    Canvas.SetTop(ell[i], localTop);
//                  Canvas.SetTop(ell[i], (140 + (i * 150)) - rS[0]);  // permanently resets
//                  if(e < x1)                                         // Canvas.SetTop()
//                     oldX2 = x2;
                    Canvas.SetLeft(ell[i], (5 + (x2 * 200)) - rS[0]);
                }
            }
        }
    }
}
