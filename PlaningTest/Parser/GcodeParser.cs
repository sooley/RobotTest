
using GlmSharp;
using PlaningTest.Parser;
using PlaningTest.Static;
using PlaningTest.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest
{

    public class gc_modal_t 
    {
        public int motion;          // {G0,G1,G2,G3,G38.2,G80}
        public int feed_rate;       // {G93,G94}
        public int units;           // {G20,G21}
        public int distance;        // {G90,G91}
                                    // uint8_t distance_arc; // {G91.1} NOTE: Don't track. Only default supported.
        public int plane_select;    // {G17,G18,G19}
                                    // uint8_t cutter_comp;  // {G40} NOTE: Don't track. Only default supported.
        public int tool_length;     // {G43.1,G49}
        public int coord_select;    // {G54,G55,G56,G57,G58,G59}
                                    // uint8_t control;      // {G61} NOTE: Don't track. Only default supported.
        public int program_flow;    // {M0,M1,M2,M30}
        public int coolant;         // {M7,M8,M9}
        public int spindle;         // {M3,M4,M5}
        public gc_modal_t()
        {

        }
    }
    public class gc_values_t
    {
        public float f;         // Feed
        public float []ijk;    // I,J,K Axis arc offsets
        public byte l;       // G10 or canned cycles parameters
        public int n;       // Line number
        public float p;         // G10 or dwell parameters
                         // float q;      // G82 peck drilling
        public float r;         // Arc radius
        public float s;         // Spindle speed
        public byte t;       // Tool selection
        public float []xyz;    // X,Y,Z Translational axes

        public gc_values_t()
        {
            ijk = new float[3];
            xyz = new float[3];
        }
    }
    public class parser_state
    {
        public gc_modal_t modal;
        public float spindle_speed;
        public float feed_rate;
        public byte tool;
        public int line_number;
        public float[] position;
        public float[] coord_system;
        public float[] coord_offset;
        public float tool_length_offset;
        public parser_state()
        {
            position = new float[3];
            coord_system = new float[3];
            coord_offset = new float[3];
        }
    }
    public class parser_block_t
    {
        public byte non_modal_command;
        public gc_modal_t modal;
        public gc_values_t values;
        public parser_block_t()
        {
            modal = new gc_modal_t();
        }
    }
    public class GcodeParser
    {
        private bool m_isMetric;
        private bool m_isAbsoluteMode;
        private bool m_isAbsoluteIJKMode;
        private float m_lastGcodeCommand;
        private vec3 m_currentPoint;
        private int m_CommandNumber;
        private planes m_CurrentPlane;
        private bool m_retractOldZ;
        private double m_Angle;
        private bool m_isRotationMove;
        private double m_lastDrillingDepth;
        //Setting
        private double m_SpeedOverride;
        private int m_truncateDecimalLength;
        private bool m_removeAllWhitespace;
        private bool m_convertArcsToLines;
        private double m_smallArcThreshold;
        // Not configurable outside, but maybe it should be.
        private double m_smallArcSegmentLength;
        private List<PointSegment> m_points;
        private double m_lastSpeed;
        private double m_traverseSpeed;
        private double m_lastSpindleSpeed;
        public GcodeParser()
        {
            m_isMetric = true;
            m_isAbsoluteMode = true;
            m_isAbsoluteIJKMode = false;
            m_lastGcodeCommand = -1;
            m_retractOldZ = true;
            m_CommandNumber = 0;
            m_Angle = 0;
            m_isRotationMove = false;
            m_SpeedOverride = -1;
            m_truncateDecimalLength = 40;
            m_removeAllWhitespace = true;
            m_convertArcsToLines = false;
            m_smallArcThreshold = 2.0;
            m_smallArcSegmentLength = 0.3;
            m_lastSpeed = 0;
            m_lastSpindleSpeed = 0;
            m_traverseSpeed = 300;
            m_lastDrillingDepth = Double.NaN;
            m_currentPoint = new vec3(0, 0, 0);
            m_points = new List<PointSegment>();
        }
        private void reset(vec3 initialPoint)
        {
            m_points.Clear();
            m_currentPoint = new vec3(0, 0, 0);
            m_CurrentPlane = planes.XY;
            m_points.Add(new PointSegment(m_currentPoint, -1));
        }
        public bool GetConvertArcsToLine()
        {
            return m_convertArcsToLines;
        }
        public void SetConvertArcToLine(bool ConvertArcToLine)
        {
            m_convertArcsToLines = ConvertArcToLine;
        }
        public bool GetRomoveAllWhiteSpace()
        {
            return m_removeAllWhitespace;
        }
        public void SetRemoveAllWhiteSpace(bool RemoveAllWhiteSpace)
        {
            m_removeAllWhitespace = RemoveAllWhiteSpace;
        }
        public void SetSmallArcSegmentLength(double SmallArcSegmentLength)
        {
            m_smallArcSegmentLength = SmallArcSegmentLength;
        }
        public double GetSmallArcSegmentLength()
        {
            return m_smallArcSegmentLength;
        }
        public double GetSmallArcThresHold()
        {
            return m_smallArcThreshold;
        }
        public void SetSmallArcThresHold(double SmallArcThresHold)
        {
            m_smallArcThreshold = SmallArcThresHold;
        }
        public double GetSpeedOverride()
        {
            return m_SpeedOverride;
        }
        public void SetSpeedOverride(double SpeedOverride)
        {
            m_SpeedOverride = SpeedOverride;
        }
        public int GetTruncateDecimalLength()
        {
            return m_truncateDecimalLength;
        }
        public void SetTruncateDecimalLength(int TruncateDecimalLength)
        {
            m_truncateDecimalLength = TruncateDecimalLength;
        }
        public PointSegment addCommand(String cmd)
        {
            String stripped = GcodePreProcessorUtils.removeComment(cmd);
            List<String> arg = GcodePreProcessorUtils.splitCommand(stripped);
            return addCommand(arg);
        }
        public PointSegment addCommand(List<String> args)
        {
            if (args.Count == 0)
            {
                return null;
            }
            return ProcessCommand(args);
        }
        public PointSegment ProcessCommand(List<String> args)
        {
            List<float> gcodes;
            PointSegment ps = new PointSegment();
            //Handle F code
            double speed = GcodePreProcessorUtils.parseCoord(args, 'F');
            if (!Double.IsNaN(speed)) m_lastSpeed = m_isMetric ? speed : speed * 25.4;
            //Handle S code
            double SpindleSpeed = GcodePreProcessorUtils.parseCoord(args, 'S');
            if (!Double.IsNaN(SpindleSpeed)) m_lastSpindleSpeed = SpindleSpeed;
            //Handle P code
            double dwell = GcodePreProcessorUtils.parseCoord(args, 'P');
            if (!Double.IsNaN(dwell)) m_points.Last().setDwell(dwell);
            gcodes = GcodePreProcessorUtils.parseCodes(args, 'G');
            if (gcodes.Count == 0 && m_lastGcodeCommand != -1)
            {
                gcodes.Add(m_lastGcodeCommand);
            }
            foreach (float code in gcodes)
            {
                ps = handleGcode(code, args);
            }
            return ps;
        }

        public PointSegment addArcPointSegment(vec3 nextpoint, bool clockwise, List<String> args)
        {
            PointSegment ps = new PointSegment(nextpoint, m_CommandNumber++);
            vec3 center = GcodePreProcessorUtils.updateCenterWithCommand(args, m_currentPoint, nextpoint, m_isAbsoluteIJKMode, clockwise);
            double radius = GcodePreProcessorUtils.parseCoord(args, 'R');
            if (Double.IsNaN(radius))
            {
                mat4 m = new mat4();
                m = mat4.Identity;

                switch (m_CurrentPlane)
                {
                    case planes.XY:
                        break;
                    case planes.ZX:
                        m = mat4.RotateX(90) * m;
                        break;
                    case planes.YZ:
                        m = mat4.RotateY(-90) * m;
                        break;
                }
                radius = Math.Sqrt(Math.Pow(MathHandle.Tranformnormal(m_currentPoint, m).x - MathHandle.Tranformnormal(center, m).x, 2) + Math.Pow(MathHandle.Tranformnormal(m_currentPoint, m).y - MathHandle.Tranformnormal(center, m).y, 2));
                //Debug.WriteLine("addArcPointSegment X:" + center.X.ToString() + "Y:" + center.Y.ToString() + "Z" + center.Z.ToString()+"R:"+radius.ToString());
            }
            ps.setIsMetric(m_isMetric);
            ps.SetArcCenter(center);
            ps.setIsArc(true);
            ps.setRadius(radius);
            ps.setIsClockwise(clockwise);
            ps.SetisAbsolute(m_isAbsoluteMode);
            ps.setSpeed(m_lastSpeed);
            ps.setSpindleSpeed(m_lastSpindleSpeed);
            ps.setPlane(m_CurrentPlane);
            m_points.Add(ps);
            m_currentPoint = nextpoint;
            //ps.displaydata();
            return ps;
        }
        private PointSegment handleGcode(float code, List<String> args)
        {
            PointSegment ps = null;
            //foreach (var t in args)
            //{
            //    Debug.WriteLine("commandArgs:" + t);
            //}
            vec4 next = GcodePreProcessorUtils.updatePointWithCommand(args, m_currentPoint, m_isAbsoluteMode);
            vec3 nextpoint = new vec3(next.x, next.y, next.z);
            //Debug.WriteLine("handleGcode:X" + nextpoint.X.ToString() + "Y" + nextpoint.Y.ToString() + "Z" + nextpoint.Z.ToString());
            if (!Double.IsNaN(next.w))
            {
                m_Angle = next.w;
                m_isRotationMove = true;
            }
            if (code == 0.0) ps = addLinearPointSegment(nextpoint, true);
            else if (code == 1.0) ps = addLinearPointSegment(nextpoint, false);
            else if (code == 38.2f) ps = addLinearPointSegment(nextpoint, false);
            else if (code == 2.0f) ps = addArcPointSegment(nextpoint, true, args);
            else if (code == 3.0f) ps = addArcPointSegment(nextpoint, false, args);
            else if (code == 17.0f) m_CurrentPlane = planes.XY;
            else if (code == 18.0f) m_CurrentPlane = planes.ZX;
            else if (code == 19.0f) m_CurrentPlane = planes.YZ;
            //else if (code == 20.0f) m_isMetric = false;
            //else if (code == 17.0f) m_isMetric = true;
            //else if (code == 33.0f) ps = addLinearPointSegment(nextpoint, false);
            //else if (code == 90.0f) m_isAbsoluteMode = true;
            //else if (code == 90.1f) m_isAbsoluteIJKMode = true;
            //else if (code == 91.0f) m_isAbsoluteMode = false;
            //else if (code == 91.1f) m_isAbsoluteIJKMode = false;
            //else if (code == 98.0f) m_retractOldZ = true;
            //else if (code == 99.0f) m_retractOldZ = false;
            //else if (code == 76.0)
            //{
            //    Vector3 dl_start = m_currentPoint;
            //    Vector3 dl_end = m_currentPoint;
            //    dl_end.Z = nextpoint.Z;
            //    char c;
            //    double p = Double.NaN;
            //    double j = Double.NaN;
            //    double k = Double.NaN;
            //    for (int i = 0; i < args.Count; i++)
            //    {
            //        if (args.ElementAt(i).Length > 0)
            //        {
            //            c = Char.Parse(args.ElementAt(i).Substring(0, 1).ToUpper());
            //            switch (c)
            //            {
            //                case 'I':
            //                    p = Double.Parse(args.ElementAt(i).Substring(1));
            //                    break;
            //                case 'J':
            //                    j = Double.Parse(args.ElementAt(i).Substring(1));
            //                    break;
            //                case 'K':
            //                    k = Double.Parse(args.ElementAt(i).Substring(1));
            //                    break;
            //            }
            //        }
            //    }
            //    float fin_depth = (float)(m_currentPoint.X + p - k);
            //    dl_start.X = fin_depth;
            //    addLinearPointSegment(dl_start, true);
            //    dl_start.Z = dl_end.Z;
            //    addLinearPointSegment(dl_start, false);
            //    ps = addLinearPointSegment(dl_end, true);
            //}
            //else if (code == 81.0f || code == 82.0f || code == 83.0f)
            //{
            //    Vector3 tmp = nextpoint;
            //    tmp.Z = m_currentPoint.Z;
            //    addLinearPointSegment(tmp, true);
            //    if (!Double.IsNaN(m_lastDrillingDepth))
            //    {
            //        nextpoint.Z = (float)m_lastDrillingDepth;
            //    }
            //    addLinearPointSegment(nextpoint, false);
            //    m_lastDrillingDepth = nextpoint.Z;
            //    addLinearPointSegment(tmp, true);
            //}
            if (code == 0.0f || code == 1.0f || code == 2.0f || code == 3.0f) m_lastGcodeCommand = code;
            //if (code > 80.0 && code < 84.0)
            //{
            //    m_lastGcodeCommand = code;
            //}
            //else
            //{
            //    m_lastDrillingDepth = Double.NaN;
            //}
            return ps;
        }

        private PointSegment addLinearPointSegment(vec3 nextpoint, bool fasttraverse)
        {
            bool zOnly = false;
            PointSegment ps = new PointSegment();
            if ((m_currentPoint.x == nextpoint.x) && (m_currentPoint.y == nextpoint.y) && (m_currentPoint.z != nextpoint.z))
            {
                zOnly = true;
            }
            if ((m_Angle < 0.0001) && (m_Angle > -0.0001))
            {
                //Debug.WriteLine("addLinearPointSegment" + "X" + nextpoint.X.ToString() + "Y" + nextpoint.Y.ToString() + "Z" + nextpoint.Z.ToString());
                ps = new PointSegment(nextpoint, m_CommandNumber++);
                ps.setIsMetric(m_isMetric);
                ps.setIsZMovement(zOnly);
                ps.setIsFastTraverse(fasttraverse);
                ps.SetisAbsolute(m_isAbsoluteMode);
                ps.setSpeed(fasttraverse ? m_traverseSpeed : m_lastSpeed);
                ps.setSpindleSpeed(m_lastSpindleSpeed);
                m_points.Add(ps);
            }
            else
            {
                vec3 n = GcodePreProcessorUtils.rotateAxis(nextpoint, m_Angle);
                ps = new PointSegment(n, m_CommandNumber++);
                if (m_isRotationMove)
                {
                    vec3 center = new vec3(m_currentPoint.x, 0, 0);
                    double radius = Math.Sqrt(Math.Pow(m_currentPoint.z, 2) + Math.Pow(m_currentPoint.y, 2));
                    m_isRotationMove = false;
                    ps.setIsMetric(m_isMetric);
                    ps.SetArcCenter(center);
                    ps.setIsArc(true);
                    ps.setRadius(radius);
                    ps.setIsClockwise(true);
                    ps.SetisAbsolute(m_isAbsoluteMode);
                    ps.setIsFastTraverse(fasttraverse);
                    ps.setSpeed(fasttraverse ? m_traverseSpeed : m_lastSpeed);
                    ps.setSpindleSpeed(m_lastSpindleSpeed);
                    ps.setPlane(planes.YZ);
                    m_points.Add(ps);
                }
                else
                {
                    ps.setIsMetric(m_isMetric);
                    ps.setIsZMovement(zOnly);
                    ps.setIsFastTraverse(fasttraverse);
                    ps.SetisAbsolute(m_isAbsoluteMode);
                    ps.setSpeed(fasttraverse ? m_traverseSpeed : m_lastSpeed);
                    ps.setSpindleSpeed(m_lastSpindleSpeed);
                    m_points.Add(ps);
                }
            }
            //ps.displaydata();
            m_currentPoint = nextpoint;
            //Debug.WriteLine("Linear_Point: X" + ps.Point().X.ToString() + "Y" + ps.Point().Y.ToString());  
            return ps;
        }
        private List<PointSegment> ExpandArc()
        {
            PointSegment StartSegment = m_points[m_points.Count - 2];
            PointSegment LastSegment = m_points[m_points.Count - 1];
            List<PointSegment> empty = new List<PointSegment>();
            if (!StartSegment.isArc())
            {
                return empty;
            }
            vec3 start = StartSegment.Point();
            vec3 end = LastSegment.Point();
            vec3 center = LastSegment.center();
            double radius = LastSegment.getRadius();
            bool clockwise = LastSegment.isClockWise();
            planes plane = StartSegment.plane();
            List<vec3> expandedpoints = GcodePreProcessorUtils.generatePointsAlongArcBDring(plane, start, end, center, clockwise, radius, m_smallArcThreshold, m_smallArcThreshold, false);
            if (expandedpoints.Count == 0)
            {
                return empty;
            }
            m_points.RemoveAt(m_points.Count - 1);
            m_CommandNumber--;
            List<PointSegment> psl = new List<PointSegment>();
            List<vec3> psl1 = expandedpoints;
            EnumeratorAdapter<vec3> psl2 = new EnumeratorAdapter<vec3>(psl1.GetEnumerator());
            PointSegment temp = new PointSegment();
            if (psl2.HasNext) psl2.Next();
            while (psl2.HasNext)
            {
                temp = new PointSegment(psl2.Next(), m_CommandNumber++);
                temp.setIsMetric(LastSegment.IsMetric());
                m_points.Add(temp);
                psl.Add(temp);
            }

            //Update the new end points
            m_currentPoint.x = m_points.Last().Point().x;
            m_currentPoint.y = m_points.Last().Point().y;
            m_currentPoint.z = m_points.Last().Point().z;
            return psl;
        }
        private List<String> preprocessCommand(List<String> Command)
        {
            List<String> result = new List<string>();
            foreach (String cmd in Command)
            {
                result.AddRange(preprocessCommand(cmd));
            }
            return result;
        }
        private List<String> preprocessCommand(String Command)
        {
            List<String> result = new List<string>();
            bool HasComment = false;
            //Remove Comment
            String newCommand = GcodePreProcessorUtils.removeComment(Command);
            String rawCommand = newCommand;
            HasComment = (newCommand.Length != Command.Length);
            if (m_removeAllWhitespace)
            {
                newCommand = GcodePreProcessorUtils.removeAllWhitespace(newCommand);
            }
            if (newCommand.Length > 0)
            {
                if (m_SpeedOverride > 0)
                {
                    newCommand = GcodePreProcessorUtils.overrideSpeed(newCommand, m_SpeedOverride);
                }
                if (m_truncateDecimalLength > 0)
                {
                    newCommand = GcodePreProcessorUtils.truncateDecimals(m_truncateDecimalLength, newCommand);
                }
                if (m_convertArcsToLines)
                {
                    List<String> ArcLines = ConvertArcToLines(newCommand);
                    if (ArcLines.Count > 0)
                    {
                        result.AddRange(ArcLines);
                    }
                    else
                    {
                        result.Add(newCommand);
                    }
                }
                else if (HasComment)
                {
                    result.Add(Command.Replace(rawCommand, newCommand));
                }
                else
                {
                    result.Add(Command);
                }
            }
            return result;
        }
        public List<String> ConvertArcToLines(String Command)
        {
            List<String> results = new List<string>();
            vec3 start = m_currentPoint;
            PointSegment ps = addCommand(Command);
            if (ps == null || !ps.isArc())
            {
                return results;
            }
            List<PointSegment> psl = ExpandArc();
            if (psl.Count == 0)
            {
                return results;
            }
            foreach (PointSegment segment in psl)
            {
                vec3 end = segment.Point();
                results.Add(GcodePreProcessorUtils.generateG1FromPoints(start, end, m_isAbsoluteMode, m_truncateDecimalLength));
                start = segment.Point();
            }

            return results;
        }
        public List<PointSegment> getPointSegMentList()
        {
            return m_points;
        }
        public double getTraverseSpeed()
        {
            return m_traverseSpeed;
        }
        public void setTraverseSpeed(double traversespeed)
        {
            m_traverseSpeed = traversespeed;
        }
        public int getCommandNumber()
        {
            return m_CommandNumber - 1;
        }
    }
    }
