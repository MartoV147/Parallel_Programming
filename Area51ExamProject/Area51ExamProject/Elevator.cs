using Area51ExamProject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Area51ExamProject
{
    public class Elevator
    {
        private List<Floor> floors;

        public Agent currentAgent;
        public FloorEnum currentFloor;
        public FloorEnum destFloor;

        private Semaphore semaphore;

        public bool isOpen = true;
        public bool isMoving = false;

        public bool canBeClicked = true;
        public bool stateChanged = false;

        object obj = new object();

        public Elevator(List<Floor> floors)
        {
            this.floors = floors;
            currentFloor = 0;
            semaphore = new Semaphore(1, 1);
        }

        public void ElevatorWorker()
        {
            while (ElevatorSimulation.isShiftOngoing)
            {
                if (isMoving & destFloor != currentFloor)
                {
                    stateChanged = false;
                    Thread.Sleep(1000);

                    currentFloor = destFloor;
                    Console.WriteLine("Elevator in on " + destFloor + " floor");

                    if (currentAgent != null)
                    {
                        if (SecurityCheck())
                        {
                            Console.WriteLine(currentAgent.Name + " isn't authorized to geto off on " + currentFloor);
                            ChangeFloor(currentAgent.SelectValidFloor());
                        }
                        else
                        {
                            if (currentAgent.destFloor == currentFloor)
                            {
                                currentAgent.ExitElevator(currentFloor);
                            }
                        }
                    }
                    else
                    {
                        UnlockElevator();
                    }
                }
                if (stateChanged)
                {
                    continue;
                }
                UnlockElevator();
            }
        }

        private bool SecurityCheck()
        {
            var securityLevel = SecurityLevel.TopSecret;
            if (currentAgent.SecurityLevel < securityLevel)
            {
                securityLevel = currentAgent.SecurityLevel;
            }

            return securityLevel < floors[(int)currentFloor].RequiredSecLevel;
        }

        private bool ChangeFloor(FloorEnum floor)
        {
            destFloor = floor;
            stateChanged = true;
            LockElevator();
            return true;
        }

        public bool CallElevator(FloorEnum floor)
        {
            if (canBeClicked)
            {
                destFloor = floor;
                stateChanged = true;
                LockElevator();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EnterElevator(Agent agent)
        {
            if (!semaphore.WaitOne(0))
            {
                return false;
            }

            lock (obj)
            {
                currentAgent = agent;
            }
            return true;
        }

        public bool ExitElevator(Agent agent)
        {
            semaphore.Release();
            lock (obj)
            {
                if (currentAgent == agent)
                {
                    currentAgent = null;
                }   
            }
            return true;
        }

        private void LockElevator()
        {
            isMoving = true;
            isOpen = false;
            canBeClicked = false;
        }

        private void UnlockElevator()
        {
            isMoving = false;
            isOpen = true;
            canBeClicked = true;
        }
    }
}
