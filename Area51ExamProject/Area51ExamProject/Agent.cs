using Area51ExamProject.Enums;
using System;
using System.Threading;

namespace Area51ExamProject
{
    public class Agent
    {
        public string Name { get; set; }
        public SecurityLevel SecurityLevel { get; set; }

        public Elevator elevator;
        public FloorEnum currentFloor;
        public FloorEnum destFloor;

        private static Random rand = new Random();

        private bool isInElevator = false;
        private bool exitElevator = false;
        private bool waitForElevator = false;

        public Agent(string name, Elevator elevator)
        {
            Name = name;
            SecurityLevel = (SecurityLevel)(rand.Next(0, 2));
            this.elevator = elevator;
        }

        public void AgentWorker()
        {
            Activity activity = GetRandomActivity();
            while (true)
            {
                if (isInElevator && exitElevator)
                {
                    elevator.ExitElevator(this);

                    exitElevator = false;
                    isInElevator = false;

                    Console.WriteLine(Name + " exited the elevator on " + currentFloor + " floor");
                }
                else
                {
                    if (!waitForElevator)
                    {
                        activity = GetRandomActivity();
                    }

                    if (activity == Activity.Work)
                    {
                        Console.WriteLine(Name + " is working");
                        Thread.Sleep(3000);
                    }
                    else if (activity == Activity.GoToDifferentFloor)
                    {
                        if (elevator.currentFloor == currentFloor & elevator.isOpen)
                        {
                            if (elevator.EnterElevator(this))
                            {
                                isInElevator = true;

                                destFloor = GetRandomFloor();
                                elevator.CallElevator(destFloor);

                                Console.WriteLine(Name + " is in the elevator and wants to go to " + destFloor);

                            }
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            if (elevator.CallElevator(currentFloor))
                            {
                                Console.WriteLine("Elevator is coming");
                                waitForElevator = true;
                            }
                            else
                            {
                                Console.WriteLine("Elevator is locked");
                                waitForElevator = false;
                            }
                            Thread.Sleep(1000);
                        }
                    }
                    else 
                    {
                        Console.WriteLine(Name + " decided to go home");
                        break;
                    }
                }
            }
        }

        private Activity GetRandomActivity()
        {
            return (Activity)rand.Next(0, 3);
        }

        private FloorEnum GetRandomFloor()
        {
            FloorEnum floor;
            do
            {
                floor = (FloorEnum)rand.Next(0, 4);
            }
            while (floor == currentFloor);

            return floor;
        }

        public FloorEnum SelectValidFloor()
        {
            FloorEnum floor;
            do
            {
                floor = (FloorEnum)rand.Next(0, 4);
            }
            while ((int)floor > (int)SecurityLevel);
            destFloor = floor;

            Console.WriteLine(Name + " wants to go to " + destFloor + " floor");
            return floor;
        }

        public void ExitElevator(FloorEnum floor)
        {
            currentFloor = floor;
            exitElevator = true;
        }
    }
}