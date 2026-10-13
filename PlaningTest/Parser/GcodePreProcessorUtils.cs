using GlmSharp;
using PlaningTest.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static PlaningTest.Parser.PointSegment;

namespace PlaningTest.Parser
{
    class GcodePreProcessorUtils
    {
        public static String overrideSpeed(String command, double speed, double[] original = null)
        {
            Regex rg = new Regex("[Ff]([0-9.]+)");
            Match result = rg.Match(command);
            if (result.Success)
            {
                GroupCollection data = result.Groups;
                double temp1 = double.Parse(data[1].Value) * speed;
                command = Regex.Replace(command, "[Ff]([0-9.]+)", "F" + temp1);
            }
            return command;
        }
        public static String removeComment(String command)
        {
            Regex rg1 = new Regex("\\(+[^\\(]*\\)+");
            Regex rg2 = new Regex(";.*");
            if (command.Contains('(')) { command = command.Remove(rg1.Match(command).Index); }
            if (command.Contains(';')) { command = command.Remove(rg2.Match(command).Index); }
            return command.Trim();
        }
        public static String parseComment(String command)
        {
            Regex rg = new Regex("(\\([^\\(\\)]*\\)|;[^;].*)");
            Match result = rg.Match(command);
            if (result.Success)
            {
                GroupCollection data = result.Groups;
                command = data[1].Value;
                return command;
            }
            return "";
        }
        public static String truncateDecimals(int length, String command)
        {
            Regex rg = new Regex("(\\d*\\.\\d*)");
            int pos = 0;
            Match result = rg.Match(command, pos);
            while (result.Success)
            {
                pos = result.Index;
                GroupCollection data = result.Groups;
                string newNum = String.Format("{0:0.###}", Double.Parse(data[1].Value));
                command = command.Substring(0, pos) + newNum + command.Substring(pos + result.Length);
                pos += newNum.Length + 1;
                result = rg.Match(command, pos);
            }
            return command;
        }
        public static String removeAllWhitespace(String command)
        {
            return Regex.Replace(command, @"\s+", ""); ;
        }
        public static List<float> parseCodes(List<String> args, char code)
        {
            List<float> l = new List<float>();
            foreach (string s in args)
            {
                if (s.Length > 0 && Char.ToUpper(s[0]) == code) l.Add(float.Parse(s.Substring(1)));
            }
            return l;
        }
        public static List<int> parseGCodes(String command)
        {
            Regex rg = new Regex("[Gg]0*(\\d+)");
            List<int> codes = new List<int>();
            int pos = 0;
            Match result = rg.Match(command, pos);
            while (result.Success)
            {
                pos = result.Index;
                GroupCollection data = result.Groups;
                codes.Add(int.Parse(data[1].Value));
                pos = +result.Length;
                result = rg.Match(command, pos);
            }
            return codes;
        }
        public static List<int> parseMCodes(String command)
        {
            Regex rg = new Regex("[Mm]0*(\\d+)");
            List<int> codes = new List<int>();
            int pos = 0;
            Match result = rg.Match(command, pos);
            while (result.Success)
            {
                pos = result.Index;
                GroupCollection data = result.Groups;
                codes.Add(int.Parse(data[1].Value));
                pos = +result.Length;
                result = rg.Match(command, pos);
            }
            return codes;
        }
        public static double parseCoord(List<String> argList, char c)
        {

            foreach (String t in argList)
            {
                if (t.Length > 0 && Char.ToUpper(t[0]) == c) return Double.Parse(t.Substring(1));
            }
            return double.NaN;
        }
        public static List<String> splitCommand(String command)
        {
            List<String> l = new List<String>();
            bool readNumeric = false;
            String sb = "";
            char c;
            char[] cmd = command.ToCharArray();
            for (int i = 0; i < cmd.Length; i++)
            {
                c = cmd[i];
                if (readNumeric && !Char.IsDigit(c) && c != '.')
                {
                    readNumeric = false;
                    l.Add(sb);
                    sb = "";
                    if (Char.IsLetter(c)) sb += c;
                }
                else if (Char.IsDigit(c) || c == '.' || c == '-')
                {
                    sb += c;
                    readNumeric = true;
                }
                else if (Char.IsLetter(c)) sb += c;
            }
            if (sb.Length > 0) l.Add(sb);
            return l;
        }
        public static vec3 updatePointWithCommand(vec3 initial, double x, double y, double z, bool absoluteMode)
        {
            vec3 newPoint = new vec3(initial.x, initial.y, initial.z);
            if (absoluteMode)
            {
                if (!Double.IsNaN(x)) newPoint.x = (float)x;
                if (!Double.IsNaN(y)) newPoint.y = (float)y;
                if (!Double.IsNaN(z)) newPoint.z = (float)z;
            }
            else
            {
                if (!Double.IsNaN(x)) newPoint.x += (float)x;
                if (!Double.IsNaN(y)) newPoint.y += (float)y;
                if (!Double.IsNaN(z)) newPoint.z += (float)z;
            }
            return newPoint;
        }
        public static vec4 updatePointWithCommand(List<String> commandArgs, vec3 initial, bool absoluteMode)
        {
            double x = Double.NaN;
            double y = Double.NaN;
            double z = Double.NaN;
            double a = Double.NaN;
            char c;
            for (int i = 0; i < commandArgs.Count; i++)
            {
                if (commandArgs.ElementAt(i).Length > 0)
                {
                    c = Char.Parse(commandArgs.ElementAt(i).Substring(0, 1).ToUpper());
                    switch (c)
                    {
                        case 'X':
                            x = Double.Parse(commandArgs.ElementAt(i).Substring(1));
                            break;
                        case 'Y':
                            y = Double.Parse(commandArgs.ElementAt(i).Substring(1));
                            break;
                        case 'Z':
                            z = Double.Parse(commandArgs.ElementAt(i).Substring(1));
                            break;
                        case 'A':
                            a = Double.Parse(commandArgs.ElementAt(i).Substring(1));
                            break;
                    }
                }
            }
            vec3 tmp = updatePointWithCommand(initial, x, y, z, absoluteMode);
            vec4 ret = new vec4(tmp.x, tmp.y, tmp.z, (float)a);
            //Debug.WriteLine("UpdatePointX:" + ret.x + ",Y:" + ret.y + ",Z:" + ret.z);
            return ret;
        }
        public static vec3 updatePointWithCommand(String command, vec3 initial, bool absoluteMode)
        {
            List<String> l = splitCommand(command);
            vec4 tmp = updatePointWithCommand(l, initial, absoluteMode);
            vec3 ret = new vec3(tmp.x, tmp.y, tmp.z);
            return ret;
        }
        public static vec3 convertRToCenter(vec3 start, vec3 end, double radius, bool absoluteIJK, bool clockwise)
        {
            double R = radius;
            vec3 center = new vec3();
            double x = end.x - start.x;
            double y = end.y - start.y;
            double h_x2_div_d = 4 * R * R - x * x - y * y;
            h_x2_div_d = (-Math.Sqrt(h_x2_div_d)) / Math.Sqrt(x * x + y * y);
            if (!clockwise) h_x2_div_d = -h_x2_div_d;
            if (R < 0)
            {
                h_x2_div_d = -h_x2_div_d;
                radius -= radius;
            }
            double offsetX = 0.5 * (x - (y * h_x2_div_d));
            double offsetY = 0.5 * (y + (x * h_x2_div_d));
            if (!absoluteIJK)
            {
                center.x = start.x + (float)offsetX;
                center.y = start.y + (float)offsetY;
            }
            else
            {
                center.x = (float)offsetX;
                center.y = (float)offsetY;
            }
            return center;
        }
        public static vec3 updateCenterWithCommand(List<String> commandArgs, vec3 initial, vec3 nextPoint, bool absoluteIJKMode, bool clockwise)
        {
            double i = Double.NaN;
            double j = Double.NaN;
            double k = Double.NaN;
            double r = Double.NaN;
            char c;
            foreach (String t in commandArgs)
            {
                if (t.Length > 0)
                {
                    c = Char.Parse(t.Substring(0, 1).ToUpper());
                    switch (c)
                    {
                        case 'I':
                            i = Double.Parse(t.Substring(1));
                            break;
                        case 'J':
                            j = Double.Parse(t.Substring(1));
                            break;
                        case 'K':
                            k = Double.Parse(t.Substring(1));
                            break;
                        case 'R':
                            r = Double.Parse(t.Substring(1));
                            break;
                    }
                }
            }
            if (Double.IsNaN(i) && Double.IsNaN(j) && Double.IsNaN(k))
            {
                return convertRToCenter(initial, nextPoint, r, absoluteIJKMode, clockwise);
            }
            return updatePointWithCommand(initial, i, j, k, absoluteIJKMode);
        }
        public static double getAngle(vec3 start, vec3 end)
        {
            double deltaX = end.x - start.x;
            double deltaY = end.y - start.y;
            double angle = 0.0;

            if (deltaX != 0)
            { // prevent div by 0
              // it helps to know what quadrant you are in
                if (deltaX > 0 && deltaY >= 0)
                { // 0 - 90
                    angle = Math.Atan(deltaY / deltaX);
                }
                else if (deltaX < 0 && deltaY >= 0)
                { // 90 to 180
                    angle = Math.PI - Math.Abs(Math.Atan(deltaY / deltaX));
                }
                else if (deltaX < 0 && deltaY < 0)
                { // 180 - 270
                    angle = Math.PI + Math.Abs(Math.Atan(deltaY / deltaX));
                }
                else if (deltaX > 0 && deltaY < 0)
                { // 270 - 360
                    angle = Math.PI * 2 - Math.Abs(Math.Atan(deltaY / deltaX));
                }
            }
            else
            {
                // 90 deg
                if (deltaY > 0)
                {
                    angle = Math.PI / 2.0;
                }
                // 270 deg
                else
                {
                    angle = Math.PI * 3.0 / 2.0;
                }
            }
            return angle;
        }
        public static double calculateSweep(double startAngle, double endAngle, bool isCw)
        {
            double sweep;
            if (startAngle == endAngle)
            {
                sweep = (Math.PI * 2);
                // Arcs
            }
            else
            {
                // Account for full circles and end angles of 0/360
                if (endAngle == 0)
                {
                    endAngle = Math.PI * 2;
                }
                // Calculate distance along arc.
                if (!isCw && endAngle < startAngle)
                {
                    sweep = ((Math.PI * 2 - startAngle) + endAngle);
                }
                else if (isCw && endAngle > startAngle)
                {
                    sweep = ((Math.PI * 2 - endAngle) + startAngle);
                }
                else
                {
                    sweep = Math.Abs(endAngle - startAngle);
                }
            }
            return sweep;
        }

