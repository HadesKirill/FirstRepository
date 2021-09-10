using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Media;
using static System.Console;

namespace KirillMeets
{
    class Meet
    {

        public string m_name;
        public string m_notification;
        public DateTime m_timeStart;
        public DateTime m_timeEnd;

        public Meet(string name, string notif, int syear, int smonth, int sday, int shour, int sminute, int eyear, int emonth, int eday, int ehour, int eminute)
        {
            m_name = name;
            m_notification = notif;
            m_timeStart = new DateTime(syear, smonth, sday, shour, sminute, 0);
            m_timeEnd = new DateTime(eyear, emonth, eday, ehour, eminute, 0);
        }

        public void ShowMeet()
        {
            Write($"Название встречи: {m_name}\n");
            Write($"Оповещение придет за {m_notification} мин.\n");
            Write($"Время начала встречи: {m_timeStart.Day}.{m_timeStart.Month}.{m_timeStart.Year} {m_timeStart.Hour}:{m_timeStart.Minute}\n");
            Write($"Время конца встречи: {m_timeEnd.Day}.{m_timeEnd.Month}.{m_timeEnd.Year} {m_timeEnd.Hour}:{m_timeEnd.Minute}\n");
        }

        public string getName()
        {
            return $"Название события: {m_name}";
        }

        public string getNotif()
        {
            return $"Оповещение придет за: {m_notification} мин.";
        }

        public string getDateStart()
        {
            return $"Старт: {m_timeStart.Day}.{m_timeStart.Month}.{m_timeStart.Year} {m_timeStart.Hour}:{m_timeStart.Minute}";
        }

        public string getDateEnd()
        {
            return $"Конец: {m_timeEnd.Day}.{m_timeEnd.Month}.{m_timeEnd.Year} {m_timeEnd.Hour}:{m_timeEnd.Minute}";
        }
    }

    class Meets
    {
        static List<Meet> LoadFromFile()
        {
            List<Meet> mts = new List<Meet>();

            string name = ""; string notif = "";
            int[] sarr = new int[5];
            int[] earr = new int[5];

            string tmp_date;
            int tmp_index;

            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Remove(path.Length - 10);
            StreamReader sr = new StreamReader($"{path}meet.txt");

            while(sr.Peek() != -1)
            {
                string line = sr.ReadLine();
                if (line.Length != 0 && line[0] == 'Н')
                {
                    name = "";                                       
                    for(int j = 0; j < line.Length; j++)
                    {
                        if(line[j] == ':')
                        {
                            for (int k = j + 2; k < line.Length; k++)
                            {
                                name += line[k];
                            }
                        }
                    }

                }

                else if (line.Length != 0 && line[0] == 'О')
                {
                    notif = "";
                    for (int j = 0; j < line.Length; j++)
                    {
                        if (line[j] == ':')
                        {
                            for (int k = j + 2; line[k] != 'м'; k++)
                            {
                                notif += line[k];
                            }
                        }
                    }

                }

                else if (line.Length != 0 && line[0] == 'С')
                {
                    tmp_date = "";
                    tmp_index = 0;
                    line += " ";

                    for (int j = 0; j < line.Length; j++)
                    {
                        if (line[j] == ':')
                        {
                            for (int k = j + 2; k < line.Length; k++)
                            {
                                if (line[k] != '.' && line[k] != ',' && line[k] != ':' && line[k] != ' ')
                                {
                                    tmp_date += line[k];
                                }

                                else if (line[k] == '.' || line[k] == ',' || line[k] == ' ' || line[k] == ':')
                                {
                                    sarr[tmp_index] = Convert.ToInt32(tmp_date);

                                    tmp_date = "";
                                    tmp_index++;
                                }
                            }
                            break;
                        }
                    }

                }

                else if (line.Length != 0 && line[0] == 'К')
                {
                    tmp_date = "";
                    tmp_index = 0;
                    line += " ";

                    for (int j = 0; j < line.Length; j++)
                    {
                        if (line[j] == ':')
                        {
                            for (int k = j + 2; k < line.Length; k++)
                            {
                                if (line[k] != '.' && line[k] != ',' && line[k] != ':' && line[k] != ' ')
                                {
                                    tmp_date += line[k];
                                }

                                else if (line[k] == '.' || line[k] == ',' || line[k] == ' ' || line[k] == ':')
                                {
                                    earr[tmp_index] = Convert.ToInt32(tmp_date);

                                    tmp_date = "";
                                    tmp_index++;
                                }
                            }

                            mts.Add(new Meet(name, notif, sarr[2], sarr[1], sarr[0], sarr[3], sarr[4], earr[2], earr[1], earr[0], earr[3], earr[4]));
                            break;
                        }
                    }

                }
               
            }

            sr.Close();
            return mts;
        }

