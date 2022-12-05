using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using PurpleMoon.Graphics;

namespace PurpleMoon.Core
{
    public static class Debug
    {
        public static string CreateFormattedString(string fmt, object[] args)
        {
            string output = string.Empty;
            int i = 0, a = 0;

            while (i < fmt.Length)
            {
                if (fmt[i] == '%')
                {
                    i++;
                    if (a < 0 || a >= args.Length) { output += fmt[i]; }
                    else
                    {
                        if (fmt[i] == 'd') { output += ((int)args[a++]).ToString(); }
                        else if (fmt[i] == 'u') { output += ((uint)args[a++]).ToString(); }
                        else if (fmt[i] == 'l') { output += ((ulong)args[a++]).ToString(); }
                        else if (fmt[i] == 'f') { output += ((float)args[a++]).ToString(); }
                        else if (fmt[i] == 'p') { output += StringUtil.ConvertUIntToHex((uint)args[a++], 4); }
                        else if (fmt[i] == 'c') { output += ((char)args[a++]); }
                        else if (fmt[i] == 's') { output += args[a++].ToString(); }
                        else { output += fmt[i]; }
                    }
                }
                else { output += fmt[i]; }
                i++;
            }
            return output;
        }

        public static void WriteArguments(string fmt, object[] args)
        {
            string str = CreateFormattedString(fmt, args);
            Write(str);
        }

        public static void Write(string fmt, params object[] args)
        {
            string str = CreateFormattedString(fmt, args);
            Write(str);
        }

        public static void Write(char c) { Console.Write(c); }

        public static void Write(string txt) { Console.Write(txt); }

        public static void WriteLine(string txt) { Console.WriteLine(txt); }

        public static void WriteHeader(string hdr, ConsoleColor color)
        {
            ConsoleColor fg = Console.ForegroundColor;
            Console.Write("[");
            Console.ForegroundColor = color;
            Console.Write(hdr);
            Console.ForegroundColor = fg;
            Console.Write("] ");
        }

        public static void Info(string fmt, params object[] args)
        {
            WriteHeader("  >>  ", ConsoleColor.Cyan);
            Write(fmt, args);
            Console.Write('\n');
        }

        public static void OK(string fmt, params object[] args)
        {
            WriteHeader("  OK  ", ConsoleColor.Green);
            Write(fmt, args);
            Console.Write('\n');
        }

        public static void Panic(string fmt, params object[] args)
        {
            if (Renderer.Enabled)
            {
                Renderer.Clear(new Color(0xFF, 0x7F, 0x00, 0x00));
                string str = CreateFormattedString(fmt, args);
                Renderer.DrawString(0, 0, str, Color.White, Color.Transparent, Assets.GetFont("Default"));
                Renderer.DrawString(0, 16, "The system has been halted to prevent damage to your computer", Color.White, Color.Transparent, Assets.GetFont("Default"));
                Renderer.Update();
                Halt();
            }
            else
            {
                Console.Clear();
                WriteHeader("  !!  ", ConsoleColor.Red);
                Write(fmt, args);
                Console.WriteLine("\nThe system has been halted to prevent damage to your computer");
                Halt();
            }
        }

        public static void Halt()
        {
            Cosmos.Core.CPU.DisableInterrupts();
            while (true) { }
        }
    }
}
