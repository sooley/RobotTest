using cszmcaux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaningTest.Motion
{
    public class ZMC
    {
        public IntPtr g_handle;
        public ZMC()
        {

        }
        public int Connect(String type, String IP_Address)
        {
            int iresult;
            string String;
            String = type;

            string Buffer;
            Buffer = IP_Address;

            int ctype = 0;
            if (String == "Ethernet")
            {
                ctype = 2;
            }
            else if (String == "Serial")
            {
                ctype = 1;
            }
            else if (String == "PCI")
            {
                ctype = 4;
            }
            else if (String == "LOCAL")
            {
                ctype = 5;
            }
            else
            {
                MessageBox.Show("Please Select Correct Connection Type!");
                return -1;
            }

            iresult = zmcaux.ZAux_FastOpen(ctype, Buffer, 1000, out g_handle);

            if (iresult != 0)
            {
                g_handle = (IntPtr)0;
                MessageBox.Show("Connect Failed");
                return -2;
            }
            else
            {
                for (int i = 0; i < 4; i++)     //initialize axis parameters
                {
                    zmcaux.ZAux_Direct_SetAtype(g_handle, i, 1);        //axis type, pulse axis
                    zmcaux.ZAux_Direct_SetUnits(g_handle, i, 1);          //UNITS (pulse amount) -- take pulse as the unit
                }
                return 1;
            }
        }

        public void Stop()
        {
            ThreadStart threadStart = new ThreadStart(run_move);
            Thread thread = new Thread(threadStart);
            thread.Priority = ThreadPriority.Highest;

            if (g_handle == (IntPtr)0)
            {
                MessageBox.Show("No Connected!", "hint");
            }
            else
            {
                thread.Abort();
                zmcaux.ZAux_Direct_Single_Cancel(g_handle, 0, 2);        //cancel main axis motion
            }
        }
        public void run_move()
        {
            int[] axislist = { 0, 1, 2, 3, 4 };     //axis list
            int iresult = 0;
            int iremain = 0;
            float[] destdis = { 0, 0, 0, 0, 100 };

            zmcaux.ZAux_Direct_SetSpeed(g_handle, axislist[0], 200); //set interpolation speed
            zmcaux.ZAux_Direct_SetAccel(g_handle, axislist[0], 200); //set interpolation acceleration
            zmcaux.ZAux_Direct_SetDecel(g_handle, axislist[0], 200); //set interpolation deceleration

            //zmcaux.ZAux_Direct_SetMerge(g_handle, axislist[0], 1); //open continuous interpolation
            //zmcaux.ZAux_Direct_SetLspeed(g_handle, axislist[0], Convert.ToSingle(textBox_lsp.Text)); ////initial speed, corner deceleration is linear deceleration by motion speed -- initial speed
            //zmcaux.ZAux_Direct_SetCornerMode(g_handle, axislist[0], corner_mode);//corner mode
            //zmcaux.ZAux_Direct_SetDecelAngle(g_handle, axislist[0], (float)(Convert.ToSingle(textBox_ang1.Text) * 3.14 / 180));//starting deceleration angle, convert to radian
            //zmcaux.ZAux_Direct_SetStopAngle(g_handle, axislist[0], (float)(Convert.ToSingle(textBox_ang2.Text) * 3.14 / 180)); //stop deceleration angle, convert to radian
            //zmcaux.ZAux_Direct_SetFullSpRadius(g_handle, axislist[0], Convert.ToSingle(textBox_radio.Text)); //small round radius
            //zmcaux.ZAux_Direct_SetZsmooth(g_handle, axislist[0], Convert.ToSingle(textBox_zsmooth.Text)); //chamfer

            zmcaux.ZAux_Direct_SetMovemark(g_handle, axislist[0], 0);	//set MARK = 0, read CURMARK to see where executes now

            zmcaux.ZAux_Direct_Base(g_handle, 4, axislist); //base axis / base axes

            for (int i = 0; i < 200; i++)
            {
                zmcaux.ZAux_Direct_SetForceSpeed(g_handle, 0, destdis[4]);    //set each segment's speed
                iresult = zmcaux.ZAux_Direct_MoveAbsSp(g_handle, 4, axislist, destdis);
                while (iresult != 0)            //if 0 is returned, it means failure, buffer may be full, resend.
                {
                    zmcaux.ZAux_Direct_SetForceSpeed(g_handle, 0, destdis[4]);    //set each segment's speed
                    iresult = zmcaux.ZAux_Direct_MoveAbsSp(g_handle, 4, axislist, destdis);
                }

                destdis[0] = destdis[0] + 20;
                destdis[1] = destdis[1] + 20;
                destdis[2] = destdis[2] + 20;
                destdis[3] = destdis[3] + 20;
                destdis[4] = 100 + (i % 10) * 10;
            }
        }
    }
}
