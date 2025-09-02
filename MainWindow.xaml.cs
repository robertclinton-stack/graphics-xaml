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

namespace wpfEllipseTranslate_6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int fncCtr_1;
        private int fncCtr_2;
        private int fncCtr_3;
        private int fncCtr_4;
        private int digit1;
        private int digit2;
        private int stepB;
        private int stepC;
        private int stepS;
//      private int x1;           assigned values in if(s == 'A'){
        private int x2;        // assigned values in if(s == 'A'){
//      private int oldX2;
        private int mWPause;
        private int ellipseN;  // assigned values in if(s == 1, 2, 3, 4){
//      private int ellipseRS;
        private int fncLeft_1;
        private int fncLeft_2;
        private int fncTop_1;
        private int fncTop_2;
        private double y1;     // assigned values in (s == 'B'){
        private double y2;     // assigned values in (s == 'B'){
        private double x3;     // assigned values in (s == 'B'){
        private double x4;     // assigned values in if(s == 'B'){
        private double localT;
        private bool inCircle;
        private bool translate;
        private bool moveAllAcross;
        private bool moveSel;
        private char charInput0;
        private char charInput1;
        private string? stringInput;
        private string? str1;
        private string? str2;
        private string? str3;
        private delegate void D1_Cir();
        private D1_Cir moveInCircle;
        private delegate void D2_ValTranslate();
        private D2_ValTranslate iterateMoveElls;
        private int[] fncAllTop = new int[4];
        private int[] fncAllLeft = new int[4];
        private int[] fncSelTop = new int[4];
        private int[] fncSelLeft = new int[4];
        private double[] rS = new double[4] {0.0, 0.0, 0.0, 0.0};
        private Ellipse[] ell = new Ellipse[4];
        public MainWindow()
        {
            InitializeComponent();
            fncCtr_1 = 0;
            fncCtr_2 = 0;
            fncCtr_3 = 0;
            fncCtr_4 = 0;
            digit1 = 0;
            digit2 = 0;
            stepB = 0;
            stepC = 0;
            stepS = 0;
//          x1 = 0;
            x2 = 0;
//          oldX2 = 0;
            mWPause = 99;
//          ellW = 0;
            ellipseN = 0;
            fncLeft_1 = 0;
            fncLeft_2 = 0;
            fncTop_1 = 0;
            fncTop_2 = 0;
//          ellipseRS = 0;
            y1 = 300.0;
            y2 = 0.0;
            x3 = 0.0;
            x4 = 0.0;
//          ellW = 0.0;
            localT = 0.0;
            inCircle = false;
            translate = true;
            moveAllAcross = false;
            moveSel = false;
            charInput0 = '\0';
            charInput1 = '\0';
            stringInput = null;
            str1 = null;
            str2 = null;
            str3 = null;
            LabelWPF1.Content = "  1: Red, 2: Yellow, 3: Green, 4: Blue,\n" +
            "  ^: Up, pg dn: Down, <: Left, >: Right,\n" +
            "  BackGround: 5: Red, 6: Yellow, 7: Green, 8: Blue, 9: Black\n" +
            "  0: White Press A to move circles across the monitor Press B\n" +
            "  to resize an individual circle Press C to resize all the circles\n" +
            "  Press S to control speed. Press Z to reset";
            moveInCircle = new D1_Cir(fncMoveInCircle);
            iterateMoveElls = new D2_ValTranslate(fncMoveEllipses);
            for(int i = 0; i < 4; i++)
               ell[i] = new Ellipse();
            ell[0] = ellipseR;
            ell[1] = ellipseY;
            ell[2] = ellipseG;
            ell[3] = ellipseB;
            for(int i = 0; i < 4; i++)
            {
                fncAllLeft[i] = (int)Canvas.GetLeft(ell[i]);
                fncAllTop[i] = (int)Canvas.GetTop(ell[i]);
                fncSelLeft[i] = fncAllLeft[i];
                fncSelTop[i] = fncAllTop[i];
            }
        }

        private void PauseIterateTr()
        {
            if(fncCtr_1 < mWPause)
            {
                startStopButton.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, iterateMoveElls);
                ++fncCtr_1;
                ++fncCtr_2;
            }
        }

        private void PauseIterateCr()
        {
            startStopButton.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, moveInCircle);
        }

        private void mW_KD(object sender, KeyEventArgs e)
        {
            stringInput = e.Key.ToString() + "a";
            charInput0 = stringInput[0];
            charInput1 = stringInput[1];

            if((stepB == 0) && (stepC == 0) && (stepS == 0))
               LabelWPF3.Content = null;

            if(charInput0.ToString() != "A")
            {
                fncCtr_2 = 0;
                fncCtr_3 = 0;
                fncCtr_4 = 0;
            }

            try
            {
                if((String.Compare('D'.ToString(), charInput0.ToString()) == 0) &&
                (Int32.TryParse(charInput1.ToString(), out digit1) == true)) // digit entry
                   fncCalls_1();
                else                                                         // character entry
                   fncCalls_2();
            }

            catch(SystemException ex)
            {
                str1 = $"\n\n\n\n{ex.Message}";
                str1 += $"\n                       {ex.StackTrace}";
                LabelWPF1.Content = str1;
            }
        }

        private void fncCalls_1()                               // digit entry
        {
            if((stepB == 0) && (stepC == 0) && (stepS == 0))    // cannot change the value of ellipseN while
            {                                                   // entering a value to resize ellipse
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

            if((stepB >= 1) && (stepB <= 4))
            {
                                                     // stepB is not incremented if the parse
                                                     // fails.
                if(stepB == 1)
                   digit2 = digit1 * 1000;

                if(stepB == 2)
                   digit2 += digit1 * 100;

                if(stepB == 3)
                   digit2 += digit1 * 10;

                if(stepB == 4)
                   digit2 += digit1;

                ++stepB;

                str2 = charInput1.ToString();
                str3 += str2;
                LabelWPF3.Content = str3;

                if(stepB == 5)
                {
                    scaleEllipses(ellipseN, digit2);
                    digit1 = 0;
                    digit2 = 0;
                    str2 = "Circle " + str1 + " is resized at " + str3;
                    LabelWPF3.Content = str2;
                    str2 = null;
                    str3 = null;
                    stepB = 0;
                }
            }
                                                     // resizes all the circles
                                                     // in order for the stepC condition to be met,
                                                     // the 'C' key must have been pressed.
            if((stepC >= 1) && (stepC <= 4))
            {
                                                     // stepC is not incremented if the parse
                                                     // fails.
                if(stepC == 1)
                   digit2 = digit1 * 1000;

                if(stepC == 2)
                   digit2 += digit1 * 100;

                if(stepC == 3)
                   digit2 += digit1 * 10;

                if(stepC == 4)
                   digit2 += digit1;

                ++stepC;

                str2 = charInput1.ToString();
                str3 += str2;

                LabelWPF3.Content = str3;

                if(stepC == 5)
                {
                    scaleEllipses(ellipseN, 10000, digit2);
                    digit1 = 0;
                    digit2 = 0;
                    str2 = "Circles are resized at " + str3;
                    LabelWPF3.Content = str2;
                    str2 = null;
                    str3 = null;
                    stepC = 0;
                }
            }
                                                     // determines speed across screen of the ellipses.
                                                     // in order for the stepS condition to be met,
                                                     // the 'S' key must have been pressed.
            if((stepS >= 1) && (stepS <= 2))
            {
                                                     // stepS is not incremented if the parse
                                                     // fails.
                if(stepS == 1)
                   digit2 = digit1 * 10;

                if(stepS == 2)
                   digit2 += digit1;

                ++stepS;

                str2 = charInput1.ToString();
                str3 += str2;

                LabelWPF3.Content = str3;

                if(stepS == 3)
                {
                    mWPause = digit2;
                    str2 = $"speed is  {mWPause}";
                    LabelWPF3.Content = str2;
                    digit1 = 0;
                    digit2 = 0;
                    str2 = null;
                    str3 = null;
                    stepS = 0;
                }
            }
        }

        private void fncCalls_2()
        {
            if((stepB != 0) || (stepC != 0) && (stepS != 0))
               LabelWPF3.Content = "Must enter a digit.";
            else
            {
                if(charInput0.ToString() == "A")
                {
                    translate = true;
                    moveAllAcross = true;
                    moveSel = false;
                    fncCtr_1 = 0;
                    fncLeft_1 = 1;
                    fncTop_1 = 0;
                    if(fncCtr_3 == 0)
                    {
                        for(int i = 0; i < 4; i++)
                        {
                            fncAllLeft[i] = (int)Canvas.GetLeft(ell[i]);
                            fncAllTop[i] = (int)Canvas.GetTop(ell[i]);
                            fncCtr_3 = 1;
                        }
                    }

                    if(fncCtr_2 > 1400)
                    {
                        fncCtr_2 = 0;
                        for(int i = 0; i < 4; i++)
                           fncAllLeft[i] = 5;
                    }

                    startStopButton.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, iterateMoveElls);
                }

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

                if(charInput0.ToString() == "S")
                {
                    LabelWPF3.Content = "Enter two digits to determine speed.";
                    stepS = 1;
                }

                if(charInput0.ToString() == "Z")
                {
                    for(int i = 0; i < 4; i++)
                       Canvas.SetTop(ell[i], (140 + (i * 150)) - rS[i]);
                }

                if((stringInput == "Righta") || (stringInput == "Lefta") ||
                (stringInput == "Upa") || (stringInput == "Downa"))
                {
                    translate = true;
                    moveAllAcross = false;
                    moveSel = true;
                    fncCtr_1 = 0;
                    if(fncCtr_4 == 0)
                    {
                        fncSelLeft[ellipseN] = (int)Canvas.GetLeft(ell[ellipseN]);
                        fncSelTop[ellipseN] = (int)Canvas.GetTop(ell[ellipseN]);
                        fncCtr_4 = 1;
                    }

                    if(stringInput == "Righta")
                    {
                        fncLeft_1 = 1;
                        fncTop_1 = 0;
                    }

                    if(stringInput == "Lefta")
                    {
                        fncLeft_1 = -1;
                        fncTop_1 = 0;
                    }

                    if(stringInput == "Upa")
                    {
                        fncLeft_1 = 0;
                        fncTop_1 = -1;
                    }

                    if(stringInput == "Downa")
                    {
                        fncLeft_1 = 0;
                        fncTop_1 = 1;
                    }

                    startStopButton.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, iterateMoveElls);
                }
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if(inCircle)
            {
                inCircle = false;
                startStopButton.Content = "MoveInCircle";
            }
            else
            {
                inCircle = true;
                startStopButton.Content = "Stop";
                startStopButton.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, moveInCircle);
            }
        }

        private void fncMoveInCircle()
        {
            x4 = x3 * Math.Cos(localT) - y1 * Math.Sin(localT);
            y2 = x3 * Math.Sin(localT) + y1 * Math.Cos(localT);
            Canvas.SetLeft(ell[ellipseN], (int)x4 + 630);
            Canvas.SetTop(ell[ellipseN], (int)y2 + 350);
            localT += .0002;
            if(localT > 6.26)
               localT = 0.0;

            if(inCircle)
               PauseIterateCr();
        }

        private void fncMoveEllipses()
        {
            int localEllipseN = 0;
            if((moveAllAcross == true) && (moveSel == false))
            {
                localEllipseN = 0;
                for(int i = 0; i < 4; i++)
                {
                    fncAllLeft[localEllipseN] += fncLeft_1;
                    fncAllTop[localEllipseN] += fncTop_1;
                    fncLeft_2 = fncAllLeft[localEllipseN];
                    fncTop_2 = fncAllTop[localEllipseN];

                    Canvas.SetLeft(ell[localEllipseN], fncLeft_2);
                    Canvas.SetTop(ell[localEllipseN], fncTop_2);

                    ++localEllipseN;
                }
            }

            if((moveAllAcross == false) && (moveSel == true))
            {
                localEllipseN = ellipseN;
                fncSelLeft[localEllipseN] += fncLeft_1;
                fncSelTop[localEllipseN] += fncTop_1;
                fncLeft_2 = fncSelLeft[localEllipseN];
                fncTop_2 = fncSelTop[localEllipseN];

                Canvas.SetLeft(ell[localEllipseN], fncLeft_2);
                Canvas.SetTop(ell[localEllipseN], fncTop_2);
            }

            if(translate == true)
               PauseIterateTr();
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
