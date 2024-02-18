using static Gurme2.Form1;

namespace Gurme2
{
    public class ReturnModel<T>
    {
        public T Data { get; set; }
        
        public string Message { get; set; }

        public ReturnTypeStatus Status { get; set; }
    }
}