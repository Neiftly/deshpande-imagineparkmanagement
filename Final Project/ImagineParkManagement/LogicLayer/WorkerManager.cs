using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class WorkerManager : IWorkerManager
    {
        private IWorkerAccessor _workerAccessor;
        
        public WorkerManager()
        {
            _workerAccessor = new WorkerAccessor();
        }
        public WorkerManager(IWorkerAccessor workerAccessor)
        {
            _workerAccessor = workerAccessor;
        }
        public bool AddNewWorker(WorkerViewModel newWorker)
        {
            bool result = false;
            int newWorkerID = _workerAccessor.InsertNewWorker(newWorker);
            try
            {
                if (newWorkerID == 0)
                {
                    throw new ApplicationException("New Worker was not Added.");
                }

                foreach (var role in newWorker.Roles)
                {
                    _workerAccessor.InsertWorkerRole(newWorkerID, role);
                }
                result = true;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Adding New Worker Failed.", ex);
            }
            

            return result;
        }

        public bool DeactivateWorkerByID(int? workerID)
        {
            bool result = false;
            try
            {
                result = (1 == _workerAccessor.DeactivateWorker(workerID));
                if(result == false)
                {
                    throw new ApplicationException("Worker not Deactivated.");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Worker not Deactivated.", ex);
            }
            return result;
        }

        public bool EditWorkerProfile(WorkerViewModel oldWorker, WorkerViewModel newWorker, List<string> oldUnassignedRoles, List<string> newUnassignedRoles)
        {
            bool result = false;

            try
            {
                result = (1 == _workerAccessor.UpdateWorkerProfile(oldWorker, newWorker));
                if (result == false)
                {
                    throw new ApplicationException("Profile Data Not Changed.");
                }

                foreach (var role in newUnassignedRoles)
                {
                    if (!oldUnassignedRoles.Contains(role))
                    {
                        _workerAccessor.DeleteWorkerRole(oldWorker.WorkerID, role);
                    }
                }

                foreach (var role in newWorker.Roles)
                {
                    if (!oldWorker.Roles.Contains(role))
                    {
                        _workerAccessor.InsertWorkerRole(oldWorker.WorkerID, role);
                    }
                }
                if(oldWorker.Active != newWorker.Active)
                {
                    if(newWorker.Active == true)
                    {
                        _workerAccessor.ReactivateWorker(oldWorker.WorkerID);
                    }
                    else
                    {
                        _workerAccessor.DeactivateWorker(oldWorker.WorkerID);
                    }
                }
               
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Update Failed.", ex);
            }

            return result;
        }

        public bool ReactivateWorkerByID(int? workerID)
        {
            bool result = false;
            try
            {
                result = (1 == _workerAccessor.ReactivateWorker(workerID));
                if (result == false)
                {
                    throw new ApplicationException("Worker not Reactivated.");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Worker not Reactivated.", ex);
            }
            return result;
        }

        public WorkerViewModel RetreiveWorkerByID(int? workerID)
        {
            WorkerViewModel worker = null;

            try
            {
                worker = _workerAccessor.SelectWorkerByID(workerID);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return worker;
        }

        public List<string> RetrieveAllRoles()
        {
            List<string> roles = null;
            try
            {
                roles = _workerAccessor.SelectAllRoles();
                if (roles == null)
                {
                    roles = new List<string>();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found.", ex);
            }

            return roles;
        }

        public List<int> RetrieveAllWorkerIDByActive(bool active = true)
        {
            List<WorkerViewModel> workers = null;
            List<int> workerIDs = new List<int>();

            try
            {
                workers = _workerAccessor.SelectWorkersByActive(active);
                foreach (var worker in workers)
                {
                    var id = worker.WorkerID;
                    workerIDs.Add((int)id);
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found.", ex);
            }

            return workerIDs;
        }

        public List<int> RetrieveAvailabilityByID(int? workerID)
        {
            List<int> availability = null;

            try
            {
                availability = _workerAccessor.SelectAvailabilityByWorkerID(workerID);
                if (availability == null)
                {
                    availability = new List<int>();
                    availability.Add(3);
                    availability.Add(3);
                    availability.Add(3);
                    availability.Add(3);
                    availability.Add(3);
                    availability.Add(3);
                    availability.Add(3);
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found.", ex); ;
            }

            return availability;
        }

        public List<string> RetrieveProjectsByWorkerID(int? workerID)
        {
            throw new NotImplementedException();
        }

        public List<string> RetrieveRolesByWorkerID(int? workerID)
        {
            List<string> roles = null;
            try
            {
                roles = _workerAccessor.SelectRolesByWorkerID(workerID);
                if (roles == null)
                {
                    roles = new List<string>();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found.", ex);
            }
            return roles;
        }

        public List<WorkerViewModel> RetrieveWorkersByActive(bool active = true)
        {
            List<WorkerViewModel> workers = null;

            try
            {
                workers = _workerAccessor.SelectWorkersByActive(active);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found.", ex);
            }

            return workers;
        }

        public void SynchronizeRoles(int? workerId, List<string> roles)
        {
            var existingRoles = _workerAccessor.SelectRolesByWorkerID(workerId);
            var rolesToRemove = existingRoles.Except(roles);

            // remove removed roles
            foreach (var role in rolesToRemove)
            {
                _workerAccessor.DeleteWorkerRole(workerId, role);
            }

            var rolesToAdd = roles.Except(existingRoles);

            // assign assigned roles
            foreach (var role in rolesToAdd)
            {
                _workerAccessor.InsertWorkerRole(workerId, role);
            }
        }

        public bool UpdateWorkerAvailability(int? workerID, List<int> oldDays, List<int> newDays)
        {
            bool result = false;

            try
            {
                result = (1 == _workerAccessor.UpdateWorkerAvailability(workerID, oldDays, newDays));
                if (result == false)
                {
                    throw new ApplicationException("Availability Data Not Changed.");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Update Failed.", ex);
            }


            return result;
        }
    }
}