        static Meet createMeet()
        {
            try
            {
                string tmp_name;
                string notif;
                string tmp_sdate;
                string tmp_edate;

                string path = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Remove(path.Length - 10);

                while (true)
                {
                    Write("Введите название встречи.\n");
                    tmp_name = ReadLine();

                    while (true)
                    {
                        WriteLine("Введите время (в минутах 0 - 120) до начала встречи, за которое вы бы хотели получить ововещение.");
                        notif = ReadLine();
                        if (Convert.ToInt32(notif) >= 0 && Convert.ToInt32(notif) <= 120)
                            break;
                        else
                            WriteLine("Повторите ввод (от 0 до 120 минут)");
                    }

                    WriteLine("Выберите способ ввода даты:\n1-долгий но упрощённый\n2-быстрый но требует четкого ввода по указанному шаблону");
                    switch(Convert.ToInt32(ReadLine()))
                    {
                        case 1:
                            Write("Начало встречи:\n");
                            WriteLine("Введите день:");
                            int sday = Convert.ToInt32(ReadLine());
                            WriteLine("Введите месяц:");
                            int smonth = Convert.ToInt32(ReadLine());
                            WriteLine("Введите год:");
                            int syear = Convert.ToInt32(ReadLine());
                            WriteLine("Введите часы:");
                            int shour = Convert.ToInt32(ReadLine());
                            WriteLine("Введите минуты:");
                            int sminute = Convert.ToInt32(ReadLine());
                            Write("Конец встречи:\n");
                            WriteLine("Введите день:");
                            int eday = Convert.ToInt32(ReadLine());
                            WriteLine("Введите месяц:");
                            int emonth = Convert.ToInt32(ReadLine());
                            WriteLine("Введите год:");
                            int eyear = Convert.ToInt32(ReadLine());
                            WriteLine("Введите часы:");
                            int ehour = Convert.ToInt32(ReadLine());
                            WriteLine("Введите минуты:");
                            int eminute = Convert.ToInt32(ReadLine());


                            if (!File.Exists($"{path}meet.txt"))
                            {
                                bool flag = false;
                                DateTime dt = new DateTime(syear, smonth, sday, shour, sminute, 0);

                                List<Meet> mts1 = LoadFromFile();
                                for (int i = 0; i < mts1.Count; i++)
                                {
                                    if (dt >= mts1[i].m_timeStart && dt <= mts1[i].m_timeEnd)
                                    {
                                        WriteLine("На это время у вас уже есть событие:");
                                        mts1[i].ShowMeet();
                                        WriteLine("Повторите ввод.");
                                        flag = true;
                                    }
                                }

                                if (flag == false)
                                {
                                    Meet mt = new Meet(tmp_name, notif, syear, smonth, sday, shour, sminute, eyear, emonth, eday, ehour, eminute);
                                    return mt;
                                }
                            }
                            else
                            {
                                Meet mt = new Meet(tmp_name, notif, syear, smonth, sday, shour, sminute, eyear, emonth, eday, ehour, eminute);
                                return mt;
                            }
                            break;
                        case 2:
                            string tmp_date = "";
                            int tmp_index = 0;

                            Write("Введите дату и время начала встречи в формате \" дд.мм.гггг чч:мм \"\n");
                            tmp_sdate = ReadLine();
                            tmp_sdate += " ";
                            int[] sarr = new int[5];
                            for (int i = 0; i < tmp_sdate.Length; i++)
                            {

                                if (tmp_sdate[i] != '.' && tmp_sdate[i] != ',' && tmp_sdate[i] != ':' && tmp_sdate[i] != ' ')
                                {
                                    tmp_date += tmp_sdate[i];
                                }

                                else if (tmp_sdate[i] == '.' || tmp_sdate[i] == ',' || tmp_sdate[i] == ' ' || tmp_sdate[i] == ':')
                                {
                                    sarr[tmp_index] = Convert.ToInt32(tmp_date);

                                    tmp_date = "";
                                    tmp_index++;
                                }

                            }

                            tmp_date = "";
                            Write("Введите примерные дату и время конца встречи в формате \" дд.мм.гггг чч:мм \"\n");
                            tmp_edate = ReadLine();
                            tmp_edate += " ";
                            int[] earr = new int[5];
                            tmp_index = 0;
                            for (int i = 0; i < tmp_edate.Length; i++)
                            {
                                if (tmp_edate[i] != '.' && tmp_edate[i] != ',' && tmp_edate[i] != ':' && tmp_edate[i] != ' ')
                                {
                                    tmp_date += tmp_edate[i];
                                }

                                else if (tmp_edate[i] == '.' || tmp_edate[i] == ',' || tmp_edate[i] == ' ' || tmp_edate[i] == ':')
                                {
                                    earr[tmp_index] = Convert.ToInt32(tmp_date);

                                    tmp_date = "";
                                    tmp_index++;
                                }
                            }

                            if (File.Exists($"{path}meet.txt"))
                            {

                                bool flag2 = false;
                                DateTime dt2 = new DateTime(sarr[2], sarr[1], sarr[0], sarr[3], sarr[4], 0);

                                List<Meet> mts2 = LoadFromFile();
                                for (int i = 0; i < mts2.Count; i++)
                                {
                                    if (dt2 >= mts2[i].m_timeStart && dt2 <= mts2[i].m_timeEnd)
                                    {
                                        WriteLine("На это время у вас уже есть событие:");
                                        mts2[i].ShowMeet();
                                        WriteLine("Повторите ввод.");
                                        flag2 = true;
                                    }
                                }

                                if (flag2 == false)
                                {
                                    Meet mt = new Meet(tmp_name, notif, sarr[2], sarr[1], sarr[0], sarr[3], sarr[4], earr[2], earr[1], earr[0], earr[3], earr[4]);
                                    return mt;
                                }
                            }
                            else
                            {
                                Meet mt = new Meet(tmp_name, notif, sarr[2], sarr[1], sarr[0], sarr[3], sarr[4], earr[2], earr[1], earr[0], earr[3], earr[4]);
                                return mt;
                            }
                            break;
                        default:
                            break;

                    }

                    
                }
            }
            catch
            {
                Write("\nВызвано исключение...\n");
                return null;
            }
        }

