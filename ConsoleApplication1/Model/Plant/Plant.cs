﻿using Emgu.CV;
using System.Collections.Generic;
using System.Drawing;

namespace ConsoleApplication1
{
    public class Plant
    {
        #region properties
        //General infos
        public int Id { get; set; }
        public Graphic Graphic { get; set; }

        //Models
        public int[] Model { get; set; }

        //Other info
        public TimeLine TimeLine { get; set; } = null;

        //Position and interaction with garden
        public Garden Garden { get; set; } = null;
        public Erosion Erosion { get; set; } = null;
        public Point? Position { get; set; } = null;
        public uint PositionOrder { get; set; } = 0;
        #endregion

        #region ctor
        public Plant()
        {

        }
        #endregion
    }
}