        public static List<vec3> generatePointsAlongArcBDring(planes plane, vec3 start, vec3 end, vec3 center, bool clockwise, double R, double minArcLength, double arcPrecision, bool arcDegreeMode)
        {
            //Debug.WriteLine("Start_Point: Start:X"+start.X.ToString()+"Y"+start.Y.ToString()+"Z"+start.Z.ToString());
            //Debug.WriteLine("End_Point: End:X"+end.X.ToString() + "Y" + end.Y.ToString() + "Z" + end.Z.ToString());
            double radius = R;
            mat4 m = new mat4();
            m = mat4.Identity;

            switch (plane)
            {
                case planes.XY:
                    break;
                case planes.ZX:
                    m = mat4.RotateX(90) * m;
                    break;
                case planes.YZ:
                    m = mat4.RotateY(-90) * m;
                    break;
            }
            start = MathHandle.Tranformnormal(start, m);
            end = MathHandle.Tranformnormal(end, m);
            center = MathHandle.Tranformnormal(center, m);
            if (Double.IsNaN(center.Length)) return new List<vec3>();
            if (radius == 0)
            {
                radius = Math.Sqrt(Math.Pow((start.x - center.x), 2) + Math.Pow((start.y - center.y), 2));
            }
            //Debug.WriteLine("radius:"+radius+"center:X"+center.X.ToString()+"Y"+center.Y.ToString()+"Z"+center.Z.ToString());
            double startAngle = getAngle(center, start);
            double endAngle = getAngle(center, end);
            double sweep = calculateSweep(startAngle, endAngle, clockwise);
            double arcLength = sweep * radius;

            int numPoints;
            if (arcDegreeMode && arcPrecision > 0)
            {
                numPoints = (int)Math.Max(1.0, sweep / (Math.PI * arcPrecision / 180));
            }
            else
            {
                if (arcPrecision <= 0 && minArcLength > 0)
                {
                    arcPrecision = minArcLength;
                }
                numPoints = (int)Math.Ceiling(arcLength / arcPrecision);
            }
            return generatePointsAlongArcBDring(plane, start, end, center, clockwise, radius, startAngle, sweep, numPoints);
        }
        public static List<vec3> generatePointsAlongArcBDring(planes plane, vec3 p1, vec3 p2, vec3 center, bool isCw, double radius, double startAngle, double sweep, int numPoints)
        {
            mat4 m = new mat4();
            m = mat4.Identity;
            switch (plane)
            {
                case planes.XY:
                    break;
                case planes.ZX:
                    m = m * mat4.RotateX(90);
                    break;
                case planes.YZ:
                    m = m * mat4.RotateY(-90);
                    break;
            }
            vec3 LineEnd = new vec3(p1.x, p1.y, p1.z);
            List<vec3> segments = new List<vec3>();
            double angle;

            if (radius == 0)
            {
                radius = Math.Sqrt(Math.Pow(p1.x - center.x, 2) + Math.Pow(p1.y - center.y, 2));
            }
            double zIncrement = (p2.z - p1.z) / numPoints;
            for (int i = 0; i < numPoints; i++)
            {
                if (isCw)
                {
                    angle = (startAngle - i * sweep / numPoints);
                }
                else
                {
                    angle = (startAngle + i * sweep / numPoints);
                }
                if (angle >= Math.PI * 2)
                {
                    angle = angle - Math.PI * 2;
                }
                LineEnd.x = (float)(radius * Math.Cos(angle) + center.x);
                LineEnd.y = (float)(radius * Math.Sin(angle) + center.y);
                LineEnd.z = (float)(LineEnd.z + zIncrement);
                segments.Add(MathHandle.Tranformnormal(LineEnd, m));
            }
            segments.Add(MathHandle.Tranformnormal(p2, m));
            return segments;
        }

        public static vec3 rotateAxis(vec3 init, double angle)
        {
            vec3 newPoint = new vec3();
            newPoint.x = init.x;
            newPoint.y = (float)(init.y * Math.Cos(angle * Math.PI / 180) + init.z * Math.Sin(angle * Math.PI / 180));
            newPoint.z = (float)(init.z * Math.Cos(angle * Math.PI / 180) - init.y * Math.Sin(angle * Math.PI / 180));
            return newPoint;
        }
        public static String generateG1FromPoints(vec3 start, vec3 end, bool absoluteMode, int precision)
        {
            //tạo code G1 từ điểm đầu và điểm cuối
            return null;
        }
    }
}
