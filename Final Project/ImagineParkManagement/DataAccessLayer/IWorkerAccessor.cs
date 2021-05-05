using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IWorkerAccessor
    {
        WorkerViewModel SelectWorkerByID(int? workerID);
        List<WorkerViewModel> SelectWorkersByActive(bool Active = true);
        List<string> SelectRolesByWorkerID(int? workerID);
        List<string> SelectAllRoles();
        List<int> SelectAvailabilityByWorkerID(int? workerID);
        List<ProjectViewModel> SelectProjectsByWorkerID(int? workerID);
        int UpdateWorkerProfile(Worker oldWorker, Worker newWorker);
        int DeactivateWorker(int? workerID);
        int ReactivateWorker(int? workerID);
        int DeleteWorkerRole(int? workerID, string roleID);
        int InsertWorkerRole(int? workerID, string roleID);
        int InsertNewWorker(Worker worker);
        int UpdateWorkerAvailability(int? workerID, List<int> oldDays, List<int> newDays);
    }
}
