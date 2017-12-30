namespace Dec.Ls.EntityBase
{
    public class GlobalObject
    {
      
        public bool IsUnitTestMode { get; set; }
        public GlobalObject()
        {
            IsUnitTestMode = false;
        }
        public static readonly GlobalObject Glb = new GlobalObject();


    }
}
