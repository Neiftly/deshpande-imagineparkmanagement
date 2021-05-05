using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IWorkerManager
    {
        void SynchronizeRoles(int? workerId, List<string> roles);
        List<WorkerViewModel> RetrieveWorkersByActive(bool active = true);
        List<string> RetrieveRolesByWorkerID(int? workerID);
        List<string> RetrieveProjectsByWorkerID(int? workerID);
        List<string> RetrieveAllRoles();
        bool UpdateWorkerAvailability(int? workerID, List<int> oldDays, List<int> newDays);
        List<int> RetrieveAvailabilityByID(int? workerID);
        bool EditWorkerProfile(WorkerViewModel oldWorker, WorkerViewModel newWorker, List<string> oldUnassignedRoles, List<string> newUnassignedRoles);
        bool AddNewWorker(WorkerViewModel newWorker);
        WorkerViewModel RetreiveWorkerByID(int? workerID);
        List<int> RetrieveAllWorkerIDByActive(bool active = true);
        bool DeactivateWorkerByID(int? workerID);
        bool ReactivateWorkerByID(int? workerID);
    }
}
