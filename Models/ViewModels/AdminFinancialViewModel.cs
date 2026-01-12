using System;
using System.Collections.Generic;

namespace MHC_AFMS.Models.ViewModels
{
    public class AdminFinancialViewModel
    {
        // 1. Overall Revenue
        public decimal RevenueToday { get; set; }
        public decimal RevenueThisWeek { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public decimal RevenueTotal { get; set; }

        // 2. Revenue Breakdown
        public decimal TotalConsultationFees { get; set; }
        public decimal TotalTestFees { get; set; }

        // 3. Revenue by Doctor
        public List<DoctorRevenueStats> DoctorStats { get; set; }

        // 5. Revenue by Test Type
        public List<TestRevenueStats> TestStats { get; set; }

        // 6. Payment Status
        public decimal PendingPayments { get; set; }
        public decimal PaidPayments { get; set; }

        // 9. Daily Summary
        public int AppointmentsToday { get; set; }
        public int TestsToday { get; set; }

        // Filter Inputs
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class DoctorRevenueStats
    {
        public string DoctorName { get; set; }
        public int AppointmentCount { get; set; }
        public decimal TotalEarnings { get; set; }
    }

    public class TestRevenueStats
    {
        public string TestName { get; set; }
        public int Count { get; set; }
        public decimal TotalIncome { get; set; }
    }
}