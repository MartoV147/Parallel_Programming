using Area51ExamProject.Enums;
using System.Collections.Generic;
using System.Threading;

namespace Area51ExamProject
{
    class ElevatorSimulation
    {
        public static volatile bool isShiftOngoing = true;

        public int AgentsNumber { get; set; }

        public void RunSimulation()
        {
            var floors = new List<Floor>
            {
                new Floor("G", SecurityLevel.Confidential),
                new Floor("S", SecurityLevel.Secret),
                new Floor("T1", SecurityLevel.TopSecret),
                new Floor("T2", SecurityLevel.TopSecret)
            };

            var agents = new List<Agent>();
            var threads = new List<Thread>();

            var elevator = new Elevator(floors);

            var elTh = new Thread(elevator.ElevatorWorker);
            elTh.Start();

            for (int i = 0; i < AgentsNumber; i++)
            {
                var agent = new Agent("Agent " + i, elevator);
                var th = new Thread(agent.AgentWorker);
                threads.Add(th);
                th.Start();
            }

            foreach (var item in threads)
            {
                item.Join();
            }
            isShiftOngoing = false;

            elTh.Join();
            System.Console.WriteLine("Shift over");
        }
    }
}
