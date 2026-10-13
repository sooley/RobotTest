using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest
{
    public static class ConstIndex
    {

        public const byte MODAL_GROUP_G0 = 0;
        public const byte MODAL_GROUP_G1 = 1;
        public const byte MODAL_GROUP_G2 = 2;
        public const byte MODAL_GROUP_G3 = 3;
        public const byte MODAL_GROUP_G4 = 4;
        public const byte MODAL_GROUP_G5 = 5;
        public const byte MODAL_GROUP_G6 = 6;
        public const byte MODAL_GROUP_G7 = 7;
        public const byte MODAL_GROUP_G8 = 8;
        public const byte MODAL_GROUP_G12 = 9;
        public const byte MODAL_GROUP_G13 = 10;
        public const byte MODAL_GROUP_M4 = 11;
        public const byte MODAL_GROUP_M7 = 12;
        public const byte MODAL_GROUP_M8 = 13;

        public const byte NON_MODAL_NO_ACTION = 0;
        public const byte NON_MODAL_DWELL = 1;
        public const byte NON_MODAL_SET_COORDINATE_DATA = 2;
        public const byte NON_MODAL_GO_HOME_0 = 3;
        public const byte NON_MODAL_SET_HOME_0 = 4;
        public const byte NON_MODAL_GO_HOME_1 = 5;
        public const byte NON_MODAL_SET_HOME_1 = 6;
        public const byte NON_MODAL_ABSOLUTE_OVERRIDE = 7;
        public const byte NON_MODAL_SET_COORDINATE_OFFSET = 8;
        public const byte NON_MODAL_RESET_COORDINATE_OFFSET = 9;

        public const byte MOTION_MODE_SEEK = 0;
        public const byte MOTION_MODE_LINEAR = 1;
        public const byte MOTION_MODE_CW_ARC = 2;
        public const byte MOTION_MODE_CCW_ARC = 3;
        public const byte MOTION_MODE_PROBE_TOWARD = 4;
        public const byte MOTION_MODE_PROBE_TOWARD_NO_ERROR = 5;
        public const byte MOTION_MODE_PROBE_AWAY = 6;
        public const byte MOTION_MODE_PROBE_AWAY_NO_ERROR = 7;
        public const byte MOTION_MODE_NONE = 8;

        public const byte PLANE_SELECT_XY = 0;
        public const byte PLANE_SELECT_ZX = 1;
        public const byte PLANE_SELECT_YZ = 2;

        public const byte DISTANCE_MODE_ABSOLUTE = 0;//G90
        public const byte DISTANCE_MODE_INCREMENTAL = 1;//G91

        public const byte DISTANCE_ARC_MODE_INCREMENTAL = 0; // G91.1 

        public const byte PROGRAM_FLOW_RUNNING = 0;
        public const byte PROGRAM_FLOW_PAUSED = 1; //M0 M1
        public const byte PROGRAM_FLOW_COMPLETED = 2; //M2 M30


        public const byte FEED_RATE_MODE_UNITS_PER_MIN = 0;// G94
        public const byte FEED_RATE_MODE_INVERSE_TIME = 1; // G93


        public const byte UNITS_MODE_MM = 0;
        public const byte UNITS_MODE_INCHES = 1;


        public const byte CUTTER_COMP_DISABLE = 0;

        public const byte CONTROL_MODE_EXACT_PATH = 0;

        public const byte SPINDLE_DISABLE = 0;
        public const byte SPINDLE_ENABLE_CW = 1;
        public const byte SPINDLE_ENABLE_CCW = 2;


        public const byte COOLANT_DISABLE = 0;
        public const byte COOLANT_MIST_ENABLE = 1;
        public const byte COOLANT_FLOOD_ENABLE = 2;


        public const byte TOOL_LENGTH_OFFSET_CANCEL = 0;
        public const byte TOOL_LENGTH_OFFSET_ENABLE_DYNAMIC = 1;


        public const byte WORD_F = 0;
        public const byte WORD_I = 1;
        public const byte WORD_J = 2;
        public const byte WORD_K = 3;
        public const byte WORD_L = 4;
        public const byte WORD_N = 5;
        public const byte WORD_P = 6;
        public const byte WORD_R = 7;
        public const byte WORD_S = 8;
        public const byte WORD_T = 9;
        public const byte WORD_X = 10;
        public const byte WORD_Y = 11;
        public const byte WORD_Z = 12;

        public const byte GC_UPDATE_POS_TARGET = 0;
        public const byte GC_UPDATE_POS_SYSTEM = 1;
        public const byte GC_UPDATE_POS_NONE = 2;


    }
}
