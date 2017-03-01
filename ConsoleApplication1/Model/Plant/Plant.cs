using Emgu.CV;
using System.Collections.Generic;
using System.Drawing;

namespace ConsoleApplication1
{
    public class Plant
    {
        //General infos
        public long Id { get; set; }
        public Graphic Graphic { get; set; }

        //Models
        public List<KeyValuePair<int, Mat>> Model { get; set; }
        public PlantInteractionModel Interaction { get; set; }

        //Other info
        public TimeLine TimeLine { get; set; } = null;

        //Position and interaction with garden
        public Garden Garden { get; set; } = null;
        public Erosion Erosion { get; set; } = null;
        public Point? Position { get; set; } = null;

        public Plant(int id, List<KeyValuePair<int, Mat>> model, PlantInteractionModel interaction)
        {
            Id = id;
            Model = model;
            Interaction = interaction;
        }
    }

    public class PlantInteractionModel
    {
        public List<KeyValuePair<int, Mat>> Model { get; set; }
    }
}
