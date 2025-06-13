// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Input
{
    internal static class KeysHelper
    {
        static HashSet<int> _map;

        static KeysHelper()
        {
            _map = new HashSet<int>();
            var allKeys = (Keys[])Enum.GetValues(typeof(Keys));
            foreach (var key in allKeys)
            {
                _map.Add((int)key);
            }
        }

        /// <summary>
        /// Checks if specified value is valid Key.
        /// </summary>
        /// <param name="value">Keys base value</param>
        /// <returns>Returns true if value is valid Key, false otherwise</returns>
        public static bool IsKey(int value)
        {
            return _map.Contains(value);
        }

        public static bool GetKeyChar(Keys key, bool isShiftDown, out char keyCode)
        {
            if (key == Keys.A) { keyCode = isShiftDown ? 'A' : 'a'; return true; }
            if (key == Keys.B) { keyCode = isShiftDown ? 'B' : 'b'; return true; }
            if (key == Keys.C) { keyCode = isShiftDown ? 'C' : 'c'; return true; }
            if (key == Keys.D) { keyCode = isShiftDown ? 'D' : 'd'; return true; }
            if (key == Keys.E) { keyCode = isShiftDown ? 'E' : 'e'; return true; }
            if (key == Keys.F) { keyCode = isShiftDown ? 'F' : 'f'; return true; }
            if (key == Keys.G) { keyCode = isShiftDown ? 'G' : 'g'; return true; }
            if (key == Keys.H) { keyCode = isShiftDown ? 'H' : 'h'; return true; }
            if (key == Keys.I) { keyCode = isShiftDown ? 'I' : 'i'; return true; }
            if (key == Keys.J) { keyCode = isShiftDown ? 'J' : 'j'; return true; }
            if (key == Keys.K) { keyCode = isShiftDown ? 'K' : 'k'; return true; }
            if (key == Keys.L) { keyCode = isShiftDown ? 'L' : 'l'; return true; }
            if (key == Keys.M) { keyCode = isShiftDown ? 'M' : 'm'; return true; }
            if (key == Keys.N) { keyCode = isShiftDown ? 'N' : 'n'; return true; }
            if (key == Keys.O) { keyCode = isShiftDown ? 'O' : 'o'; return true; }
            if (key == Keys.P) { keyCode = isShiftDown ? 'P' : 'p'; return true; }
            if (key == Keys.Q) { keyCode = isShiftDown ? 'Q' : 'q'; return true; }
            if (key == Keys.R) { keyCode = isShiftDown ? 'R' : 'r'; return true; }
            if (key == Keys.S) { keyCode = isShiftDown ? 'S' : 's'; return true; }
            if (key == Keys.T) { keyCode = isShiftDown ? 'T' : 't'; return true; }
            if (key == Keys.U) { keyCode = isShiftDown ? 'U' : 'u'; return true; }
            if (key == Keys.V) { keyCode = isShiftDown ? 'V' : 'v'; return true; }
            if (key == Keys.W) { keyCode = isShiftDown ? 'W' : 'w'; return true; }
            if (key == Keys.X) { keyCode = isShiftDown ? 'X' : 'x'; return true; }
            if (key == Keys.Y) { keyCode = isShiftDown ? 'Y' : 'y'; return true; }
            if (key == Keys.Z) { keyCode = isShiftDown ? 'Z' : 'z'; return true; }

            if (((key == Keys.D0) && !isShiftDown) || (key == Keys.NumPad0)) { keyCode = '0'; return true; }
            if (((key == Keys.D1) && !isShiftDown) || (key == Keys.NumPad1)) { keyCode = '1'; return true; }
            if (((key == Keys.D2) && !isShiftDown) || (key == Keys.NumPad2)) { keyCode = '2'; return true; }
            if (((key == Keys.D3) && !isShiftDown) || (key == Keys.NumPad3)) { keyCode = '3'; return true; }
            if (((key == Keys.D4) && !isShiftDown) || (key == Keys.NumPad4)) { keyCode = '4'; return true; }
            if (((key == Keys.D5) && !isShiftDown) || (key == Keys.NumPad5)) { keyCode = '5'; return true; }
            if (((key == Keys.D6) && !isShiftDown) || (key == Keys.NumPad6)) { keyCode = '6'; return true; }
            if (((key == Keys.D7) && !isShiftDown) || (key == Keys.NumPad7)) { keyCode = '7'; return true; }
            if (((key == Keys.D8) && !isShiftDown) || (key == Keys.NumPad8)) { keyCode = '8'; return true; }
            if (((key == Keys.D9) && !isShiftDown) || (key == Keys.NumPad9)) { keyCode = '9'; return true; }

            if ((key == Keys.D0) && isShiftDown) { keyCode = ')'; return true; }
            if ((key == Keys.D1) && isShiftDown) { keyCode = '!'; return true; }
            if ((key == Keys.D2) && isShiftDown) { keyCode = '@'; return true; }
            if ((key == Keys.D3) && isShiftDown) { keyCode = '#'; return true; }
            if ((key == Keys.D4) && isShiftDown) { keyCode = '$'; return true; }
            if ((key == Keys.D5) && isShiftDown) { keyCode = '%'; return true; }
            if ((key == Keys.D6) && isShiftDown) { keyCode = '^'; return true; }
            if ((key == Keys.D7) && isShiftDown) { keyCode = '&'; return true; }
            if ((key == Keys.D8) && isShiftDown) { keyCode = '*'; return true; }
            if ((key == Keys.D9) && isShiftDown) { keyCode = '('; return true; }

            if (key == Keys.Space) { keyCode = ' '; return true; }
            if (key == Keys.Tab) { keyCode = '\t'; return true; }
            if (key == Keys.Enter) { keyCode = (char)13; return true; }
            if (key == Keys.Back) { keyCode = (char)8; return true; }

            if (key == Keys.Add) { keyCode = '+'; return true; }
            if (key == Keys.Decimal) { keyCode = '.'; return true; }
            if (key == Keys.Divide) { keyCode = '/'; return true; }
            if (key == Keys.Multiply) { keyCode = '*'; return true; }
            if (key == Keys.OemBackslash) { keyCode = '\\'; return true; }
            if ((key == Keys.OemComma) && !isShiftDown) { keyCode = ','; return true; }
            if ((key == Keys.OemComma) && isShiftDown) { keyCode = '<'; return true; }
            if ((key == Keys.OemOpenBrackets) && !isShiftDown) { keyCode =  '['; return true; }
            if ((key == Keys.OemOpenBrackets) && isShiftDown) { keyCode =  '{'; return true; }
            if ((key == Keys.OemCloseBrackets) && !isShiftDown) { keyCode = ']'; return true; }
            if ((key == Keys.OemCloseBrackets) && isShiftDown) { keyCode = '}'; return true; }
            if ((key == Keys.OemPeriod) && !isShiftDown) { keyCode = '.'; return true; }
            if ((key == Keys.OemPeriod) && isShiftDown) { keyCode = '>'; return true; }
            if ((key == Keys.OemPipe) && !isShiftDown) { keyCode = '\\'; return true; }
            if ((key == Keys.OemPipe) && isShiftDown) { keyCode = '|'; return true; }
            if ((key == Keys.OemPlus) && !isShiftDown) { keyCode = '='; return true; }
            if ((key == Keys.OemPlus) && isShiftDown) { keyCode = '+'; return true; }
            if ((key == Keys.OemMinus) && !isShiftDown) { keyCode = '-'; return true; }
            if ((key == Keys.OemMinus) && isShiftDown) { keyCode = '_'; return true; }
            if ((key == Keys.OemQuestion) && !isShiftDown) { keyCode = '/'; return true; }
            if ((key == Keys.OemQuestion) && isShiftDown) { keyCode = '?'; return true; }
            if ((key == Keys.OemQuotes) && !isShiftDown) { keyCode = '\''; return true; }
            if ((key == Keys.OemQuotes) && isShiftDown) { keyCode = '"'; return true; }
            if ((key == Keys.OemSemicolon) && !isShiftDown) { keyCode = ';'; return true; }
            if ((key == Keys.OemSemicolon) && isShiftDown) { keyCode = ':'; return true; }
            if ((key == Keys.OemTilde) && !isShiftDown) { keyCode = '`'; return true; }
            if ((key == Keys.OemTilde) && isShiftDown) { keyCode = '~'; return true; }
            if (key == Keys.Subtract) { keyCode = '-'; return true; }

            keyCode = (char)0;
            return false;
        }
    }
}
