using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Repositories;
using ZdravoCorp.Core.Scheduling.Repositories.Interfaces;

namespace ZdravoCorp.Core.Scheduling.Services
{
    public class DoctorAvailabilityByAppointment : DoctorAvailability
    {
        private ScheduleService scheduleService = new ScheduleService();

        public bool IsAvailable(TimeSlot timeSlot, int doctorId, int appointmentId=-1)
        {
            foreach (Appointment appointment in scheduleService.GetAppointmentsForDoctor(doctorId))
            {
                if (appointment.Id == appointmentId || appointment.IsCanceled) continue;
                if (appointment.TimeSlot.OverlapWith(timeSlot))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