        static bool WriteMeets()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Remove(path.Length - 10);

            try
            {
                List<Meet> mts = new List<Meet>();
                int choise;
                while (true)
                {
                    WriteLine("\n1 - добавить еще одно событие\n0 - закончить добавление\n");
                    choise = Convert.ToInt32(ReadLine());

                    if (choise == 1)
                        mts.Add(createMeet());
                    else
                        break;
                }

                StreamWriter sw = File.AppendText($"{path}meet.txt");

                for (int i = 0; i < mts.Count; i++)
                {
                    sw.WriteLine("\n");
                    sw.WriteLine(mts[i].getName());
                    sw.WriteLine(mts[i].getNotif());
                    sw.WriteLine(mts[i].getDateStart());
                    sw.WriteLine(mts[i].getDateEnd());
                }
                sw.Close();
                return true;
            }
            catch
            {
                WriteLine("Окончание добавления | Вызвано исключение при создании событий...");
                return false;
            }
           
        }

        static void ShowDateMeets(int day, int month, int year)
        {
            List<Meet> mts = LoadFromFile();

            WriteLine($"События за {day}.{month}.{year}\n\n");
            for(int i = 0; i < mts.Count; i++)
            {
                if(mts[i].m_timeStart.Day == day && mts[i].m_timeStart.Month == month && mts[i].m_timeStart.Year == year)
                {
                    mts[i].ShowMeet();
                }
            }
        }

