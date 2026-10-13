using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static PlaningTest.Parser.PointSegment;

namespace PlaningTest.Parser
{
    public class LineSegment
    {
        private int m_toolHead { get; set; }
        private double m_speed { get; set; }
        private double m_spindlespeed { get; set; }
        private double m_dwell { get; set; }
        private vec3 m_first { get; set; }
        private vec3 m_second { get; set; }

        private bool m_isZMovement { get; set; }
        private bool m_isArc { get; set; }
        private bool m_isClockwise { get; set; }
        private bool m_isFastTraverse { get; set; }
        private int m_linenumber { get; set; }
        private bool m_drawn { get; set; }
        private bool m_isMetric { get; set; }
        private bool m_isAbsolute { get; set; }
        private bool m_isHightlight { get; set; }
        private int m_VertexIndex { get; set; }
        private planes m_plane;

        public LineSegment()
        {
            m_toolHead = 0;
            m_isZMovement = false;
            m_isArc = false;
            m_isFastTraverse = false;
            m_drawn = false;
            m_isMetric = true;
            m_isAbsolute = true;
            m_isHightlight = false;
            m_VertexIndex = -1;
        }
        public LineSegment(vec3 a, vec3 b, int num) : this()
        {
            m_first = a;
            m_second = b;
            m_linenumber = num;
        }
        public LineSegment(LineSegment initial)
        {
            m_toolHead = initial.getToolHead();
            m_isZMovement = initial.IsZMovement();
            m_isArc = initial.isArc();
            m_isFastTraverse = initial.isFastTraverse();
            m_drawn = initial.drawn();
            m_first = initial.getStart();
            m_second = initial.getEnd();
            m_linenumber = initial.GetLineNumber();
            m_speed = initial.getSpeed();
            m_isMetric = initial.isMetric();
            m_isAbsolute = initial.IsAbsolute();
            m_isHightlight = initial.isHightLight();
            m_VertexIndex = initial.VertexIndex();
        }
        public int GetLineNumber()
        {
            return m_linenumber;
        }
        public List<vec3> getPointArray()
        {
            List<vec3> pointarr = new List<vec3>();
            pointarr.Add(m_first);
            pointarr.Add(m_second);
            return pointarr;
        }
        public List<double> getPoints()
        {
            List<double> points = new List<double>();
            points.Add(m_first.x);
            points.Add(m_first.y);
            points.Add(m_first.z);
            points.Add(m_second.x);
            points.Add(m_second.y);
            points.Add(m_second.z);
            return points;
        }
        public bool isZMovement()
        {
            return m_isZMovement;
        }
        public void setisZMovement(bool isZ)
        {
            m_isZMovement = isZ;
        }
        public vec3 getStart()
        {
            return m_first;
        }
        public void setStart(vec3 vector_)
        {
            m_first = vector_;
        }
        public vec3 getEnd()
        {
            return m_second;
        }
        public void setEnd(vec3 vector_)
        {
            m_second = vector_;
        }
        public int getToolHead()
        {
            return m_toolHead;
        }
        public void setToolHead(int head)
        {
            m_toolHead = head;
        }
        public void setSpeed(double s)
        {
            m_speed = s;
        }
        public double getSpeed()
        {
            return m_speed;
        }
        public void setIsMovement(bool isZ)
        {
            m_isZMovement = isZ;
        }
        public bool IsZMovement()
        {
            return m_isZMovement;
        }
        public void setisArc(bool isA)
        {
            m_isArc = isA;
        }
        public bool isArc()
        {
            return m_isArc;
        }
        public void setisFastTrverse(bool isF)
        {
            m_isFastTraverse = isF;
        }
        public bool isFastTraverse()
        {
            return m_isFastTraverse;
        }
        public bool contains(vec3 point)
        {
            double delta;
            vec3 line = getEnd() - getStart();
            vec3 pt = point - getStart();
            delta = (line - pt).Length - (line.Length - pt.Length);
            return delta < 0.01;
        }
        public bool drawn()
        {
            return m_drawn;
        }
        public void setDrawn(bool drawn)
        {
            m_drawn = drawn;
        }
        public bool isMetric()
        {
            return m_isMetric;
        }
        public void setIsMetric(bool isMetric)
        {
            m_isMetric = isMetric;
        }
        public bool IsAbsolute()
        {
            return m_isAbsolute;
        }
        public void setIsAbsolute(bool IsAbsolute)
        {
            m_isAbsolute = IsAbsolute;
        }
        public bool isHightLight()
        {
            return m_isHightlight;
        }
        public void setisHightLight(bool isHightLight)
        {
            m_isHightlight = isHightLight;
        }
        public int VertexIndex()
        {
            return m_VertexIndex;
        }
        public void SetVertexIndex(int vertexindex)
        {
            m_VertexIndex = vertexindex;
        }
        public double getSpindleSpeed()
        {
            return m_spindlespeed;
        }
        public void setSpindleSpeed(double spindleSpeed)
        {
            m_spindlespeed = spindleSpeed;
        }
        public double getDwell()
        {
            return m_dwell;
        }
        public void setDwell(double dwell)
        {
            m_dwell = dwell;
        }
        public bool isClockwise()
        {
            return m_isClockwise;
        }
        public void setIsClockwise(bool isClockwise)
        {
            m_isClockwise = isClockwise;
        }
        public planes plane()
        {
            return m_plane;
        }
        public void Setplane(planes plane)
        {
            m_plane = plane;
        }
    }
}
