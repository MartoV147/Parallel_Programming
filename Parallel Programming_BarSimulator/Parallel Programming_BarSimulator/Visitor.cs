using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Parallel_Programming_BarSimulator
{
    class Visitor
    {
        public enum NightlifeActivities { Walk, VisitBar, GoHome };
        public enum BarActivities { Drink, Dance, Leave };

        Random random = new Random();

        public string Name { get; set; }
        public int Age { get; set; }
        public double Budget { get; set; }
        public Bar Bar { get; set; }

        public Visitor(string name, int age, double budget, Bar bar)
        {
            Name = name;
            Age = age;
            Budget = budget;
            Bar = bar;
        }

        public NightlifeActivities GetRandomNightlifeActivity()
        {
            int n = random.Next(10);
            if (n < 3) return NightlifeActivities.Walk;
            if (n < 8) return NightlifeActivities.VisitBar;
            return NightlifeActivities.GoHome;
        }

        public BarActivities GetRandomBarActivity()
        {
            int n = random.Next(10);
            if (n < 4) return BarActivities.Dance;
            if (n < 9) return BarActivities.Drink;
            return BarActivities.Leave;
        }

        public void WalkOut()
        {
            Console.WriteLine($"{Name} is walking in the streets.");
            Thread.Sleep(100);
        }

        public void VisitBar()
        {
            Console.WriteLine($"{Name} is getting in the line to enter the bar.");
            if (random.Next(0, 10) < 5)
            {
                Console.WriteLine($"{Name} had enough and leaves the line");
                return;
            }
            if (Bar.Enter(this))
            {
                Console.WriteLine($"{Name} entered the bar!");
                bool staysAtBar = true;
                while (staysAtBar)
                {
                    var nextActivity = GetRandomBarActivity();

                    if (!Bar.isOpen)
                    {
                        nextActivity = BarActivities.Leave;
                    }

                    switch (nextActivity)
                    {
                        case BarActivities.Dance:
                            Console.WriteLine($"{Name} is dancing.");
                            Thread.Sleep(100);
                            break;
                        case BarActivities.Drink:
                            Bar.GetDrink(this, Bar.drinks[random.Next(Bar.drinks.Count)]);
                            Thread.Sleep(100);
                            break;
                        case BarActivities.Leave:
                            Console.WriteLine($"{Name} is leaving the bar.");
                            Bar.Leave(this);
                            staysAtBar = false;
                            break;
                    }
                }
            }
        }

        public void PaintTheTownRed()
        {
            WalkOut();
            bool staysOut = true;
            while (staysOut)
            {
                var nextActivity = GetRandomNightlifeActivity();
                switch (nextActivity)
                {
                    case NightlifeActivities.Walk:
                        WalkOut();
                        break;
                    case NightlifeActivities.VisitBar:
                        VisitBar();
                        staysOut = false;
                        break;
                    case NightlifeActivities.GoHome:
                        staysOut = false;
                        break;
                }
            }
            Console.WriteLine($"{Name} is going back home.");
        }
    }
}
