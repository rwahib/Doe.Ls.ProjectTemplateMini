

namespace Doe.Ls.EntityBase.Models
{
    public class Result : ResultBase
    {
        public int  TotalCount { get; set; }
        public object Data{ get; set; }
    public  virtual  string HeaderText { get; set; }
        public override string ToString()
        {
            return $"{Status}-{Message}";
        }
    }
}
