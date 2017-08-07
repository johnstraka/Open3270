﻿using System;
using Open3270;
//
namespace Sample
{
    /// <summary
    /// Summary description for TheDemo.
    /// </summary>
    public class TheDemo : IAudit
    {
        public TheDemo()
        {
        }
        public void Run()
        {
            TNEmulator emulator = new TNEmulator();
            emulator.Audit = this;
            emulator.Debug = false;
            emulator.Config.TermType = "IBM-3278-2-E";
            emulator.Config.FastScreenMode = true;

            emulator.Connect("localhost", 3270, null);
            // wait for the host to startup
            //
            if (emulator.WaitForTextOnScreen(2000, "Multi-User System for Interactive Computing / System Product", "MUSIC/SP ESA/390")==-1)
            {
                Console.WriteLine("Connection failed - didn't find 'LIBRARY OF CONGRESS' on screen");
                Console.WriteLine(emulator.CurrentScreenXML.Dump());
                return;
            }
            //
            //
            //
            emulator.Refresh(true, 20000);
            bool done = false;
            do
            {
                string currentScreen = emulator.CurrentScreenXML.Dump();
                Console.WriteLine(currentScreen);


                Console.WriteLine("Enter command : - refresh, text <text>, key <key>, quit");
                Console.WriteLine("key is one of : Attn, Backspace,BackTab,CircumNot,Clear,CursorSelect,Delete,DeleteField, DeleteWord,Down,Dup,Enter,Erase,EraseEOF,EraseInput,FieldEnd, FieldMark,FieldExit,Home,Insert,Interrupt,Key,Left,Left2,Newline,NextWord, PAnn, PFnn, PreviousWord,Reset,Right,Right2,SysReq,Tab,Toggle,ToggleInsert,ToggleReverse,Up");

                string command = Console.ReadLine();
                if (command == "quit")
                    done = true;
                else if (command == "refresh")
                {
                    emulator.Refresh(false, 0);
                }
                else if (command.Length > 4 && command.Substring(0, 4) == "key ")
                {
                    emulator.SendKeyFromText(true, command.Substring(4));
                }
                else if (command.Length > 5 && command.Substring(0, 5) == "text ")
                {
                    emulator.SendText(command.Substring(5));
                }
                else
                    Console.WriteLine("unknown command '" + command + "'");
            }
            while (!done);
            emulator.Close();

        }
        #region IAudit Members

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void Write(string text)
        {
            Console.Write(text);
        }

        #endregion
    }
}
