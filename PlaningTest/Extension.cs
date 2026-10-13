using GlmSharp;
using PlaningTest.Graphic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest
{
    public static class Extensions
    {
        public static Vect3f ToVect3f(this vec3 v)
        {
            return new Vect3f { X = (float)v.x, Y = (float)v.y, Z = (float)v.z };
        }
        public static Vect4f ToVect4f(this vec4 v)
        {
            return new Vect4f { X = (float)v.x, Y = (float)v.y, Z = (float)v.z, W = (float)v.w };
        }
        public static OpenGLVertex ToOpenGLVertex(this Vert v)
        {
            return new OpenGLVertex { Position = v.Position, Normal = v.Normal, Colour = v.Colour};
        }
        public static IEnumerable<OpenGLVertex> ToOpenGLVertices(this IEnumerable<Vert> verticies)
        {
            return verticies.Select(v => v.ToOpenGLVertex());
        }
        //public static float[] ToColumnMajorArray(this mat4 v)
        //{
        //    return new[]
        //        {
        //            (float)v.M11,(float)v.M21,(float)v.M31,(float)v.M41,
        //            (float)v.M12,(float)v.M22,(float)v.M32,(float)v.M42,
        //            (float)v.M13,(float)v.M23,(float)v.M33,(float)v.M43,
        //            (float)v.M14,(float)v.M24,(float)v.M34,(float)v.M44
        //        };
        //}

        public static vec4 ToVector4(this Color col)
        {
            return new vec4(col.R / 255f, col.G / 255f, col.B / 255f, col.A / 255f);
        }

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OpenGLVertex
    {
        public vec3 Position;
        public vec3 Normal;
        public vec4 Colour;

        public static readonly int SizeInBytes = Vect3f.SizeInBytes * 2 + Vect4f.SizeInBytes;
    }

    public struct Vect3f
    {
        public float X;
        public float Y;
        public float Z;

        public static readonly int SizeInBytes = Marshal.SizeOf(new Vect3f());
    }

    public struct Vect4f
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public static readonly int SizeInBytes = Marshal.SizeOf(new Vect4f());
    }
}
