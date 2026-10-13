using PlaningTest;
using PlaningTest.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.User
{
    public class LoadFileTest
    {
        private setting m_setting;
        public GcodeViewParse _gvp;
        private static bool m_programLoading=false;
        public LoadFileTest()
        {
            _gvp = new GcodeViewParse();
        }
        public void LoadFile(List<String> data)
        {
            m_setting = new setting();
            m_setting.setArcLength(8);
            GcodeParser gp = new GcodeParser();
            //gp.SetSmallArcThresHold(2);
            //gp.SetConvertArcToLine(true);
            m_programLoading = true;
            String command;
            String stripped;
            String trimmed;
            List<String> args;
            //GcodeItem item;
            while (data.Count != 0)
            {
                command = data.Take(1).First();
                //Debug.WriteLine("Cmd:"+command);
                //Trim command
                trimmed = command.Trim();
                data.Remove(command);
                
                if (trimmed.Length != 0)
                {
                    //Split command
                    stripped = GcodePreProcessorUtils.removeComment(command);
                    args = GcodePreProcessorUtils.splitCommand(stripped);
                    //display(args);
                    PointSegment m_p = gp.addCommand(args);
                    //data.Remove(command);

                    //item.command = trimmed;
                    //item.state = GcodeItem.States.InQueue;
                    //item.Line = gp.getCommandNumber();
                    //item.args = args;
                }

            }
            _gvp.getLinesFromParser(gp, m_setting.arcLength(), m_setting.ArcDeGreeMode);
        }
        public void display(List<string> a)
        {
            foreach (string t in a)
            {
                Debug.WriteLine(t);
            }
        }
        public void LoadFile(String filename)
        {
            List<string> data = new List<string>();

            using (StreamReader sr = File.OpenText(filename))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    data.Add(s);
                }
                LoadFile(data);
            }
        }
    }
}
