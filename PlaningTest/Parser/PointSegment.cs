using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;
using GlmSharp;

namespace PlaningTest.Parser
{
        public enum planes
        {
            XY,
            ZX,
            YZ
        };
        public class PointSegmentJson
        {
            public ArcProperties arcproperties { get; set; }
            public int toolhead { get; set; }
            public double speed { get; set; }
            public double spindleSpeed { get; set; }
            public double dwell { get; set; }
            public vec3 point { get; set; }
            public bool isMetric { get; set; }
            public bool isZMovement { get; set; }
            public bool isArc { get; set; }
            public bool isFastTraverse { get; set; }
            public bool isAbsolute { get; set; }
            public int lineNumber { get; set; }
            public planes plane { get; set; }
            public Dictionary<string, Datapoint> vector { get; set; }
        }
        public class Datapoint
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
        }
        public class PointSegment
        {
        private ArcProperties m_arcproperties;
        private int m_toolhead;
        private double m_speed;
        private double m_spindleSpeed;
        private double m_dwell;
        private vec3 m_point;
        private bool m_isMetric;
        private bool m_isZMovement;
        private bool m_isArc;
        private bool m_isFastTraverse;
        private bool m_isAbsolute;
        private int m_lineNumber;
        private planes m_plane;
        public PointSegment()
        {
            m_arcproperties = null;
            m_toolhead = 0;
            m_isMetric = true;
            m_isAbsolute = true;
            m_isZMovement = false;
            m_isArc = false;
            m_isFastTraverse = false;
            m_lineNumber = -1;
            m_speed = 0;
            m_spindleSpeed = 0;
            m_dwell = 0;
            m_plane = planes.XY;
        }
        public PointSegment(vec3 b, int num)
        {
            m_point = new vec3(b.x, b.y, b.z);
            m_lineNumber = num;
        }
        public PointSegment(PointSegment ps)
        {
            this.m_toolhead = ps.getToolHead();
            this.m_speed = ps.GetSpeed();
            this.m_isMetric = ps.IsMetric();
            this.m_isZMovement = ps.isZMovement();
            this.m_isFastTraverse = ps.IsFastTraverse();
            this.m_isAbsolute = ps.isAbsolute();

            if (ps.isArc())
            {
                this.SetArcCenter(ps.center());
                this.setRadius(ps.getRadius());
                this.setIsClockwise(ps.isClockWise());
                this.m_plane = ps.plane();
            }
        }
        public vec3 Point()
        {
            return m_point;
        }
        public void SetPoint(vec3 point)
        {
            m_point = new vec3(point.x, point.y, point.z);
        }
        public void setToolHead(int head)
        {
            m_toolhead = head;
        }
        public int getToolHead()
        {
            return m_toolhead;
        }
        public int GetLineNumber()
        {
            return m_lineNumber;
        }
        public void setSpeed(double s)
        {
            m_speed = s;
        }
        public double GetSpeed()
        {
            return m_speed;
        }
        public void setIsZMovement(bool isZ)
        {
            m_isZMovement = isZ;
        }
        public bool isZMovement()
        {
            return m_isZMovement;
        }
        public void setIsMetric(bool isMetric)
        {
            m_isMetric = isMetric;
        }
        public bool IsMetric()
        {
            return m_isMetric;
        }
        public void setIsArc(bool isA)
        {
            m_isArc = isA;
        }
        public bool isArc()
        {
            return m_isArc;
        }
        public void setIsFastTraverse(bool isF)
        {
            m_isFastTraverse = isF;
        }
        public bool IsFastTraverse()
        {
            return m_isFastTraverse;
        }
        public void SetArcCenter(vec3 center)
        {
            if (m_arcproperties == null) m_arcproperties = new ArcProperties();
            m_arcproperties.center = new vec3(center.x, center.y, center.z);
            setIsArc(true);
        }
        public List<double> centerPoints()
        {
            List<double> points = new List<double>();
            if (m_arcproperties != null && m_arcproperties.center != null)
            {
                points.Add(m_arcproperties.center.x);
                points.Add(m_arcproperties.center.y);
                points.Add(m_arcproperties.center.z);
            }
            return points;
        }
        public vec3 center()
        {
            if (m_arcproperties != null && m_arcproperties.center != null) return m_arcproperties.center;
            else return vec3.Zero;
        }
        public void setIsClockwise(bool clockwise)
        {
            if (m_arcproperties == null) m_arcproperties = new ArcProperties();
            m_arcproperties.isClockWise = clockwise;
        }
        public bool isClockWise()
        {
            if (m_arcproperties != null && m_arcproperties.center != null) return m_arcproperties.isClockWise;
            return false;
        }
        public void setRadius(double rad)
        {
            if (m_arcproperties == null) m_arcproperties = new ArcProperties();
            m_arcproperties.radius = rad;
        }
        public double getRadius()
        {
            if (m_arcproperties != null && m_arcproperties.center != null) return m_arcproperties.radius;
            return 0;
        }
        public void converttoMeTric()
        {
            if (m_isMetric) return;
            m_isMetric = true;
            m_point.x = (float)(m_point.x * 25.4);
            m_point.y = (float)(m_point.y * 25.4);
            m_point.z = (float)(m_point.z * 25.4);

            if (m_isArc && m_arcproperties != null)
            {
                m_arcproperties.center.x = (float)(m_arcproperties.center.x * 25.4);
                m_arcproperties.center.y = (float)(m_arcproperties.center.y * 25.4);
                m_arcproperties.center.z = (float)(m_arcproperties.center.z * 25.4);
                m_arcproperties.radius *= 25.4;
            }
        }
        public bool isAbsolute()
        {
            return m_isAbsolute;
        }
        public void SetisAbsolute(bool isAbsolute)
        {
            m_isAbsolute = isAbsolute;
        }
        public planes plane()
        {
            return m_plane;
        }
        public void setPlane(planes plane)
        {
            m_plane = plane;
        }
        public void setSpindleSpeed(double spindlespeed)
        {
            m_spindleSpeed = spindlespeed;
        }
        public double getSpindleSpeed()
        {
            return m_spindleSpeed;
        }
        public double getDwell()
        {
            return m_dwell;
        }
        public void setDwell(double dwell)
        {
            m_dwell = dwell;
        }
    }
}
