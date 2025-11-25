namespace PipeProject.Models
{
    public class PipeType
    {
        public PipeTypeEnum Type { get; set; }
        public string Name => Type.ToString();
        public bool RequiresYoungsModulus => Type == PipeTypeEnum.DINStandard;
    }
}