using SharpGL;
using GlmSharp;
using PlaningTest.Buffer;
using PlaningTest.Drawer;
using PlaningTest.Graphic;
using PlaningTest.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace PlaningTest.Drawer
{
    public class gcodedrawer:ShaderDrawable
    {
        public enum GrayscaleCode { S, Z };
        public enum DrawMode { Vectors,Raster};
        public gcodedrawer(OpenGL gl) : base(gl)
        {
            m_geometryUpdated = false;
            m_PointSize = 6;
            m_ignoreZ = false;
            m_grayscaleSegments = false;
            m_grayscaleCode = GrayscaleCode.S;
            m_grayscaleMin = 0;
            m_grayscaleMax = 255;
            m_DrawMode = DrawMode.Vectors;
            m_indexes =new List<int>();
            m_lines = new List<Vert>();
            m_points = new List<Vert>();
            m_triangles = new List<Vert>();
            m_viewParser = new GcodeViewParse();
            m_vbo = new VBO(gl,SharpGL.Enumerations.BeginMode.Lines);
            RunLineNumber = 0;
            LastRunLineNumber = 0;
        }
        public override void update()
        {
            m_indexes.Clear();
            m_geometryUpdated = false;
            base.update();
        }
        public void update(List<int>indexes)
        {
            m_indexes.AddRange(indexes);
        }
        public override bool UpdateData()
        {
            switch(m_DrawMode)
            {
                case DrawMode.Vectors:
                    if (m_indexes.Count == 0) return prepareVectors(); else return updateVectors();
                case DrawMode.Raster:
                    if (m_indexes.Count == 0) return prepareRaster(); else return updateRaster();
            }
            return false;
        }
        public Bitmap m_image;
        private GrayscaleCode m_grayscaleCode;
        private GcodeViewParse m_viewParser;
        private DrawMode m_DrawMode;
        private bool m_simplify;
        private double m_simplifyPrecision;
        private bool m_ignoreZ;
        private bool m_grayscaleSegments;
        private int m_grayscaleMin;
        private int m_grayscaleMax;
        private Color m_colorNormal;
        private Color m_colorDrawn;
        private Color m_colorHighLight;
        private Color m_colorZMovement;
        private Color m_colorStart;
        private Color m_colorEnd;
        private Timer m_timerVertexUpdate;
        private List<int> m_indexes;
        private bool m_geometryUpdated;
        private int RunLineNumber;
        private int LastRunLineNumber;
        public void UpdateLineNumber(int linenumber)
        {
            RunLineNumber = linenumber;
        }
        public void UpdateRuningLine()
        {
            if (m_lines.Count == 0) return;
            
            Vert temp = m_lines.ElementAt(RunLineNumber);
            temp.Colour = Color.Yellow.ToVector4();
            m_lines[RunLineNumber] = temp;
            LastRunLineNumber = RunLineNumber;
        }
        private bool prepareVectors()
        {
            List<LineSegment> list = m_viewParser.getlines();
            Vert vertex = new Vert();
            //Clear all vertex data
            m_lines.Clear();
            m_points.Clear();
            m_triangles.Clear();
            bool drawFirstPoint = true;
            for(int i=0;i<list.Count();i++)
            {
                if (double.IsNaN(list.ElementAt(i).getEnd().z)) continue;
                if(drawFirstPoint)
                {
                    if (double.IsNaN(list.ElementAt(i).getEnd().x ) || double.IsNaN(list.ElementAt(i).getEnd().y)) continue;
                    vertex.Colour = m_colorStart.ToVector4();
                    vertex.Position = list.ElementAt(i).getEnd();
                    if (m_ignoreZ) vertex.Position.z = 0;
                    vertex.Normal = new vec3(float.NaN, float.NaN, (float)m_PointSize);
                    m_points.Add(vertex);
                    drawFirstPoint = false;
                    continue;
                }
                //prepare vertices
                if (list.ElementAt(i).isFastTraverse()) vertex.Normal = list.ElementAt(i).getStart();
                else vertex.Normal = list.ElementAt(i).getStart();
                //simplify geometry
                int j = i;
                if (m_simplify && i < list.Count-1)
                {
                    vec3 start = list.ElementAt(i).getEnd() - list.ElementAt(i).getStart();
                    vec3 next;
                    double length = start.Length;
                    bool straight = false;
                    do
                    {
                        list.ElementAt(i).SetVertexIndex(m_lines.Count);
                        i++;
                        if (i < list.Count - 1)
                        {
                            next = list.ElementAt(i).getEnd() - list.ElementAt(i).getStart();
                            length += next.Length;
                        }
                    } while ((length < m_simplifyPrecision || straight) && i < list.Count && getSegmentType(list.ElementAt(i)) == getSegmentType(list.ElementAt(j)));
                    i--;

                }else
                {
                    list.ElementAt(i).SetVertexIndex(m_lines.Count);
                }
                //set color
                vertex.Colour = getSegmentColorVector(list.ElementAt(i));
                if (i < RunLineNumber/2) vertex.Colour = Color.Yellow.ToVector4();
                //line start
                vertex.Position = list.ElementAt(i).getStart();
                if (m_ignoreZ) vertex.Position.z = 0;
                m_lines.Add(vertex);
                //line end
                vertex.Position = list.ElementAt(i).getEnd();
                if (m_ignoreZ) vertex.Position.z = 0;
                m_lines.Add(vertex);

                //draw last toolpath point
                if (i==list.Count-1)
                {
                    vertex.Colour = m_colorEnd.ToVector4();
                    vertex.Position = list.ElementAt(i).getEnd();
                    if (m_ignoreZ) vertex.Position.z = 0;
                    vertex.Normal = new vec3(float.NaN, float.NaN, (float)m_PointSize);
                    m_points.Add(vertex);
                }    
                    
            }
            m_geometryUpdated = true;
            m_indexes.Clear();
            return true;
        }
        private bool updateVectors()
        {
            List<LineSegment> list = m_viewParser.getlines();
            unsafe
            {
                Vert* data = (Vert*)m_vbo.map();
                int vertexIndex;
                foreach (int i in m_indexes)
                {
                    if (i < 0 || i > list.Count - 1) continue;
                    vertexIndex = list.ElementAt(i).VertexIndex();
                    if (vertexIndex > 0)
                    {
                        if (data == null)
                        {
                            data[vertexIndex].Colour = getSegmentColorVector(list.ElementAt(i));
                            data[vertexIndex + 1].Colour = data[vertexIndex].Colour;
                        }
                        else
                        {
                            Vert temp = m_lines[vertexIndex];
                            temp.Colour= getSegmentColorVector(list.ElementAt(i));
                            m_lines[vertexIndex] = temp;
                            m_lines[vertexIndex + 1] = temp;
                        }
                    }
                }
                m_indexes.Clear();
                if (data!=null) m_vbo.Unmap();
                return data==null?false:true;
            }
        }

        private bool prepareRaster()
        {
            int maxImageSize = 8192;
            Bitmap image= new Bitmap(0, 0, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            if (m_viewParser.GetResolution().Width<=maxImageSize&&m_viewParser.GetResolution().Height<=maxImageSize)
            {
                image = new Bitmap(m_viewParser.GetResolution().Width, m_viewParser.GetResolution().Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                using(Graphics gfx = Graphics.FromImage(image))
                using (SolidBrush brush=new SolidBrush(Color.White))
                {
                    gfx.FillRectangle(brush, new Rectangle(0,0, m_viewParser.GetResolution().Width, m_viewParser.GetResolution().Height));
                }
                List<LineSegment> list = m_viewParser.getlines();
                double pixelsize = m_viewParser.GetminLength();
                vec3 origin = m_viewParser.GetMinimumExtremes();
                for(int i=0;i<list.Count;i++)
                {
                    if(!float.IsNaN(list.ElementAt(i).getEnd().Length))
                    {
                        setImagePixelColor(image, (int)((list.ElementAt(i).getEnd().x - origin.x) / pixelsize), (int)((list.ElementAt(i).getEnd().y - origin.y) / pixelsize), getSegmentColor(list.ElementAt(i)));
                    }    
                }    
            }
            m_indexes.Clear();
            m_points.Clear();
            m_triangles.Clear();
            List<Vert> vertices=new List<Vert>();
            Vert vertex = new Vert();
            //SetColor
            vertex.Colour = Color.Red.ToVector4();
            //Rect
            vertex.Normal = new vec3(float.NaN, 0, 0);
            vertex.Position = new vec3(getMinimumExtremes().x, getMinimumExtremes().y, 0);
            vertices.Append(vertex);

            vertex.Normal = new vec3(float.NaN, 1, 1);
            vertex.Position = new vec3(getMaximumExtremes().x, getMaximumExtremes().y, 0);
            vertices.Append(vertex);

            vertex.Normal = new vec3(float.NaN, 0, 1);
            vertex.Position = new vec3(getMinimumExtremes().x, getMaximumExtremes().y, 0);
            vertices.Append(vertex);

            vertex.Normal = new vec3(float.NaN, 0, 0);
            vertex.Position = new vec3(getMinimumExtremes().x, getMinimumExtremes().y, 0);
            vertices.Append(vertex);

            vertex.Normal = new vec3(float.NaN, 1, 0);
            vertex.Position = new vec3(getMaximumExtremes().x, getMinimumExtremes().y, 0);
            vertices.Append(vertex);

            vertex.Normal = new vec3(float.NaN, 1, 1);
            vertex.Position = new vec3(getMaximumExtremes().x, getMaximumExtremes().y, 0);
            vertices.Append(vertex);
            if(image.Size.Width != 0&&image.Size.Height!=0)
            {
                m_triangles.AddRange(vertices);
                m_image = image;
            }    
            else
            {
                vec3 normal1 = new vec3(float.NaN, float.NaN, float.NaN);
                for (int i = 0; i < vertices.Count; i++)
                {
                    Vert temp = vertices[i];
                    temp.Normal = normal1;
                    vertices[i] = temp;
                }
                m_lines.AddRange(vertices);
                m_image= new Bitmap(0, 0, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }
            m_geometryUpdated = true;
            m_indexes.Clear();
            return true;
        }

        private bool updateRaster()
        {
            if(m_image.Size.Width != 0 && m_image.Size.Height != 0)
            {
                List<LineSegment> list = m_viewParser.getlines();
                double pixelSize = m_viewParser.GetminLength();
                vec3 origin = m_viewParser.GetMinimumExtremes();
                foreach(int i in m_indexes) setImagePixelColor(m_image, (int)((list.ElementAt(i).getEnd().x - origin.x) / pixelSize), (int)((list.ElementAt(i).getEnd().y - origin.y) / pixelSize), getSegmentColor(list.ElementAt(i)));
            }
            m_indexes.Clear();
            return false;
        }
        public override vec3 getSize()
        {
            vec3 min = m_viewParser.GetMinimumExtremes();
            vec3 max = m_viewParser.GetMaximumExtremes();
            return new vec3(max.x - min.x, max.y - min.y, max.z - min.z);
        }
        public override vec3 getMinimumExtremes()
        {
            vec3 v = m_viewParser.GetMinimumExtremes();
            if (m_ignoreZ) v.z = 0;
            return v;
        }
        public override vec3 getMaximumExtremes()
        {
            vec3 v = m_viewParser.GetMaximumExtremes();
            if (m_ignoreZ) v.z = 0;
            return v;
        }

        public void SetViewParser(GcodeViewParse viewParser)
        {
            m_viewParser = viewParser;
        }
        public GcodeViewParse viewParser()
        {
            return m_viewParser;
        }

        public bool simplify()
        {
            return m_simplify;
        }

        public void setSimplify(bool simplify)
        {
            m_simplify = simplify;
        }

        public double simplifyPrecision()
        {
            return m_simplifyPrecision;
        }    

        public void setSimplifyPrecision(double simplifyPrecision)
        {
            m_simplifyPrecision = simplifyPrecision;
        }
        public bool geometryUpdated()
        {
            return m_geometryUpdated;
        }
        public Color colorNormal()
        {
            return m_colorNormal;
        }
        public void setColorNormal(Color colorNormal)
        {
            m_colorNormal = colorNormal;
        }
        public Color colorHighlight()
        {
            return m_colorHighLight;
        }
        public void setColorHighlight(Color colorHighlight)
        {
            m_colorHighLight = colorHighlight;
        }
        public Color colorZMovement()
        {
            return m_colorZMovement;
        }
        public void setColorZMovement(Color colorZMovement)
        {
            m_colorZMovement = colorZMovement;
        }
        public Color colorDrawn()
        {
            return m_colorDrawn;
        }
        public void setColorDrawn(Color colorDrawn)
        {
            m_colorDrawn = colorDrawn;
        }
        public Color colorStart()
        {
            return m_colorStart;
        }    
        public void setColorStart(Color colorStart)
        {
            m_colorStart = colorStart;
        }    
        public Color colorEnd()
        {
            return m_colorEnd;
        }
        public void setColorEnd(Color colorEnd)
        {
            m_colorEnd = colorEnd;
        }
        public void setIgnoreZ(bool ignoreZ)
        {
            m_ignoreZ = ignoreZ;
        }
        public DrawMode drawMode()
        {
            return m_DrawMode;
        }
        public List<Vert> GetLine()
        {
            return m_lines;
        }
        public void setDrawMode(DrawMode drawMode)
        {
            m_DrawMode = drawMode;
        }
        public int grayscaleMax()
        {
            return m_grayscaleMax;
        }    
        public void setGrayscaleMax(int grayscaleMax)
        {
            m_grayscaleMax = grayscaleMax;
        }
        public int grayscaleMin()
        {
            return m_grayscaleMin;
        }
        public void setGrayscaleMin(int grayscaleMin)
        {
            m_grayscaleMin = grayscaleMin;
        }
        public GrayscaleCode grayscaleCode()
        {
            return m_grayscaleCode;
        }
        public void setGrayscaleCode(GrayscaleCode grayscaleCode)
        {
            m_grayscaleCode = grayscaleCode;
        }
        public bool getGrayscaleSegments()
        {
            return m_grayscaleSegments;
        }
        public void setGrayscaleSegments(bool grayscaleSegments)
        {
            m_grayscaleSegments = grayscaleSegments;
        }
        private int getSegmentType(LineSegment segment)
        {
            return Convert.ToInt16(segment.isFastTraverse()) + Convert.ToInt16(segment.IsZMovement()) * 2;
        }
        private vec4 getSegmentColorVector(LineSegment segment)
        {
            return getSegmentColor(segment).ToVector4();
        }
        private Color getSegmentColor(LineSegment segment)
        {
            if (segment.drawn()) return m_colorDrawn;
            else if (segment.isHightLight()) return m_colorHighLight;
            else if (segment.isFastTraverse()) return m_colorNormal;
            else if (segment.IsZMovement()) return m_colorZMovement;
            else if(m_grayscaleSegments)
            {
                switch (m_grayscaleCode)
                {

                    case GrayscaleCode.S:
                        {
                            ColorHelper.RGB rgb = ColorHelper.ColorConverter.HslToRgb(new ColorHelper.HSL(0, 0, (byte)(255 - 255.0 / (m_grayscaleMax - m_grayscaleMin) * segment.getSpindleSpeed())));
                            return Color.FromArgb(rgb.R, rgb.G, rgb.B);
                        }
                    case GrayscaleCode.Z:
                        {
                            ColorHelper.RGB rgb = ColorHelper.ColorConverter.HslToRgb(new ColorHelper.HSL(0, 0, (byte)(255 - 255.0 / (m_grayscaleMax - m_grayscaleMin) * segment.getStart().z)));
                            return Color.FromArgb(rgb.R, rgb.G, rgb.B);
                        }
                }
            }
            return m_colorNormal;

        }
        private void setImagePixelColor(Bitmap image, int x, int y, Color color)
        {
            Bitmap newBitmap = new Bitmap(image.Width, image.Height);
            newBitmap.SetPixel(x, y, color);  
        }
    }
}
