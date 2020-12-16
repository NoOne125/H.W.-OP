using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Арканоид
{
    //Много лишнего кода, бывают ГЛЮКИ, но программа рабочая и без БАГОВ ~ :)
    class Program
    {
        static int[,] arr = new int[7, 7];
        const int bl_w = 11, bl_h = 3;
        static int c_w = 85, c_h = 80, pl_x, pl_y, pl_w = 14, life = 3, blocks = 0;
        static bool sti = false;
        static Random rand = new Random();
        static ConsoleKey k;
        static int[] colors = { 11, 12, 13, 10, 14, 9 };
        static Timer time;
        static double angle, sh_x, sh_y, step_x, step_y;
        static void Game(object obj)
        {
            if (life > 0)
            {
                if (blocks != 0)
                {
                    //Движение платформы
                    if (k == ConsoleKey.Spacebar)
                        sti = false;
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    if (k == ConsoleKey.LeftArrow)
                    {
                        if (pl_x > 2)
                            pl_x -= 3;
                        if (sti)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Draw_p((int)sh_x, (int)sh_y, "   ");
                            Draw_p((int)sh_x, (int)sh_y + 1, "   ");
                            sh_x = pl_x + pl_w / 2 - 1;
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                        }
                        k = 0;
                        Draw_p(pl_x, pl_y, "   ");
                    }
                    if (k == ConsoleKey.RightArrow)
                    {
                        if (pl_x < c_w - pl_w - 2)
                            pl_x += 3;
                        if (sti)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Draw_p((int)sh_x, (int)sh_y, "   ");
                            Draw_p((int)sh_x, (int)sh_y + 1, "   ");
                            sh_x = pl_x + pl_w / 2 - 1;
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                        }
                        k = 0;
                        Draw_p(pl_x + pl_w - 3, pl_y, "   ");
                    }
                    Console.BackgroundColor = ConsoleColor.Gray;
                    if (pl_x + pl_w < c_w - 2)
                    {
                        Draw_p(pl_x + pl_w, pl_y, "   ");
                    }
                    if (pl_x > 2)
                    {
                        Draw_p(pl_x - 3, pl_y, "   ");
                    }
                    //Движение шара
                    if (!sti)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Draw_p((int)sh_x, (int)sh_y, "   ");
                        Draw_p((int)sh_x, (int)sh_y + 1, "   ");
                        sh_x = sh_x + step_x;
                        sh_y = sh_y + step_y;
                    }
                    if (sh_y > pl_y + 2)
                    {
                        life--;
                        Life_upd();
                        sh_y = c_h - 11;
                        sh_x = pl_x + pl_w / 2 - 1;
                        k = 0;
                        sti = true;
                    }
                    if ((int)Math.Abs(pl_y - (int)sh_y - 1.5) <= 0.5 && sh_x >= pl_x - 2 && sh_x <= pl_x + pl_w)
                    {
                        step_y = -step_y;
                        sh_y = pl_y - 2;
                        if (Math.Min(Math.Abs(pl_x + pl_w / 2 - sh_x - 2), Math.Abs(pl_x + pl_w / 2 - sh_x)) == Math.Abs(pl_x + pl_w / 2 - sh_x - 2))
                            angle = 2 * Math.PI / 3 - Math.PI / 6 / (pl_w / 2) * (sh_x - pl_x - pl_w / 2 + 2);
                        else
                            angle = Math.PI / 3 - Math.PI / 6 / (pl_w / 2) * (sh_x - pl_x - pl_w / 2);
                        step_x = Math.Cos(angle) * 2;
                        step_y = -Math.Sin(angle) * 2;
                    }
                    else
                    {
                        BUHHH();
                    }
                    if (sh_x < 0)
                    {
                        step_x = -step_x;
                        sh_x = 0;
                    }
                    if (sh_x > c_w - 3)
                    {
                        step_x = -step_x;
                        sh_x = c_w - 3;
                    }
                    if (sh_y < 0)
                    {
                        step_y = -step_y;
                        sh_y = 0;
                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Draw_p((int)sh_x, (int)sh_y, "   ");
                    Draw_p((int)sh_x, (int)sh_y + 1, "   ");
                }
                else
                {
                    if (Console.Title == "Арканоид (раунд 3)" || Console.Title == "Арканоид (Финал)")
                    {
                        Console.Title = "Арканоид (Финал)";
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        //Знаю, выглядит жутко :)
                        Draw_p(c_w / 2 - 10, c_h / 2 - 4, "      ##    ##      ");
                        Draw_p(c_w / 2 - 10, c_h / 2 - 3, "      ##    ##      ");
                        Draw_p(c_w / 2 - 10, c_h / 2 - 2, "                    ");
                        Draw_p(c_w / 2 - 10, c_h / 2 - 1, "#                  #");
                        Draw_p(c_w / 2 - 10, c_h / 2, " ###            ### ");
                        Draw_p(c_w / 2 - 10, c_h / 2 + 1, "    ############    ");
                        //Вот мелодия, что тут должна звучать (через бит не решился делать)): https://www.youtube.com/watch?v=eUcThHVbrXY&list=PLpJl5XaLHtLX-pDk4kctGxtF4nq6BIyjg&index=79
                        if (k == ConsoleKey.Enter)
                        {
                            Round_one();
                            Go();
                        }
                    }

                    else if (Console.Title == "Арканоид (раунд 2)")
                    {
                        Round_three();
                        Go();
                    }

                    else if (Console.Title == "Арканоид (раунд 1)")
                    {
                        Round_two();
                        Go();
                    }
                    else if (Console.Title == "Арканоид")
                    {
                        Round_one();
                        Go();
                    }
                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Draw_p((int)sh_x, (int)sh_y, "   ");
                Draw_p((int)sh_x, (int)sh_y + 1, "   ");
                sh_x = sh_x + step_x;
                sh_y = sh_y + step_y;
                if (sh_x < 0)
                {
                    step_x = -step_x;
                    sh_x = 0;
                }
                if (sh_x > c_w - 3)
                {
                    sh_x = c_w - 3;
                    step_x = -step_x;
                }
                if (sh_y < 0)
                {
                    step_y = -step_y;
                    sh_y = 0;
                }
                if (sh_y > c_h - 2)
                {
                    step_y = -step_y;
                    sh_y = c_h - 2;
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Draw_p((int)sh_x, (int)sh_y, "   ");
                Draw_p((int)sh_x, (int)sh_y + 1, "   ");
                if (k == ConsoleKey.Enter)
                {
                    Round_one();
                    Go();
                }
            }
        }

        static void Go()
        {
            life = 3;
            sh_x = c_w / 2 - 2;
            sh_y = c_h - 11;
            pl_x = c_w / 2 - pl_w / 2 - (c_w / 2 - pl_w / 2) % 3 / 2;
            pl_y = c_h - 9;
            sti = true;
            angle = Math.PI / 3;
            step_x = Math.Cos(angle) * 2;
            step_y = -Math.Sin(angle) * 2;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            for (int i = 0; i < pl_w; i++)
                Draw_p(pl_x + i, pl_y, " ");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i < c_w; i++)
                Draw_p(i, pl_y + 4, "—");
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = 0; i < life; i++)
            {
                Draw_p(i * 4 + 1, (int)pl_y + 6, "   ");
                Draw_p(i * 4 + 1, (int)pl_y + 7, "   ");
            }
        }


        static void BUHHH()
        {
            // Господи помилуй
            int N_x = -1, N_y = -1, X_or_Y = -1;
            if (Math.Abs((int)sh_x % (bl_w + 1) - bl_w + 0.5) == 0.5 || (int)sh_x % (bl_w + 1) == 0)
            {
                N_x = (int)sh_x / (bl_w + 1);
                if (step_x > 0 && (int)sh_x % (bl_w + 1) != 0 && N_x < 6)
                    N_x++;
                N_y = (int)sh_y / (bl_h + 1);
                if ((int)sh_y % (bl_h + 1) == bl_h)
                    N_y++;
                X_or_Y = 0;
            }
            if (Math.Abs((int)sh_y % (bl_h + 1) - bl_h + 1.5) == 0.5 && step_y < 0)
            {
                N_x = (int)sh_x / (bl_w + 1);
                N_y = (int)sh_y / (bl_h + 1);
                if ((int)sh_x % (bl_w + 1) == bl_w && N_y < 7)
                    if (arr[N_y, N_x] == 0)
                        N_x++;
                X_or_Y = 1;
            }
            if (Math.Abs((int)sh_y % (bl_h + 1) - (double)bl_h / 2) == (double)bl_h / 2 && step_y > 0)
            {
                N_x = (int)sh_x / (bl_w + 1);
                N_y = (int)sh_y / (bl_h + 1);
                if ((int)sh_y % (bl_h + 1) == bl_h)
                    N_y++;
                if ((int)sh_x % (bl_w + 1) == bl_w && N_y < 7)
                    if (arr[N_y, N_x] == 0)
                        N_x++;
                X_or_Y = 1;
            }
            if (N_y > -1 && N_y < 7 && N_x > -1 && N_x < 7)
            {
                if (arr[N_y, N_x] == 1)
                {
                    sh_x -= step_x;
                    sh_y -= step_y;
                    if (X_or_Y == 0)
                    {
                        step_x = -step_x;
                    }
                    if (X_or_Y == 1)
                    {
                        step_y = -step_y;
                    }

                    Del_bl(N_x, N_y);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.SetWindowSize(85, 80);
            Console.Title = "Арканоид";
            Console.ReadKey();
            Console.CursorVisible = false;
            time = new Timer(Game, null, 0, 40); //Такие глюки я ещё не видел...
            do
            {
                k = Console.ReadKey(true).Key;
            } while (k != ConsoleKey.Escape);
        }

        static void Round_one()
        {
            Console.Title = "Арканоид (раунд 1)";
            int[,] arr_r_1 = {
                { 1, 1, 0, 0, 1, 1, 0},
                { 0, 0, 1, 1, 0, 0, 1},
                { 1, 1, 0, 0, 1, 1, 0},
                { 0, 0, 1, 1, 0, 0, 1},
                { 1, 1, 0, 0, 1, 1, 0},
                { 0, 0, 1, 1, 0, 0, 1},
                { 1, 1, 0, 0, 1, 1, 0}

            };
            arr = arr_r_1;
            Draw_bl();
        }
        static void Round_two()
        {
            Console.Title = "Арканоид (раунд 2)";
            int[,] arr_r_2 = {
                { 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0},
                { 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0},
                { 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0},
                { 1, 1, 1, 1, 1, 1, 1}
            };
            arr = arr_r_2;
            Draw_bl();
        }
        static void Round_three()
        {
            Console.Title = "Арканоид (раунд 3)";
            int[,] arr_r_3 = {
                { 1, 1, 1, 1, 1, 1, 1},
                { 0, 1, 0, 1, 0, 1, 0},
                { 1, 1, 1, 1, 1, 1, 1},
                { 0, 1, 0, 1, 0, 1, 0},
                { 1, 1, 1, 1, 1, 1, 1},
                { 0, 1, 0, 1, 0, 1, 0},
                { 1, 1, 1, 1, 1, 1, 1}
            };
            arr = arr_r_3;
            Draw_bl();
        }

        static void Draw_bl()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Clear();
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (arr[i, j] == 1)
                    {
                        blocks++;
                        Console.BackgroundColor = (ConsoleColor)colors[rand.Next(0, 6)];
                        for (int i_2 = 0; i_2 < bl_h; i_2++)
                        {
                            Draw_p(j * (bl_w + 1) + 1, i * (bl_h + 1) + i_2, "           ");
                        }
                    }
                }
            }
        }

        static void Draw_p(int x, int y, string symb)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(symb);
        }

        static void Life_upd()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            if (life > 0)
            {
                Draw_p(life * 4 + 1, (int)pl_y + 6, "   ");
                Draw_p(life * 4 + 1, (int)pl_y + 7, "   ");
            }
            else
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Draw_p(c_w / 2 - 10, c_h / 2 - 4, "      ##    ##      ");
                Draw_p(c_w / 2 - 10, c_h / 2 - 3, "      ##    ##      ");
                Draw_p(c_w / 2 - 10, c_h / 2 - 2, "                    ");
                Draw_p(c_w / 2 - 10, c_h / 2 - 1, "    ############    ");
                Draw_p(c_w / 2 - 10, c_h / 2, " ###            ### ");
                Draw_p(c_w / 2 - 10, c_h / 2 + 1, "#                  #");
            }
        }

        static void Del_bl(int N_x, int N_y)
        {
            blocks--;
            arr[N_y, N_x] = 0;
            Console.BackgroundColor = ConsoleColor.Gray;
            for (int i_2 = 0; i_2 < bl_h; i_2++)
            {
                Draw_p(N_x * (bl_w + 1) + 1, N_y * (bl_h + 1) + i_2, "           ");
            }
        }

    }
}
