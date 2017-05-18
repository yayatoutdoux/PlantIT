using Emgu.CV;
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

        public List<Interaction> Interactions { get; set; }
        #endregion

        #region ctor
        public Plant()
        {

        }
        #endregion
    }
}
