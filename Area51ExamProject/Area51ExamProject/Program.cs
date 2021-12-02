using System;

namespace Area51ExamProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("How many agents would you like to sent to work: ");
            int agentsNumber = 0;

            while (agentsNumber <= 0)
            {
                try
                {
                    agentsNumber = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Input");
                }
            }

            ElevatorSimulation es = new ElevatorSimulation();
            es.AgentsNumber = agentsNumber;

            es.RunSimulation();
        }
    }
}
