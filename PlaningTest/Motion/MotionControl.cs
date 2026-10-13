using GlmSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PlaningTest.Motion
{
     public enum State
    {
        ACCEL = 1,
        CONST = 2,
        DECEL = 3,
        STOP=4
    }
    public class MotionControl
    {
        private int maxtravel = 500;
        private int mintravel = 0;
        private const float dt = 0.02f;
        private float acceleration = 1000;     // mm/s²
        private float maxVelocity = 100;     // mm/s
        public bool isrun;
        private float velocity = 0.0f;           // mm/s
        private float position = 0.0f;
        private float decel_pos;
        private State mc_state;
        private float target_pos;
        private int direct;
        private Timer timer;
        public void Settravel(int min,int max)//travel set
        {
            mintravel = min;
            maxtravel = max;
        }
        public bool IsRun()
        {
            return isrun;
        }
        public MotionControl()
        {
            timer = new Timer(20);
            timer.Elapsed += OnTimer;
            timer.AutoReset = true;
            target_pos = 0;
            isrun = false;
            direct = 1;
        }
        public void GoPos(float pos)
        {
            target_pos = pos;
            if (position > target_pos) direct = -1;
            else direct = 1;
        }
        public float getPos()
        {
            return position;
        }
        public void setPos(float _pos)
        {
            position = _pos;
        }
        public void Start()
        {
            timer.Start();
            mc_state = State.ACCEL;
            float dc_time =(float) maxVelocity / acceleration;// cho accel=decel
            if(direct==1)
            decel_pos = target_pos- maxVelocity * dc_time + 0.5f * acceleration * dc_time * dc_time;
            else
                decel_pos= target_pos + maxVelocity * dc_time - 0.5f * acceleration * dc_time * dc_time;
                isrun = true;
        }
        public void Stop()
        {
            if(mc_state==State.ACCEL||mc_state==State.CONST)
            {
                mc_state = State.DECEL;
            }
            isrun = false;
        }
        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            switch (mc_state)
            {
                case State.ACCEL:
                    velocity += acceleration * dt;
                    if (velocity > maxVelocity)
                    {
                        velocity = maxVelocity;
                        mc_state = State.CONST;
                    }
                    position += direct*(velocity * dt + 0.5f * acceleration * dt * dt);
                    break;
                case State.CONST:
                    if (position >= decel_pos&&direct==1)
                    {
                        mc_state = State.DECEL;
                        break;
                    }
                    else if(position<=decel_pos&&direct==-1)
                    {
                        mc_state = State.DECEL;
                        break;
                    }    
                    position += direct * (velocity * dt + 0.5f * acceleration * dt * dt);
                    break;
                    
                case State.DECEL:
                    velocity -= acceleration * dt;
                    if (velocity <= 0||position>=target_pos)
                    {
                        velocity = 0;
                        mc_state = State.STOP;
                        //position = target_pos;
                        timer.Stop();
                        break;
                    }
                    position += direct * (velocity * dt + 0.5f * acceleration * dt * dt);
                    break;
                case State.STOP:
                    velocity = 0;
                    break;

            }
            if (position < mintravel) position = mintravel;
            else if (position > maxtravel) position = maxtravel;
            


        }

    }
}
