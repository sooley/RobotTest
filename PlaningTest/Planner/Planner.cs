using PlaningTest.ConstValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.Planner
{
    public class plan_block_t_motioncard
    {
        public float[] position;
        public float entry_speed;
        public plan_block_t_motioncard()
        {
            position = new float[5];
            entry_speed = 0;
        }
    }
    public class plan_block_t
    {
        public int direction_bits;    // The direction bit set for this block (refers to *_DIRECTION_BIT in config.h)
        public int[] steps;   // Step count along each axis
        public int step_event_count; // The maximum step axis count and number of steps required to complete this block. 

        // Fields used by the motion planner to manage acceleration
        public float entry_speed_sqr;         // The current planned entry speed at block junction in (mm/min)^2
        public float max_entry_speed_sqr;     // Maximum allowable entry speed based on the minimum of junction limit and 
                                       //   neighboring nominal speeds with overrides in (mm/min)^2
        public float max_junction_speed_sqr;  // Junction entry speed limit based on direction vectors in (mm/min)^2
        public float nominal_speed_sqr;       // Axis-limit adjusted nominal speed for this block in (mm/min)^2
        public float acceleration;            // Axis-limit adjusted line acceleration in (mm/min^2)
        public float millimeters;

        public int line_number;
        public plan_block_t()
        {
            steps = new int[PlannerConfig.N_AXIS];
        }

    }
    public class planner_t
    {
        public double[] position;          // The planner position of the tool in absolute steps. Kept separate
                                           // from g-code position for movements requiring multiple line motions,
                                           // i.e. arcs, canned cycles, and backlash compensation.
        public float[] previous_unit_vec;   // Unit vector of previous path line segment
        float previous_nominal_speed_sqr;  // Nominal speed of previous path line segment
        public planner_t()
        {
            position = new double[PlannerConfig.N_AXIS];
            previous_unit_vec = new float[PlannerConfig.N_AXIS];
        }

    }

    public class Planner_ring
    {
        public static int BLOCK_BUFFER_SIZE=16;
        public plan_block_t_motioncard[] block_buffer;
        public int block_buffer_tail;     // Index of the block to process now
        public int block_buffer_head;     // Index of the next block to be pushed
        public int  next_buffer_head;      // Index of the next buffer head
        public int block_buffer_planned;  // Index of the optimally planned block


        public Planner_ring()
        {
            block_buffer = new plan_block_t_motioncard[BLOCK_BUFFER_SIZE];

        }
        public void plan_buffer_line(float[] target,float feed_rate)
        {
            plan_block_t_motioncard block = block_buffer[block_buffer_head];
            for(int idx=0;idx<5;idx++)
            {
                block.position[idx] = target[idx];
            }
            block.entry_speed = feed_rate;

        }
        public void plan_discard_current_block()
        {
            if(block_buffer_head!=block_buffer_tail)
            {
                int block_index = plan_next_block_index(block_buffer_tail);
                if (block_buffer_tail == block_buffer_planned)
                {
                    block_buffer_planned = block_index;
                    block_buffer_tail = block_index;
                }
            }    
        }
        public plan_block_t_motioncard plan_get_current_block()
        {
            if (block_buffer_head == block_buffer_tail) return null;
            return block_buffer[block_buffer_tail];
        }
        public int plan_next_block_index(int block_index)
        {
            block_index++;
            if (block_index == BLOCK_BUFFER_SIZE) { block_index = 0; }
            return block_index;
        }
        public int plan_prev_block_index(int block_index)
        {
            if (block_index == 0)
            {
                block_index = BLOCK_BUFFER_SIZE;
            }
            block_index--;
            return block_index;
        }

        //public float plan_get_exec_block_exit_speed()
        //{
        //    int block_index = plan_next_block_index(block_buffer_tail);
        //    if (block_index == block_buffer_head) { return 0; }
        //    return (float)Math.Sqrt(block_buffer[block_index].entry_speed_sqr);
        //}
        public void plan_cycle_reinitialize()
        {

        }
        public int plan_get_block_buffer_count()
        {
            if (block_buffer_head >= block_buffer_tail) { return (block_buffer_head - block_buffer_tail); }
            return (BLOCK_BUFFER_SIZE - (block_buffer_tail - block_buffer_head));
        }
        public bool plan_check_full_buffer()
        {
            if (block_buffer_tail == next_buffer_head) return true;
            return false;
        }

    }
}
