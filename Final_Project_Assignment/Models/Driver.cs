﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Final_Project_Assignment.Models
{
    public partial class Driver
    {
        public Driver()
        {
            DriverInfractions = new HashSet<DriverInfraction>();
            Vehicles = new HashSet<Vehicle>();
        }

        public int DriverId { get; set; }
        public string DriverFirstName { get; set; }
        public string DriverLastName { get; set; }
        public int? DriverSocialSecurityNo { get; set; }
        public string DriverLicensePlateNo { get; set; }
        public int? DmvId { get; set; }

        public virtual Dmv Dmv { get; set; }
        public virtual ICollection<DriverInfraction> DriverInfractions { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}