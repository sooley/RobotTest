using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningTest.ConstValue
{
    public static class SettingConfig
    {
        public const byte SETTINGS_VERSION = 9;

        // Boolean settings flags (settings.flags)
        public const byte BITFLAG_REPORT_INCHES = 1 << 0;
        // public const byte BITFLAG_AUTO_START      = 1 << 1; // Obsolete
        public const byte BITFLAG_INVERT_ST_ENABLE = 1 << 2;
        public const byte BITFLAG_HARD_LIMIT_ENABLE = 1 << 3;
        public const byte BITFLAG_HOMING_ENABLE = 1 << 4;
        public const byte BITFLAG_SOFT_LIMIT_ENABLE = 1 << 5;
        public const byte BITFLAG_INVERT_LIMIT_PINS = 1 << 6;
        public const byte BITFLAG_INVERT_PROBE_PIN = 1 << 7;

        // Status report mask
        public const byte BITFLAG_RT_STATUS_MACHINE_POSITION = 1 << 0;
        public const byte BITFLAG_RT_STATUS_WORK_POSITION = 1 << 1;
        public const byte BITFLAG_RT_STATUS_PLANNER_BUFFER = 1 << 2;
        public const byte BITFLAG_RT_STATUS_SERIAL_RX = 1 << 3;
        public const byte BITFLAG_RT_STATUS_LIMIT_PINS = 1 << 4;

        // Restore flags
        public const byte SETTINGS_RESTORE_ALL = 0xFF;
        public const byte SETTINGS_RESTORE_DEFAULTS = 1 << 0;
        public const byte SETTINGS_RESTORE_PARAMETERS = 1 << 1;
        public const byte SETTINGS_RESTORE_STARTUP_LINES = 1 << 2;
        public const byte SETTINGS_RESTORE_BUILD_INFO = 1 << 3;

        // EEPROM addresses
        public const ushort EEPROM_ADDR_GLOBAL = 1;
        public const ushort EEPROM_ADDR_PARAMETERS = 512;
        public const ushort EEPROM_ADDR_STARTUP_BLOCK = 768;
        public const ushort EEPROM_ADDR_BUILD_INFO = 942;

        // Coordinate systems
        public const byte N_COORDINATE_SYSTEM = 6;
        public const byte SETTING_INDEX_NCOORD = N_COORDINATE_SYSTEM + 1;

        public const byte SETTING_INDEX_G28 = N_COORDINATE_SYSTEM;
        public const byte SETTING_INDEX_G30 = N_COORDINATE_SYSTEM + 1;

        // Axis settings
        public const byte AXIS_N_SETTINGS = 4;
        public const byte AXIS_SETTINGS_START_VAL = 100;
        public const byte AXIS_SETTINGS_INCREMENT = 10;
    }
}
