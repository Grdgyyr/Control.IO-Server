using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Control_IO;
using Control_IO.Misc;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;


namespace udptest.Operation
{
    class ComputeData
    {

        
        
        


        public static void DecodeByteMic(string receivedData, byte[] data)
        {
            
            string[] array = receivedData.Split('|');

            IWavePlayer player = new WaveOut(WaveCallbackInfo.FunctionCallback());
            

            IWaveProvider provider = new RawSourceWaveStream(
                     new System.IO.MemoryStream(data), new WaveFormat(48000, 16, 1));



            player.Init(provider);
            player.Play();
            //mic
            
        }

        public static void ComputeReceivedData(string receivedData, string _decimalSeparator)
        {
            FormMain_UpdatedUI form;
            float _x, _y;
            string[] array = receivedData.Split('|');        
            


            //Gesture
            if (array[0].Contains("GESTURE"))
            {
                if (array.Count() == 2 && !array[0].Contains("GESTUREARRAY"))
                    ComputeGesture.Compute(array[1]);
                else if (array[0].Contains("GESTUREARRAY"))
                    ComputeGesture.UpdateGestures(array);
            }

            

            if (array.Count() == 1)
            {                
                if (array[0] == "delete")              
                    SendKeys.SendWait("{BACKSPACE}");                
                else if (array[0] == "enter")                
                    SendKeys.SendWait("{ENTER}");            
                else if (array[0] == "shake")
                    System.Diagnostics.Process.Start("notepad.exe");
                else if (array[0] == "swipeup")
                    System.Diagnostics.Process.Start("Calc");
                else if (array[0] == "swipedown")
                    System.Diagnostics.Process.Start("mspaint");
                else if (array[0] == "wave")
                    System.Diagnostics.Process.Start("http://www.google.com");
                else if (array[0] == "present")
                    SendKeys.SendWait("{F11}");
                else if (array[0] == "right")
                    SendKeys.SendWait("{RIGHT}");
                else if (array[0] == "forward")
                    SendKeys.SendWait("{RIGHT}");
                else if (array[0] == "left")
                    SendKeys.SendWait("{LEFT}");
                else if (array[0] == "back")
                    SendKeys.SendWait("{LEFT}");
                else if (array[0] == "arrowup")
                    SendKeys.SendWait("{UP}");
                else if (array[0] == "arrowdown")
                    SendKeys.SendWait("{DOWN}");
                else if (array[0] == "escape")
                    SendKeys.SendWait("{ESC}");

            }
            if (array.Count() == 2)
            {
                string text = array[0];
                string text2 = array[1];
                if (text == "keyboard")
                {
                    //text2 = text2.Replace("+", "{+}");
                    //text2 = text2.Replace("^", "{^}");
                    //text2 = text2.Replace("%", "{%}");
                    //text2 = text2.Replace("~", "{~}");
                    //text2 = text2.Replace("(", "{(}");
                    //text2 = text2.Replace(")", "{)}");
                    int unicode = Int32.Parse(text2);
                    SendKeys.SendWait((char)unicode + "");
                    return;
                }
                float x = default(float);
                float y = default(float);
                if (float.TryParse(text, out x) && float.TryParse(text2, out y))
                {
                    _x = x;
                    _y = y;
                }
                if (text == "volume")
                {
                    if (text2 == "down")
                    {
                        OperationData.VolumeDown();
                    }
                    else if (text2 == "up")
                    {
                        OperationData.VolumeUP();
                    }
                }
                else if (text == "down")
                {
                    if (text2 == "left")
                    {
                        OperationData.MouseLeftDown();
                    }
                    else if (text2 == "right")
                    {
                        OperationData.MouseRightDown();
                    }
                }
                else if (text == "up")
                {
                    if (text2 == "left")
                    {
                        OperationData.MouseLeftUp();
                    }
                    else if (text2 == "right")
                    {
                        OperationData.MouseRightUp();
                    }
                }
                else if (text == "wheel")
                {
                    if (text2 == "up")
                    {
                        OperationData.MouseWheelClick();
                    }
                    else if (string.Empty != text2)
                    {
                        text2 = text2.Replace(".", _decimalSeparator);
                        float value = default(float);
                        if (float.TryParse(text2, out value))
                        {
                            OperationData.MousewheelScroll(Convert.ToInt32(value));
                        }
                    }
                }
                else
                {
                    text = text.Replace(".", _decimalSeparator);
                    text2 = text2.Replace(".", _decimalSeparator);
                    int num = Cursor.Position.X;
                    int num2 = Cursor.Position.Y;
                    int num3 = 0;
                    int num4 = 0;
                    double num5 = default(double);
                    if (double.TryParse(text, out num5) && Math.Abs(num5) > 0.035000000149011612)
                    {
                        num4 = (int)num5;
                        num2 -= num4;
                    }
                    double num6 = default(double);
                    if (double.TryParse(text2, out num6) && Math.Abs(num6) > 0.035000000149011612)
                    {
                        num3 = (int)num6;
                        num -= num3;
                    }
                    Cursor.Position = new Point(num, num2);
                }
            }
        }
    }
}
