﻿using System;
using System.IO;
using System.Threading;
using JustPoChess.Client.MVC.View.Art;
using JustPoChess.Client.MVC.View.Sounds;

namespace JustPoChess.Client.MVC.View.Menu
{
    public static class Menu
    {
        public static void InitialScreen()
        {
            OST.PlayInitialScreenOST();
            Thread.Sleep(300);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(MenuArt.JustPoChessLogo);
            Console.WriteLine();

            Thread.Sleep(300);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(MenuArt.JustPoChessText);

            Console.SetCursorPosition(0, 35);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(MenuArt.CopyrightLogo);

            bool visible = false;

            Console.ForegroundColor = ConsoleColor.Gray;
            do
            {
                string alert = visible ? $"{MenuArt.Continue}" : "";
                Console.SetCursorPosition(0, 31);
                visible = !visible;
                Console.WriteLine(alert);
                Thread.Sleep(1000);
                Console.SetCursorPosition(0, 31);
                Console.Write(new string(' ', Console.WindowWidth));

            } while (!Console.KeyAvailable);

            SoundEffects.PlaySelectionSound();
            OST.Stop();

            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < 18; i++)
            {
                string alert = visible ? $"{MenuArt.Continue}" : "";
                Console.SetCursorPosition(0, 31);
                visible = !visible;
                Console.WriteLine(alert);
                Thread.Sleep(100);
                Console.SetCursorPosition(0, 31);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.Clear();
            Console.ResetColor();
        }

        public static void InitializeMenu()
        {
            OST.PlayMenuOST();
            
            var menuOptions = 1;

            while (true)
            {
                if (Console.KeyAvailable)
                {    
                    var userInput = Console.ReadKey();
                    switch (userInput.Key)
                    {
                        case ConsoleKey.UpArrow:
                            SoundEffects.PlayTraverseSound();
                            if (menuOptions == 1)
                            {
                                menuOptions = 5;
                            }
                            else
                            {
                                menuOptions--;
                            }
                            break;

                        case ConsoleKey.DownArrow:
                            SoundEffects.PlayTraverseSound();
                            if (menuOptions == 5)
                            {
                                menuOptions = 1;
                            }
                            else
                            {
                                menuOptions++;
                            }
                            break;

                        case ConsoleKey.Enter:
                            switch (menuOptions)
                            {
                                case 1:
                                    break;

                                case 2:
                                    break;

                                case 3:
                                    break;

                                case 4:                                
                                    break;

                                case 5:
                                    Environment.Exit(0);
                                    break;
                            }
                            break;
                    }

                    Console.Clear();
                    switch (menuOptions)
                    {
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(MenuArt.HotSeatText);
                            Console.ResetColor();
                            Console.WriteLine(MenuArt.OnlineText);
                            Console.WriteLine(MenuArt.VersusAiText);
                            Console.WriteLine(MenuArt.AiVersusAiText);
                            Console.WriteLine(MenuArt.ExitText);
                            break;

                        case 2:
                            Console.WriteLine(MenuArt.HotSeatText);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(MenuArt.OnlineText);
                            Console.ResetColor();
                            Console.WriteLine(MenuArt.VersusAiText);
                            Console.WriteLine(MenuArt.AiVersusAiText);
                            Console.WriteLine(MenuArt.ExitText);
                            break;

                        case 3:
                            Console.WriteLine(MenuArt.HotSeatText);
                            Console.WriteLine(MenuArt.OnlineText);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(MenuArt.VersusAiText);
                            Console.ResetColor();
                            Console.WriteLine(MenuArt.AiVersusAiText);
                            Console.WriteLine(MenuArt.ExitText);
                            break;
                        case 4:
                            Console.WriteLine(MenuArt.HotSeatText);
                            Console.WriteLine(MenuArt.OnlineText);
                            Console.WriteLine(MenuArt.VersusAiText);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(MenuArt.AiVersusAiText);
                            Console.ResetColor();
                            Console.WriteLine(MenuArt.ExitText);
                            break;

                        case 5:
                            Console.WriteLine(MenuArt.HotSeatText);
                            Console.WriteLine(MenuArt.OnlineText);
                            Console.WriteLine(MenuArt.VersusAiText);
                            Console.WriteLine(MenuArt.AiVersusAiText);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(MenuArt.ExitText);
                            Console.ResetColor();
                            break;
                    }
                }
            }
        }
    }
}