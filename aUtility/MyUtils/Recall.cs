namespace aUtility
{

    using Aimtec;

    class Recall
    {
        public float StartTime;
        public float EndTime;
        public Obj_AI_Hero Caster;

        public Recall(float startTime, float endTime, Obj_AI_Hero caster)
        {
            StartTime = startTime;
            EndTime = endTime;
            Caster = caster;
        }
    }
}