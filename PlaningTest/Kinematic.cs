using GlmSharp;
using PlaningTest.ConstValue;
using PlaningTest.DataManager;
using PlaningTest.Planner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest
{
    public struct rtcp_target_block_t
    {
        public vec3 XYZ;
        public float B;
        public float C;
    }
    public class Kinematic
    {
        private float L;// Bán kính quay của trục B và trục A
        private float Cx, Cy,Cz;   
        private bool rtcp_mode;

        private float _gamma = -45;//góc nghiêng của trục phụ

        private int RTCP_ARC_SAMPLES = 64;
        private int RTCP_MAX_SEGMENTS = 1000;

        private float[] rtcp_s_tab;
        private vec3[] rtcp_mcs_tab;
        private float[] rtcp_len_tab;
        private rtcp_target_block_t start_rtcp;
        private float RTCP_segment_length;
        
        float L_Default = 100;
        public Kinematic()
        {
            L = L_Default;
            Cx = 0;Cy = 0;Cz = 0;
            rtcp_mode = false;
            RTCP_segment_length = 1;
            rtcp_s_tab = new float[RTCP_ARC_SAMPLES + 1];
            rtcp_mcs_tab = new vec3[RTCP_ARC_SAMPLES + 1];
            rtcp_len_tab = new float[RTCP_ARC_SAMPLES + 1];

            start_rtcp = calc_pos_in_rtcp();// Đọc vị trí hiện tại sang toạ độ RTCP
        }

        public float get_Pivot_Length()
        {
            return L;
        }    

        float DEG2RAD = (float)(Math.PI / 180.0f);
        float Radian(float angle)
        {
            return (float)(angle * 6.28) / (float)360.0;
        }
        public void Calib_Caculate_A(float gama1,float gama2, float x1,float x2,float y1,float y2)
        {
            L = Math.Abs((x2 - x1) / (float)(Math.Sin(Radian(gama2)) - Math.Sin(Radian(gama1))));
        }// Tính toán L của Tool. 
        public void Calib_Caclulate_C(float x1,float y1,float x2,float y2,float x3,float y3,float x4,float y4)
        {
            Cx = (x1 + x2) / 2;
            Cy = (y3 + y4) / 2;
        }

        public rtcp_target_block_t rtcp_forward(vec3 target, float B_deg,float C_deg)// động học thuận từ toạ độ RTCP->Mcs
        {
            float B = B_deg * DEG2RAD;
            float C = C_deg * DEG2RAD;
            float _Y = _gamma * DEG2RAD;
            float CosB = (float)Math.Cos(B);
            float SinB = (float)Math.Sin(B);
            float CosC = (float)Math.Cos(C);
            float SinC = (float)Math.Sin(C);
            float SinY = (float)Math.Sin(_Y);
            float CosY = (float)Math.Cos(_Y);

            //float x_tran = (target.x - Cx) * CosC - (target.y - Cy) * SinC + Cx + L * SinB;
            //float y_tran = (target.x - Cx) * SinC + (target.y - Cy) * CosC + Cy;
            //float z_tran = target.z + L * (CosB - 1);


            //float x_tran = ((target.x - Cx) * CosC - (target.y - Cy) * SinC) * CosY + SinY * (target.z - Cz) + Cx;// + L * SinB;// _gamma là góc nghiêng
            //float y_tran = (target.x - Cx) * SinC + (target.y - Cy) * CosC + Cy;
            //float z_tran = ((Cx - target.x) * CosC - (Cy - target.y) * SinC) * SinY + CosY * (target.z - Cz);// + L * (CosB - 1);// check lại 

            float x_tran = ((target.x - Cx) * CosY - (target.z - Cz) * SinY) * CosY*CosC - SinC*(target.y-Cy)*CosY+ SinY * ((target.x-Cx)*SinY+(target.z - Cz)*CosY) + Cx+L * SinB;// _gamma là góc nghiêng
            float y_tran = CosY*(target.x - Cx) * SinC - SinY*(target.z - Cz) * SinC+CosC*(target.y-Cy) + Cy;
            float z_tran = -((target.x - Cx) * CosY - (target.z - Cz) * SinY) * SinY * CosC+SinC*SinY*(target.y-Cy)+ CosY * ((target.x - Cx) * SinY + (target.z - Cz) * CosY)+Cy+ L * (CosB - 1);// check lại 

            //Debug.WriteLine($"targetx:{target.x}, targety: {target.y}, targetz: {target.z}, x_tran: {x_tran}, y_tran: {y_tran}, z_tran: {z_tran}, C_tran: {C_deg}");
            //float x_tran = L * SinB;
            //float y_tran = 0;
            //float z_tran = -L* CosB;
            rtcp_target_block_t out_ = new rtcp_target_block_t();
            out_.XYZ = new vec3(x_tran,y_tran,z_tran);
            out_.B = B_deg;
            out_.C = C_deg;

            return out_; 
        }

        public vec3 pivot_point(vec3 B_center, float B_deg)
        {
            float B = B_deg * DEG2RAD;
            float CosB = (float)Math.Cos(B);
            float SinB = (float)Math.Sin(B);
            float x_tran = B_center.x - L * SinB;
            float z_tran = B_center.z - L * CosB;
            return new vec3(x_tran, B_center.y, z_tran);
        }

        public rtcp_target_block_t rtcp_inverse(vec3 XYZ, float B_deg, float C_deg)// Động học ngược từ Mcs->RTCP
        {
            float B = B_deg * DEG2RAD;
            float C = C_deg * DEG2RAD;
            float _Y = _gamma * DEG2RAD;


            float CosB = (float)Math.Cos(B);
            float SinB = (float)Math.Sin(B);
            float CosC = (float)Math.Cos(C);
            float SinC = (float)Math.Sin(C);
            float SinY = (float)Math.Sin(_Y);
            float CosY = (float)Math.Cos(_Y);


            //float x_tran = (XYZ.x - Cx) * CosC + (XYZ.y - Cy) * SinC - L * SinB * CosC + Cx;
            //float y_tran = Cy - (XYZ.x-Cx-L*SinB)* SinC + XYZ.y * CosC - Cy * CosC;
            //float z_tran = XYZ.z - L * (CosB - 1);

            float x_tran = -(1 + CosC) * (-XYZ.x + Cx) * CosY * CosY + (SinY * (Cz - XYZ.z) * CosC + SinY * (Cz - XYZ.z) - SinC * (Cy - XYZ.y)) * CosY + Cx - XYZ.x;
            float y_tran = (Cx - XYZ.x) * CosY - SinY * (Cz - XYZ.z) * SinC - CosC * (Cy - XYZ.y);
            float z_tran = -(1 + CosC) * (Cz - XYZ.z) * CosY * CosY - SinY * (1 + CosC) * (Cx - XYZ.x) * CosY + (Cz - XYZ.z) * CosC - SinC * SinY * (Cy - XYZ.y);


            rtcp_target_block_t out_ = new rtcp_target_block_t();
            out_.XYZ = new vec3(x_tran, y_tran, z_tran);
            out_.B = B_deg;
            out_.C = C_deg;

            return out_;
        }

        public rtcp_target_block_t calc_pos_in_rtcp()
        {
            vec3 XYZ = new vec3();
            XYZ.x = CoordinateSystem.mcs_coord[0];
            XYZ.y = CoordinateSystem.mcs_coord[1];
            XYZ.z = CoordinateSystem.mcs_coord[2];
            float B_deg = CoordinateSystem.mcs_coord[3];
            float C_deg = CoordinateSystem.mcs_coord[4]; //mcs coord là toạ độ máy của động cơ nhé

            return rtcp_inverse(XYZ, B_deg, C_deg);
        }
        public List<Pr_Point> rtcp_segment(vec3 target_mcs, float B_deg, float C_deg,float feedrate,Planner_ring _plan)
        {

            List<Pr_Point> out1 = new List<Pr_Point>();
            rtcp_target_block_t end_rtcp = new rtcp_target_block_t();
            end_rtcp.XYZ.x = target_mcs.x;
            end_rtcp.XYZ.y = target_mcs.y;
            end_rtcp.XYZ.z = target_mcs.z;
            end_rtcp.B = B_deg;
            end_rtcp.C = C_deg;

            rtcp_target_block_t delta;
            delta.XYZ.x = end_rtcp.XYZ.x - start_rtcp.XYZ.x;
            delta.XYZ.y = end_rtcp.XYZ.y - start_rtcp.XYZ.y;
            delta.XYZ.z = end_rtcp.XYZ.z - start_rtcp.XYZ.z;
            delta.B = end_rtcp.B - start_rtcp.B;
            delta.C = end_rtcp.C - start_rtcp.C;

            rtcp_s_tab[0] = 0.0f;
            rtcp_mcs_tab[0] = rtcp_forward(start_rtcp.XYZ, start_rtcp.B, start_rtcp.C).XYZ;
            rtcp_len_tab[0] = 0.0f;

            for(int i=1;i<=RTCP_ARC_SAMPLES;i++)
            {
                float s = (float)i / RTCP_ARC_SAMPLES;
                vec3 xyz_s = new vec3 { x = start_rtcp.XYZ.x+s*delta.XYZ.x, y = start_rtcp.XYZ.y+s*delta.XYZ.y, z = start_rtcp.XYZ.z+s*delta.XYZ.z };
                float b_s = start_rtcp.B + s * delta.B;
                float c_s = start_rtcp.C + s * delta.C;

                vec3 mcs = rtcp_forward(xyz_s, b_s, c_s).XYZ;
                float dx = mcs.x - rtcp_mcs_tab[i - 1].x;
                float dy = mcs.y - rtcp_mcs_tab[i - 1].y;
                float dz = mcs.z - rtcp_mcs_tab[i - 1].z;

                rtcp_s_tab[i] = s;
                rtcp_mcs_tab[i] = mcs;
                rtcp_len_tab[i] = rtcp_len_tab[i - 1] + (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);

            }
            float total_len = rtcp_len_tab[RTCP_ARC_SAMPLES];
            
            int n_seg = (int)(total_len / RTCP_segment_length + 0.5f);
            if (n_seg < 1) n_seg = 1;
            if (n_seg > RTCP_MAX_SEGMENTS) n_seg = RTCP_MAX_SEGMENTS;

            
            //Pass2//
            float step_len = total_len / n_seg;
            //Debug.WriteLine($"dodaidoan {total_len}, Sodoan {step_len}");
            int idx = 0;
            for(int seg=1;seg<=n_seg;seg++)
            {
                float target_len = (seg == n_seg) ? total_len : step_len * (float)seg;
                while (idx < RTCP_ARC_SAMPLES && rtcp_len_tab[idx] < target_len)
                {
                    idx++;
                }
                float s_out;
                if(idx==0)
                {
                    s_out = 0.0f;
                }    
                else
                {
                    float len0 = rtcp_len_tab[idx - 1];
                    float len1 = rtcp_len_tab[idx];
                    float frac = (len1 > len0) ? (target_len - len0) / (len1 - len0) : 0.0f;
                    s_out = rtcp_s_tab[idx - 1] + frac*(rtcp_s_tab[idx] - rtcp_s_tab[idx - 1]);
                }
                if (seg == n_seg) s_out = 1.0f;

                vec3 xyz_out = new vec3(start_rtcp.XYZ.x + s_out * delta.XYZ.x,start_rtcp.XYZ.y+s_out*delta.XYZ.y,start_rtcp.XYZ.z+s_out*delta.XYZ.z);
                float b_out = start_rtcp.B + s_out * delta.B;
                float c_out = start_rtcp.C + s_out * delta.C;
                out1.Add(new Pr_Point { x = xyz_out.x, y = xyz_out.y, z = xyz_out.z, a=b_out, b=c_out });
                rtcp_target_block_t _out= rtcp_forward(xyz_out, b_out, c_out);
                float[] output_2float = new float[5]{ _out.XYZ.x, _out.XYZ.y, _out.XYZ.z, _out.B, _out.C };

                //_plan.plan_buffer_line(output_2float, feedrate);
            }
            start_rtcp = end_rtcp;
            return out1;
        }

        public void Kinematic_Point()// Convert từ điểm chia ra toạ độ x để chạy
        {

        }

    }

}
