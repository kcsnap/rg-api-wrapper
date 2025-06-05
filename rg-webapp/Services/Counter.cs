namespace RgWebapp.Services
{
    public class CounterLogic
    {
        public int currentCount { get; private set; } = 0;
        
        public void Increment()
        {
            currentCount++;
        }
    }
}
