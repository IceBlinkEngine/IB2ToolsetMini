﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace IB2ToolsetMini
{
    public class Schedule
    {
        private string name = "";
        private List<WayPoint> _WayPointList = new List<WayPoint>();

        [CategoryAttribute("05 - Project Living World"), DescriptionAttribute("Name of schedule.")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [CategoryAttribute("05 - Project Living World"), DescriptionAttribute("List of waypoints for patrolling type movements.")]
        public List<WayPoint> WayPointList
        {
            get { return _WayPointList; }
            set { _WayPointList = value; }
        }

        public Schedule()
        {
        }
        public Schedule DeepCopy()
        {
            Schedule other = (Schedule)this.MemberwiseClone();
            other.WayPointList = new List<WayPoint>();
            foreach (WayPoint bs in this.WayPointList)
            {
                other.WayPointList.Add(bs.DeepCopy());
            }
            return other;
        }
    }
}
