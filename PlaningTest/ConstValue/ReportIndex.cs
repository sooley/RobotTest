using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest
{
    public static class ReportIndex
    {
        public const byte STATUS_OK = 0;
        public const byte STATUS_EXPECTED_COMMAND_LETTER = 1;
        public const byte STATUS_BAD_NUMBER_FORMAT = 2;
        public const byte STATUS_INVALID_STATEMENT = 3;
        public const byte STATUS_NEGATIVE_VALUE = 4;
        public const byte STATUS_SETTING_DISABLED = 5;
        public const byte STATUS_SETTING_STEP_PULSE_MIN = 6;
        public const byte STATUS_SETTING_READ_FAIL = 7;
        public const byte STATUS_IDLE_ERROR = 8;
        public const byte STATUS_ALARM_LOCK = 9;
        public const byte STATUS_SOFT_LIMIT_ERROR = 10;
        public const byte STATUS_OVERFLOW = 11;
        public const byte STATUS_MAX_STEP_RATE_EXCEEDED = 12;

        public const byte STATUS_GCODE_UNSUPPORTED_COMMAND = 20;
        public const byte STATUS_GCODE_MODAL_GROUP_VIOLATION = 21;
        public const byte STATUS_GCODE_UNDEFINED_FEED_RATE = 22;
        public const byte STATUS_GCODE_COMMAND_VALUE_NOT_INTEGER = 23;
        public const byte STATUS_GCODE_AXIS_COMMAND_CONFLICT = 24;
        public const byte STATUS_GCODE_WORD_REPEATED = 25;
        public const byte STATUS_GCODE_NO_AXIS_WORDS = 26;
        public const byte STATUS_GCODE_INVALID_LINE_NUMBER = 27;
        public const byte STATUS_GCODE_VALUE_WORD_MISSING = 28;
        public const byte STATUS_GCODE_UNSUPPORTED_COORD_SYS = 29;
        public const byte STATUS_GCODE_G53_INVALID_MOTION_MODE = 30;
        public const byte STATUS_GCODE_AXIS_WORDS_EXIST = 31;
        public const byte STATUS_GCODE_NO_AXIS_WORDS_IN_PLANE = 32;
        public const byte STATUS_GCODE_INVALID_TARGET = 33;
        public const byte STATUS_GCODE_ARC_RADIUS_ERROR = 34;
        public const byte STATUS_GCODE_NO_OFFSETS_IN_PLANE = 35;
        public const byte STATUS_GCODE_UNUSED_WORDS = 36;
        public const byte STATUS_GCODE_G43_DYNAMIC_AXIS_ERROR = 37;

        public const byte ALARM_HARD_LIMIT_ERROR = 1;
        public const byte ALARM_SOFT_LIMIT_ERROR = 2;
        public const byte ALARM_ABORT_CYCLE = 3;
        public const byte ALARM_PROBE_FAIL = 4;
        public const byte ALARM_HOMING_FAIL = 5;

        // Define Grbl feedback message codes.
        public const byte MESSAGE_CRITICAL_EVENT = 1;
        public const byte MESSAGE_ALARM_LOCK = 2;
        public const byte MESSAGE_ALARM_UNLOCK = 3;
        public const byte MESSAGE_ENABLED = 4;
        public const byte MESSAGE_DISABLED = 5;
        public const byte MESSAGE_SAFETY_DOOR_AJAR = 6;
        public const byte MESSAGE_PROGRAM_END=7;
        public const byte MESSAGE_RESTORE_DEFAULTS = 8;


    }
}
