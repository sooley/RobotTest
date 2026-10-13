using GlmSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaningTest.Drawer;

namespace PlaningTest.Camera
{
    public class MainCamera : ICamera
    {
        private float _width;
        private float _height;
        private float m_xRot, m_yRot, m_xLastRot, m_yLastRot;
        private float m_xPan, m_yPan, m_xLastPan, m_yLastPan;
        private float m_xLookAt, m_yLookAt, m_zLookAt;
        private Point m_lastPos;
        private float m_xRotTarget;
        private float m_yRotTarget;
        private float m_zoom;
        private float m_distance;
        private bool m_animateView;
        private float m_xMin, m_xMax, m_yMin, m_yMax, m_zMin, m_zMax, m_xSize, m_ySize, m_zSize;
        public MainCamera()
        {

            Near = 1.0f;
            Far = 512.0f;
            Model = mat4.Identity;
            Eye = vec3.Zero;
            Target = vec3.Zero;
            Up = vec3.UnitY;
            Eye = new vec3(0.0f, 0.0f, 0.0f);
            m_xRot = 0;
            m_yRot = 0;
            m_zoom = 1f;
            m_xPan = 0;
            m_yPan = 0;
            m_distance = 200;
            m_xLookAt = 0;
            m_yLookAt = 0;
            m_zLookAt = 0;
            m_xMin = 0;
            m_xMax = 0;
            m_yMin = 0;
            m_yMax = 0;
            m_zMin = 0;
            m_zMax = 0;
            m_xSize = 0;
            m_ySize = 0;
            m_zSize = 0;
            //updateProjection();
            updateView();
            printdone = 0;
        }
        //debug
        private int printdone;
        //public void Cliped_Debug(ShaderDrawable model)//fordebug
        //{ 
        //    if(model.get_Line_ForDebug().Count!=0&&printdone==0)
        //    {
        //        foreach (var i in model.get_Line_ForDebug())
        //        {
        //            vec4 temp = this.PVM * new vec4(i.Position, 1);
        //            //Debug.WriteLine("|XPan:" + m_xPan/ temp.w + "|YPan:" +m_yPan);
        //        }
        //        printdone = 1;
        //    }
        //}
        public void fitDrawable(ShaderDrawable drawable)
        {
            stopViewAnimation();
            if (drawable != null)
            {
                updateExtremes(drawable);
                Debug.WriteLine($"maxsize:{drawable.getMaximumExtremes()}+minsize:{drawable.getMinimumExtremes()}");
                float a = (float)(m_ySize / 2 / 0.25 * 1.3 + (m_zMax - m_zMin) / 2);
                float b = (float)(m_xSize / 2 / 0.25 * 1.3 / (_width) / _height + (m_zMax - m_zMin) / 2);
                m_distance = Math.Max(a, b);
                if (m_distance == 0) m_distance = 100;
                m_xLookAt = (m_xMax - m_xMin) / 2 + m_xMin;
                m_zLookAt = ((m_yMax - m_yMin) / 2 + m_yMin);
                m_yLookAt = (m_zMax - m_zMin) / 2 + m_zMin;
            }
            else
            {
                m_distance = 100;
                m_xLookAt = 0;
                m_yLookAt = 0;
                m_zLookAt = 0;
                m_xMin = 0;
                m_xMax = 0;
                m_yMin = 0;
                m_yMax = 0;
                m_zMin = 0;
                m_zMax = 0;

                m_xSize = 0;
                m_ySize = 0;
                m_zSize = 0;
            }
        }
        public void updateExtremes(ShaderDrawable drawable)
        {
            if (!float.IsNaN(drawable.getMinimumExtremes().x)) m_xMin = drawable.getMinimumExtremes().x; else m_xMin = 0;
            if (!float.IsNaN(drawable.getMaximumExtremes().x)) m_xMax = drawable.getMaximumExtremes().x; else m_xMax = 0;
            if (!float.IsNaN(drawable.getMinimumExtremes().y)) m_yMin = drawable.getMinimumExtremes().y; else m_yMin = 0;
            if (!float.IsNaN(drawable.getMaximumExtremes().y)) m_yMax = drawable.getMaximumExtremes().y; else m_yMax = 0;
            if (!float.IsNaN(drawable.getMinimumExtremes().z)) m_zMin = drawable.getMinimumExtremes().z; else m_zMin = 0;
            if (!float.IsNaN(drawable.getMaximumExtremes().z)) m_zMax = drawable.getMaximumExtremes().z; else m_zMax = 0;
            m_xSize = m_xMax - m_xMin;
            m_ySize = m_yMax - m_yMin;
            m_zSize = m_zMax - m_zMin;
        }
        public void UpdateRot(float x_rot, float y_rot)
        {
            m_xRot = x_rot;
            m_yRot = y_rot;
        }
        public void UpdatePan(float x_pan, float y_pan)
        {
            m_xPan = x_pan;
            m_yPan = y_pan;
        }
        public void UpdateZoom(float zoom)
        {
            m_zoom = zoom;
        }
        public void stopViewAnimation()
        {
            m_animateView = false;
        }
        private mat4 CreatePerspectiveOffCenter(float left, float right,float bottom, float top,float nearZ, float farZ)
        {
            mat4 result = mat4.Zero;
            result.m00 = 2.0f * nearZ / (right - left);
            result.m11 = 2.0f * nearZ / (top - bottom);

            result.m20 = (right + left) / (right - left);
            result.m21 = (top + bottom) / (top - bottom);
            result.m22 = -(farZ + nearZ) / (farZ - nearZ);
            result.m23 = -1.0f;

            result.m32 = -(2.0f * farZ * nearZ) / (farZ - nearZ);
            return result;
        }
        public void updateProjection()
        {
            Projection = mat4.Identity;
            float asp = (float)_width / _height;
            //Projection = CreatePerspectiveOffCenter((float)(-0.5 + m_xPan) * asp, (float)(0.5 + m_xPan) * asp, (float)(-0.5 + m_yPan), (float)(0.5 + m_yPan), 2, m_distance * 2);
            //Projection = Perspective(glm.Radians(60f), _width / _height, 0.1f, 200.0f);
            Projection = Ortho(-_width / ((float)(2*m_zoom))+(float)m_xPan, _width / ((float)(2 * m_zoom)) + (float)m_xPan, -_height/ ((float)(2 * m_zoom)) + m_yPan, _height/ ((float)(2 * m_zoom)) + m_yPan, 0.01f, m_distance*5);
            //Projection = mat4.Ortho(0, _width, 0, _height, 0.1f, 100f);
            //Projection = mat4.Frustum(-_width/2, _width/2,- _height/2, _height/2, 50f, 500f);
            //Debug.WriteLine("m_zoom" + m_zoom + "m_yRot" + m_yRot+"m_xRot"+m_xRot+ "_width"+ _width+ "_height"+ _height);

        }
        public mat4 GetOrthoProj()
        {
            return Ortho(-_width / 2 , _width / 2 , -_height / 2 , _height / 2 , -10f, m_distance * 5);
        }
        public mat4 Ortho(float left, float right, float bottom, float top, float near, float far)
        {
            mat4 result = mat4.Identity;
            result.m00 = 2.0f / (right - left);
            result.m11 = 2.0f / (top - bottom);
            result.m22 = -2.0f / (far - near);
            result.m30 = -(right + left) / (right - left);
            result.m31 = -(top + bottom) / (top - bottom);
            result.m32 = -(far + near) / (far - near);
            result.m33 = 1;
            return result;
        }
        private mat4 Perspective(float fov, float aspect, float near, float far)
        {
            float tanHalfFov = (float)Math.Tan(fov / 2.0f);

            mat4 result = mat4.Identity;
            result.m00 = 1.0f / (aspect * tanHalfFov);
            result.m11 = 1.0f / tanHalfFov;
            result.m22 = -(far + near) / (far - near);
            result.m23 = -1.0f;
            result.m32 = -(2.0f * far * near) / (far - near);
            result.m33 = 0.0f;
            return result;
        }
        public void updateView()
        {
            mat4 _View = mat4.Identity;
            //float r = m_distance;
            //float angY = (float)Math.PI / 180 * m_yRot;
            //float angX = (float)Math.PI / 180 * m_xRot;
            //Eye = new vec3((float)(r * Math.Cos(angX) * Math.Sin(angY) + m_xLookAt), (float)(r * Math.Sin(angX) + m_yLookAt), (float)(r * Math.Cos(angX) * Math.Cos(angY) + m_zLookAt));
            //Target = new vec3(m_xLookAt, m_zLookAt, m_yLookAt);
            //Up = new vec3(Math.Abs(m_xRot) == 90 ? -(float)Math.Sin(angY + (m_xRot < 0 ? Math.PI : 0)) : 0, (float)Math.Cos(angX), Math.Abs(m_xRot) == 90 ? -(float)Math.Sin(angY + (m_xRot < 0 ? Math.PI : 0)) : 0);
            //mat4 _lookat = mat4.LookAt(Eye, Target, new vec3(0,1,0));
            float r = m_distance;

            float yaw = glm.Radians(m_yRot);
            float pitch = glm.Radians(m_xRot);

            vec3 target = new vec3(
                m_xLookAt,
                m_yLookAt,
                m_zLookAt
            );

            vec3 direction = new vec3(
                (float)(Math.Cos(pitch) * Math.Sin(yaw)),
                (float)Math.Sin(pitch),
                (float)(Math.Cos(pitch) * Math.Cos(yaw))
            );

            Eye = target + direction * r;

            Target = target;

            //Up = new vec3(0, 1, 0);
            Up = new vec3(Math.Abs(m_xRot) == 90 ? -(float)Math.Sin(yaw + (m_xRot < 0 ? Math.PI : 0)) : 0, (float)Math.Cos(pitch), Math.Abs(m_xRot) == 90 ? -(float)Math.Sin(yaw + (m_xRot < 0 ? Math.PI : 0)) : 0);
            View = mat4.LookAt(
                Eye,
                Target,
                Up
            );
            mat4 _scale = mat4.Scale(m_zoom);
            //View = _scale * _lookat * _View; ;
            View *= mat4.Translate(-100, 0, 0);
            //View = mat4.Identity;
            //View *= mat4.Translate(0.0f, 0.0f, -15.0f);
        }
        public void rotate(float angle,vec3 vector)
        {
            Model *= mat4.Rotate(glm.Radians(angle), vector);
        }
        public double Near { get; private set; }
        public double Far { get; private set; }
        public mat4 Model { get; set; }


        public mat4 View
        {
            get; set;
        }

        public mat4 Projection { get; set; }

        public mat4 MVP
        {
            get { return Projection * View * Model; }
        }
        public mat4 PVM
        {
            get { return Projection * View * Model; }
        }
        public vec3 Eye { get; set; }

        public vec3 Target { get; set; }

        public vec3 Up { get; set; }

        public void Update(double delta)
        {

        }

        public void Resize(int width, int height)
        {
            _width = width;
            _height = height;
            //Projection = Matrix4x4.Identity;
        }
    }

}
