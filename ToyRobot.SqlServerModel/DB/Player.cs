﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ToyRobot.SqlServerModel.DB
{
    public partial class Player
    {
        public Player()
        {
            Robot = new HashSet<Robot>();
        }

        public int PlayerId { get; set; }
        public Guid Identifier { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeletionDate { get; set; }

        public virtual ICollection<Robot> Robot { get; set; }
    }
}