        static void ExportDateMeets(int day, int month, int year)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Remove(path.Length - 10);

            try
            {
                string file_name;
                List<Meet> mts = LoadFromFile();
                WriteLine("Придумайте название файла:");
                while (true)
                {
                    file_name = ReadLine();
                    if (File.Exists($"{path}{file_name}.txt") == true)
                    {
                        WriteLine("Файл с таким названием уже существует, придумайте другое название");
                    }
                    else
                        break;
                }

                StreamWriter sw = File.CreateText($"{path}{file_name}.txt");

                sw.WriteLine($"События за {day}.{month}.{year}\n");
                for (int i = 0; i < mts.Count; i++)
                {
                    if (mts[i].m_timeStart.Day == day && mts[i].m_timeStart.Month == month && mts[i].m_timeStart.Year == year)
                    {
                        sw.WriteLine("\n");
                        sw.WriteLine(mts[i].getName());
                        sw.WriteLine(mts[i].getNotif());
                        sw.WriteLine(mts[i].getDateStart());
                        sw.WriteLine(mts[i].getDateEnd());
                    }
                }
                WriteLine("\nЭкспорт событий за указанный период произведён успешно...\n");
                sw.Close();
            }

            catch
            {
                WriteLine("\nВызвано исключение...\n");
                return;
            }
        }

        static void ChangeMeet()
        {
            try
            {
                WriteLine("Редактирование события...");
                List<Meet> mts = LoadFromFile();
                WriteLine("Ваши события:\n");
                for (int i = 0; i < mts.Count; i++)
                {
                    WriteLine($"\nСобытие номер {i + 1}.");
                    mts[i].ShowMeet();
                }

                int choise;
                WriteLine("\nВведите номер события, которое вы бы хотели редактировать...");
                choise = Convert.ToInt32(ReadLine()) - 1;

                int wh_choise;
                while (true)
                {
                    WriteLine("\n1-изменить название\n2-изменить время оповещения\n3-изменить дату начала\n4-изменить дату окончания\n0-закончить добавление\n");
                    wh_choise = Convert.ToInt32(ReadLine());

                    if (wh_choise == 1)
                    {
                        WriteLine("Введите название:");
                        string name = ReadLine();
                        mts[choise].m_name = name;
                    }

                    else if (wh_choise == 2)
                    {
                        WriteLine("Введите время:");
                        string notif = ReadLine();
                        mts[choise].m_notification = notif;
                    }

                    else if (wh_choise == 3)
                    {
                        WriteLine("Введите дату в формате \" дд.мм.гггг чч:мм \":");
                        string sdate = ReadLine();
                        sdate += " ";
                        string tmp_date = "";
                        int tmp_index = 0;

                        int[] sarr = new int[5];
                        for (int i = 0; i < sdate.Length; i++)
                        {

                            if (sdate[i] != '.' && sdate[i] != ',' && sdate[i] != ':' && sdate[i] != ' ')
                            {
                                tmp_date += sdate[i];
                            }

                            else if (sdate[i] == '.' || sdate[i] == ',' || sdate[i] == ' ' || sdate[i] == ':')
                            {
                                sarr[tmp_index] = Convert.ToInt32(tmp_date);

                                tmp_date = "";
                                tmp_index++;
                            }

                        }

                        mts[choise].m_timeStart = new DateTime(sarr[2], sarr[1], sarr[0], sarr[3], sarr[4], 0);
                    }

                    else if (wh_choise == 4)
                    {

                        WriteLine("Введите дату в формате \" дд.мм.гггг чч:мм \":");
                        string edate = ReadLine();
                        edate += " ";
                        string tmp_date = "";
                        int tmp_index = 0;

                        int[] earr = new int[5];
                        for (int i = 0; i < edate.Length; i++)
                        {

                            if (edate[i] != '.' && edate[i] != ',' && edate[i] != ':' && edate[i] != ' ')
                            {
                                tmp_date += edate[i];
                            }

                            else if (edate[i] == '.' || edate[i] == ',' || edate[i] == ' ' || edate[i] == ':')
                            {
                                earr[tmp_index] = Convert.ToInt32(tmp_date);

                                tmp_date = "";
                                tmp_index++;
                            }

                        }

                        mts[choise].m_timeEnd = new DateTime(earr[2], earr[1], earr[0], earr[3], earr[4], 0);
                    }

                    else
                        break;

                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    path = path.Remove(path.Length - 10);

                    StreamWriter sw = File.CreateText($"{path}meet.txt");

                    if (mts[choise].m_timeStart >= mts[choise].m_timeEnd)
                    {
                        WriteLine("\nВремя начала встречи больше времени конца встречи.");
                        WriteLine("Время конца встречи автоматически устанавливается на 30 мин. больше чем время начала встречи.\nЕсли вас это не устраивает, отредактируйте через меню.");
                        mts[choise].m_timeEnd = mts[choise].m_timeStart;
                        mts[choise].m_timeEnd = mts[choise].m_timeEnd.AddMinutes(30);
                    }

                    for (int i = 0; i < mts.Count; i++)
                    {
                        sw.WriteLine("\n");
                        sw.WriteLine(mts[i].getName());
                        sw.WriteLine(mts[i].getNotif());
                        sw.WriteLine(mts[i].getDateStart());
                        sw.WriteLine(mts[i].getDateEnd());
                    }
                    WriteLine("\nУстановка правок произведена успешно...\n");
                    sw.Close();

                }
            }

            catch
            {
                WriteLine("\nВызвано исключение...\n");
                return;
            }
        }
        static void DeleteMeet()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Remove(path.Length - 10);

            try
            {
                WriteLine("Удаление события...");
                List<Meet> mts = LoadFromFile();
                WriteLine("Ваши события:\n");
                for (int i = 0; i < mts.Count; i++)
                {
                    WriteLine($"\nСобытие номер {i + 1}.");
                    mts[i].ShowMeet();
                }

                int choise;
                WriteLine("\nВведите номер события, которое вы бы хотели удалить...");
                choise = Convert.ToInt32(ReadLine()) - 1;

                mts.RemoveAt(choise);

                StreamWriter sw = File.CreateText($"{path}meet.txt");

                for (int i = 0; i < mts.Count; i++)
                {
                    sw.WriteLine("\n");
                    sw.WriteLine(mts[i].getName());
                    sw.WriteLine(mts[i].getNotif());
                    sw.WriteLine(mts[i].getDateStart());
                    sw.WriteLine(mts[i].getDateEnd());
                }

                WriteLine("\nУдаление произведено успешно...\n");
                sw.Close();
            }

            catch
            {
                WriteLine("\nВызвано исключение...\n");
                return;
            }

        }

        static void MeetConfig()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Remove(path.Length - 10);

            if (!File.Exists($"{path}meet.txt"))
            {                
                WriteLine("Вы еще не создали ни одного события.");
                WriteMeets();
            }

            while (true)
            {
                Write("\nВыберите действие:\n1-добавить событие\n2-изменить событие\n3-удалить событие\n4-посмотреть события\n5-экспортировать расписание встреч\n6-информация о программе\n0-выйти из программы\n");
                bool fl1 = false;
                int choise = 0;
                while (fl1 == false)
                {
                    try
                    {
                        choise = Convert.ToInt32(ReadLine());
                        fl1 = true;
                    }
                    catch
                    {
                        Write("Вызвано исключение, повторите ввод.\n");
                    }
                }

                if (choise == 0 || (choise != 1 && choise != 2 && choise != 3 && choise != 4 && choise != 5 && choise != 6))
                {
                    WriteLine("Выход из программы | Функции меню под таким номером не существует...");
                    break;
                }

                switch (choise)
                {
                    case 1:
                        WriteMeets();
                        break;
                    case 2:
                        ChangeMeet();
                        break;
                    case 3:
                        DeleteMeet();
                        break;
                    case 4:
                        bool fl2 = false;
                        int sw_choise = 0;
                        while (fl2 == false)
                        {
                            try
                            {                               
                                WriteLine("1-просмотреть все события\n2-просмотреть события за выбранный день");
                                sw_choise = Convert.ToInt32(ReadLine());
                                fl2 = true;
                            }
                            catch
                            {
                                Write("Вызвано исключение, повторите ввод.\n");
                            }
                        }

                        if(sw_choise == 1)
                        {
                            List<Meet> mts = LoadFromFile();
                            for (int i = 0; i < mts.Count; i++)
                            {
                                mts[i].ShowMeet();
                                WriteLine("");
                            }
                        }

                        else if (sw_choise == 2)
                        {
                            bool fl3 = false;
                            int day = 0; int month = 0; int year = 0;
                            while (fl3 == false)
                            {
                                try
                                {
                                    WriteLine("Введите день:");
                                    day = Convert.ToInt32(ReadLine());
                                    WriteLine("Введите месяц:");
                                    month = Convert.ToInt32(ReadLine());
                                    WriteLine("Введите год:");
                                    year = Convert.ToInt32(ReadLine());
                                    fl3 = true;
                                }
                                catch
                                {
                                    Write("Вызвано исключение, повторите ввод.\n");
                                }
                            }
                            ShowDateMeets(day, month, year);
                        }

                        break;
                    case 5:
                        bool fl4 = false;
                        int exp_day = 0; int exp_month = 0; int exp_year = 0;
                        while (fl4 == false)
                        {
                            try
                            {
                                WriteLine("Введите день:");
                                exp_day = Convert.ToInt32(ReadLine());
                                WriteLine("Введите месяц:");
                                exp_month = Convert.ToInt32(ReadLine());
                                WriteLine("Введите год:");
                                exp_year = Convert.ToInt32(ReadLine());
                                fl4 = true;
                            }
                            catch
                            {
                                Write("Вызвано исключение, повторите ввод.\n");
                            }
                        }
                        ExportDateMeets(exp_day, exp_month, exp_year);
                        break;
                    case 6:
                        WriteLine("\nИнформация.\nВыполнил: Ахмеров Кирилл\nЧтобы получать оповещения приложение должно быть включено (можете свернуть его).\nЕсли настанет время события вы услышите характерный звук." +
                            "\nКогда программа находилась у меня в родном каталоге всё работало отлично, перенося на другой диск, \nодин тест не получился: вылезла неопознанная ошибка, во второй же раз " +
                            "проделал кучу действий и ничего не крашнуло...");
                        break;
                    default:
                        break;
                }

            }

        }

        private static System.Timers.Timer aTimer;

        static void ThreadTimer()
        {
            aTimer = new System.Timers.Timer(10000);

            aTimer.Elapsed += CheckMeet;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        static void CheckMeet(Object source, ElapsedEventArgs e)
        {
            List<Meet> mts = LoadFromFile();
            for (int i = 0; i < mts.Count(); i++)
            {
                DateTime dt_tmp = mts[i].m_timeStart.AddMinutes(-(Convert.ToInt32(mts[i].m_notification)));
                if (dt_tmp.Date == DateTime.Now.Date && dt_tmp.Hour == DateTime.Now.Hour && dt_tmp.Minute == DateTime.Now.Minute)
                {
                    string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\Radar-1_01.wav";
                    SoundPlayer sp = new SoundPlayer(path);
                    sp.Play();
                    WriteLine("\nОповещение!\n" +
                        "Встреча:");
                    mts[i].ShowMeet();
                    WriteLine("\n\n");
                    break;
                }
            }
        }



        static void Main(string[] args)
        {
            WriteLine("\nДобро пожаловать в приложение по управлению личными встречами!\n");

            Thread th1 = new Thread(ThreadTimer);
            th1.Start();

            th1.Join();
            MeetConfig();

            Write("\n\n\n");
            ReadKey();
        }
    }
}
