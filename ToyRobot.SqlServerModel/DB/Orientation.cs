﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ToyRobot.SqlServerModel.DB
{
    public partial class Orientation
    {
        public Orientation()
        {
            Robot = new HashSet<Robot>();
        }

        public int OrientationId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Robot> Robot { get; set; }
    }
}