namespace aUtility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Aimtec;
    using System.Drawing;

    class ward
    {
        public readonly float EndTime;
        public readonly Vector3 Position;
        public readonly Color Color;

        public ward(float endTime, Vector3 position, Color color)
        {
            EndTime = endTime;
            Position = position;
            Color = color;
        }
    }
}