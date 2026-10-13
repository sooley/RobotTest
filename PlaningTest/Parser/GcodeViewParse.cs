using GlmSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Parser
{
    public class GcodeViewParse
    {
        private bool absoluteMode;
        private bool absoluteIJK;
        private vec3 m_min, m_max;
        private double m_minlength;
        private List<LineSegment> m_lines;
        private List<int>[] m_lineIndexes;
        private int currentLine;
        public GcodeViewParse()
        {
            absoluteMode = true;
            absoluteIJK = false;
            currentLine = 0;
            m_min = new vec3(float.NaN, float.NaN, float.NaN);
            m_max = new vec3(float.NaN, float.NaN, float.NaN);
            m_minlength = Double.NaN;
            m_lines = new List<LineSegment>();
        }
        public vec3 GetMinimumExtremes()
        {
            return m_min;
        }
        public vec3 GetMaximumExtremes()
        {
            return m_max;
        }
        public double GetminLength()
        {
            return m_minlength;
        }
        public Size GetResolution()
        {
            return new Size((int)((m_max.x - m_min.x) / m_minlength + 1), (int)((m_max.y - m_min.y) / m_minlength + 1));
        }
        public List<LineSegment> toObjRedux(List<String> gcode, double arcPrecision, bool arcDegreeMode)
        {
            GcodeParser gp = new GcodeParser();
            foreach (String s in gcode)
            {
                gp.addCommand(s);
            }
            return getLinesFromParser(gp, arcPrecision, arcDegreeMode);
        }
        public List<LineSegment> getLineSegmentList()
        {
            return m_lines;
        }
        public List<LineSegment> getLinesFromParser(GcodeParser gp, double arcPrecision, bool arcDegreeMode)
        {
            List<PointSegment> psl = gp.getPointSegMentList();
            m_lines.Clear();
            double minArcLength = 1;
            vec3 start, end;
            start = vec3.Zero;
            end = vec3.Zero;
            LineSegment ls = new LineSegment();
            //Array.Resize(ref m_lineIndexes,psl.Count);
            int lineIndex = 0;
            m_lineIndexes = new List<int>[psl.Count];
            for (int i = 0; i < m_lineIndexes.Length; i++)
            {
                m_lineIndexes[i] = new List<int>();
            }
            foreach (PointSegment segment in psl)
            {
                PointSegment ps = segment;
                bool isMetric = ps.IsMetric();
                ps.converttoMeTric();
                end = ps.Point();
                //Debugdisplay.display_PointSeg(ps);
                if (start != null)
                {
                    if (ps.isArc())
                    {
                        List<vec3> points = GcodePreProcessorUtils.generatePointsAlongArcBDring(ps.plane(), start, end, ps.center(), ps.isClockWise(), ps.getRadius(), minArcLength, arcPrecision, arcDegreeMode);
                        //Debug.WriteLine("generatePointsAlongArcBDring");
                        //Debugdisplay.display_PointVecList(points);
                        //Create Linesegment from point
                        if (points.Count > 0)
                        {
                            vec3 StartPoint = start;
                            foreach (vec3 nextpoint in points)
                            {
                                if (nextpoint == StartPoint) continue;
                                ls = new LineSegment(StartPoint, nextpoint, lineIndex);
                                ls.setisArc(ps.isArc());
                                ls.setIsClockwise(ps.isClockWise());
                                ls.Setplane(ps.plane());
                                ls.setisFastTrverse(ps.IsFastTraverse());
                                ls.setisZMovement(ps.isZMovement());
                                ls.setIsMetric(isMetric);
                                ls.setIsAbsolute(ps.isAbsolute());
                                ls.setSpeed(ps.GetSpeed());
                                ls.setSpindleSpeed(ps.getSpindleSpeed());
                                ls.setDwell(ps.getDwell());
                                testExtremes(nextpoint);
                                m_lines.Add(ls);
                                m_lineIndexes[ps.GetLineNumber()].Add(m_lines.Count - 1);
                                StartPoint = nextpoint;
                            }
                            lineIndex++;
                        }
                    }
                    else
                    {
                        //Debugdisplay.display_PointVec(end);
                        ls = new LineSegment(start, end, lineIndex++);
                        ls.setisArc(ps.isArc());
                        ls.setisFastTrverse(ps.IsFastTraverse());
                        ls.setisZMovement(ps.isZMovement());
                        ls.setIsMetric(isMetric);
                        ls.setIsAbsolute(ps.isAbsolute());
                        ls.setSpeed(ps.GetSpeed());
                        ls.setSpindleSpeed(ps.getSpindleSpeed());
                        ls.setDwell(ps.getDwell());
                        m_lines.Add(ls);
                        testExtremes(end);
                        testLength(start, end);
                        m_lineIndexes[ps.GetLineNumber()].Add(m_lines.Count - 1);
                    }

                }
                start = end;
            }
            return m_lines;
        }
        public List<LineSegment> getlines()
        {
            return m_lines;
        }
        public List<int>[] GetLineIndexes()
        {
            return m_lineIndexes;
        }
        private void testExtremes(vec3 p3d)
        {
            testExtremes(p3d.x, p3d.y, p3d.z);
        }
        private void testExtremes(double x, double y, double z)
        {
            m_min.x = Math.Min(m_min.x, (float)x);
            m_min.y = Math.Min(m_min.y, (float)y);
            m_min.z = Math.Min(m_min.z, (float)z);
            m_max.x = Math.Max(m_max.x, (float)x);
            m_max.y = Math.Max(m_max.y, (float)y);
            m_max.z = Math.Max(m_max.z, (float)z);
        }
        private void testLength(vec3 start, vec3 end)
        {
            double length = (start - end).Length;
            if (!double.IsNaN(length) && length != 0) m_minlength = double.IsNaN(m_minlength) ? length : Math.Min(m_minlength, length);
        }
    }
}
