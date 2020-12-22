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
        static int[,] arr;
        const int bl_h = 3, bl_w = 8;
        static int c_w, c_h, pl_x, pl_y, pl_w = 14, life = 3, blocks = 0, time_end = 0, N_bl_w, bonus_active = 0, bonus_time = -1, sh_pl_x;
        static bool sti = true, sti_bonus = false, rage = false;
        static ConsoleKey k;
        static int[] colors = { 11, 12, 13, 10, 14, 9 };
        static Timer time;
        static double angle, sh_x, sh_y, step_x, step_y, sh_speed = 1.5;
        static string bl;
        static Random rand = new Random();
        static List<int> bonus_x = new List<int> { }, bonus_y = new List<int> { }, bonus_type = new List<int> { };
        static void Game(object obj)
        {
            if (life > 0)
            {
                if (blocks != 0)
                {
                    if (k != ConsoleKey.Escape)
                    {
                        //Движение платформы
                        if (k == ConsoleKey.Spacebar)
                            sti = false;
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        if (k == ConsoleKey.LeftArrow || k == ConsoleKey.A)
                        {
                            if (pl_x > 2)
                                pl_x -= 3;
                            if (sti)
                            {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Draw_p((int)sh_x, (int)sh_y, "   ");
                                Draw_p((int)sh_x, (int)sh_y + 1, "   ");
                                sh_x = pl_x + sh_pl_x;
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                            }
                            k = 0;
                            Draw_p(pl_x, pl_y, "   ");
                        }
                        if (k == ConsoleKey.RightArrow || k == ConsoleKey.D)
                        {
                            if (pl_x < c_w - pl_w - 2)
                                pl_x += 3;
                            if (sti)
                            {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Draw_p((int)sh_x, (int)sh_y, "   ");
                                Draw_p((int)sh_x, (int)sh_y + 1, "   ");
                                sh_x = pl_x + sh_pl_x;
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
                            Console.BackgroundColor = ConsoleColor.Gray;
                            if (life > 0)
                            {
                                Draw_p(life * 4 + 1, (int)pl_y + 6, "   ");
                                Draw_p(life * 4 + 1, (int)pl_y + 7, "   ");
                            }
                            else
                            {
                                angle = Math.PI / 6;
                                step_x = Math.Cos(angle) * 2;
                                step_y = -Math.Sin(angle) * 2;
                                Console.Clear();
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                                Draw_p(c_w / 2 - 10, c_h / 2 - 4, "      ##    ##      ");
                                Draw_p(c_w / 2 - 10, c_h / 2 - 3, "      ##    ##      ");
                                Draw_p(c_w / 2 - 10, c_h / 2 - 2, "                    ");
                                Draw_p(c_w / 2 - 10, c_h / 2 - 1, "    ############    ");
                                Draw_p(c_w / 2 - 10, c_h / 2,     " ###            ### ");
                                Draw_p(c_w / 2 - 10, c_h / 2 + 1, "#                  #");
                            }
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            for (int i = 0; i < pl_w; i++)
                                Draw_p(pl_x + i, pl_y, " ");
                            sh_y = c_h - 11;
                            sh_x = pl_x + pl_w / 2 - 1;
                            k = 0;
                            sti = true;
                            Bonus_cansel();
                            bonus_active = 0;
                            bonus_time = -1;
                            for (int i = 0; i < bonus_type.Count; i++)
                            {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Draw_p((int)bonus_x[i], (int)bonus_y[i] - 1, "   ");
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Draw_p((int)bonus_x[i], (int)bonus_y[i], "   ");
                                bonus_x.RemoveAt(i);
                                bonus_y.RemoveAt(i);
                                bonus_type.RemoveAt(i);
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                                for (int j = 0; j < pl_w; j++)
                                    Draw_p(pl_x + j, pl_y, " ");
                            }
                            bonus_x.Clear();
                            bonus_y.Clear();
                            bonus_type.Clear();
                        }
                        if ((int)Math.Abs(pl_y - (int)sh_y - 1.5) <= 0.5 && sh_x >= pl_x - 2 && sh_x <= pl_x + pl_w)
                        {
                            step_y = -step_y;
                            sh_y = pl_y - 2;
                            if (Math.Min(Math.Abs(pl_x + pl_w / 2 - sh_x - 2), Math.Abs(pl_x + pl_w / 2 - sh_x)) == Math.Abs(pl_x + pl_w / 2 - sh_x - 2))
                                angle = 2 * Math.PI / 3 - Math.PI / 4 / (pl_w / 2) * (sh_x - pl_x - pl_w / 2 + 2);
                            else
                                angle = Math.PI / 3 - Math.PI / 4 / (pl_w / 2) * (sh_x - pl_x - pl_w / 2);
                            step_x = Math.Cos(angle) * sh_speed;
                            step_y = -Math.Sin(angle) * sh_speed;
                            if (sti_bonus)
                            {
                                sh_pl_x = (int)sh_x - pl_x;
                                sti = true;
                            }
                        }
                        else
                        {
                            BUHHH();
                        }
                        for(int i = 0; i < bonus_type.Count; i++)
                        {
                            bonus_y[i]++;
                            if ((int)Math.Abs(pl_y - (int)bonus_y[i] - 0.5) <= 0.5 && bonus_x[i] >= pl_x - 2 && bonus_x[i] <= pl_x + pl_w)
                            {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Draw_p((int)bonus_x[i], (int)bonus_y[i] - 1, "   ");
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Draw_p((int)bonus_x[i], (int)bonus_y[i], "   ");
                                Bonus_cansel();
                                bonus_active = bonus_type[i];
                                bonus_time = 250;
                                if (bonus_type[i] == 1)
                                {
                                    sh_speed = 2;
                                    step_x = step_x * 4 / 3;
                                    step_y = step_y * 4 / 3;
                                }
                                else if (bonus_type[i] == 2)
                                {
                                    Console.BackgroundColor = ConsoleColor.Gray;
                                    for (int j = 0; j < pl_w; j++)
                                        Draw_p(pl_x + j, pl_y, " ");
                                    pl_x -= 3;
                                    pl_w = 8;
                                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                                    for (int j = 0; j < pl_w; j++)
                                        Draw_p(pl_x + j, pl_y, " ");
                                }
                                else if (bonus_type[i] == 3)
                                {
                                    sh_speed = 1;
                                    step_x = step_x * 2 / 3;
                                    step_y = step_y * 2 / 3;
                                }
                                else if (bonus_type[i] == 4)
                                {
                                    Console.BackgroundColor = ConsoleColor.Gray;
                                    for (int j = 0; j < pl_w; j++)
                                        Draw_p(pl_x + j, pl_y, " ");
                                    pl_x -= 3;
                                    pl_w = 20;
                                    if (pl_x < 1)
                                        pl_x = 1;
                                    else if (pl_x > c_w - pl_w - 1)
                                        pl_x = c_w - pl_w - 1;
                                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                                    for (int j = 0; j < pl_w; j++)
                                        Draw_p(pl_x + j, pl_y, " ");
                                }
                                else if (bonus_type[i] == 5)
                                    sti_bonus = true;
                                else
                                    rage = true;
                                bonus_x.RemoveAt(i);
                                bonus_y.RemoveAt(i);
                                bonus_type.RemoveAt(i);
                            }
                            else if (bonus_y[i] > pl_y + 2)
                            {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Draw_p((int)bonus_x[i], (int)bonus_y[i] - 1, "   ");
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Draw_p((int)bonus_x[i], (int)bonus_y[i], "   ");
                                bonus_x.RemoveAt(i);
                                bonus_y.RemoveAt(i);
                                bonus_type.RemoveAt(i);
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                                for (int j = 0; j < pl_w; j++)
                                    Draw_p(pl_x + j, pl_y, " ");
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Draw_p((int)bonus_x[i], (int)bonus_y[i] - 1, "   ");
                                if (bonus_type[i] == 1 || bonus_type[i] == 2)
                                    Console.BackgroundColor = ConsoleColor.Red;
                                else
                                    Console.BackgroundColor = ConsoleColor.Green;
                                Draw_p((int)bonus_x[i], (int)bonus_y[i], "   ");
                                if (bonus_type[i] == 1 || bonus_type[i] == 3)
                                    Console.BackgroundColor = ConsoleColor.Yellow;
                                else if (bonus_type[i] == 2 || bonus_type[i] == 4)
                                    Console.BackgroundColor = ConsoleColor.Blue;
                                else if (bonus_type[i] == 5)
                                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                                else
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                Draw_p((int)bonus_x[i], (int)bonus_y[i] + 1, "   ");
                            }
                        }
                        bonus_time--;
                        if (bonus_time == 0)
                        {
                            Bonus_cansel();
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
                        if (rage)
                            Console.BackgroundColor = ConsoleColor.Red;
                        Draw_p((int)sh_x, (int)sh_y, "   ");
                        Draw_p((int)sh_x, (int)sh_y + 1, "   ");
                    }
                }
                else
                {
                    if (Console.Title == "Арканоид (раунд 3)" || Console.Title == "Арканоид (Конец)")
                    {
                        Console.Title = "Арканоид (Конец)";
                        time_end++;
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        //Знаю, выглядит жутко :)
                        if (time_end/2 % 10 == 9)
                        {
                            Draw_p(c_w / 2 - 10, c_h / 2 - 4, "      ##    ##      ");
                            Draw_p(c_w / 2 - 10, c_h / 2 - 3, "      ##    ##      ");
                            Draw_p(c_w / 2 - 10, c_h / 2 - 2, "                    ");
                            Draw_p(c_w / 2 - 10, c_h / 2 - 1, "#                  #");
                            Draw_p(c_w / 2 - 10, c_h / 2,     "####            ####");
                            Draw_p(c_w / 2 - 10, c_h / 2 + 1, "#   ############   #");
                            Draw_p(c_w / 2 - 10, c_h / 2 + 2, " ##  ##      ##  ## ");
                            Draw_p(c_w / 2 - 10, c_h / 2 + 3, "   ##############   ");
                        }
                        else
                        {
                            Draw_p(c_w / 2 - 10, c_h / 2 - 4, "      ##    ##      ");
                            Draw_p(c_w / 2 - 10, c_h / 2 - 3, "      ##    ##      ");
                            Draw_p(c_w / 2 - 10, c_h / 2 - 2, "                    ");
                            Draw_p(c_w / 2 - 10, c_h / 2 - 1, "#                  #");
                            Draw_p(c_w / 2 - 10, c_h / 2,     " ###            ### ");
                            Draw_p(c_w / 2 - 10, c_h / 2 + 1, "    ############    ");
                            Draw_p(c_w / 2 - 10, c_h / 2 + 2, "                    ");
                            Draw_p(c_w / 2 - 10, c_h / 2 + 3, "                    ");
                        }
                        //Вот мелодия, что тут должна звучать (через бит не решился делать)): https://www.youtube.com/watch?v=eUcThHVbrXY&list=PLpJl5XaLHtLX-pDk4kctGxtF4nq6BIyjg&index=79
                        if (k == ConsoleKey.Enter)
                        {
                            Round_one();
                            Go();
                        }
                    }

                    if (Console.Title == "Арканоид (раунд 2)")
                    {
                        Console.Title = "Арканоид (раунд 3)";
                        int[,] arr_r_3 = {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };
                        arr = arr_r_3;
                        Draw_bl();
                        Go();
                    }

                    if (Console.Title == "Арканоид (раунд 1)")
                    {
                        Console.Title = "Арканоид (раунд 2)";
                        int[,] arr_r_2 = {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };
                        arr = arr_r_2;
                        Draw_bl();
                        Go();
                    }
                    if (Console.Title == "Арканоид")
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
            bonus_x.Clear();
            bonus_y.Clear();
            bonus_type.Clear();
            bonus_active = 0;
            bonus_time = 0;
            sti = true;
            sh_speed = 1.5;
            pl_w = 14;
            time_end = 0;
            life = 3;
            sh_x = c_w / 2 - 2;
            sh_y = c_h - 11;
            pl_x = c_w / 2 - pl_w / 2 - (c_w / 2 - pl_w / 2) % 3 / 2;
            pl_y = c_h - 9;
            sh_pl_x = (int)sh_x - pl_x;
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
            int N_x = -1, N_y = -1, X_or_Y = -1;
            N_x = (int)sh_x / (bl_w + 1);
            N_y = (int)sh_y / (bl_h + 1);
            if (Math.Abs((int)sh_x % (bl_w + 1) - bl_w + 0.5) == 0.5 || (int)sh_x % (bl_w + 1) == 0)
            {
                X_or_Y = 0;
            }
            if (Math.Abs((int)sh_y % (bl_h + 1) - bl_h + 1.5) == 0.5 && step_y < 0 || Math.Abs((int)sh_y % (bl_h + 1) - (double)bl_h / 2) == (double)bl_h / 2 && step_y > 0)
            {
                X_or_Y = 1;
            }
            if ((int)sh_y % (bl_h + 1) == bl_h)
            {
                N_y++;
            }
            if ((int)sh_x % (bl_w + 1) == bl_w)
            {
                if (N_y > -1 && N_y < 7 && N_x < N_bl_w - 1)
                    if (arr[N_y, N_x + 1] == 1 && arr[N_y, N_x] == 0 && step_x > 0)
                    {
                        X_or_Y = 0;
                        if (N_x < N_bl_w - 1)
                            N_x++;
                    }
                    else if (arr[N_y, N_x + 1] == 0 && arr[N_y, N_x] == 1 && step_x < 0) 
                    {
                        X_or_Y = 0;
                    }
                    else
                    {
                        if ((int)sh_x % (bl_w + 1) == bl_w && N_y < 7)
                            if (arr[N_y, N_x] == 0)
                                N_x++;
                    }
            }

            if (N_y > -1 && N_y < 7 && N_x > -1 && N_x < N_bl_w)
            {
                if (arr[N_y, N_x] == 1)
                {
                    if (!rage)
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
                    }
                    blocks--;
                    arr[N_y, N_x] = 0;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    for (int i_2 = 0; i_2 < bl_h; i_2++)
                    {
                        Draw_p(N_x * (bl_w + 1) + 1, N_y * (bl_h + 1) + i_2, bl);
                    }
                    bool road_empty = true;
                    for (int i = N_y; i < 7; i++)
                    {
                        if (arr[i, N_x] == 1)
                        {
                            road_empty = false;
                        }
                    }
                    if(rand.Next(1,4) == 1 && road_empty)
                    {
                        bonus_x.Add(N_x * (bl_w + 1) + bl_w/2);
                        bonus_y.Add(N_y * (bl_h + 1));
                        bonus_type.Add(rand.Next(1, 7));
                    }

                }
            }
        }

        static void Main(string[] args)
        {

            try
            {
                Console.SetWindowSize(91, 80);
                Console.SetBufferSize(91, 80);
                c_h = 80;
                c_w = 91;
            }
            catch
            {
                Console.SetWindowSize(50, 50);
                Console.SetBufferSize(50, 50);
                c_h = 50;
                c_w = 54;
            }
            N_bl_w = (c_w - 1) / (bl_w + 1);
            Console.Title = "Арканоид";
            Console.WriteLine("Enter - начать\nSpaceBar - запустить мяч\nСтрелочки / A, D - движение платформы\nBackspace - выйти\nПриветсвую, я создатель игры, которую вы каким-то образом смогли у себя запустить, Чеботок Никита\nПоскольку я сильно сомневаюсь, что вы оценете игру позитивно, я решил написать краткое объяснение (оправдание) почему она так работает, заодно и интсрукцию накалякаю\nУ моего Арканоида есть две главные проблемы: \n1) Запуск на других компьютерах. Это связано с отличием в максимальной высоте окна консоли, но я это постарался исправить и вроде вышло. Однако, вместе с этим, я, считайте, создал хард уровень с уменьшенным расстоянием от платформы до блоков для некоторых компов :)\n2) ОЧЕНЬ МНОГО ГЛЮКОВ!!! У меня всё работает исправно, однако у абсолютно всех, кто загружал мою програму себе на компьютер возникали дикие глюки. Скажу так: у меня тоже они были поначалу. Уже не помню как от них избавился, но сейчас вроде норм, попробуйте полазить в диспетчере задач, вроде там можно что-то сделать. Понятия не имею почему они появляются, но во всём виноват дурацкий таймер, явно он шалит, проблема не в програме, точнее..., не совсем в ней)\nЛадно, если не смотря на всё это вы хотите сыграть, я вас не держу, но я вас предупредил. Удачи!\nP. S. Уменьшение окна консоли ничего не меняет, знаю");
            Console.ReadKey();
            Console.CursorVisible = false;
            time = new Timer(Game, null, 0, 40);
            do
            {
                k = Console.ReadKey(true).Key;
            } while (k != ConsoleKey.Backspace);
        }
        static void Round_one()
        {
            Console.Title = "Арканоид (раунд 1)";
            int[,] arr_r_1 = {
                { 1, 1, 0, 0, 1, 1, 0, 0, 1, 1},
                { 0, 0, 1, 1, 0, 0, 1, 1, 0, 0},
                { 1, 1, 0, 0, 1, 1, 0, 0, 1, 1},
                { 0, 0, 1, 1, 0, 0, 1, 1, 0, 0},
                { 1, 1, 0, 0, 1, 1, 0, 0, 1, 1},
                { 0, 0, 1, 1, 0, 0, 1, 1, 0, 0},
                { 1, 1, 0, 0, 1, 1, 0, 0, 1, 1}

            };
            arr = arr_r_1;
            Draw_bl();
        }

        static void Draw_bl()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Clear();
            blocks = 0;
            bl = "";
            for (int i = 0; i < bl_w; i++)
                bl += " ";
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < N_bl_w; j++)
                {
                    if (arr[i, j] == 1)
                    {
                        blocks++;
                        Console.BackgroundColor = (ConsoleColor)colors[rand.Next(0, 6)];
                        for (int i_2 = 0; i_2 < bl_h; i_2++)
                        {
                            Draw_p(j * (bl_w + 1) + 1, i * (bl_h + 1) + i_2, bl);
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
        static void Bonus_cansel()
        {
            if (bonus_active == 1 || bonus_active == 3)
            {
                sh_speed = 1.5;
                step_x = step_x / 2 * 3;
                step_y = step_y / 2 * 3;
            }
            else if (bonus_active == 2)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                for (int j = 0; j < pl_w; j++)
                    Draw_p(pl_x + j, pl_y, " ");
                pl_x -= 3;
                pl_w = 14;
                if (pl_x < 1)
                    pl_x = 1;
                else if (pl_x > c_w - pl_w - 1)
                    pl_x = c_w - pl_w - 1;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                for (int j = 0; j < pl_w; j++)
                    Draw_p(pl_x + j, pl_y, " ");
            }
            else if (bonus_active == 3)
            {
                sh_speed = 1.5;
                step_x = step_x / 4 * 3;
                step_y = step_y / 4 * 3;
            }
            else if (bonus_active == 4)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                for (int j = 0; j < pl_w; j++)
                    Draw_p(pl_x + j, pl_y, " ");
                pl_x += 3;
                pl_w = 14;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                for (int j = 0; j < pl_w; j++)
                    Draw_p(pl_x + j, pl_y, " ");
            }
            else if (bonus_active == 5)
                sti_bonus = false;
            else
                rage = false;

        }
    }
}
